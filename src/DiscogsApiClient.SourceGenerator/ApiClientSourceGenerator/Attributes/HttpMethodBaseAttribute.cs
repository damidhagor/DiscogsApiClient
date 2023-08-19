namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class HttpMethodBaseAttribute
{
    public const string Name = "HttpMethodBaseAttribute";

    public const string SourceHint = "HttpMethodBaseAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        internal abstract class {{Name}} : System.Attribute
        {
            public string Route { get; set; }
        
            public {{Name}}(string route)
            {
                Route = route;
            }
        }
        """;
}
