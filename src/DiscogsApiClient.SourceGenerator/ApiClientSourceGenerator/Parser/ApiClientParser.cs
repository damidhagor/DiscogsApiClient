using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.Diagnostics;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Parser;

internal static class ApiClientParser
{
    public static GeneratorResult<ApiClient>? ParseApiClient(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol interfaceSymbol)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var location = DiagnosticLocation.From(context.TargetNode.GetLocation());

        INamedTypeSymbol? jsonSerializerContextTypeSymbol = null;
        string? clientName = null;
        string? clientNamespace = null;

        foreach (var attributeData in context.Attributes)
        {
            if (attributeData.ConstructorArguments.Length > 0
                && attributeData.ConstructorArguments[0].Value is INamedTypeSymbol contextType)
            {
                jsonSerializerContextTypeSymbol = contextType;
            }
            else if (attributeData.ConstructorArguments.Length > 0
                && attributeData.ConstructorArguments[0] is { Kind: TypedConstantKind.Type, Value: ITypeSymbol typeSymbol }
                && typeSymbol is INamedTypeSymbol namedType)
            {
                // Fallback for compilation scenarios where the Value is an ITypeSymbol
                // but was not directly resolved as INamedTypeSymbol in the first branch.
                jsonSerializerContextTypeSymbol = namedType;
            }

            foreach (var namedArg in attributeData.NamedArguments)
            {
                if (namedArg.Key == ApiClientAttribute.NamePropertyName
                    && namedArg.Value.Value is string name)
                {
                    clientName = name;
                }
                else if (namedArg.Key == ApiClientAttribute.NamespacePropertyName
                    && namedArg.Value.Value is string ns)
                {
                    clientNamespace = ns;
                }
            }
        }

        if (jsonSerializerContextTypeSymbol is null)
        {
            return null;
        }

        var typeInfo = interfaceSymbol.GetSymbolTypeInfo();
        var jsonSerializerContextTypeInfo = jsonSerializerContextTypeSymbol.GetSymbolTypeInfo();

        clientName ??= typeInfo.Name.Substring(1);
        clientNamespace ??= typeInfo.Namespace;

        var diagnostics = ImmutableArray.CreateBuilder<DiagnosticInfo>();

        var apiClient = new ApiClient(
            typeInfo,
            jsonSerializerContextTypeInfo,
            clientName,
            clientNamespace,
            interfaceSymbol.ParseApiMethods(location, diagnostics, cancellationToken));

        return diagnostics.Count > 0
            ? GeneratorResult<ApiClient>.Success(apiClient, diagnostics.ToImmutable())
            : GeneratorResult<ApiClient>.Success(apiClient);
    }
}
