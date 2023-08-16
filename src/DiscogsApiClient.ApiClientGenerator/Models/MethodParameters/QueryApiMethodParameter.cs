namespace DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

internal sealed class QueryApiMethodParameter : ApiMethodParameter
{
    public List<QueryParameter> QueryParameters { get; private set; }

    public QueryApiMethodParameter(string name, string fullName, string typeFullName, List<QueryParameter> queryParameters)
        : base(name, fullName, typeFullName, ApiMethodParameterType.Query)
    {
        QueryParameters = queryParameters;
    }
}
