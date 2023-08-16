namespace DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

internal sealed class RouteApiMethodParameter : ApiMethodParameter
{
    public string RoutePart { get; private set; }

    public RouteApiMethodParameter(string name, string fullName, string routePart)
        : base(name, fullName, ApiMethodParameterType.Route)
    {
        RoutePart = routePart;
    }
}
