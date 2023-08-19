namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class HttpPutAttribute
{
    public const string Name = "HttpPutAttribute";

    public const string SourceHint = "HttpPutAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable

        namespace {{Constants.ApiClientNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        internal sealed class {{Name}} : global::{{Constants.ApiClientNamespace}}.{{HttpMethodBaseAttribute.Name}}
        {
            public const string Method = "Put";
        
            public {{Name}}(string route)
                : base(route)
            { }
        }
        """;
}
