namespace DiscogsApiClient.Contract;

public record ReleaseArtist(
    int Id,
    string Name,
    string ResourceUrl,
    string ThumbnailUrl);
