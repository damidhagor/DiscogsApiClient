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

        builder.GenerateQueryParameterExtensions(apiClient.Methods, cancellationToken);
        builder.GenerateQueryParameterPropertyExtensions(apiClient.Methods, cancellationToken);

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

    private static void GenerateQueryParameterExtensions(this StringBuilder builder, List<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        var implementedExtensions = new HashSet<string>();
        var queryParameters = apiMethods
            .SelectMany(m => m.Parameters)
            .OfType<QueryApiMethodParameter>();

        foreach (var parameter in queryParameters)
        {
            if (implementedExtensions.Contains(parameter.FullName))
            {
                continue;
            }

            builder.AppendLine(
                $$"""
                

                #if NET7_0_OR_GREATER
                file static class {{parameter.TypeFullName.Replace(".", "").Replace("?", "")}}Extensions
                #else
                internal static class {{parameter.TypeFullName.Replace(".", "").Replace("?", "")}}Extensions
                #endif
                {
                    public static void CalculateQuerySize(this {{parameter.FullName}}, ref int capacity, ref int parameterCount)
                    {
                        if ({{parameter.Name}} is not null)
                        {
                """);

            foreach (var property in parameter.QueryParameters)
            {
                builder.AppendLine(
                    $$"""
                                    if ({{parameter.Name}}.{{property.PropertyName}} is not null)
                                    {
                                        capacity += {{property.ParameterName.Length + 1}}; // {{property.ParameterName}}
                                        capacity += QueryParameterHelper.CalculateQuerySize({{parameter.Name}}.{{property.PropertyName}});
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

                    public static void AppendQuery(this {{parameter.FullName}}, System.Text.StringBuilder queryBuilder, int routeLength)
                    {
                        if ({{parameter.Name}} is not null)
                        {
                """);

            foreach (var property in parameter.QueryParameters)
            {
                builder.AppendLine(
                    $$"""
                                if ({{parameter.Name}}.{{property.PropertyName}} is not null)
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
                                        queryBuilder.Append({{parameter.Name}} switch
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
                                        queryBuilder.Append({{parameter.Name}}.{{property.PropertyName}});
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


            builder.AppendLine("}");

            implementedExtensions.Add(parameter.FullName);
        }
    }

    private static void GenerateQueryParameterPropertyExtensions(this StringBuilder builder, List<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        var implementedExtensions = new HashSet<string>();
        var queryParameters = apiMethods
            .SelectMany(m => m.Parameters)
            .OfType<QueryApiMethodParameter>()
            .SelectMany(p => p.QueryParameters);

        builder.AppendLine(
            $$"""
                

            #if NET7_0_OR_GREATER
            file static class QueryParameterHelper
            #else
            internal static class QueryParameterHelper
            #endif
            {
            """
        );

        foreach (var parameter in queryParameters)
        {
            if (implementedExtensions.Contains(parameter.PropertyType))
            {
                continue;
            }

            if (parameter.ParameterType == QueryParameterType.String)
            {
                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize(string? text)
                        {
                            return text?.Length ?? 0;
                        }
                    """);
            }
            else if (parameter.ParameterType == QueryParameterType.Integer)
            {
                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize(int? number)
                        {
                            return number?.ToString()?.Length ?? 0;
                        }
                    """);
            }
            else if (parameter.ParameterType == QueryParameterType.Enum)
            {
                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize({{parameter.PropertyType}} enumValue)
                        {
                            return enumValue switch
                            {
                    """);

                if (parameter.EnumValues is not null)
                {
                    foreach (var enumValue in parameter.EnumValues)
                    {
                        builder.AppendLine(
                            $$"""
                                        {{parameter.PropertyType}}.{{enumValue.MemberName}} => {{enumValue.DisplayName.Length}}, // {{enumValue.DisplayName}}
                            """);
                    }
                }

                builder.AppendLine(
                    $$"""
                                _ => throw new ArgumentOutOfRangeException(nameof(enumValue))
                            };
                        }
                    """);

                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize({{parameter.PropertyType}}? enumValue)
                        {
                            return enumValue is not null ? CalculateQuerySize(enumValue.Value) : 0;
                        }
                    """);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(parameter.ParameterType));
            }

            implementedExtensions.Add(parameter.PropertyType);
        }

        builder.AppendLine("}");
    }
}
