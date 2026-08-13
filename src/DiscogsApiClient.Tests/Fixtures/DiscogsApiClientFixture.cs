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
        var userToken = TestContext.Configuration.Get("DiscogsUserToken") ?? "token";

        _authenticatedProvider = BuildServiceProvider(userToken);
        _unauthenticatedProvider = BuildServiceProvider(userToken: null);

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _authenticatedProvider.Dispose();
        _unauthenticatedProvider.Dispose();
        return ValueTask.CompletedTask;
    }

    private ServiceProvider BuildServiceProvider(string? userToken)
    {
        var isRecording = RecordingHelper.IsRecording;

        var services = new ServiceCollection()
            .AddDiscogsApiClient(o =>
            {
                o.BaseUrl = "https://api.discogs.com";
                o.UserAgent = "DiscogsApiClientTests/1.0";
            });

        if (userToken is not null)
        {
            services.WithPatAuthentication(o => o.Token = userToken);
        }

        if (isRecording)
        {
            services.AddScoped<RecordingDelegatingHandler>();
            services.AddSingleton(RecordingFixture);
            services.AddSingleton<IHttpMessageHandlerBuilderFilter, RecordingHttpMessageHandlerBuilderFilter>();
        }
        else
        {
            services.AddScoped<PlaybackDelegatingHandler>();
            services.AddSingleton(PlaybackFixture);
            services.AddSingleton<IHttpMessageHandlerBuilderFilter, PlaybackHttpMessageHandlerBuilderFilter>();
        }

        return services.BuildServiceProvider();
    }
}
