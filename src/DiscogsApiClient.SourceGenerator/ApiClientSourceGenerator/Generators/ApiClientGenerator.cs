using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class ApiClientGenerator
{
    public static (string hint, SourceText) GenerateApiClient(this ApiClient apiClient, CancellationToken cancellationToken)
    {
        var source = GenerateApiClientSource(apiClient, cancellationToken);
        var hint = $"{apiClient.InterfaceTypeInfo.Namespace}.{apiClient.ClientName}.g.cs";

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

            namespace {{apiClient.ClientNamespace}};
        
            internal partial class {{apiClient.ClientName}} : {{apiClient.InterfaceTypeInfo.FullTypeName}}
            {
                private readonly global::System.Net.Http.HttpClient _httpClient;
                private readonly global::{{ApiClientSettingsGenerator.Namespace}}.{{ApiClientSettingsGenerator.Name}}<{{apiClient.InterfaceTypeInfo.FullTypeName}}> _apiClientSettings;

                public {{apiClient.ClientName}}(
                    global::System.Net.Http.HttpClient httpClient,
                    global::{{ApiClientSettingsGenerator.Namespace}}.{{ApiClientSettingsGenerator.Name}}<{{apiClient.InterfaceTypeInfo.FullTypeName}}> apiClientSettings)
                {
                    _httpClient = httpClient;
                    _apiClientSettings = apiClientSettings;
                }
            """);
    }

    private static void GenerateApiClientEnd(this StringBuilder builder)
    {
        builder.Append(
            """

            private void Send(global::System.Net.Http.HttpMethod httpMethod, string route, object? payload = null, global::System.Threading.CancellationToken cancellationToken = default)
            {
                var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);

                if (payload is not null)
                {
                    var content = global::System.Text.Json.JsonSerializer.Serialize(payload, _apiClientSettings.JsonSerializerOptions);
                    request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                }

                var response = _httpClient.Send(request, cancellationToken);
                response.EnsureSuccessStatusCode();
            }

            private T Send<T>(global::System.Net.Http.HttpMethod httpMethod, string route, object? payload = null, global::System.Threading.CancellationToken cancellationToken = default)
            {
                var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
        
                if (payload is not null)
                {
                    var content = global::System.Text.Json.JsonSerializer.Serialize(payload, _apiClientSettings.JsonSerializerOptions);
                    request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                }
        
                var response = _httpClient.Send(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseStream = response.Content.ReadAsStream();

                return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, _apiClientSettings.JsonSerializerOptions)
                    ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
            }

            private async Task SendAsync(global::System.Net.Http.HttpMethod httpMethod, string route, object? payload = null, global::System.Threading.CancellationToken cancellationToken = default)
            {
                var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
        
                if (payload is not null)
                {
                    var content = global::System.Text.Json.JsonSerializer.Serialize(payload, _apiClientSettings.JsonSerializerOptions);
                    request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                }
        
                var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
        
            private async Task<T> SendAsync<T>(global::System.Net.Http.HttpMethod httpMethod, string route, object? payload = null, global::System.Threading.CancellationToken cancellationToken = default)
            {
                var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
        
                if (payload is not null)
                {
                    var content = global::System.Text.Json.JsonSerializer.Serialize(payload, _apiClientSettings.JsonSerializerOptions);
                    request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                }
        
                var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseStream = response.Content.ReadAsStream();
        
                return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, _apiClientSettings.JsonSerializerOptions, cancellationToken)
                    ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
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

                    builder.Append(queryParameter.TypeInfo.FullTypeName);
                    builder.Append(' ');
                    builder.Append(queryParameter.TypeInfo.ParameterName);

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
                    builder.AppendLine($"\t\t{queryParameters[i].TypeInfo.ParameterName}.CalculateQuerySize(ref capacity, ref parameterCount);");
                }

                builder.AppendLine(
                    """
                    
                            capacity += parameterCount;

                            var queryBuilder = new global::System.Text.StringBuilder(route, capacity);
                            queryBuilder.Append('?');

                    """);

                builder.AppendLine("\t\tvar routeLength = queryBuilder.Length + 1;");

                builder.AppendLine();

                for (var i = 0; i < queryParameters.Length; i++)
                {
                    builder.AppendLine($"\t\t{queryParameters[i].TypeInfo.ParameterName}.AppendQuery(queryBuilder, routeLength);");
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
