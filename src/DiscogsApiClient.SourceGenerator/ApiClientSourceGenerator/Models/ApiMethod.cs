using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;

internal sealed record ApiMethod(
    string Name,
    string Route,
    ApiMethodType Method,
    string AccessModifier,
    EquatableArray<ApiMethodParameter> Parameters,
    ApiMethodReturnType ReturnType);
