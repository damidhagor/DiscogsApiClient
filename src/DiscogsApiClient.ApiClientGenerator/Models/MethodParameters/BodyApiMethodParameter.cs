namespace DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

internal sealed class BodyApiMethodParameter : ApiMethodParameter
{
    public BodyApiMethodParameter(string name, string fullName)
        : base(name, fullName, ApiMethodParameterType.Body)
    { }
}
