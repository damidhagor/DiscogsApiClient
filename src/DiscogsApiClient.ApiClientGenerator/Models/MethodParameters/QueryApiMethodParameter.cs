namespace DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

internal sealed class QueryApiMethodParameter : ApiMethodParameter
{
    public List<QueryParameter> QueryParameters { get; private set; }

    public QueryApiMethodParameter(string name, string fullName, List<QueryParameter> queryParameters)
        : base(name, fullName, ApiMethodParameterType.Query)
    {
        QueryParameters = queryParameters;
    }
}
