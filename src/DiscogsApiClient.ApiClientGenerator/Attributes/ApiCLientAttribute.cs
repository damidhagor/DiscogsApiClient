namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class ApiCLientAttribute
{
    public const string Namespace = "DiscogsApiClient.ApiClientGenerator";

    public const string Name = "ApiClientAttribute";

    public const string SourceHint = "ApiClientAttribute.g.cs";

    public const string Source =
        $$"""
        namespace {{Namespace}};

        [System.AttributeUsage(System.AttributeTargets.Interface)]
        public sealed class {{Name}} : System.Attribute { }
        """;
}
