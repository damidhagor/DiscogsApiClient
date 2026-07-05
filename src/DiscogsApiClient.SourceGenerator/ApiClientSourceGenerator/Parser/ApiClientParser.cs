using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.Diagnostics;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Parser;

internal static class ApiClientParser
{
    public static GeneratorResult<ApiClient>? ParseApiClient(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol classSymbol)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var location = DiagnosticLocation.From(context.TargetNode.GetLocation());

        INamedTypeSymbol? jsonSerializerContextTypeSymbol = null;

        foreach (var attributeData in context.Attributes)
        {
            if (attributeData.ConstructorArguments.Length > 0
                && attributeData.ConstructorArguments[0].Value is INamedTypeSymbol contextType)
            {
                jsonSerializerContextTypeSymbol = contextType;
            }
            else if (attributeData.ConstructorArguments.Length > 0
                && attributeData.ConstructorArguments[0] is { Kind: TypedConstantKind.Type, Value: ITypeSymbol typeSymbol }
                && typeSymbol is INamedTypeSymbol namedType)
            {
                // Fallback for compilation scenarios where the Value is an ITypeSymbol
                // but was not directly resolved as INamedTypeSymbol in the first branch.
                jsonSerializerContextTypeSymbol = namedType;
            }
        }

        if (jsonSerializerContextTypeSymbol is null)
        {
            return null;
        }

        var diagnostics = ImmutableArray.CreateBuilder<DiagnosticInfo>();

        var isPartial = context.TargetNode is ClassDeclarationSyntax classDeclaration
            && classDeclaration.Modifiers.Any(static m => m.Text == "partial");

        if (!isPartial)
        {
            diagnostics.Add(new(DiagnosticDescriptors.ApiClientMustBePartial, location, [classSymbol.Name]));
        }

        var httpClientMemberName = classSymbol.FindMemberName(static type => type.IsHttpClient());
        if (httpClientMemberName is null)
        {
            diagnostics.Add(new(DiagnosticDescriptors.MissingHttpClientMember, location, [classSymbol.Name]));
        }

        var contextMemberName = classSymbol.FindMemberName(type => type.IsOrDerivesFrom(jsonSerializerContextTypeSymbol));
        if (contextMemberName is null)
        {
            diagnostics.Add(new(
                DiagnosticDescriptors.MissingJsonSerializerContextMember,
                location,
                [classSymbol.Name, jsonSerializerContextTypeSymbol.GetSymbolTypeInfo().FullTypeName]));
        }

        if (!isPartial || httpClientMemberName is null || contextMemberName is null)
        {
            return GeneratorResult<ApiClient>.Failure(diagnostics.ToImmutable());
        }

        var apiClient = new ApiClient(
            classSymbol.GetSymbolTypeInfo(),
            jsonSerializerContextTypeSymbol.GetSymbolTypeInfo(),
            httpClientMemberName,
            contextMemberName,
            classSymbol.ParseApiMethods(location, diagnostics, cancellationToken));

        return diagnostics.Count > 0
            ? GeneratorResult<ApiClient>.Success(apiClient, diagnostics.ToImmutable())
            : GeneratorResult<ApiClient>.Success(apiClient);
    }

    private static string? FindMemberName(this INamedTypeSymbol classSymbol, Func<ITypeSymbol, bool> typePredicate)
    {
        foreach (var member in classSymbol.GetMembers())
        {
            var memberType = member switch
            {
                IFieldSymbol { AssociatedSymbol: null } field => field.Type,
                IPropertySymbol property => property.Type,
                _ => null
            };

            if (memberType is not null && typePredicate(memberType))
            {
                return member.Name;
            }
        }

        return null;
    }

    private static bool IsHttpClient(this ITypeSymbol type)
        => type.Name == "HttpClient" && type.GetNamespace() == "System.Net.Http";

    private static bool IsOrDerivesFrom(this ITypeSymbol type, INamedTypeSymbol baseType)
    {
        var current = type;
        while (current is not null)
        {
            if (SymbolEqualityComparer.Default.Equals(current, baseType))
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }
}
