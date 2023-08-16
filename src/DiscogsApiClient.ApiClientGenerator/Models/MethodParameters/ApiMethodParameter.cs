namespace DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

internal abstract class ApiMethodParameter
{
    public string Name { get; private set; }

    public string FullName { get; private set; }

    public ApiMethodParameterType Type { get; private set; }

    public ApiMethodParameter(string name, string fullName, ApiMethodParameterType parameterType)
    {
        Name = name;
        FullName = fullName;
        Type = parameterType;
    }
}
