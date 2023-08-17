using DiscogsApiClient.SourceGenerator.Shared.Attributes;

namespace DiscogsApiClient.SourceGenerator.Shared.Generators;

internal static class AttributeGenerator
{
    public static IncrementalGeneratorPostInitializationContext AddAliasAsAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(AliasAsAttribute.SourceHint, CreateSourceText(AliasAsAttribute.Source));
        return context;
    }

    private static SourceText CreateSourceText(string source)
        => SourceText.From(source, Encoding.UTF8);
}
