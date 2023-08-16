namespace DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

internal sealed class BodyApiMethodParameter : ApiMethodParameter
{
    public BodyApiMethodParameter(string name, string fullName, string typeFullName)
        : base(name, fullName, typeFullName, ApiMethodParameterType.Body)
    { }
}
