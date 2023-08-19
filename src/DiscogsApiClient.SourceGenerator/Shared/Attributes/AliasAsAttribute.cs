namespace DiscogsApiClient.SourceGenerator.Shared.Attributes;

internal static class AliasAsAttribute
{
    public const string Name = "AliasAsAttribute";

    public const string SourceHint = "AliasAsAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable

        namespace {{Constants.SharedNamespace}};

        [global::System.AttributeUsage(global::System.AttributeTargets.Property | global::System.AttributeTargets.Field)]
        internal sealed class {{Name}} : global::System.Attribute
        {
            public string Alias { get; set; }
        
            public {{Name}}(string alias)
            {
                Alias = alias;
            }
        }
        """;
}
