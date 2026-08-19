namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class HttpGetAttribute
{
    public const string Name = "HttpGetAttribute";

    public const string SourceHint = "HttpGetAttribute.g.cs";

    public const string Source =
        $$"""
        {{Constants.GeneratedFileHeader}}
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        {{Constants.GeneratedCodeAttribute}}
        [global::System.AttributeUsage(global::System.AttributeTargets.Method)]
        internal sealed class {{Name}}(string route) : global::{{Constants.ApiClientNamespace}}.{{HttpMethodBaseAttribute.Name}}(route)
        {
            public const string Method = "Get";
        }
        """;
}
