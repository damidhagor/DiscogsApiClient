namespace DiscogsApiClient.Tests.Fixtures;

public sealed class FakeHttpClientFactory(HttpClient httpClient) : IHttpClientFactory
{
    private readonly HttpClient _httpClient = httpClient;

    public HttpClient CreateClient(string name) => _httpClient;
}
