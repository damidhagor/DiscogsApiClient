namespace DiscogsApiClient.SourceGenerator.Tests.SourceTexts;

public static class DiagnosticsSources
{
    public const string Discogs001Tests_WhenQueryParamHasUnsupportedPropertyType =
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

    public const string Discogs001Tests_WhenAllQueryParamPropertiesAreSupported =
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

    public const string Discogs002Tests_WhenMethodReturnsVoid =
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

    public const string Discogs002Tests_WhenMethodReturnsNonTaskType =
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

    public const string Discogs002Tests_WhenMethodReturnsTask =
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

    public const string Discogs003Tests_WhenMethodHasNoCancellationToken =
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
            Task<string> GetTestAsync();
        }
        """;

    public const string Discogs003Tests_WhenMethodHasCancellationToken =
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

    public const string Discogs004Tests_WhenRouteParamHasNoMatchingMethodParam =
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
            Task<string> GetItemAsync(CancellationToken cancellationToken);
        }
        """;

    public const string Discogs004Tests_WhenMultipleRouteParamsOneMissing =
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
            [HttpGet("/users/{userId}/items/{itemId}")]
            Task<string> GetUserItemAsync(string userId, CancellationToken cancellationToken);
        }
        """;

    public const string Discogs004Tests_WhenAllRouteParamsHaveMatchingMethodParams =
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
        }
        """;

    public const string Discogs004Tests_WhenRouteHasNoParams =
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

    public const string Discogs005Tests_WhenEnumIsEmpty =
        """
        using DiscogsApiClient.SourceGenerator.JsonSerialization;

        namespace TestNamespace;

        [GenerateJsonConverter]
        public enum EmptyEnum
        {
        }
        """;

    public const string Discogs005Tests_WhenEnumHasMembers =
        """
        using DiscogsApiClient.SourceGenerator.JsonSerialization;

        namespace TestNamespace;

        [GenerateJsonConverter]
        public enum StatusEnum
        {
            Active,
            Inactive,
        }
        """;

    public const string Discogs006Tests_WhenHttpMethodTypeIsUnrecognized =
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

    public const string Discogs006Tests_WhenHttpMethodsAreStandard =
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

    public const string Discogs007Tests_WhenMethodHasMultipleBodyParams =
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

    public const string Discogs007Tests_WhenMethodHasSingleBodyParam =
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

    public const string Discogs007Tests_WhenMethodHasNoBodyParam =
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
