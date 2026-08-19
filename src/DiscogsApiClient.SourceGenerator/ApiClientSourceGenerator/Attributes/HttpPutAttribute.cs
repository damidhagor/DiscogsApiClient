namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class HttpPutAttribute
{
    public const string Name = "HttpPutAttribute";

    public const string SourceHint = "HttpPutAttribute.g.cs";

    public const string Source =
        $$"""
        {{Constants.GeneratedFileHeader}}
        #nullable enable

        namespace {{Constants.ApiClientNamespace}};

        {{Constants.GeneratedCodeAttribute}}
        [global::System.AttributeUsage(global::System.AttributeTargets.Method)]
        internal sealed class {{Name}}(string route) : global::{{Constants.ApiClientNamespace}}.{{HttpMethodBaseAttribute.Name}}(route)
        {
            public const string Method = "Put";
        }
        """;
}
