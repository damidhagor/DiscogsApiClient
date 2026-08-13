namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Attributes;

internal static class GenerateJsonConverterAttribute
{
    public const string Name = "GenerateJsonConverterAttribute";

    public const string SourceHint = "GenerateJsonConverterAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable

        namespace {{Constants.JsonSerializationNamespace}};

        [global::System.AttributeUsage(global::System.AttributeTargets.Enum)]
        internal sealed class {{Name}} : global::System.Attribute { }
        """;
}
