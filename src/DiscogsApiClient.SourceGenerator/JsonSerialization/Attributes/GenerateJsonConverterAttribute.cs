namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Attributes;

internal static class GenerateJsonConverterAttribute
{
    public const string Name = "GenerateJsonConverterAttribute";

    public const string SourceHint = "GenerateJsonConverterAttribute.g.cs";

    public const string Source =
        $$"""
        {{Constants.GeneratedFileHeader}}
        #nullable enable

        namespace {{Constants.JsonSerializationNamespace}};

        {{Constants.GeneratedCodeAttribute}}
        [global::System.AttributeUsage(global::System.AttributeTargets.Enum)]
        internal sealed class {{Name}} : global::System.Attribute;
        """;
}
