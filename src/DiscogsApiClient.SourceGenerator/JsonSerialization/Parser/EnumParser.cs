using DiscogsApiClient.SourceGenerator.JsonSerialization.Models;
using DiscogsApiClient.SourceGenerator.Shared.Attributes;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Parser;

internal static class EnumParser
{
    public static Enumeration? ParseEnum(this EnumDeclarationSyntax enumSyntax, Compilation compilation, CancellationToken cancellationToken)
    {
        var enumModel = compilation.GetSemanticModel(enumSyntax.SyntaxTree);
        if (enumModel.GetDeclaredSymbol(enumSyntax) is not INamedTypeSymbol enumSymbol)
        {
            return null;
        }

        var enumFullName = enumSymbol.ToDisplayString();
        var enumName = enumSymbol.Name;
        var enumNamespace = enumSymbol.ContainingNamespace.ToDisplayString();
        var members = enumSymbol.ParseEnumMembers(cancellationToken);

        return new(enumFullName, enumNamespace, enumName, members);
    }

    public static List<EnumerationMember> ParseEnumMembers(this INamedTypeSymbol enumSymbol, CancellationToken cancellationToken)
    {
        var fieldSymbols = enumSymbol
            .GetMembers()
            .Where(m => m.Kind == SymbolKind.Field && m.IsStatic)
            .Cast<IFieldSymbol>();

        var enumMembers = new List<EnumerationMember>();
        foreach (var fieldSymbol in fieldSymbols)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var enumMember = fieldSymbol.ParseEnumMember(cancellationToken);

            if (enumMember is not null)
            {
                enumMembers.Add(enumMember);
            }
        }

        return enumMembers;
    }

    public static EnumerationMember? ParseEnumMember(this IFieldSymbol memberSymbol, CancellationToken cancellationToken)
    {
        var fieldName = memberSymbol.Name;
        var displayName = fieldName;

        if (memberSymbol.TryGetAttributeConstructorArgument<string>(
            Constants.SharedNamespace,
            AliasAsAttribute.Name,
            out var altName))
        {
            displayName = altName!;
        }

        return new(fieldName, displayName);
    }
}
