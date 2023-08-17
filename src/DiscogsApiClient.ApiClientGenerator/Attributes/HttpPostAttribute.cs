namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class HttpPostAttribute
{
    public const string Name = "HttpPostAttribute";

    public const string SourceHint = "HttpPostAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.GeneratorNamespace}};

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
