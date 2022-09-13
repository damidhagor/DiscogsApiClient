namespace DiscogsApiClient.Contract;

public sealed record Video(
    string Uri,
    string Title,
    string Description,
    int Duration,
    bool Embed);
