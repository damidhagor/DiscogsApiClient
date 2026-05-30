namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal sealed record ApiMethodParameter(
    Shared.Models.ParsedParameterTypeInfo TypeInfo,
    ApiMethodParameterType ParameterType,
    string? RoutePart = null,
    EquatableArray<QueryParameter> QueryParameters = default);
