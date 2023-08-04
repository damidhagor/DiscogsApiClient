namespace DiscogsApiClient.ApiClientGenerator.Generators;

internal static class ApiClientGenerator
{
    private const string _clientStart =
        """
        #nullable enable

        namespace {0};
        
        public partial class {1} : {2}
        {{
            private readonly System.Net.Http.HttpClient _httpClient;

            public {1}(System.Net.Http.HttpClient httpClient)
            {{
                _httpClient = httpClient;
            }}
        """;
    private const string _clientEnd =
        """

            private void Send(System.Net.Http.HttpMethod httpMethod, string route, object? payload = null, System.Threading.CancellationToken cancellationToken = default)
            {
                var request = new HttpRequestMessage(httpMethod, route);

                if (payload is not null)
                {
                    var content = System.Text.Json.JsonSerializer.Serialize(payload);
                    request.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                }

                var response = _httpClient.Send(request, cancellationToken);
                response.EnsureSuccessStatusCode();
            }

            private T Send<T>(System.Net.Http.HttpMethod httpMethod, string route, object? payload = null, System.Threading.CancellationToken cancellationToken = default)
            {
                var request = new System.Net.Http.HttpRequestMessage(httpMethod, route);
        
                if (payload is not null)
                {
                    var content = System.Text.Json.JsonSerializer.Serialize(payload);
                    request.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                }
        
                var response = _httpClient.Send(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseStream = response.Content.ReadAsStream();

                return System.Text.Json.JsonSerializer.Deserialize<T>(responseStream);
            }

            private async Task SendAsync(System.Net.Http.HttpMethod httpMethod, string route, object? payload = null, System.Threading.CancellationToken cancellationToken = default)
            {
                var request = new HttpRequestMessage(httpMethod, route);
        
                if (payload is not null)
                {
                    var content = System.Text.Json.JsonSerializer.Serialize(payload);
                    request.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                }
        
                var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
        
            private async Task<T> SendAsync<T>(System.Net.Http.HttpMethod httpMethod, string route, object? payload = null, System.Threading.CancellationToken cancellationToken = default)
            {
                var request = new System.Net.Http.HttpRequestMessage(httpMethod, route);
        
                if (payload is not null)
                {
                    var content = System.Text.Json.JsonSerializer.Serialize(payload);
                    request.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                }
        
                var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseStream = response.Content.ReadAsStream();
        
                return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, cancellationToken: cancellationToken);
            }
        }
        """;

    public static (string hint, SourceText) GenerateApiClient(this ApiClient apiClient, CancellationToken cancellationToken)
    {
        var source = GenerateApiClientSource(apiClient, cancellationToken);
        var hint = $"{apiClient.NamespaceName}.{apiClient.ClientName}.g.cs";

        return (hint, SourceText.From(source, Encoding.UTF8));
    }

    private static string GenerateApiClientSource(ApiClient apiClient, CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();

        builder.GenerateApiClientStart(apiClient);

        foreach (var apiMethod in apiClient.Methods)
        {
            cancellationToken.ThrowIfCancellationRequested();
            builder.GenerateApiMethod(apiMethod, cancellationToken);
        }

        builder.GenerateApiClientEnd();

        return builder.ToString();
    }

    private static void GenerateApiClientStart(this StringBuilder builder, ApiClient apiClient)
    {
        builder.AppendLine(string.Format(_clientStart,
            apiClient.NamespaceName,
            apiClient.ClientName,
            apiClient.InterfaceName));
    }

    private static void GenerateApiClientEnd(this StringBuilder builder)
    {
        builder.Append(_clientEnd);
    }
}
