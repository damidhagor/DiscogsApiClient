namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class HttpMethodBaseAttribute
{
    public const string Namespace = "DiscogsApiClient.ApiClientGenerator";

    public const string Name = "HttpMethodBaseAttribute";

    public const string SourceHint = "HttpMethodBaseAttribute.g.cs";

    public const string Source =
        $$"""
        namespace {{Namespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public abstract class {{Name}} : System.Attribute
        {
            public string Route { get; set; }
        
            public {{Name}}(string route)
            {
                Route = route;
            }
        }
        """;
}
