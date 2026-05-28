using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;
using DiscogsApiClient.SourceGenerator.Diagnostics;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Parser;

internal static class ApiMethodParser
{
    public static EquatableArray<ApiMethod> ParseApiMethods(
        this INamedTypeSymbol interfaceSymbol,
        DiagnosticLocation location,
        ImmutableArray<DiagnosticInfo>.Builder diagnostics,
        CancellationToken cancellationToken)
    {
        var builder = ImmutableArray.CreateBuilder<ApiMethod>();

        foreach (var methodSymbol in interfaceSymbol.GetMembers().OfType<IMethodSymbol>())
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (methodSymbol.ParseApiMethod(location, diagnostics, cancellationToken) is { } apiMethod)
            {
                builder.Add(apiMethod);
            }
        }

        return new(builder.ToImmutable());
    }

    private static ApiMethod? ParseApiMethod(
        this IMethodSymbol methodSymbol,
        DiagnosticLocation location,
        ImmutableArray<DiagnosticInfo>.Builder diagnostics,
        CancellationToken cancellationToken)
    {
        if (!methodSymbol.TryParseHttpMethodAttribute(out var methodType, out var route))
        {
            if (methodSymbol.HasAttribute(Constants.ApiClientNamespace, HttpMethodBaseAttribute.Name))
            {
                diagnostics.Add(new(DiagnosticDescriptors.UnknownHttpMethod, location, [methodSymbol.Name, "unrecognized"]));
            }

            return null;
        }

        if (!methodSymbol.ReturnType.TryParseApiMethodReturnType(out var returnType))
        {
            diagnostics.Add(new(DiagnosticDescriptors.InvalidMethodReturnType, location, [methodSymbol.Name]));
            return null;
        }

        // Validate route parameters (DISCOGS004)
        var index = 0;
        while (index < route.Length)
        {
            var open = route.IndexOf('{', index);
            if (open < 0)
            {
                break;
            }

            var close = route.IndexOf('}', open);
            if (close < 0)
            {
                break;
            }

            var routeParam = route.Substring(open + 1, close - open - 1);
            if (!methodSymbol.Parameters.Any(p => p.Name == routeParam))
            {
                diagnostics.Add(new(DiagnosticDescriptors.RouteParameterMismatch, location, [routeParam, route]));
            }

            index = close + 1;
        }

        var parameters = methodSymbol.ParseApiMethodParameters(route, location, diagnostics, cancellationToken);

        return new(methodSymbol.Name, route, methodType, parameters, returnType!);
    }

    private static bool TryParseHttpMethodAttribute(this IMethodSymbol methodSymbol, out ApiMethodType apiMethodType, out string route)
    {
        apiMethodType = ApiMethodType.Unknown;
        route = string.Empty;

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

        return apiMethodType != ApiMethodType.Unknown;
    }

    private static bool TryParseApiMethodReturnType(this ITypeSymbol typeSymbol, out ApiMethodReturnType? returnType)
    {
        returnType = new(typeSymbol.GetSymbolTypeInfo());

        if (!returnType.IsTask)
        {
            returnType = null;
            return false;
        }

        return true;
    }

    private static EquatableArray<ApiMethodParameter> ParseApiMethodParameters(
        this IMethodSymbol methodSymbol,
        string route,
        DiagnosticLocation location,
        ImmutableArray<DiagnosticInfo>.Builder diagnostics,
        CancellationToken cancellationToken)
    {
        var builder = ImmutableArray.CreateBuilder<ApiMethodParameter>();
        var hasBody = false;
        var hasCancellationToken = false;

        foreach (var parameter in methodSymbol.Parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var typeInfo = parameter.GetParameterSymbolTypeInfo();

            if (typeInfo.IsType<CancellationToken>())
            {
                hasCancellationToken = true;
                builder.Add(new(typeInfo, ApiMethodParameterType.CancellationToken));
            }
            else if (parameter.HasAttribute(Constants.ApiClientNamespace, BodyAttribute.Name))
            {
                if (hasBody)
                {
                    diagnostics.Add(new(DiagnosticDescriptors.DuplicateBodyParameter, location, [methodSymbol.Name]));
                }
                else
                {
                    hasBody = true;
                    builder.Add(new(typeInfo, ApiMethodParameterType.Body));
                }
            }
            else if (route.Contains($"{{{typeInfo.ParameterName}}}"))
            {
                builder.Add(new(typeInfo, ApiMethodParameterType.Route, RoutePart: $"{{{typeInfo.ParameterName}}}"));
            }
            else
            {
                builder.Add(new(
                    typeInfo,
                    ApiMethodParameterType.Query,
                    QueryParameters: parameter.Type.ParseAsQueryParameters(
                        location,
                        diagnostics,
                        cancellationToken)));
            }
        }

        if (!hasCancellationToken)
        {
            diagnostics.Add(new(DiagnosticDescriptors.MissingCancellationToken, location, [methodSymbol.Name]));
        }

        return new(builder.ToImmutable());
    }

    private static EquatableArray<QueryParameter> ParseAsQueryParameters(
        this ITypeSymbol type,
        DiagnosticLocation location,
        ImmutableArray<DiagnosticInfo>.Builder diagnostics,
        CancellationToken cancellationToken)
    {
        var builder = ImmutableArray.CreateBuilder<QueryParameter>();

        var properties = type
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(m => m.DeclaredAccessibility == Accessibility.Public);

        foreach (var property in properties)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var typeInfo = property.GetParameterSymbolTypeInfo();

            if (typeInfo.IsType<string>())
            {
                builder.Add(new(typeInfo, QueryParameterType.String));
            }
            else if (typeInfo.IsType<int>())
            {
                builder.Add(new(typeInfo, QueryParameterType.Integer));
            }
            else if (typeInfo.IsEnum)
            {
                builder.Add(new(typeInfo, QueryParameterType.Enum));
            }
            else
            {
                diagnostics.Add(
                    new(
                        DiagnosticDescriptors.UnsupportedQueryParameterType,
                        location,
                        [property.Name, type.Name, property.Type.ToDisplayString()]));
            }
        }

        return new(builder.ToImmutable());
    }
}
