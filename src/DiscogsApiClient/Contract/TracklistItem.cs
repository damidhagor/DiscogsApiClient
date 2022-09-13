namespace DiscogsApiClient.Contract;

public sealed record TracklistItem(
    string Position,
    string Type_,
    string Title,
    string Duration);
