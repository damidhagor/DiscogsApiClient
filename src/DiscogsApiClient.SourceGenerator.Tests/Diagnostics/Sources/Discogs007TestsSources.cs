namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

public static class Discogs007TestsSources
{
    public const string WhenMethodHasMultipleBodyParams =
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

        public class RequestA { }
        public class RequestB { }

        [ApiClient(typeof(TestJsonContext))]
        public interface ITestApiClient
        {
            [HttpPost("/test")]
            Task CreateAsync([Body] RequestA a, [Body] RequestB b, CancellationToken cancellationToken);
        }
        """;

    public const string WhenMethodHasSingleBodyParam =
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

        public class TestRequest { }

        [ApiClient(typeof(TestJsonContext))]
        public interface ITestApiClient
        {
            [HttpPost("/test")]
            Task CreateAsync([Body] TestRequest request, CancellationToken cancellationToken);
        }
        """;

    public const string WhenMethodHasNoBodyParam =
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
        public interface ITestApiClient
        {
            [HttpGet("/items")]
            Task<string> GetItemsAsync(CancellationToken cancellationToken);
        }
        """;
}
