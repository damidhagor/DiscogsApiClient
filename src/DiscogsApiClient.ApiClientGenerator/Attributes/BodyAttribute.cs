namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class BodyAttribute
{
    public const string Namespace = "DiscogsApiClient.ApiClientGenerator";

    public const string Name = "BodyAttribute";

    public const string SourceHint = "BodyAttribute.g.cs";

    public const string Source =
        $$"""
        namespace {{Namespace}};

        [System.AttributeUsage(System.AttributeTargets.Parameter)]
        public sealed class {{Name}} : System.Attribute { }
        """;
}
