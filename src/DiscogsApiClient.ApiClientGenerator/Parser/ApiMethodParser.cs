using DiscogsApiClient.ApiClientGenerator.Helpers;

namespace DiscogsApiClient.ApiClientGenerator.Parser;

internal static class ApiMethodParser
{
    public static List<ApiMethod> ParseApiMethods(this INamedTypeSymbol interfaceSymbol, CancellationToken cancellationToken)
    {
        var methodSymbols = interfaceSymbol.GetMembers().Where(m => m.Kind == SymbolKind.Method).Cast<IMethodSymbol>();

        var apiMethodsToGenerate = new List<ApiMethod>();
        foreach (var methodSymbol in methodSymbols)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var apiMethod = methodSymbol.ParseApiMethod(cancellationToken);

            if (apiMethod is not null)
            {
                apiMethodsToGenerate.Add(apiMethod);
            }
        }

        return apiMethodsToGenerate;
    }

    private static ApiMethod? ParseApiMethod(this IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        using var writer = FileOutputDebugHelper.GetOutputStreamWriter("access.txt", true);

        if (!methodSymbol.TryGetAttributeBase(
            AttributeSourceHelpers.AttributesNamespace,
            AttributeSourceHelpers.HttpMethodAttributeName,
            out var httpMethodAttributeData))
        {
            return null;
        }

        if (!httpMethodAttributeData!.TryGetAttributeConstructorArgument<string>(out var route))
        {
            return null;
        }

        var methodName = methodSymbol.Name;

        ApiMethodType? methodType = httpMethodAttributeData?.AttributeClass switch
        {
            { Name: AttributeSourceHelpers.HttpGetAttributeName } => ApiMethodType.Get,
            { Name: AttributeSourceHelpers.HttpPostAttributeName } => ApiMethodType.Post,
            { Name: AttributeSourceHelpers.HttpPutAttributeName } => ApiMethodType.Put,
            { Name: AttributeSourceHelpers.HttpDeleteAttributeName } => ApiMethodType.Delete,
            _ => null
        };

        if (methodType is null)
        {
            return null;
        }

        if (methodSymbol?.ReturnType is not INamedTypeSymbol returnTypeSymbol)
        {
            return null;
        }

        var returnType = returnTypeSymbol.ParseApiMethodReturnType(cancellationToken);

        if (returnType is null)
        {
            return null;
        }

        var parameters = methodSymbol.ParseApiMethodParameters(route!, cancellationToken);

        return new(methodName, route!, methodType.Value, parameters, returnType);
    }

    private static ApiMethodReturnType? ParseApiMethodReturnType(this INamedTypeSymbol typeSymbol, CancellationToken cancellationToken)
    {
        if (typeSymbol.SpecialType == SpecialType.System_Void)
        {
            return ApiMethodReturnType.CreateVoid();
        }

        var fullName = typeSymbol.ToDisplayString();
        var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();

        if (namespaceName == "System.Threading.Tasks"
            && typeSymbol.Name == "Task")
        {
            if (typeSymbol.IsGenericType
                && typeSymbol.TypeArguments.Length == 1)
            {
                var resultTypeFullName = typeSymbol.TypeArguments[0].ToDisplayString();
                return ApiMethodReturnType.CreateTaskWithResult(fullName, resultTypeFullName);
            }
            else if (!typeSymbol.IsGenericType)
            {
                return ApiMethodReturnType.CreateTask(fullName);
            }
        }
        else
        {
            return ApiMethodReturnType.CreateNoTask(fullName);
        }

        return null;
    }

    private static List<ApiMethodParameter> ParseApiMethodParameters(this IMethodSymbol methodSymbol, string route, CancellationToken cancellationToken)
    {
        using var writer = FileOutputDebugHelper.GetOutputStreamWriter("parameters.txt", true);

        var parameters = new List<ApiMethodParameter>();

        foreach (var parameter in methodSymbol.Parameters)
        {
            var name = parameter.Name;
            var fullName = parameter.ToDisplayString();

            ApiMethodParameter apiMethodParameter;

            if (parameter.Type.ToDisplayString() == "System.Threading.CancellationToken")
            {
                apiMethodParameter = new CancellationTokenApiMethodParameter(name, fullName);
            }
            else if (parameter.TryGetAttribute(AttributeSourceHelpers.AttributesNamespace, AttributeSourceHelpers.BodyAttributeName, out _))
            {
                apiMethodParameter = new BodyApiMethodParameter(name, fullName);
            }
            else if (route.Contains($"{{{name}}}"))
            {
                apiMethodParameter = new RouteApiMethodParameter(name, fullName, $"{{{name}}}");
            }
            else
            {
                var queryParameters = parameter.Type.ParseAsQueryParameters();
                apiMethodParameter = new QueryApiMethodParameter(name, fullName, queryParameters);
            }

            writer.WriteLine(parameter.ToDisplayString());
            writer.WriteLine($"  Name: {apiMethodParameter.Name}");
            writer.WriteLine($"  FullName: {apiMethodParameter.FullName}");
            writer.WriteLine($"  Origin: {apiMethodParameter.Type}");
            if (apiMethodParameter is QueryApiMethodParameter p)
            {
                writer.WriteLine($"  Parameters: {string.Join(", ", p.QueryParameters.Select(qp => $"({qp.Parameter} - {qp.Value})"))}");
            }
            writer.WriteLine();

            parameters.Add(apiMethodParameter);
        }

        return parameters;
    }

    private static List<(string Parameter, string Value)> ParseAsQueryParameters(this ITypeSymbol type)
    {
        var parameters = new List<(string Parameter, string Value)>();

        var properties = type
            .GetMembers()
            .Where(m => m.DeclaredAccessibility == Accessibility.Public
                     && m.Kind == SymbolKind.Property);

        foreach (var property in properties)
        {
            var name = property.Name;

            if(property.TryGetAttribute(
                AttributeSourceHelpers.AttributesNamespace,
                AttributeSourceHelpers.AliasAsAttributeName,
                out var attribute)
                && attribute!.TryGetAttributeConstructorArgument<string>(out var altName))
            {
                name = altName!;
            }

            parameters.Add((name, property.Name));
        }

        return parameters;
    }
}
