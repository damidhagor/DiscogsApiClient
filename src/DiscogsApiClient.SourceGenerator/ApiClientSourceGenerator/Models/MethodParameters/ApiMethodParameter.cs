using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal abstract class ApiMethodParameter
{
    public ParsedParameterTypeInfo TypeInfo { get; private set; }

    public ApiMethodParameterType Type { get; private set; }

    public ApiMethodParameter(ParsedParameterTypeInfo typeInfo, ApiMethodParameterType parameterType)
    {
        TypeInfo = typeInfo;
        Type = parameterType;
    }
}
