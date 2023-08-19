using Microsoft.CodeAnalysis.CSharp;

namespace DiscogsApiClient.SourceGenerator.Shared.Helpers;

internal static class SyntaxExtensions
{
    public static bool HasMarkerAttribute(
        this MemberDeclarationSyntax syntax,
        GeneratorSyntaxContext syntaxContext,
        string attributeNamespace,
        string attributeName)
    {
        foreach (var attributeListSyntax in syntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                var isMarkerAttribute = syntaxContext.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is ISymbol attributeSymbol
                    && attributeSymbol.ContainingNamespace.ToDisplayString() == attributeNamespace
                    && attributeSymbol.ContainingType.Name == attributeName;

                if (isMarkerAttribute)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
