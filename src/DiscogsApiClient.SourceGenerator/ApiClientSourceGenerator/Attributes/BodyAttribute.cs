namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class BodyAttribute
{
    public const string Name = "BodyAttribute";

    public const string SourceHint = "BodyAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        [global::System.AttributeUsage(global::System.AttributeTargets.Parameter)]
        internal sealed class {{Name}} : global::System.Attribute { }
        """;
}
