using DiscogsApiClient.ApiClientGenerator.Helpers;
using DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

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
        if (!methodSymbol.TryParseHttpMethodAttribute(out var methodType, out var route))
        {
            return null;
        }

        var methodName = methodSymbol.Name;

        if (!methodSymbol.ReturnType.TryParseApiMethodReturnType(out var returnType))
        {
            return null;
        }

        var parameters = methodSymbol.ParseApiMethodParameters(route!, cancellationToken);

        return new(methodName, route, methodType, parameters, returnType!);
    }

    private static bool TryParseHttpMethodAttribute(this IMethodSymbol methodSymbol, out ApiMethodType apiMethodType, out string route)
    {
        apiMethodType = ApiMethodType.Unknown;
        route = "";

        if (!methodSymbol.TryGetAttributeBase(
            AttributeSourceHelpers.AttributesNamespace,
            AttributeSourceHelpers.HttpMethodAttributeName,
            out var httpMethodAttributeData))
        {
            return false;
        }

        if (!httpMethodAttributeData!.TryGetAttributeConstructorArgument<string>(out var parsedRoute))
        {
            return false;
        }

        route = parsedRoute!;

        apiMethodType = httpMethodAttributeData?.AttributeClass switch
        {
            { Name: AttributeSourceHelpers.HttpGetAttributeName } => ApiMethodType.Get,
            { Name: AttributeSourceHelpers.HttpPostAttributeName } => ApiMethodType.Post,
            { Name: AttributeSourceHelpers.HttpPutAttributeName } => ApiMethodType.Put,
            { Name: AttributeSourceHelpers.HttpDeleteAttributeName } => ApiMethodType.Delete,
            _ => ApiMethodType.Unknown
        };

        if (apiMethodType == ApiMethodType.Unknown)
        {
            return false;
        }

        return true;
    }

    private static bool TryParseApiMethodReturnType(this ITypeSymbol typeSymbol, out ApiMethodReturnType? returnType)
    {
        returnType = null;

        if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return false;
        }

        if (namedTypeSymbol.SpecialType == SpecialType.System_Void)
        {
            returnType = ApiMethodReturnType.CreateVoid();
            return true;
        }

        var fullName = namedTypeSymbol.ToDisplayString();
        var namespaceName = namedTypeSymbol.ContainingNamespace.ToDisplayString();

        if (namespaceName == "System.Threading.Tasks"
            && namedTypeSymbol.Name == "Task")
        {
            if (namedTypeSymbol.IsGenericType
                && namedTypeSymbol.Arity == 1)
            {
                var resultTypeFullName = namedTypeSymbol.TypeArguments[0].ToDisplayString();
                returnType = ApiMethodReturnType.CreateTaskWithResult(fullName, resultTypeFullName);
            }
            else if (!namedTypeSymbol.IsGenericType)
            {
                returnType = ApiMethodReturnType.CreateTask(fullName);
            }
        }
        else
        {
            returnType = ApiMethodReturnType.CreateNoTask(fullName);
        }

        return returnType is not null;
    }

    private static List<ApiMethodParameter> ParseApiMethodParameters(this IMethodSymbol methodSymbol, string route, CancellationToken cancellationToken)
    {
        var parameters = new List<ApiMethodParameter>();

        foreach (var parameter in methodSymbol.Parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

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

            parameters.Add(apiMethodParameter);
        }

        return parameters;
    }

    private static List<QueryParameter> ParseAsQueryParameters(this ITypeSymbol type)
    {
        var parameters = new List<QueryParameter>();

        var properties = type
            .GetMembers()
            .Where(m => m.DeclaredAccessibility == Accessibility.Public
                     && m.Kind == SymbolKind.Property);

        foreach (var property in properties.OfType<IPropertySymbol>())
        {
            var propertyName = property.Name;
            var parameterName = property.Name;
            var isNullable = property.Type.NullableAnnotation == NullableAnnotation.Annotated;

            var propertyType = isNullable
                && ((INamedTypeSymbol)property.Type).IsGenericType
                && ((INamedTypeSymbol)property.Type).Arity == 1
                    ? ((INamedTypeSymbol)property.Type).TypeArguments[0] as INamedTypeSymbol
                    : ((INamedTypeSymbol)property.Type);
            if (propertyType is null)
            {
                continue;
            }

            var propertyTypeName = propertyType.ToDisplayString();

            if (property.TryGetAttribute(
                AttributeSourceHelpers.AttributesNamespace,
                AttributeSourceHelpers.AliasAsAttributeName,
                out var attribute)
                && attribute!.TryGetAttributeConstructorArgument<string>(out var altName))
            {
                parameterName = altName!;
            }

            if (propertyType.SpecialType == SpecialType.System_String)
            {
                parameters.Add(new QueryParameter(parameterName, propertyName, propertyTypeName, QueryParameterType.String, isNullable));
            }
            else if (propertyType.SpecialType == SpecialType.System_Int32)
            {
                parameters.Add(new QueryParameter(parameterName, propertyName, propertyTypeName, QueryParameterType.Integer, isNullable));
            }
            else if (propertyType.TypeKind == TypeKind.Enum)
            {
                var memberSymbols = propertyType.GetMembers()
                    .Where(m => m.Kind == SymbolKind.Field && m.IsStatic)
                    .OfType<IFieldSymbol>();

                var enumValues = new List<(string MemberName, string DisplayName)>();
                foreach (var memberSymbol in memberSymbols)
                {
                    var memberName = memberSymbol.Name;
                    var displayName = memberName;

                    if (memberSymbol.TryGetAttribute(
                        AttributeSourceHelpers.AttributesNamespace,
                        AttributeSourceHelpers.AliasAsAttributeName,
                        out var aliasAsAttribute)
                        && aliasAsAttribute!.TryGetAttributeConstructorArgument<string>(out var memberAltName))
                    {
                        displayName = memberAltName!;
                    }

                    enumValues.Add((memberName, displayName));
                }

                parameters.Add(new QueryParameter(parameterName, propertyName, propertyTypeName, QueryParameterType.Enum, isNullable, enumValues));
            }
        }

        return parameters;
    }
}
