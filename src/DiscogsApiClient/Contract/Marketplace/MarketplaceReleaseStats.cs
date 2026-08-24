namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// Community want/collection statistics for the release a Marketplace listing is for.
/// </summary>
/// <param name="Community">The community-wide want/collection statistics for the release.</param>
/// <param name="User">
/// The authenticated user's own want/collection statistics for the release. Only present for authenticated requests.
/// </param>
public sealed record MarketplaceReleaseStats(
    [property: JsonPropertyName("community")]
    ReleaseStatValues Community,
    [property: JsonPropertyName("user")]
    ReleaseStatValues? User);
