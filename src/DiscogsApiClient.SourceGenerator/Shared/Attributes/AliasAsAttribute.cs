namespace DiscogsApiClient.SourceGenerator.Shared.Attributes;

internal static class AliasAsAttribute
{
    public const string Name = "AliasAsAttribute";

    public const string SourceHint = "AliasAsAttribute.g.cs";

    public const string Source =
        $$"""
        {{Constants.GeneratedFileHeader}}
        #nullable enable

        namespace {{Constants.SharedNamespace}};

        {{Constants.GeneratedCodeAttribute}}
        [global::System.AttributeUsage(global::System.AttributeTargets.Property | global::System.AttributeTargets.Field)]
        internal sealed class {{Name}}(string alias) : global::System.Attribute
        {
            public string Alias { get; } = alias;
        }
        """;
}
