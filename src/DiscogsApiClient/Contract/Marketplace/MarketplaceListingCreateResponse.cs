namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// The response returned after successfully creating a new Marketplace listing.
/// </summary>
/// <param name="ListingId">The id of the newly created listing.</param>
/// <param name="ResourceUrl">The Api url to the newly created listing.</param>
public sealed record MarketplaceListingCreateResponse(
    [property: JsonPropertyName("listing_id")]
    long ListingId,
    [property: JsonPropertyName("resource_url")]
    string ResourceUrl);
