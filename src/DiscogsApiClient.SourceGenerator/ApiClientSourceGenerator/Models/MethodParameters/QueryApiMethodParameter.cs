using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal sealed class QueryApiMethodParameter : ApiMethodParameter
{
    public List<QueryParameter> QueryParameters { get; private set; }

    public QueryApiMethodParameter(ParsedParameterTypeInfo typeInfo, List<QueryParameter> queryParameters)
        : base(typeInfo, ApiMethodParameterType.Query)
    {
        QueryParameters = queryParameters;
    }
}
