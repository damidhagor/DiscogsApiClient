using System.Collections.Immutable;
using DiscogsApiClient.ApiClientGenerator.Attributes;
using DiscogsApiClient.ApiClientGenerator.Generators;
using DiscogsApiClient.ApiClientGenerator.Helpers;
using DiscogsApiClient.ApiClientGenerator.Parser;

namespace DiscogsApiClient.ApiClientGenerator;

[Generator]
public class ApiClientSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(DoPostInitialization);

        var interfaceDeclarations = context.SyntaxProvider.CreateSyntaxProvider(
            static (syntaxNode, cancellationToken) => IsSyntaxNodeGenerationTarget(syntaxNode),
            static (syntaxContext, cancellationToken) => GetSemanticTargetForGeneration(syntaxContext))
            .Where(static syntax => syntax is not null);

        var compilationAndInterfaces = context.CompilationProvider.Combine(interfaceDeclarations.Collect());

        context.RegisterSourceOutput(
            compilationAndInterfaces,
            static (sourceProductionContext, source) => Execute(source.Left, source.Right!, sourceProductionContext));
    }

    private static void DoPostInitialization(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddApiClientAttribute()
               .AddHttpMethodAttribute()
               .AddHttpGetAttribute()
               .AddHttpPostAttribute()
               .AddHttpPutAttribute()
               .AddHttpDeleteAttribute()
               .AddBodyAttribute()
               .AddAliasAsAttribute();
    }

    private static bool IsSyntaxNodeGenerationTarget(SyntaxNode syntaxNode)
    {
        return syntaxNode is InterfaceDeclarationSyntax { AttributeLists.Count: > 0 };
    }

    private static InterfaceDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var interfaceSyntax = (InterfaceDeclarationSyntax)context.Node;

        foreach (var attributeListSyntax in interfaceSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (IsMarkerAttribute(attributeSyntax, context))
                {
                    return interfaceSyntax;
                }
            }
        }

        return null;
    }

    private static bool IsMarkerAttribute(AttributeSyntax attributeSyntax, GeneratorSyntaxContext context)
    {
        return context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is IMethodSymbol attributeSymbol
                    && attributeSymbol.ContainingNamespace.ToDisplayString() == ApiCLientAttribute.Namespace
                    && attributeSymbol.ContainingType.Name == ApiCLientAttribute.Name;
    }

    private static void Execute(Compilation compilation, ImmutableArray<InterfaceDeclarationSyntax> interfaceDeclarations, SourceProductionContext context)
    {
        if (interfaceDeclarations.IsDefaultOrEmpty)
        {
            return;
        }

        var apiClients = new List<ApiClient>();
        foreach (var interfaceDeclaration in interfaceDeclarations.Distinct())
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var apiClient = interfaceDeclaration.ParseApiClient(compilation, context.CancellationToken);
            if (apiClient is not null)
            {
                apiClients.Add(apiClient);
            }
        }

        foreach (var apiClient in apiClients)
        {
            var (hint, source) = apiClient.GenerateApiClient(context.CancellationToken);

            using var writer = FileOutputDebugHelper.GetOutputStreamWriter($"client-{hint}", false);
            writer.WriteLine(source.ToString());

            context.AddSource(hint, source);
        }
    }
}
