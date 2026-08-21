using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;

internal sealed record ApiClient(
    ParsedTypeInfo ClassTypeInfo,
    ParsedTypeInfo JsonSerializerContextTypeInfo,
    string HttpClientMemberName,
    string ContextMemberName,
    EquatableArray<ApiMethod> Methods);
