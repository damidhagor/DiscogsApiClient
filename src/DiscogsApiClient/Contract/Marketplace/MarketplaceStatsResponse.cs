namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// Current marketplace statistics for a release. <see cref="LowestPrice"/> is <see langword="null"/> if the
/// release has no items for sale.
/// </summary>
/// <param name="LowestPrice">The lowest listed price of any item for sale.</param>
/// <param name="NumForSale">The number of items currently for sale.</param>
/// <param name="BlockedFromSale">Whether the release is blocked for sale in the marketplace.</param>
public sealed record MarketplaceStatsResponse(
    [property: JsonPropertyName("lowest_price")]
    MarketplacePrice? LowestPrice,
    [property: JsonPropertyName("num_for_sale")]
    int? NumForSale,
    [property: JsonPropertyName("blocked_from_sale")]
    bool BlockedFromSale);
