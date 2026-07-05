namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

public static class Discogs010TestsSources
{
    public const string WhenApiClientHasNoContextMember =
        """
        using System.Text.Json.Serialization;
        using DiscogsApiClient.SourceGenerator.ApiClient;

        namespace TestNamespace;

        [JsonSerializable(typeof(string))]
        internal partial class TestJsonContext : JsonSerializerContext
        {
            protected TestJsonContext(System.Text.Json.JsonSerializerOptions? options) : base(options) { }
        }

        [ApiClient(typeof(TestJsonContext))]
        internal sealed partial class TestApiClient(System.Net.Http.HttpClient httpClient)
        {
            private readonly System.Net.Http.HttpClient _httpClient = httpClient;
        }
        """;

    public const string WhenApiClientHasContextMember =
        """
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
        }
        """;
}