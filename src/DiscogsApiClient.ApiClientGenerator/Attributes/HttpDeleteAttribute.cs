namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class HttpDeleteAttribute
{
    public const string Namespace = "DiscogsApiClient.ApiClientGenerator";

    public const string Name = "HttpDeleteAttribute";

    public const string SourceHint = "HttpDeleteAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Namespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class {{Name}} : global::{{HttpMethodBaseAttribute.Namespace}}.{{HttpMethodBaseAttribute.Name}}
        {
            public const string Method = "Delete";
        
            public {{Name}}(string route)
                : base(route)
            { }
        }
        """;
}
