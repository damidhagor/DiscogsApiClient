using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;
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
        var typeInfo = typeSymbol.GetSymbolTypeInfo();

        if (typeSymbol.SpecialType == SpecialType.System_Void)
        {
            returnType = new ApiMethodReturnType(typeInfo, true);
            return true;
        }

        returnType = new ApiMethodReturnType(typeInfo, false);
        return true;
    }

    private static List<ApiMethodParameter> ParseApiMethodParameters(this IMethodSymbol methodSymbol, string route, CancellationToken cancellationToken)
    {
        var parameters = new List<ApiMethodParameter>();

        foreach (var parameter in methodSymbol.Parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var typeInfo = parameter.GetParameterSymbolTypeInfo();

            ApiMethodParameter apiMethodParameter;

            if (typeInfo.IsType<CancellationToken>())
            {
                apiMethodParameter = new CancellationTokenApiMethodParameter(typeInfo);
            }
            else if (parameter.HasAttribute(Constants.ApiClientNamespace, BodyAttribute.Name))
            {
                apiMethodParameter = new BodyApiMethodParameter(typeInfo);
            }
            else if (route.Contains($"{{{typeInfo.ParameterName}}}"))
            {
                apiMethodParameter = new RouteApiMethodParameter(typeInfo, $"{{{typeInfo.ParameterName}}}");
            }
            else
            {
                var queryParameters = parameter.Type.ParseAsQueryParameters(cancellationToken);
                apiMethodParameter = new QueryApiMethodParameter(typeInfo, queryParameters);
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

            var typeInfo = property.GetParameterSymbolTypeInfo();

            if (typeInfo.IsType<string>())
            {
                parameters.Add(new QueryParameter(typeInfo, QueryParameterType.String));
            }
            else if (typeInfo.IsType<int>())
            {
                parameters.Add(new QueryParameter(typeInfo, QueryParameterType.Integer));
            }
            else if (typeInfo.IsEnum)
            {
                parameters.Add(new QueryParameter(typeInfo, QueryParameterType.Enum));
            }
        }

        return parameters;
    }
}
