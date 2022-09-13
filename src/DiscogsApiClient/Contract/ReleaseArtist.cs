namespace DiscogsApiClient.Contract;

public sealed record ReleaseArtist(
    int Id,
    string Name,
    string ResourceUrl,
    string ThumbnailUrl);
