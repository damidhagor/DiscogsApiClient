namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class ApiCLientAttribute
{
    public const string Name = "ApiClientAttribute";

    public const string NamePropertyName = "Name";

    public const string NamespacePropertyName = "Namespace";

    public const string SourceHint = "ApiClientAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        [global::System.AttributeUsage(global::System.AttributeTargets.Interface)]
        internal sealed class {{Name}} : global::System.Attribute
        {
            public string? {{NamePropertyName}} { get; set; }

            public string? {{NamespacePropertyName}} { get; set; }
        }
        """;
}
