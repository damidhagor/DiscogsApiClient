using DiscogsApiClient.SourceGenerator.Shared.Attributes;
using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.Shared.Helpers;

internal static class SymbolExtensions
{
    public static bool IsType<T>(this ITypeSymbol symbol) => symbol.IsType(typeof(T));

    public static bool IsType(this ITypeSymbol symbol, Type type) => symbol.GetNamespace() == type.Namespace && type.Name == symbol.Name;

    public static bool TryGetConstFieldValue<T>(this INamedTypeSymbol? symbol, string fieldName, out T? value)
    {
        value = default;

        var inspectedSymbol = symbol;
        while (inspectedSymbol is not null)
        {
            var field = inspectedSymbol.GetMembers()
                .OfType<IFieldSymbol>()
                .FirstOrDefault(f => f.IsConst && f.Name == fieldName);

            if (field is not null)
            {
                if (field.ConstantValue is T castValue)
                {
                    value = castValue;
                    return true;
                }

                return false;
            }

            inspectedSymbol = inspectedSymbol.BaseType;
        }

        return false;
    }

    public static bool TryGetGenericTypeArgument(this INamedTypeSymbol? symbol, int index, out ITypeSymbol? genericArgument)
    {
        genericArgument = default;
        if (symbol is null
            || !symbol.IsGenericType
            || index >= symbol.Arity)
        {
            return false;
        }

        genericArgument = symbol.TypeArguments[index];
        return true;
    }

    public static ParsedTypeInfo GetSymbolTypeInfo(this ITypeSymbol symbol)
    {
        var isNullable = symbol.NullableAnnotation == NullableAnnotation.Annotated;

        if (isNullable
            && symbol.IsType(typeof(Nullable))
            && symbol is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.TryGetGenericTypeArgument(0, out var genericArgument))
            {
                symbol = genericArgument!;
            }
        }

        var name = symbol.Name;
        var @namespace = symbol.GetNamespace();
        var needsGlobalPrefix = true;
        var isEnum = symbol.BaseType?.IsType<Enum>() ?? false;

        var genericTypeArgs = symbol is INamedTypeSymbol namedSymbol && namedSymbol.IsGenericType
            ? namedSymbol.TypeArguments.Select(a => a.GetSymbolTypeInfo()).ToList()
            : new();

        var enumMembers = isEnum
            ? symbol.GetEnumMembers()
            : new();

        return new(name, @namespace, needsGlobalPrefix, isNullable, genericTypeArgs, enumMembers);
    }

    public static ParsedParameterTypeInfo GetParameterSymbolTypeInfo(this IParameterSymbol symbol) => GetParameterSymbolTypeInfo(symbol, symbol.Type);

    public static ParsedParameterTypeInfo GetParameterSymbolTypeInfo(this IPropertySymbol symbol) => GetParameterSymbolTypeInfo(symbol, symbol.Type);

    public static ParsedParameterTypeInfo GetParameterSymbolTypeInfo(ISymbol parameterSymbol, ITypeSymbol typeSymbol)
    {
        var typeInfo = typeSymbol.GetSymbolTypeInfo();

        var (parameterName, parameterNameAlias) = parameterSymbol.GetSymbolNameWithAlias();

        return new ParsedParameterTypeInfo(parameterName, parameterNameAlias, typeInfo.Name, typeInfo.Namespace, typeInfo.NeedsGlobalPrefix, typeInfo.IsNullable, typeInfo.GenericTypeArguments, typeInfo.EnumMembers);
    }

    public static (string Name, string NameAlias) GetSymbolNameWithAlias(this ISymbol symbol)
    {
        var name = symbol.Name;
        var nameAlias = name;

        if (symbol.TryGetAttributeConstructorArgument<string>(
            Constants.SharedNamespace,
            AliasAsAttribute.Name,
            out var parsedNameAlias))
        {
            nameAlias = parsedNameAlias!;
        }

        return new(name, nameAlias);
    }

    public static string GetNamespace(this ISymbol symbol)
    {
        var namespaceParts = new List<string>();

        var currentSymbol = symbol;
        while (currentSymbol.ContainingType is not null)
        {
            namespaceParts.Add(currentSymbol.ContainingType.Name);
            currentSymbol = currentSymbol.ContainingType;
        }

        var currentNamespaceSymbol = currentSymbol.ContainingNamespace;
        while (currentNamespaceSymbol is not null && !currentNamespaceSymbol.IsGlobalNamespace)
        {
            namespaceParts.Add(currentNamespaceSymbol.Name);
            currentNamespaceSymbol = currentNamespaceSymbol.ContainingNamespace;
        }

        namespaceParts.Reverse();
        return string.Join(".", namespaceParts);
    }

    public static List<EnumerationMember> GetEnumMembers(this ITypeSymbol enumSymbol)
    {
        var fieldSymbols = enumSymbol
            .GetMembers()
            .Where(m => m.Kind == SymbolKind.Field && m.IsStatic)
            .Cast<IFieldSymbol>();

        var enumMembers = new List<EnumerationMember>();
        foreach (var fieldSymbol in fieldSymbols)
        {
            var (memberName, memberNameAlias) = fieldSymbol.GetSymbolNameWithAlias();
            enumMembers.Add(new(memberName, memberNameAlias));
        }

        return enumMembers;
    }
}
