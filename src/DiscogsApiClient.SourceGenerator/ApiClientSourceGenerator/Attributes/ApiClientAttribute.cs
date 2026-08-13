namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class ApiClientAttribute
{
    public const string Name = "ApiClientAttribute";

    public const string JsonSerializerContextTypePropertyName = "JsonSerializerContextType";

    public const string SourceHint = "ApiClientAttribute.g.cs";

    public const string Source =
        $$"""
        {{Constants.GeneratedFileHeader}}
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        {{Constants.GeneratedCodeAttribute}}
        [global::System.AttributeUsage(global::System.AttributeTargets.Class)]
        internal sealed class {{Name}}(global::System.Type jsonSerializerContextType) : global::System.Attribute
        {
            public global::System.Type {{JsonSerializerContextTypePropertyName}} { get; } = jsonSerializerContextType;
        }
        """;
}
