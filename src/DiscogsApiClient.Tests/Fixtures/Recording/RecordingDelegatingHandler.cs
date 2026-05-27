namespace DiscogsApiClient.Tests.Fixtures.Recording;

public sealed class RecordingDelegatingHandler(RecordingFixture fixture) : DelegatingHandler
{
    private readonly RecordingFixture _fixture = fixture;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        await _fixture.Record(request, response, cancellationToken);

        return response;
    }
}
