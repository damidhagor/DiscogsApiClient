namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class HttpGetAttribute
{
    public const string Name = "HttpGetAttribute";

    public const string SourceHint = "HttpGetAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.GeneratorNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class {{Name}} : global::{{HttpMethodBaseAttribute.Namespace}}.{{HttpMethodBaseAttribute.Name}}
        {
            public const string Method = "Get";

            public {{Name}}(string route)
                : base(route)
            { }
        }
        """;
}
