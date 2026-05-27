using System.Text.Json;

namespace DiscogsApiClient.Tests.Fixtures.Recording;

public sealed class RecordingFixture : IAsyncDisposable
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly Dictionary<string, List<Recording>> _recordings = [];

    public async ValueTask DisposeAsync()
    {
        ArgumentNullException.ThrowIfNull(TestContext.Current);
        await SaveRecordings(TestContext.Current.Execution.CancellationToken);
        _lock.Dispose();
    }

    public async Task Record(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.RequestUri);

        await _lock.WaitAsync(cancellationToken);

        try
        {
            var key = RecordingHelper.GetRecordingKey();

            if (!_recordings.TryGetValue(key, out var recordings))
            {
                recordings = _recordings[key] = [];
            }

            recordings.Add(
                new(
                    request.Method.Method,
                    request.RequestUri.ToString(),
                    request.Headers.GetCleanedUpHeaders(),
                    request.Content is not null ? await request.Content.ReadAsStringAsync(cancellationToken) : null,
                    (int)response.StatusCode,
                    response.Headers.GetCleanedUpHeaders(),
                    response.Content is not null ? await response.Content.ReadAsStringAsync(cancellationToken) : null));
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task SaveRecordings(CancellationToken cancellationToken)
    {
        foreach (var (key, recordings) in _recordings)
        {
            Directory.CreateDirectory(RecordingHelper.RecordingPath);

            var filename = Path.Combine(RecordingHelper.RecordingPath, $"{key}.json");

            await using var stream = File.Open(filename, FileMode.Create);

            await JsonSerializer.SerializeAsync(stream, recordings, RecordingHelper.JsonOptions, cancellationToken);
        }
    }
}
