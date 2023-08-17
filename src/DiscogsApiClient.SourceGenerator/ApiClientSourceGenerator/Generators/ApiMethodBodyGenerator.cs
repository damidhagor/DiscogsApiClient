using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class ApiMethodBodyGenerator
{
    private const string _route =
        """
                string route = $"{0}";
        """;

    public static void GenerateApiMethodBody(this StringBuilder builder, ApiMethod apiMethod)
    {
        builder.GenerateRoute(apiMethod, apiMethod.Parameters);
        builder.AppendLine();
        builder.GenerateHttpCall(apiMethod);
    }

    private static void GenerateRoute(this StringBuilder builder, ApiMethod apiMethod, List<ApiMethodParameter> parameters)
    {
        var constructedRoute = $"{apiMethod.Route}";
        foreach (var parameter in parameters.OfType<RouteApiMethodParameter>())
        {
            constructedRoute = constructedRoute.Replace(parameter.RoutePart, $"{{{parameter.Name}}}");
        }

        var queryParameters = parameters.OfType<QueryApiMethodParameter>().ToArray();
        if (queryParameters.Length > 0)
        {
            builder.Append($"\t\tvar route = BuildRouteFor{apiMethod.Name}($\"{constructedRoute}\", ");

            for (var i = 0; i < queryParameters.Length; i++)
            {
                var queryParameter = queryParameters[i];

                builder.Append(queryParameter.Name);

                if (i < queryParameters.Length - 1)
                {
                    builder = builder.Append(", ");
                }
            }

            builder.AppendLine(");");
        }
        else
        {
            builder.AppendLine($"\t\tvar route = $\"{constructedRoute}\";");
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
