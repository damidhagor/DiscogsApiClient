using DiscogsApiClient.Tests.Fixtures.WireMock;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Core.Interfaces;

namespace DiscogsApiClient.Tests.Fixtures;

public sealed class DiscogsApiClientFixture : IAsyncInitializer, IAsyncDisposable
{
    private ServiceProvider _authenticatedProvider = null!;
    private ServiceProvider _unauthenticatedProvider = null!;

    [ClassDataSource<WireMockServerFixture>(Shared = SharedType.PerTestSession)]
    public required WireMockServerFixture WireMockServer { get; init; }

    public IDiscogsApiClient GetAuthenticatedClient() => _authenticatedProvider.GetRequiredService<IDiscogsApiClient>();

    public IDiscogsApiClient GetUnauthenticatedClient() => _unauthenticatedProvider.GetRequiredService<IDiscogsApiClient>();

    public Task InitializeAsync()
    {
        _authenticatedProvider = new ServiceCollection()
            .AddDiscogsApiClient(o =>
            {
                o.BaseUrl = WireMockServer.Url;
                o.UserAgent = "DiscogsApiClientTests/1.0";
            })
            .BuildServiceProvider();

        var userToken = Environment.GetEnvironmentVariable("DISCOGS_USER_TOKEN") ?? "dummy-token";
        _authenticatedProvider.GetRequiredService<IDiscogsAuthenticationService>()
            .AuthenticateWithPersonalAccessToken(userToken);

        _unauthenticatedProvider = new ServiceCollection()
            .AddDiscogsApiClient(o =>
            {
                o.BaseUrl = WireMockServer.Url;
                o.UserAgent = "DiscogsApiClientTests/1.0";
            })
            .BuildServiceProvider();

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _authenticatedProvider.Dispose();
        _unauthenticatedProvider.Dispose();
        return ValueTask.CompletedTask;
    }
}
