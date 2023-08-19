using System.Collections.Immutable;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Parser;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator;

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
               .AddApiClientSettings();
    }

    private static bool IsSyntaxNodeGenerationTarget(SyntaxNode syntaxNode)
    {
        return syntaxNode is InterfaceDeclarationSyntax { AttributeLists.Count: > 0 };
    }

    private static InterfaceDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var interfaceSyntax = (InterfaceDeclarationSyntax)context.Node;

        return interfaceSyntax.HasMarkerAttribute(context, Constants.ApiClientNamespace, ApiCLientAttribute.Name)
            ? interfaceSyntax
            : null;
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
