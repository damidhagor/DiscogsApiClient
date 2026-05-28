using DiscogsApiClient.SourceGenerator.JsonSerialization.Attributes;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Generators;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Models;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Parser;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization;

[Generator]
public class JsonConverterSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(DoPostInitialization);

        var enumerations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{Constants.JsonSerializationNamespace}.{GenerateJsonConverterAttribute.Name}",
                predicate: static (node, _) => node is EnumDeclarationSyntax,
                transform: static (ctx, ct) => EnumParser.ParseEnum(ctx, ct))
            .Where(static result => result is not null)
            .WithTrackingName("EnumTransform");

        var collected = enumerations.Collect().WithTrackingName("EnumCollect");

        context.RegisterSourceOutput(collected, static (spc, results) =>
        {
            var validEnums = new List<Enumeration>();

            foreach (var result in results)
            {
                if (result is null)
                {
                    continue;
                }

                foreach (var diagnostic in result.Diagnostics)
                {
                    spc.ReportDiagnostic(diagnostic.ToDiagnostic());
                }

                if (result.TryGetModel(out var model))
                {
                    validEnums.Add(model);
                }
            }

            if (validEnums.Count > 0)
            {
                var (hint, source) = validEnums.GenerateEnumJsonConverters(spc.CancellationToken);
                spc.AddSource(hint, source);
            }
        });
    }

    private static void DoPostInitialization(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddGenerateJsonConverterAttribute();
    }
}
