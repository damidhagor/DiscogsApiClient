namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

public static class Discogs005TestsSources
{
    public const string WhenEnumIsEmpty =
        """
        using DiscogsApiClient.SourceGenerator.JsonSerialization;

        namespace TestNamespace;

        [GenerateJsonConverter]
        public enum EmptyEnum
        {
        }
        """;

    public const string WhenEnumHasMembers =
        """
        using DiscogsApiClient.SourceGenerator.JsonSerialization;

        namespace TestNamespace;

        [GenerateJsonConverter]
        public enum StatusEnum
        {
            Active,
            Inactive,
        }
        """;
}
