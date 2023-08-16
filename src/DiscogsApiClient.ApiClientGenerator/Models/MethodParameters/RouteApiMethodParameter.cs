namespace DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

internal sealed class RouteApiMethodParameter : ApiMethodParameter
{
    public string RoutePart { get; private set; }

    public RouteApiMethodParameter(string name, string fullName, string typeFullName, string routePart)
        : base(name, fullName, typeFullName, ApiMethodParameterType.Route)
    {
        RoutePart = routePart;
    }
}
