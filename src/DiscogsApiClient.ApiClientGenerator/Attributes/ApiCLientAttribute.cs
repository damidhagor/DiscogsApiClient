namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class ApiCLientAttribute
{
    public const string Name = "ApiClientAttribute";

    public const string SourceHint = "ApiClientAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.GeneratorNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Interface)]
        public sealed class {{Name}} : System.Attribute { }
        """;
}
