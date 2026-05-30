namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

public static class Discogs001TestsSources
{
    public const string WhenQueryParamHasUnsupportedPropertyType =
        """
        using System.Threading;
        using System.Threading.Tasks;
        using System.Text.Json.Serialization;
        using DiscogsApiClient.SourceGenerator.ApiClient;

        namespace TestNamespace;

        [JsonSerializable(typeof(string))]
        internal partial class TestJsonContext : JsonSerializerContext
        {
            protected TestJsonContext(System.Text.Json.JsonSerializerOptions? options) : base(options) { }
        }

        public class QueryParams
        {
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }

        [ApiClient(typeof(TestJsonContext))]
        public interface ITestApiClient
        {
            [HttpGet("/items")]
            Task<string> GetItemsAsync(QueryParams queryParams, CancellationToken cancellationToken);
        }
        """;

    public const string WhenAllQueryParamPropertiesAreSupported =
        """
        using System.Threading;
        using System.Threading.Tasks;
        using System.Text.Json.Serialization;
        using DiscogsApiClient.SourceGenerator.ApiClient;

        namespace TestNamespace;

        [JsonSerializable(typeof(string))]
        internal partial class TestJsonContext : JsonSerializerContext
        {
            protected TestJsonContext(System.Text.Json.JsonSerializerOptions? options) : base(options) { }
        }

        public enum SortDirection { Asc, Desc }

        public class QueryParams
        {
            public string Name { get; set; }
            public int Page { get; set; }
            public SortDirection Sort { get; set; }
        }

        [ApiClient(typeof(TestJsonContext))]
        public interface ITestApiClient
        {
            [HttpGet("/items")]
            Task<string> GetItemsAsync(QueryParams queryParams, CancellationToken cancellationToken);
        }
        """;
}
