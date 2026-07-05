using DiscogsApiClient.SourceGenerator.Tests.ApiClient.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.ApiClient;

public sealed class ApiClientSourceGeneratorTests
{
    [Test]
    public async Task ShouldGenerateClient_WhenValidClass()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenValidClass);

        var expected =
            """
            #nullable enable

            namespace TestNamespace;

            partial class TestApiClient
            {
                public async partial global::System.Threading.Tasks.Task<global::System.String> GetItemAsync(global::System.Int32 id, global::System.Threading.CancellationToken cancellationToken)
                {
                    var route = $"/items/{id}";

                    return await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _context.String, content: null, cancellationToken: cancellationToken);
                }

                public async partial global::System.Threading.Tasks.Task CreateItemAsync(global::System.String item, global::System.Threading.CancellationToken cancellationToken)
                {
                    var route = "/items";

                    var content = SerializeContent(item, _context.String);
                    await SendAsync(global::System.Net.Http.HttpMethod.Post, route, content: content, cancellationToken: cancellationToken);
                }

                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }

                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(GetClientSource(result)))
            .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGenerateClient_WhenClassIsEmpty()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenClassIsEmpty);

        var expected =
            """
            #nullable enable

            namespace TestNamespace;

            partial class TestApiClient
            {
                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }

                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(GetClientSource(result)))
            .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGenerateClient_WhenClassHasPropertyMembers()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenClassHasPropertyMembers);

        var expected =
            """
            #nullable enable

            namespace TestNamespace;

            partial class TestApiClient
            {
                public async partial global::System.Threading.Tasks.Task<global::System.String> GetItemAsync(global::System.Int32 id, global::System.Threading.CancellationToken cancellationToken)
                {
                    var route = $"/items/{id}";

                    return await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, Context.String, content: null, cancellationToken: cancellationToken);
                }

                private string SerializeContent<T>(T payload, global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo)
                {
                    return global::System.Text.Json.JsonSerializer.Serialize(payload, jsonTypeInfo);
                }

                private void Send(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);

                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }

                    using var response = Client.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }

                private T Send<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);

                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }

                    using var response = Client.Send(request, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    using var responseStream = response.Content.ReadAsStream();

                    return global::System.Text.Json.JsonSerializer.Deserialize<T>(responseStream, jsonTypeInfo)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }

                private async Task SendAsync(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);

                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }

                    using var response = await Client.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }

                private async Task<T> SendAsync<T>(
                    global::System.Net.Http.HttpMethod httpMethod,
                    string route,
                    global::System.Text.Json.Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo,
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
                {
                    using var request = new global::System.Net.Http.HttpRequestMessage(httpMethod, route);

                    if (content is not null)
                    {
                        request.Content = new global::System.Net.Http.StringContent(content, global::System.Text.Encoding.UTF8, "application/json");
                    }

                    using var response = await Client.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();

                    using var responseStream = response.Content.ReadAsStream();

                    return await global::System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, jsonTypeInfo, cancellationToken)
                        ?? throw new global::System.InvalidOperationException($"The response for the request '{route}' could not be deserialized.");
                }
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(GetClientSource(result)))
            .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGenerateClientWithQueryParameters_WhenClassHasQueryParameters()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            ApiClientSourceGeneratorTestsSources.WhenClassHasQueryParameters);

        var expected =
            """
            #nullable enable

            namespace TestNamespace;

            partial class TestApiClient
            {
                public async partial global::System.Threading.Tasks.Task<global::System.String> GetItemsAsync(global::TestNamespace.QueryParams queryParams, global::System.Threading.CancellationToken cancellationToken)
                {
                    var route = BuildRouteForGetItemsAsync("/items", queryParams);

                    return await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _context.String, content: null, cancellationToken: cancellationToken);
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

                    if (queryBuilder[^1] == '?')
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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
                    string? content,
                    global::System.Threading.CancellationToken cancellationToken)
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

            file static class TestNamespaceQueryParamsExtensions
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

            file static class QueryParameterHelper
            {
                public static int CalculateQuerySize(string? text)
                {
                    return text?.Length ?? 0;
                }
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(GetClientSource(result)))
            .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    private static string GetClientSource(GeneratorDriverRunResult result)
        => result.Results.Single().GeneratedSources
            .First(s => s.HintName == "TestNamespace.TestApiClient.g.cs")
            .SourceText.ToString();
}
