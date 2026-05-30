namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

public static class Discogs006TestsSources
{
    public const string WhenHttpMethodTypeIsUnrecognized =
        """
        using System;
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

        namespace DiscogsApiClient.SourceGenerator.ApiClient
        {
            [AttributeUsage(AttributeTargets.Method)]
            internal sealed class HttpPatchAttribute : HttpMethodBaseAttribute
            {
                public new const string Method = "Patch";

                public HttpPatchAttribute(string route) : base(route) { }
            }
        }

        namespace TestNamespace
        {
            [ApiClient(typeof(TestJsonContext))]
            public interface ITestApiClient
            {
                [DiscogsApiClient.SourceGenerator.ApiClient.HttpPatch("/test")]
                Task<string> PatchTestAsync(CancellationToken cancellationToken);
            }
        }
        """;

    public const string WhenHttpMethodsAreStandard =
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

            [HttpPost("/items")]
            Task CreateItemAsync(CancellationToken cancellationToken);
        }
        """;
}
