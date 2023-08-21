using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal sealed class QueryParameter
{
    public ParsedParameterTypeInfo TypeInfo { get; private set; }

    public QueryParameterType ParameterType { get; private set; }

    public QueryParameter(ParsedParameterTypeInfo typeInfo, QueryParameterType parameterType)
    {
        TypeInfo = typeInfo;
        ParameterType = parameterType;
    }
}
