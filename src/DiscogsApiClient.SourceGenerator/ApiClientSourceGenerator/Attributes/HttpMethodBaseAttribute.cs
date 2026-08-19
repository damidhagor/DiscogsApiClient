namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class HttpMethodBaseAttribute
{
    public const string Name = "HttpMethodBaseAttribute";

    public const string SourceHint = "HttpMethodBaseAttribute.g.cs";

    public const string Source =
        $$"""
        {{Constants.GeneratedFileHeader}}
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        {{Constants.GeneratedCodeAttribute}}
        [global::System.AttributeUsage(global::System.AttributeTargets.Method)]
        internal abstract class {{Name}}(string route) : global::System.Attribute
        {
            public string Route { get; } = route;
        }
        """;
}
