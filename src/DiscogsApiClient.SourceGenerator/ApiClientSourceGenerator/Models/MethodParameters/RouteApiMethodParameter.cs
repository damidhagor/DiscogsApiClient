using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

internal sealed class RouteApiMethodParameter : ApiMethodParameter
{
    public string RoutePart { get; private set; }

    public RouteApiMethodParameter(ParsedParameterTypeInfo typeInfo, string routePart)
        : base(typeInfo, ApiMethodParameterType.Route)
    {
        RoutePart = routePart;
    }
}
