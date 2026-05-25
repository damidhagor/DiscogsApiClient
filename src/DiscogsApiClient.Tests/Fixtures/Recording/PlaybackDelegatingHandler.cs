namespace DiscogsApiClient.Tests.Fixtures.Recording;

public sealed class PlaybackDelegatingHandler(PlaybackFixture fixture) : DelegatingHandler
{
    private readonly PlaybackFixture _fixture = fixture;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await _fixture.GetResponse(request, cancellationToken)
            ?? throw new InvalidOperationException("No recording found for the current test and request.");

        return response;
    }
}
