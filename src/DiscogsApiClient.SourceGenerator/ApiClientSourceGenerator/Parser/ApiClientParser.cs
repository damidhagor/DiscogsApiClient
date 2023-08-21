using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Parser;

internal static class ApiClientParser
{
    public static ApiClient? ParseApiClient(this InterfaceDeclarationSyntax interfaceSyntax, Compilation compilation, CancellationToken cancellationToken)
    {
        var interfaceModel = compilation.GetSemanticModel(interfaceSyntax.SyntaxTree);
        if (interfaceModel.GetDeclaredSymbol(interfaceSyntax) is not INamedTypeSymbol interfaceSymbol)
        {
            return null;
        }

        var typeInfo = interfaceSymbol.GetSymbolTypeInfo();
        var implementationName = typeInfo.Name.AsSpan().Slice(1, typeInfo.Name.Length - 1).ToString();
        var apiMethodsToGenerate = interfaceSymbol.ParseApiMethods(cancellationToken);

        return new(typeInfo, implementationName, apiMethodsToGenerate);
    }
}
