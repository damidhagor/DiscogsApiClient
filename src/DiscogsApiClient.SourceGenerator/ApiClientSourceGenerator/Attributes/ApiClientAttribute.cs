namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class ApiClientAttribute
{
    public const string Name = "ApiClientAttribute";

    public const string NamePropertyName = "Name";

    public const string NamespacePropertyName = "Namespace";

    public const string JsonSerializerContextTypePropertyName = "JsonSerializerContextType";

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

            public global::System.Type {{JsonSerializerContextTypePropertyName}} { get; set; }

            public {{Name}}(global::System.Type jsonSerializerContextType)
            {
                {{JsonSerializerContextTypePropertyName}} = jsonSerializerContextType;
            }
        }
        """;
}
