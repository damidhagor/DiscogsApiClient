using DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

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

        builder.GenerateRouteBuilderMethods(apiClient.Methods, cancellationToken);

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

    private static void GenerateRouteBuilderMethods(this StringBuilder builder, List<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        foreach (var apiMethod in apiMethods)
        {
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
                    builder.AppendLine($"\t\tCalculateQuerySize({queryParameters[i].Name}, ref capacity, ref parameterCount);");
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
                    builder.AppendLine($"\t\tAppendQuery(queryBuilder, routeLength, {queryParameters[i].Name});");
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

        var implementedQueryBuilders = new HashSet<string>();

        foreach (var apiMethod in apiMethods)
        {
            foreach (var queryParameter in apiMethod.Parameters.OfType<QueryApiMethodParameter>())
            {
                if (implementedQueryBuilders.Contains(queryParameter.FullName))
                {
                    continue;
                }

                builder.AppendLine(
                    $$"""

                        private void CalculateQuerySize({{queryParameter.FullName}}, ref int capacity, ref int parameterCount)
                        {
                            if ({{queryParameter.Name}} is not null)
                            {
                    """);

                foreach (var property in queryParameter.QueryParameters)
                {
                    builder.AppendLine(
                        $$"""
                                    if ({{queryParameter.Name}}.{{property.PropertyName}} is not null)
                                    {
                                        capacity += {{property.ParameterName.Length}} + {{queryParameter.Name}}.{{property.PropertyName}}.ToString().Length;
                                        parameterCount++;
                                    }
                        """);
                }

                builder.AppendLine(
                    """
                            }
                        }
                    """);

                builder.AppendLine(
                    $$"""

                        private void AppendQuery(System.Text.StringBuilder queryBuilder, int routeLength, {{queryParameter.FullName}})
                        {
                            if ({{queryParameter.Name}} is not null)
                            {
                    """);

                foreach (var property in queryParameter.QueryParameters)
                {
                    builder.AppendLine(
                        $$"""
                                    if ({{queryParameter.Name}}.{{property.PropertyName}} is not null)
                                    {
                                        if (queryBuilder.Length > routeLength)
                                        {
                                            queryBuilder.Append('&');
                                        }

                                        queryBuilder.Append("{{property.ParameterName}}=");
                        """);

                    if (property.ParameterType == QueryParameterType.Enum)
                    {
                        builder.AppendLine(
                            $$"""
                                            queryBuilder.Append({{queryParameter.Name}} switch
                                            {
                            """);

                        foreach (var enumValue in property.EnumValues!)
                        {
                            builder.AppendLine(
                                $$"""
                                                    { {{property.PropertyName}}: {{property.PropertyType}}.{{enumValue.MemberName}} } => "{{enumValue.DisplayName}}",
                                """);
                        }

                        builder.AppendLine(
                            """
                                            });
                            """);
                    }
                    else
                    {
                        builder.AppendLine(
                            $$"""
                                            queryBuilder.Append({{queryParameter.Name}}.{{property.PropertyName}});
                            """);
                    }

                    builder.AppendLine(
                        """
                                    }
                        """);
                }

                builder.AppendLine(
                    """
                            }
                        }
                    """);

                implementedQueryBuilders.Add(queryParameter.FullName);
            }
        }
    }
}
