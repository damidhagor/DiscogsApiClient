namespace DiscogsApiClient.Tests.Fixtures.Recording;

public sealed class Playback
{
    public required Recording[] Recordings { get; init; }

    public int Index { get; set; }
}
