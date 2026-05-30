namespace DiscogsApiClient.SourceGenerator.Tests.JsonSerialization.Sources;

public static class IncrementalCachingTestsSources
{
    public const string WhenSourceUnchanged_First =
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

    public const string WhenSourceUnchanged_Second =
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

    public const string WhenSourceChanged_First =
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

    public const string WhenSourceChanged_Second =
        """
        using DiscogsApiClient.SourceGenerator.JsonSerialization;

        namespace TestNamespace;

        [GenerateJsonConverter]
        public enum TestStatus
        {
            Active,
            Inactive,
            Pending,
            Suspended
        }
        """;
}
