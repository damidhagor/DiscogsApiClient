namespace DiscogsApiClient.SourceGenerator.Tests.SourceTexts;

public static class EnumJsonConverterGeneratorSources
{
    public const string EnumJsonConverterGeneratorTests_WhenMinimalEnum =
        """
        using DiscogsApiClient.SourceGenerator.JsonSerialization;

        namespace TestNamespace;

        [GenerateJsonConverter]
        public enum TestStatus
        {
            Active,
            Inactive,
            Pending
        }
        """;

    public const string EnumJsonConverterGeneratorTests_WhenEnumHasAliases =
        """
        using DiscogsApiClient.SourceGenerator.JsonSerialization;
        using DiscogsApiClient.SourceGenerator.Shared;

        namespace TestNamespace;

        [GenerateJsonConverter]
        public enum SortOrder
        {
            [AliasAs("asc")]
            Ascending,
            [AliasAs("desc")]
            Descending
        }
        """;
}
