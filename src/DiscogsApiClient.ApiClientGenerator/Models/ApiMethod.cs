using DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

namespace DiscogsApiClient.ApiClientGenerator.Models;

internal sealed class ApiMethod
{
    public string Name { get; private set; }

    public string Route { get; private set; }

    public ApiMethodType Method { get; private set; }

    public List<ApiMethodParameter> Parameters { get; private set; }

    public ApiMethodReturnType ReturnType { get; private set; }

    public ApiMethod(string name, string route, ApiMethodType method, List<ApiMethodParameter> parameters, ApiMethodReturnType returnType)
    {
        Name = name;
        Route = route;
        Method = method;
        Parameters = parameters;
        ReturnType = returnType;
    }
}
