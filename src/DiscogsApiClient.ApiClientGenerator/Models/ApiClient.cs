namespace DiscogsApiClient.ApiClientGenerator.Models;

internal sealed class ApiClient
{
    public string NamespaceName { get; private set; }

    public string ClientName { get; private set; }

    public string InterfaceName { get; private set; }

    public List<ApiMethod> Methods { get; private set; }

    public ApiClient(string namespaceName, string clientName, string interfaceName, List<ApiMethod> methods)
    {
        NamespaceName = namespaceName;
        ClientName = clientName;
        InterfaceName = interfaceName;
        Methods = methods;
    }
}
