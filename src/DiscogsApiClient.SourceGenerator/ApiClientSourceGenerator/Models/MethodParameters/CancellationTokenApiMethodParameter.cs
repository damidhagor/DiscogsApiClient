using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal sealed class CancellationTokenApiMethodParameter : ApiMethodParameter
{
    public CancellationTokenApiMethodParameter(ParsedParameterTypeInfo typeInfo)
        : base(typeInfo, ApiMethodParameterType.CancellationToken)
    { }
}
