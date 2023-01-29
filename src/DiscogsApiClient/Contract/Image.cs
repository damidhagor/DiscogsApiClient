namespace DiscogsApiClient.Contract;

public sealed record Image(
    ImageType Type,
    string Uri,
    string ResourceUrl,
    string Uri150,
    int Width,
    int Height);

public enum ImageType : int
{
    None = 0,
    Primary = 1,
    Secondary = 2
}
