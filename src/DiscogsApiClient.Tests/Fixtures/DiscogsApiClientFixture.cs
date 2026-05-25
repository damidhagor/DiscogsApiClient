using DiscogsApiClient.Tests.Fixtures.Recording;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using TUnit.Core.Interfaces;

namespace DiscogsApiClient.Tests.Fixtures;

public sealed class DiscogsApiClientFixture : IAsyncInitializer, IAsyncDisposable
{
    private ServiceProvider _authenticatedProvider = null!;
    private ServiceProvider _unauthenticatedProvider = null!;

    [ClassDataSource<RecordingFixture>(Shared = SharedType.PerTestSession)]
    public required RecordingFixture RecordingFixture { get; init; }

    [ClassDataSource<PlaybackFixture>(Shared = SharedType.PerTestSession)]
    public required PlaybackFixture PlaybackFixture { get; init; }

    public IDiscogsApiClient GetAuthenticatedClient() => _authenticatedProvider.GetRequiredService<IDiscogsApiClient>();

    public IDiscogsApiClient GetUnauthenticatedClient() => _unauthenticatedProvider.GetRequiredService<IDiscogsApiClient>();

    public Task InitializeAsync()
    {
        _authenticatedProvider = BuildServiceProvider();
        _unauthenticatedProvider = BuildServiceProvider();

        var userToken = TestContext.Configuration.Get("DiscogsUserToken") ?? "token";
        _authenticatedProvider.GetRequiredService<IDiscogsAuthenticationService>()
            .AuthenticateWithPersonalAccessToken(userToken);

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _authenticatedProvider.Dispose();
        _unauthenticatedProvider.Dispose();
        return ValueTask.CompletedTask;
    }

    private ServiceProvider BuildServiceProvider()
        => new ServiceCollection()
            .AddScoped<RecordingDelegatingHandler>()
            .AddScoped<PlaybackDelegatingHandler>()
            .AddSingleton(RecordingFixture)
            .AddSingleton(PlaybackFixture)
            .AddSingleton<IHttpMessageHandlerBuilderFilter, RecordingHttpMessageHandlerBuilderFilter>()
            .AddDiscogsApiClient(o =>
            {
                o.BaseUrl = "https://api.discogs.com";
                o.UserAgent = "DiscogsApiClientTests/1.0";
            })
            .BuildServiceProvider();
}
