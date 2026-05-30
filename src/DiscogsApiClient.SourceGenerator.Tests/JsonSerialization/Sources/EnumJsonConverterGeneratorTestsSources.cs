namespace DiscogsApiClient.SourceGenerator.Tests.JsonSerialization.Sources;

public static class EnumJsonConverterGeneratorTestsSources
{
    public const string WhenMinimalEnum =
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

    public const string WhenEnumHasAliases =
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
