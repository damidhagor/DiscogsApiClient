using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class AttributeGenerator
{
    public static IncrementalGeneratorPostInitializationContext AddApiClientAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(ApiCLientAttribute.SourceHint, CreateSourceText(ApiCLientAttribute.Source));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpMethodAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(HttpMethodBaseAttribute.SourceHint, CreateSourceText(HttpMethodBaseAttribute.Source));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpGetAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(HttpGetAttribute.SourceHint, CreateSourceText(HttpGetAttribute.Source));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpPostAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(HttpPostAttribute.SourceHint, CreateSourceText(HttpPostAttribute.Source));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpPutAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(HttpPutAttribute.SourceHint, CreateSourceText(HttpPutAttribute.Source));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpDeleteAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(HttpDeleteAttribute.SourceHint, CreateSourceText(HttpDeleteAttribute.Source));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddBodyAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(BodyAttribute.SourceHint, CreateSourceText(BodyAttribute.Source));
        return context;
    }

    private static SourceText CreateSourceText(string source)
        => SourceText.From(source, Encoding.UTF8);
}
