namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// Returns a paginated list of listings in a user's Marketplace inventory.
/// </summary>
/// <param name="Pagination">Pagination information.</param>
/// <param name="Listings">The listings in the user's inventory.</param>
public sealed record MarketplaceInventoryResponse(
    [property: JsonPropertyName("pagination")]
    Pagination Pagination,
    [property: JsonPropertyName("listings")]
    IReadOnlyList<MarketplaceListing> Listings);
