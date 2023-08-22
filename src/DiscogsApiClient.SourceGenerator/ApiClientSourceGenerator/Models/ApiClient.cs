using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;

internal sealed class ApiClient
{
    public ParsedTypeInfo InterfaceTypeInfo { get; private set; }

    public string ClientName { get; private set; }

    public string ClientNamespace { get; private set; }

    public List<ApiMethod> Methods { get; private set; }

    public ApiClient(ParsedTypeInfo interfaceTypeInfo, string clientName, string clientNamespace, List<ApiMethod> methods)
    {
        InterfaceTypeInfo = interfaceTypeInfo;
        ClientName = clientName;
        ClientNamespace = clientNamespace;
        Methods = methods;
    }
}
