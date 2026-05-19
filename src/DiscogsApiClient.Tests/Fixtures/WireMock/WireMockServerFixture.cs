using System.Text.RegularExpressions;
using TUnit.Core.Interfaces;
using WireMock.Handlers;
using WireMock.Server;
using WireMock.Settings;

namespace DiscogsApiClient.Tests.Fixtures.WireMock;

public sealed partial class WireMockServerFixture : IAsyncInitializer, IAsyncDisposable
{
    [GeneratedRegex(@"""Discogs token=[^""]+""")]
    private static partial Regex TokenPattern();

    private WireMockServer _server = null!;
    private bool _isRecording;

    public string Url => _server.Url!;

    public string MappingsPath { get; } = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Fixtures", "WireMock", "Mappings"));

    public Task InitializeAsync()
    {
        var record = Environment.GetEnvironmentVariable("WIREMOCK_RECORD");
        _isRecording = string.Equals(record, "true", StringComparison.OrdinalIgnoreCase);

        var settings = new WireMockServerSettings
        {
            FileSystemHandler = new LocalFileSystemHandler(MappingsPath)
        };

        if (_isRecording)
        {
            settings.ProxyAndRecordSettings = new()
            {
                Url = "https://api.discogs.com",
                SaveMapping = true,
                SaveMappingToFile = true,
                SaveMappingSettings = new() { StatusCodePattern = "*" },
                ExcludedHeaders = ["Host", "traceparent"],
                AppendGuidToSavedMappingFile = true
            };
        }
        else
        {
            settings.ReadStaticMappings = true;
        }

        _server = WireMockServer.Start(settings);

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        _server.Stop();
        _server.Dispose();

        if (_isRecording)
        {
            CleanupMappings();
        }

        return ValueTask.CompletedTask;
    }

    private void CleanupMappings()
    {
        if (!Directory.Exists(MappingsPath))
        {
            return;
        }

        var adminMappingsPath = Path.Combine(MappingsPath, "__admin", "mappings");

        if (!Directory.Exists(adminMappingsPath))
        {
            return;
        }

        foreach (var file in Directory.GetFiles(adminMappingsPath, "*.json"))
        {
            var json = File.ReadAllText(file);
            var cleaned = TokenPattern().Replace(json, @"""Discogs token=*""");
            if (!ReferenceEquals(json, cleaned))
            {
                File.WriteAllText(file, cleaned);
            }
        }
    }
}
