namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class BodyAttribute
{
    public const string Name = "BodyAttribute";

    public const string SourceHint = "BodyAttribute.g.cs";

    public const string Source =
        $$"""
        {{Constants.GeneratedFileHeader}}
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        {{Constants.GeneratedCodeAttribute}}
        [global::System.AttributeUsage(global::System.AttributeTargets.Parameter)]
        internal sealed class {{Name}} : global::System.Attribute;
        """;
}
