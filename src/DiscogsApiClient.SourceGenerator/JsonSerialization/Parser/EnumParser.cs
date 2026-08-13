using DiscogsApiClient.SourceGenerator.JsonSerialization.Models;
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

        var typeInfo = enumSymbol.GetSymbolTypeInfo();

        return new(typeInfo);
    }
}
