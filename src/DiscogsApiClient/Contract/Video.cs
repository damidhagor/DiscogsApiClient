namespace DiscogsApiClient.Contract;

public record Video(
    string Uri,
    string Title,
    string Description,
    int Duration,
    bool Embed);
