namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class BodyAttribute
{
    public const string Name = "BodyAttribute";

    public const string SourceHint = "BodyAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Parameter)]
        public sealed class {{Name}} : System.Attribute { }
        """;
}
