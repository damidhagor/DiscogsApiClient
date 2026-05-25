using System.Net;
using System.Text.Json;
using TUnit.Core.Interfaces;

namespace DiscogsApiClient.Tests.Fixtures.Recording;

public sealed class PlaybackFixture : IAsyncInitializer
{
    private TextWriter _logger = null!;
    private readonly Dictionary<string, (Recording[] Recordings, int Index)> _recordings = [];

    public async Task InitializeAsync()
    {
        ArgumentNullException.ThrowIfNull(TestContext.Current);
        _logger = TestContext.Current.OutputWriter;
        await LoadRecordings(TestContext.Current.Execution.CancellationToken);
    }

    public async Task<HttpResponseMessage?> GetResponse(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var key = RecordingHelper.GetRecordingKey();

        if (!_recordings.TryGetValue(key, out var recordingState))
        {
            _logger.WriteLine($"No recordings found for key: {key}");
            return null;
        }

        if (recordingState.Index >= recordingState.Recordings.Length)
        {
            _logger.WriteLine($"No more responses available for recording with key: {key}");
            return null;
        }

        var recording = recordingState.Recordings[recordingState.Index++];

        if (!await CheckRequestsMatch(recording, request, cancellationToken))
        {
            _logger.WriteLine($"Request does not match recording with key: {key}");
            return null;
        }

        var response = new HttpResponseMessage
        {
            RequestMessage = request,
            StatusCode = (HttpStatusCode)recording.ResponseStatusCode,
            Version = request.Version
        };

        if (recording.ResponseBody is not null)
        {
            response.Content = new StringContent(recording.ResponseBody);
        }

        foreach (var header in recording.ResponseHeaders)
        {
            response.Headers.Add(header.Key, header.Value);
        }

        return response;
    }

    private async Task LoadRecordings(CancellationToken cancellationToken)
    {
        foreach (var file in Directory.GetFiles(RecordingHelper.RecordingPath, "*.json", SearchOption.AllDirectories))
        {
            await using var stream = File.OpenRead(file);
            var recordings = await JsonSerializer.DeserializeAsync<Recording[]>(stream, RecordingHelper.JsonOptions, cancellationToken)
                ?? throw new InvalidOperationException($"Failed to deserialize recording from file: {file}");

            var key = Path.GetFileNameWithoutExtension(file);

            _recordings[key] = (recordings, 0);
        }
    }

    private async Task<bool> CheckRequestsMatch(Recording recording, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (recording.RequestMethod != request.Method.Method)
        {
            _logger.WriteLine($"Request method mismatch. Expected: {recording.RequestMethod}, Actual: {request.Method.Method}");
            return false;
        }

        if (recording.RequestUrl != request.RequestUri?.ToString())
        {
            _logger.WriteLine($"Request URL mismatch. Expected: {recording.RequestUrl}, Actual: {request.RequestUri}");
            return false;
        }

        var headers = request.Headers.GetCleanedUpHeaders();

        if (recording.RequestHeaders.Count != headers.Count)
        {
            _logger.WriteLine($"Request headers count mismatch. Expected: {recording.RequestHeaders.Count}, Actual: {headers.Count}");
            return false;
        }

        foreach (var header in recording.RequestHeaders)
        {
            if (!headers.TryGetValue(header.Key, out var values))
            {
                _logger.WriteLine($"Request is missing expected header: {header.Key}");
                return false;
            }

            if (!header.Value.SequenceEqual(values))
            {
                _logger.WriteLine($"Request header values mismatch for header: {header.Key}. Expected: {string.Join(", ", header.Value)}, Actual: {string.Join(", ", values)}");
                return false;
            }
        }

        var body = request.Content is not null ? await request.Content.ReadAsStringAsync(cancellationToken) : null;

        if (recording.RequestBody != body)
        {
            _logger.WriteLine($"Request body mismatch. Expected: {recording.ResponseBody}, Actual: {body}");
            return false;
        }

        return true;
    }
}
