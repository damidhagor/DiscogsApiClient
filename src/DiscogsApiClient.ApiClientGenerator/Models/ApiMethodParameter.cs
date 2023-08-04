namespace DiscogsApiClient.ApiClientGenerator.Models;

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

internal sealed class RouteApiMethodParameter : ApiMethodParameter
{
    public string RoutePart { get; private set; }

    public RouteApiMethodParameter(string name, string fullName, string routePart)
        : base(name, fullName, ApiMethodParameterType.Route)
    {
        RoutePart = routePart;
    }
}

internal sealed class BodyApiMethodParameter : ApiMethodParameter
{
    public BodyApiMethodParameter(string name, string fullName)
        : base(name, fullName, ApiMethodParameterType.Body)
    { }
}

internal sealed class CancellationTokenApiMethodParameter : ApiMethodParameter
{
    public CancellationTokenApiMethodParameter(string name, string fullName)
        : base(name, fullName, ApiMethodParameterType.CancellationToken)
    { }
}

internal sealed class QueryApiMethodParameter : ApiMethodParameter
{
    public List<(string Parameter, string Value)> QueryParameters { get; private set; }

    public QueryApiMethodParameter(string name, string fullName, List<(string Parameter, string Value)> queryParameters)
        : base(name, fullName, ApiMethodParameterType.Query)
    {
        QueryParameters = queryParameters;
    }
}
