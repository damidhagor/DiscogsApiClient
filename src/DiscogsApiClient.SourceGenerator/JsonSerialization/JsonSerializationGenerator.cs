using DiscogsApiClient.SourceGenerator.JsonSerialization.Generators;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization;

[Generator]
public class JsonSerializationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(DoPostInitialization);
    }

    private static void DoPostInitialization(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddGenerateJsonConverterAttribute();
    }
}
