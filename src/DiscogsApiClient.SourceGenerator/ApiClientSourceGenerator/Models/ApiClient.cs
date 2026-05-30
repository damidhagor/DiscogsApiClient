using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;

internal sealed record ApiClient(
    ParsedTypeInfo InterfaceTypeInfo,
    ParsedTypeInfo JsonSerializerContextTypeSymbol,
    string ClientName,
    string ClientNamespace,
    EquatableArray<ApiMethod> Methods);
