using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;

internal sealed class ApiClient
{
    public ParsedTypeInfo InterfaceTypeInfo { get; private set; }

    public string ClientName { get; private set; }

    public List<ApiMethod> Methods { get; private set; }

    public ApiClient(ParsedTypeInfo interfaceTypeInfo, string clientName, List<ApiMethod> methods)
    {
        InterfaceTypeInfo = interfaceTypeInfo;
        ClientName = clientName;
        Methods = methods;
    }
}
