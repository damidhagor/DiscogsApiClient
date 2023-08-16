namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class HttpPostAttribute
{
    public const string Namespace = "DiscogsApiClient.ApiClientGenerator";

    public const string Name = "HttpPostAttribute";

    public const string SourceHint = "HttpPostAttribute.g.cs";

    public const string Source =
        $$"""
        namespace {{Namespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class {{Name}} : global::{{HttpMethodBaseAttribute.Namespace}}.{{HttpMethodBaseAttribute.Name}}
        {
            public const string Method = "Post";
        
            public {{Name}}(string route)
                : base(route)
            { }
        }
        """;
}
