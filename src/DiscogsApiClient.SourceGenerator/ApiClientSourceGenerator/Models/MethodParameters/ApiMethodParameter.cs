namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal abstract class ApiMethodParameter
{
    public string Name { get; private set; }

    public string FullName { get; private set; }

    public string TypeFullName { get; private set; }

    public ApiMethodParameterType Type { get; private set; }

    public ApiMethodParameter(string name, string fullName, string typeFullName, ApiMethodParameterType parameterType)
    {
        Name = name;
        FullName = fullName;
        TypeFullName = typeFullName;
        Type = parameterType;
    }
}
