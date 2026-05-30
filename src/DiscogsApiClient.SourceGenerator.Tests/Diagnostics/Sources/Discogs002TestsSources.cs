namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

public static class Discogs002TestsSources
{
    public const string WhenMethodReturnsVoid =
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
            [HttpGet("/test")]
            void GetSync();
        }
        """;

    public const string WhenMethodReturnsNonTaskType =
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
            [HttpGet("/test")]
            string GetSync(CancellationToken cancellationToken);
        }
        """;

    public const string WhenMethodReturnsTask =
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
            [HttpGet("/test")]
            Task<string> GetTestAsync(CancellationToken cancellationToken);
        }
        """;
}
