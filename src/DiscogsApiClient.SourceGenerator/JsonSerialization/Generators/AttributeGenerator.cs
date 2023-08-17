using DiscogsApiClient.SourceGenerator.JsonSerialization.Attributes;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Generators;

internal static class AttributeGenerator
{
    public static IncrementalGeneratorPostInitializationContext AddGenerateJsonConverterAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(GenerateJsonConverterAttribute.SourceHint, CreateSourceText(GenerateJsonConverterAttribute.Source));
        return context;
    }

    private static SourceText CreateSourceText(string source)
        => SourceText.From(source, Encoding.UTF8);
}
