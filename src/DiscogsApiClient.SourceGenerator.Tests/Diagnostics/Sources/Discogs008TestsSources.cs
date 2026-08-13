namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

public static class Discogs008TestsSources
{
    public const string WhenApiClientClassIsNotPartial =
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
        internal sealed class TestApiClient(System.Net.Http.HttpClient httpClient, TestJsonContext context)
        {
            private readonly System.Net.Http.HttpClient _httpClient = httpClient;
            private readonly TestJsonContext _context = context;
        }
        """;

    public const string WhenApiClientClassIsPartial =
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