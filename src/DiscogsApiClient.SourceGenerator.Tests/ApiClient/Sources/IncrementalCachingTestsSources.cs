namespace DiscogsApiClient.SourceGenerator.Tests.ApiClient.Sources;

public static class IncrementalCachingTestsSources
{
    public const string WhenSourceUnchanged_First =
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

        [ApiClient(typeof(TestJsonContext))]
        internal sealed partial class TestApiClient(System.Net.Http.HttpClient httpClient, TestJsonContext context)
        {
            private readonly System.Net.Http.HttpClient _httpClient = httpClient;
            private readonly TestJsonContext _context = context;

            [HttpGet("/test")]
            public partial Task<string> GetTestAsync(CancellationToken cancellationToken);
        }
        """;

    public const string WhenSourceUnchanged_Second =
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

        [ApiClient(typeof(TestJsonContext))]
        internal sealed partial class TestApiClient(System.Net.Http.HttpClient httpClient, TestJsonContext context)
        {
            private readonly System.Net.Http.HttpClient _httpClient = httpClient;
            private readonly TestJsonContext _context = context;

            [HttpGet("/test")]
            public partial Task<string> GetTestAsync(CancellationToken cancellationToken);
        }
        """;

    public const string WhenSourceChanged_First =
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

        [ApiClient(typeof(TestJsonContext))]
        internal sealed partial class TestApiClient(System.Net.Http.HttpClient httpClient, TestJsonContext context)
        {
            private readonly System.Net.Http.HttpClient _httpClient = httpClient;
            private readonly TestJsonContext _context = context;

            [HttpGet("/test")]
            public partial Task<string> GetTestAsync(CancellationToken cancellationToken);
        }
        """;

    public const string WhenSourceChanged_Second =
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

        [ApiClient(typeof(TestJsonContext))]
        internal sealed partial class TestApiClient(System.Net.Http.HttpClient httpClient, TestJsonContext context)
        {
            private readonly System.Net.Http.HttpClient _httpClient = httpClient;
            private readonly TestJsonContext _context = context;

            [HttpGet("/test")]
            public partial Task<string> GetTestAsync(CancellationToken cancellationToken);

            [HttpGet("/test2")]
            public partial Task<string> GetTest2Async(CancellationToken cancellationToken);
        }
        """;
}