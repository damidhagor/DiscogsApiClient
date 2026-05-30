namespace DiscogsApiClient.SourceGenerator.Tests.SourceTexts;

public static class ApiClientSources
{
    public const string ApiClientSourceGeneratorTests_WhenValidInterface =
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
            [HttpGet("/items/{id}")]
            Task<string> GetItemAsync(int id, CancellationToken cancellationToken);

            [HttpPost("/items")]
            Task CreateItemAsync([Body] string item, CancellationToken cancellationToken);
        }
        """;

    public const string ApiClientSourceGeneratorTests_WhenInterfaceIsEmpty =
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
        }
        """;

    public const string ApiClientSourceGeneratorTests_WhenAttributeHasCustomName =
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

        [ApiClient(typeof(TestJsonContext), Name = "MyCustomClient")]
        public interface ITestApiClient
        {
        }
        """;

    public const string ApiClientSourceGeneratorTests_WhenAttributeHasCustomNamespace =
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

        [ApiClient(typeof(TestJsonContext), Namespace = "MyCustomNamespace")]
        public interface ITestApiClient
        {
        }
        """;

    public const string ApiClientSourceGeneratorTests_WhenAttributeHasCustomNameAndNamespace =
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

        [ApiClient(typeof(TestJsonContext), Name = "MyCustomClient", Namespace = "MyCustomNamespace")]
        public interface ITestApiClient
        {
        }
        """;

    public const string ApiClientSourceGeneratorTests_WhenInterfaceHasQueryParameters =
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
        }

        [ApiClient(typeof(TestJsonContext))]
        public interface ITestApiClient
        {
            [HttpGet("/items")]
            Task<string> GetItemsAsync(QueryParams queryParams, CancellationToken cancellationToken);
        }
        """;
}
