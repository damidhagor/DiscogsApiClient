using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.Shared.Helpers;

internal static class SymbolExtensions
{
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

    public static ParsedTypeInfo GetSymbolTypeInfo(this ISymbol symbol)
    {
        using var writer = FileOutputDebugHelper.GetOutputStreamWriter("typeinfos.txt", true);

        var name = symbol.Name;
        var @namespace = symbol.GetNamespace();
        var needsGlobalPrefix = true;

        writer.WriteLine($"{symbol.ToDisplayString()} ({symbol.GetType().FullName})");
        writer.WriteLine($"\tName: {name}");
        writer.WriteLine($"\tNamespace: {@namespace}");
        writer.WriteLine($"\tGlobal: {needsGlobalPrefix}");

        return new(name, @namespace, needsGlobalPrefix);
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
}
