using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class ApiClientGenerator
{
    public static (string hint, SourceText) GenerateApiClient(this ApiClient apiClient, CancellationToken cancellationToken)
    {
        var source = GenerateApiClientSource(apiClient, cancellationToken);
        var hint = $"{apiClient.ClassTypeInfo.Namespace}.{apiClient.ClassTypeInfo.Name}.g.cs";

        return (hint, SourceText.From(source, Encoding.UTF8));
    }

    private static string GenerateApiClientSource(ApiClient apiClient, CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();

        builder.GenerateApiClientStart(apiClient);

        foreach (var apiMethod in apiClient.Methods)
        {
            cancellationToken.ThrowIfCancellationRequested();
            builder.GenerateApiMethod(apiMethod, apiClient, cancellationToken);
        }

        builder.GenerateRouteBuilderMethods(apiClient.Methods, cancellationToken);

        builder.GenerateApiClientEnd(apiClient.HttpClientMemberName);

        builder.GenerateQueryParameterClasses(apiClient.Methods, cancellationToken);

        return builder.ToString();
    }

    private static void GenerateApiClientStart(this StringBuilder builder, ApiClient apiClient)
    {
        builder.Append(
            $$"""
            #nullable enable

            namespace {{apiClient.ClassTypeInfo.Namespace}};

            partial class {{apiClient.ClassTypeInfo.Name}}
            {
            """);
    }

    private static void GenerateApiClientEnd(this StringBuilder builder, string httpClientMemberName)
    {
        builder.Append(
            $$"""

            private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
            {
                return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
            }

            private void Send(
                global::System.Net.Http.HttpMethod httpMethod,
                string route,
                string? content,
                global::System.Threading.CancellationToken cancellationToken)
            {
                using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);

                if (content is not null)
                {
                    request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                }

                using var response = {{httpClientMemberName}}.Send(request, cancellationToken);
                response.EnsureSuccessStatusCode();
            }

            private T Send<T>(
                global::System.Net.Http.HttpMethod httpMethod,
                string route,
                global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                string? content,
                global::System.Threading.CancellationToken cancellationToken)
            {
                using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
        
                if (content is not null)
                {
                    request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                }
        
                using var response = {{httpClientMemberName}}.Send(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                using var responseStream = response.Content.ReadAsStream();

                return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, jsonTypeInfo)
                    ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
            }

            private async Task SendAsync(
                global::System.Net.Http.HttpMethod httpMethod,
                string route,
                string? content,
                global::System.Threading.CancellationToken cancellationToken)
            {
                using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
        
                if (content is not null)
                {
                    request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                }
        
                using var response = await {{httpClientMemberName}}.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
        
            private async Task<T> SendAsync<T>(
                global::System.Net.Http.HttpMethod httpMethod,
                string route,
                global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                string? content,
                global::System.Threading.CancellationToken cancellationToken)
            {
                using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
        
                if (content is not null)
                {
                    request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                }
        
                using var response = await {{httpClientMemberName}}.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                using var responseStream = response.Content.ReadAsStream();
        
                return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, jsonTypeInfo, cancellationToken)
                    ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
            }
        }
        """);
    }

    private static void GenerateRouteBuilderMethods(this StringBuilder builder, EquatableArray<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        foreach (var apiMethod in apiMethods)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var queryParameters = apiMethod.Parameters
                .Where(p => p.ParameterType == ApiMethodParameterType.Query)
                .ToArray();

            if (queryParameters.Length > 0)
            {
                builder.AppendLine();
                builder.Append($"    private string BuildRouteFor{apiMethod.Name}(string route, ");

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
                builder.AppendLine("    {");

                builder.AppendLine("        var capacity = route.Length;");
                builder.AppendLine("        var parameterCount = 0;");
                builder.AppendLine();

                for (var i = 0; i < queryParameters.Length; i++)
                {
                    builder.AppendLine($"        {queryParameters[i].TypeInfo.ParameterName}.CalculateQuerySize(ref capacity, ref parameterCount);");
                }

                builder.AppendLine(
                    """
                    
                            capacity += parameterCount;

                            var queryBuilder = new global::System.Text.StringBuilder(route, capacity);
                            queryBuilder.Append('?');

                    """);

                builder.AppendLine("        var routeLength = queryBuilder.Length + 1;");

                builder.AppendLine();

                for (var i = 0; i < queryParameters.Length; i++)
                {
                    builder.AppendLine($"        {queryParameters[i].TypeInfo.ParameterName}.AppendQuery(queryBuilder, routeLength);");
                }

                builder.AppendLine(
                    """

                            if (queryBuilder[^1] == '?')
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
