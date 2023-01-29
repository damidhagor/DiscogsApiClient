namespace DiscogsApiClient.Contract;

public sealed record ReleaseStats(
    ReleaseStatValues Community,
    ReleaseStatValues User);

public sealed record ReleaseStatValues(
    int InWantlist,
    int InCollection);
