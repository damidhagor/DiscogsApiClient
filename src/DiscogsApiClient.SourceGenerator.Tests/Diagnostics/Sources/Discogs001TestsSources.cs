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
        internal sealed partial class TestApiClient(System.Net.Http.HttpClient httpClient, TestJsonContext context)
        {
            private readonly System.Net.Http.HttpClient _httpClient = httpClient;
            private readonly TestJsonContext _context = context;

            [HttpGet("/items")]
            public partial Task<string> GetItemsAsync(QueryParams queryParams, CancellationToken cancellationToken);
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
        internal sealed partial class TestApiClient(System.Net.Http.HttpClient httpClient, TestJsonContext context)
        {
            private readonly System.Net.Http.HttpClient _httpClient = httpClient;
            private readonly TestJsonContext _context = context;

            [HttpGet("/items")]
            public partial Task<string> GetItemsAsync(QueryParams queryParams, CancellationToken cancellationToken);
        }
        """;
}
