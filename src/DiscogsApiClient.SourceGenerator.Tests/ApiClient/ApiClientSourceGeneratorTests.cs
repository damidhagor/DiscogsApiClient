using DiscogsApiClient.SourceGenerator.Tests.ApiClient.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.ApiClient;

public sealed class ApiClientSourceGeneratorTests
{
    [Test]
    public async Task ShouldGenerateClient_WhenValidInterface()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenValidInterface);

        await Assert.That(result.Diagnostics).IsEmpty();

        var clientSource = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "TestNamespace.TestApiClient.g.cs")
            .SourceText.ToString();

        var expected =
            """
            #nullable enable

            namespace TestNamespace;
            
            internal partial class TestApiClient : global::TestNamespace.ITestApiClient
            {
                private readonly global::System.Net.Http.HttpClient _httpClient;
                private readonly global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> _apiClientSettings;
            
                public TestApiClient(
                    global::System.Net.Http.HttpClient httpClient,
                    global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> apiClientSettings)
                {
                    _httpClient = httpClient;
                    _apiClientSettings = apiClientSettings;
                }
            
                public async global::System.Threading.Tasks.Task<global::System.String> GetItemAsync(global::System.Int32 id, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items/{id}";
            
            		var result = await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _apiClientSettings.JsonSerializerContext.String, cancellationToken: cancellationToken);
            
            		return result;
                }
            
                public async global::System.Threading.Tasks.Task CreateItemAsync(global::System.String item, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items";
            
                    var content = SerializeContent(item, _apiClientSettings.JsonSerializerContext.String);
            		await SendAsync(global::System.Net.Http.HttpMethod.Post, route, content: content, cancellationToken: cancellationToken);
                }
            
                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }
            
                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private T Send<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, jsonTypeInfo)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            
                private async Task SendAsync(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private async Task<T> SendAsync<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, jsonTypeInfo, cancellationToken)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            }
            
            #if NET7_0_OR_GREATER
            file static class QueryParameterHelper
            #else
            internal static class QueryParameterHelper
            #endif
            {
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(clientSource))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateClient_WhenInterfaceIsEmpty()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenInterfaceIsEmpty);

        await Assert.That(result.Diagnostics).IsEmpty();

        var clientSource = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "TestNamespace.TestApiClient.g.cs")
            .SourceText.ToString();

        var expected =
            """
            #nullable enable

            namespace TestNamespace;
            
            internal partial class TestApiClient : global::TestNamespace.ITestApiClient
            {
                private readonly global::System.Net.Http.HttpClient _httpClient;
                private readonly global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> _apiClientSettings;
            
                public TestApiClient(
                    global::System.Net.Http.HttpClient httpClient,
                    global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> apiClientSettings)
                {
                    _httpClient = httpClient;
                    _apiClientSettings = apiClientSettings;
                }
            
                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }
            
                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private T Send<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, jsonTypeInfo)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            
                private async Task SendAsync(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private async Task<T> SendAsync<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, jsonTypeInfo, cancellationToken)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            }
            
            #if NET7_0_OR_GREATER
            file static class QueryParameterHelper
            #else
            internal static class QueryParameterHelper
            #endif
            {
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(clientSource))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateClientWithCustomName_WhenAttributeHasCustomName()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenAttributeHasCustomName);

        await Assert.That(result.Diagnostics).IsEmpty();

        var clientSource = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "TestNamespace.MyCustomClient.g.cs")
            .SourceText.ToString();

        var expected =
    """
            #nullable enable

            namespace TestNamespace;
            
            internal partial class MyCustomClient : global::TestNamespace.ITestApiClient
            {
                private readonly global::System.Net.Http.HttpClient _httpClient;
                private readonly global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> _apiClientSettings;
            
                public MyCustomClient(
                    global::System.Net.Http.HttpClient httpClient,
                    global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> apiClientSettings)
                {
                    _httpClient = httpClient;
                    _apiClientSettings = apiClientSettings;
                }
            
                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }
            
                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private T Send<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, jsonTypeInfo)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            
                private async Task SendAsync(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private async Task<T> SendAsync<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, jsonTypeInfo, cancellationToken)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            }
            
            #if NET7_0_OR_GREATER
            file static class QueryParameterHelper
            #else
            internal static class QueryParameterHelper
            #endif
            {
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(clientSource))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateClientWithCustomNamespace_WhenAttributeHasCustomNamespace()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenAttributeHasCustomNamespace);

        await Assert.That(result.Diagnostics).IsEmpty();

        var clientSource = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "TestNamespace.TestApiClient.g.cs")
            .SourceText.ToString();

        var expected =
    """
            #nullable enable

            namespace MyCustomNamespace;
            
            internal partial class TestApiClient : global::TestNamespace.ITestApiClient
            {
                private readonly global::System.Net.Http.HttpClient _httpClient;
                private readonly global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> _apiClientSettings;
            
                public TestApiClient(
                    global::System.Net.Http.HttpClient httpClient,
                    global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> apiClientSettings)
                {
                    _httpClient = httpClient;
                    _apiClientSettings = apiClientSettings;
                }
            
                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }
            
                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private T Send<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, jsonTypeInfo)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            
                private async Task SendAsync(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private async Task<T> SendAsync<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, jsonTypeInfo, cancellationToken)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            }
            
            #if NET7_0_OR_GREATER
            file static class QueryParameterHelper
            #else
            internal static class QueryParameterHelper
            #endif
            {
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(clientSource))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateClientWithCustomNameAndNamespace_WhenAttributeHasCustomNameAndNamespace()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenAttributeHasCustomNameAndNamespace);

        await Assert.That(result.Diagnostics).IsEmpty();

        var clientSource = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "TestNamespace.MyCustomClient.g.cs")
            .SourceText.ToString();

        var expected =
    """
            #nullable enable

            namespace MyCustomNamespace;
            
            internal partial class MyCustomClient : global::TestNamespace.ITestApiClient
            {
                private readonly global::System.Net.Http.HttpClient _httpClient;
                private readonly global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> _apiClientSettings;
            
                public MyCustomClient(
                    global::System.Net.Http.HttpClient httpClient,
                    global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> apiClientSettings)
                {
                    _httpClient = httpClient;
                    _apiClientSettings = apiClientSettings;
                }
            
                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }
            
                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private T Send<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, jsonTypeInfo)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            
                private async Task SendAsync(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private async Task<T> SendAsync<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, jsonTypeInfo, cancellationToken)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            }
            
            #if NET7_0_OR_GREATER
            file static class QueryParameterHelper
            #else
            internal static class QueryParameterHelper
            #endif
            {
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(clientSource))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateClientWithQueryParameters_WhenInterfaceHasQueryParameters()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenInterfaceHasQueryParameters);

        await Assert.That(result.Diagnostics).IsEmpty();

        var clientSource = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "TestNamespace.TestApiClient.g.cs")
            .SourceText.ToString();

        var expected =
            """
            #nullable enable

            namespace TestNamespace;
            
            internal partial class TestApiClient : global::TestNamespace.ITestApiClient
            {
                private readonly global::System.Net.Http.HttpClient _httpClient;
                private readonly global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> _apiClientSettings;
            
                public TestApiClient(
                    global::System.Net.Http.HttpClient httpClient,
                    global::DiscogsApiClient.SourceGenerator.ApiClient.ApiClientSettings<global::TestNamespace.ITestApiClient, global::TestNamespace.TestJsonContext> apiClientSettings)
                {
                    _httpClient = httpClient;
                    _apiClientSettings = apiClientSettings;
                }
            
                public async global::System.Threading.Tasks.Task<global::System.String> GetItemsAsync(global::TestNamespace.QueryParams queryParams, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = BuildRouteForGetItemsAsync($"/items", queryParams);
            
            		var result = await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _apiClientSettings.JsonSerializerContext.String, cancellationToken: cancellationToken);
            
            		return result;
                }
            
            	private string BuildRouteForGetItemsAsync(string route, global::TestNamespace.QueryParams queryParams)
            	{
            		var capacity = route.Length;
            		var parameterCount = 0;
            
            		queryParams.CalculateQuerySize(ref capacity, ref parameterCount);
            
                    capacity += parameterCount;
            
                    var queryBuilder = new global::System.Text.StringBuilder(route, capacity);
                    queryBuilder.Append('?');
            
            		var routeLength = queryBuilder.Length + 1;
            
            		queryParams.AppendQuery(queryBuilder, routeLength);
            
                    if (queryBuilder[queryBuilder.Length - 1] == '?')
                    {
                        queryBuilder.Length--;
                    }
            
                    return queryBuilder.ToString();
                }
            
                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }
            
                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private T Send<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = _httpClient.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, jsonTypeInfo)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            
                private async Task SendAsync(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }
            
                private async Task<T> SendAsync<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content = null,
                    global::System.Threading.CancellationToken cancellationToken = default)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);
            
                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }
            
                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
            
                    using var responseStream = response.Content.ReadAsStream();
            
                    return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, jsonTypeInfo, cancellationToken)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            }
            
            #if NET7_0_OR_GREATER
            file static class TestNamespaceQueryParamsExtensions
            #else
            internal static class TestNamespaceQueryParamsExtensions
            #endif
            {
                public static void CalculateQuerySize(this global::TestNamespace.QueryParams queryParams, ref int capacity, ref int parameterCount)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Name is not null)
                        {
                            capacity += 5; // Name
                            capacity += QueryParameterHelper.CalculateQuerySize(queryParams.Name);
                            parameterCount++;
                        }
                    }
                }
            
            
                public static void AppendQuery(this global::TestNamespace.QueryParams queryParams, global::System.Text.StringBuilder queryBuilder, int routeLength)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Name is not null)
                        {
                            if (queryBuilder.Length > routeLength)
                            {
                                queryBuilder.Append('&');
                            }
            
                            queryBuilder.Append("Name=");
                            queryBuilder.Append(queryParams.Name);
                        }
                    }
                }
            }
                
            
            #if NET7_0_OR_GREATER
            file static class QueryParameterHelper
            #else
            internal static class QueryParameterHelper
            #endif
            {
                public static int CalculateQuerySize(string? text)
                {
                    return text?.Length ?? 0;
                }
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(clientSource))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }
}
