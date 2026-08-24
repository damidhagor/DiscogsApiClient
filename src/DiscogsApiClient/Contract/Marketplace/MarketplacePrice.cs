namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// A price in the Marketplace, in a specific currency. Represented as an empty JSON object by Discogs
/// (deserializing to all-<see langword="null"/> properties) when no price applies (e.g. no shipping price set).
/// </summary>
/// <param name="Currency">The currency of the price.</param>
/// <param name="Value">The price value.</param>
public sealed record MarketplacePrice(
    [property: JsonPropertyName("currency")]
    MarketplaceCurrency? Currency,
    [property: JsonPropertyName("value")]
    decimal? Value);
