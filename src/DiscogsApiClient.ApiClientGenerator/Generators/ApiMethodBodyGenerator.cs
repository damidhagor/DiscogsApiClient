namespace DiscogsApiClient.ApiClientGenerator.Generators;

internal static class ApiMethodBodyGenerator
{
    private const string _route =
        """
                string route = $"{0}";
        """;

    public static void GenerateApiMethodBody(this StringBuilder builder, ApiMethod apiMethod, CancellationToken cancellationToken)
    {
        builder.GenerateRoute(apiMethod.Route, apiMethod.Parameters);
        builder.AppendLine();
        builder.GenerateHttpCall(apiMethod);
    }

    private static void GenerateRoute(this StringBuilder builder, string route, List<ApiMethodParameter> parameters)
    {
        var constructedRoute = $"{route}";
        foreach (var parameter in parameters.OfType<RouteApiMethodParameter>())
        {
            constructedRoute = constructedRoute.Replace(parameter.RoutePart, $"{{{parameter.Name}}}");
        }

        builder.AppendLine($"\t\tvar route = $\"{constructedRoute}\";");
        builder.AppendLine();

        if (parameters.Any(p => p.Type == ApiMethodParameterType.Query))
        {
            builder.AppendLine("\t\tvar queryBuilder = new System.Text.StringBuilder(\"?\");");
            builder.AppendLine();

            foreach (var parameter in parameters.OfType<QueryApiMethodParameter>())
            {
                builder.AppendLine($"\t\tif ({parameter.Name} is not null)");
                builder.AppendLine("\t\t{");

                foreach (var queryParam in parameter.QueryParameters)
                {
                    builder.AppendLine($"\t\t\tif ({parameter.Name}.{queryParam.Value} is not null)");
                    builder.AppendLine("\t\t\t{");

                    builder.AppendLine(
                        """
                                        if (queryBuilder.Length > 1)
                                        {
                                            queryBuilder.Append("&");
                                        }

                        """);

                    builder.AppendLine($"\t\t\t\tqueryBuilder.Append(\"{queryParam.Parameter}=\");");
                    builder.AppendLine($"\t\t\t\tqueryBuilder.Append({parameter.Name}.{queryParam.Value});");
                    builder.AppendLine("\t\t\t}");
                }

                builder.AppendLine("\t\t}");
                builder.AppendLine();
            }

            builder.AppendLine(
                """
                        if (queryBuilder.Length > 1)
                        {
                            route += queryBuilder.ToString();
                        }
                """);
        }
    }

    private static void GenerateHttpCall(this StringBuilder builder, ApiMethod apiMethod)
    {
        var httpMethod = apiMethod switch
        {
            { Method: ApiMethodType.Get } => "System.Net.Http.HttpMethod.Get",
            { Method: ApiMethodType.Post } => "System.Net.Http.HttpMethod.Post",
            { Method: ApiMethodType.Put } => "System.Net.Http.HttpMethod.Put",
            { Method: ApiMethodType.Delete } => "System.Net.Http.HttpMethod.Delete",
            _ => ""
        };

        builder.Append("\t\t");

        if (apiMethod.ReturnType.HasResult)
        {
            builder.Append("var result = ");
        }

        if (apiMethod.ReturnType.IsTask)
        {
            builder.Append("await SendAsync");
        }
        else
        {
            builder.Append("Send");
        }

        if (apiMethod.ReturnType.IsTask
            && apiMethod.ReturnType.HasResult)
        {
            builder.Append($"<{apiMethod.ReturnType.TaskResultTypeFullName}>(");
        }
        else if (!apiMethod.ReturnType.IsVoid
            && !apiMethod.ReturnType.IsTask)
        {
            builder.Append($"<{apiMethod.ReturnType.FullName}>(");
        }
        else
        {
            builder.Append("(");
        }

        builder.Append($"{httpMethod}, route");

        var bodyParameter = apiMethod.Parameters
            .FirstOrDefault(p => p.Type == ApiMethodParameterType.Body);
        if (bodyParameter is not null)
        {
            builder.Append(", payload: ");
            builder.Append(bodyParameter.Name);
        }

        var cancellationTokenParameter = apiMethod.Parameters
            .FirstOrDefault(p => p.Type == ApiMethodParameterType.CancellationToken);
        if (cancellationTokenParameter is not null)
        {
            builder.Append(", cancellationToken: ");
            builder.Append(cancellationTokenParameter.Name);
        }

        builder.AppendLine(");");

        if (apiMethod.ReturnType.HasResult)
        {
            builder.AppendLine();
            builder.AppendLine("\t\treturn result;");
        }
    }
}
