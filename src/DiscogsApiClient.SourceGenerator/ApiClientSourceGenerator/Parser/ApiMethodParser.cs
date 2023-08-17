using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;
using DiscogsApiClient.SourceGenerator.Shared.Attributes;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Parser;

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

        if (!methodSymbol.TryGetAttributeConstructorArgument<string>(
            Constants.ApiClientNamespace,
            HttpMethodBaseAttribute.Name,
            out var parsedRoute))
        {
            return false;
        }

        route = parsedRoute!;

        if (!methodSymbol.TryGetConstAttributeFieldValue<string>(
            Constants.ApiClientNamespace,
            HttpMethodBaseAttribute.Name,
            "Method",
            out var parsedMethod))
        {
            return false;
        }

        apiMethodType = parsedMethod switch
        {
            "Get" => ApiMethodType.Get,
            "Post" => ApiMethodType.Post,
            "Put" => ApiMethodType.Put,
            "Delete" => ApiMethodType.Delete,
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
            if (!namedTypeSymbol.IsGenericType)
            {
                returnType = ApiMethodReturnType.CreateTask(fullName);
            }
            else if (namedTypeSymbol.TryGetGenericTypeArgument(0, out var genericArgument))
            {
                returnType = ApiMethodReturnType.CreateTaskWithResult(fullName, genericArgument.ToDisplayString());
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
            var typeFullName = parameter.Type.ToDisplayString();

            ApiMethodParameter apiMethodParameter;

            if (parameter.Type.ToDisplayString() == "System.Threading.CancellationToken")
            {
                apiMethodParameter = new CancellationTokenApiMethodParameter(name, fullName, typeFullName);
            }
            else if (parameter.HasAttribute(Constants.ApiClientNamespace, BodyAttribute.Name))
            {
                apiMethodParameter = new BodyApiMethodParameter(name, fullName, typeFullName);
            }
            else if (route.Contains($"{{{name}}}"))
            {
                apiMethodParameter = new RouteApiMethodParameter(name, fullName, typeFullName, $"{{{name}}}");
            }
            else
            {
                var queryParameters = parameter.Type.ParseAsQueryParameters(cancellationToken);
                apiMethodParameter = new QueryApiMethodParameter(name, fullName, typeFullName, queryParameters);
            }

            parameters.Add(apiMethodParameter);
        }

        return parameters;
    }

    private static List<QueryParameter> ParseAsQueryParameters(this ITypeSymbol type, CancellationToken cancellationToken)
    {
        var parameters = new List<QueryParameter>();

        var properties = type
            .GetMembers()
            .Where(m => m.DeclaredAccessibility == Accessibility.Public
                     && m.Kind == SymbolKind.Property);

        foreach (var property in properties.OfType<IPropertySymbol>())
        {
            cancellationToken.ThrowIfCancellationRequested();

            var propertyName = property.Name;
            var parameterName = property.Name;
            var isNullable = property.Type.NullableAnnotation == NullableAnnotation.Annotated;

            var propertyType = isNullable
                && ((INamedTypeSymbol)property.Type).TryGetGenericTypeArgument(0, out var genericPropertyType)
                    ? genericPropertyType as INamedTypeSymbol
                    : property.Type as INamedTypeSymbol;
            if (propertyType is null)
            {
                continue;
            }

            var propertyTypeName = propertyType.ToDisplayString();

            if (property.TryGetAttributeConstructorArgument<string>(
                Constants.SharedNamespace,
                AliasAsAttribute.Name,
                out var altName))
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
                    cancellationToken.ThrowIfCancellationRequested();

                    var memberName = memberSymbol.Name;
                    var displayName = memberName;

                    if (memberSymbol.TryGetAttributeConstructorArgument<string>(
                        Constants.SharedNamespace,
                        AliasAsAttribute.Name,
                        out var memberAltName))
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
