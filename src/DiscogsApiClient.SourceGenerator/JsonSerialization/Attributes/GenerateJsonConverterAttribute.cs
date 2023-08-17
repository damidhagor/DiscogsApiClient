namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Attributes;

internal static class GenerateJsonConverterAttribute
{
    public const string Name = "GenerateJsonConverterAttribute";

    public const string SourceHint = "GenerateJsonConverterAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable

        namespace {{Constants.JsonSerializationNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Enum)]
        public sealed class {{Name}} : System.Attribute { }
        """;
}
