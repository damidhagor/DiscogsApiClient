using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Parser;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator;

[Generator]
public class ApiClientSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(DoPostInitialization);

        var apiClients = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{Constants.ApiClientNamespace}.{ApiClientAttribute.Name}",
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, ct) => ApiClientParser.ParseApiClient(ctx, ct))
            .WithTrackingName("ApiClientTransform");

        context.RegisterSourceOutput(apiClients, static (spc, result) =>
        {
            if (result is null)
            {
                return;
            }

            foreach (var diagnostic in result.Diagnostics)
            {
                spc.ReportDiagnostic(diagnostic.ToDiagnostic());
            }

            if (!result.TryGetModel(out var model))
            {
                return;
            }

            var (hint, source) = model.GenerateApiClient(spc.CancellationToken);
            spc.AddSource(hint, source);
        });
    }

    private static void DoPostInitialization(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddApiClientAttribute()
               .AddHttpMethodAttribute()
               .AddHttpGetAttribute()
               .AddHttpPostAttribute()
               .AddHttpPutAttribute()
               .AddHttpDeleteAttribute()
               .AddBodyAttribute();
    }
}
