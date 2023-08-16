using DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

namespace DiscogsApiClient.ApiClientGenerator.Generators;

internal static class ApiClientGenerator
{
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

        builder.GenerateRouteBuilderMethods(apiClient.Methods, cancellationToken);

        builder.GenerateApiClientEnd();

        builder.GenerateQueryParameterClasses(apiClient.Methods, cancellationToken);

        return builder.ToString();
    }

    private static void GenerateApiClientStart(this StringBuilder builder, ApiClient apiClient)
    {
        builder.AppendLine(
            $$"""
            #nullable enable

            namespace {{apiClient.NamespaceName}};
        
            public partial class {{apiClient.ClientName}} : {{apiClient.InterfaceName}}
            {
                private readonly System.Net.Http.HttpClient _httpClient;

                public {{apiClient.ClientName}}(System.Net.Http.HttpClient httpClient)
                {
                    _httpClient = httpClient;
                }
            """);
    }

    private static void GenerateApiClientEnd(this StringBuilder builder)
    {
        builder.Append(
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
        """);
    }

    private static void GenerateRouteBuilderMethods(this StringBuilder builder, List<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        foreach (var apiMethod in apiMethods)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var queryParameters = apiMethod.Parameters.OfType<QueryApiMethodParameter>().ToArray();
            if (queryParameters.Length > 0)
            {
                builder.AppendLine();
                builder.Append($"\tprivate string BuildRouteFor{apiMethod.Name}(string route, ");

                for (var i = 0; i < queryParameters.Length; i++)
                {
                    var queryParameter = queryParameters[i];

                    builder.Append(queryParameter.FullName);

                    if (i < queryParameters.Length - 1)
                    {
                        builder = builder.Append(", ");
                    }
                }

                builder.AppendLine(")");
                builder.AppendLine("\t{");

                builder.AppendLine("\t\tvar capacity = route.Length;");
                builder.AppendLine("\t\tvar parameterCount = 0;");
                builder.AppendLine();


                for (var i = 0; i < queryParameters.Length; i++)
                {
                    builder.AppendLine($"\t\t{queryParameters[i].Name}.CalculateQuerySize(ref capacity, ref parameterCount);");
                }

                builder.AppendLine(
                    """
                    
                            capacity += parameterCount;

                            var queryBuilder = new System.Text.StringBuilder(route, capacity);
                            queryBuilder.Append('?');

                    """);

                builder.AppendLine("\t\tvar routeLength = queryBuilder.Length + 1;");

                builder.AppendLine();

                for (var i = 0; i < queryParameters.Length; i++)
                {
                    builder.AppendLine($"\t\t{queryParameters[i].Name}.AppendQuery(queryBuilder, routeLength);");
                }

                builder.AppendLine(
                    """

                            if (queryBuilder[queryBuilder.Length - 1] == '?')
                            {
                                queryBuilder.Length--;
                            }

                            return queryBuilder.ToString();
                        }
                    """);
            }
        }
    }
}
