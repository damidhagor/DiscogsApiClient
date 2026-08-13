using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal sealed class BodyApiMethodParameter : ApiMethodParameter
{
    public BodyApiMethodParameter(ParsedParameterTypeInfo typeInfo)
        : base(typeInfo, ApiMethodParameterType.Body)
    { }
}
