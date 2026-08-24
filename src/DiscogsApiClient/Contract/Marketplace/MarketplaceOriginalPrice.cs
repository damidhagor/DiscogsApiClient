namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// A price converted into the requesting user's own currency. Represented as an empty JSON object by
/// Discogs (deserializing to all-<see langword="null"/> properties) when no price applies (e.g. no
/// shipping price set).
/// </summary>
/// <param name="CurrencyAbbreviation">The currency of the converted price.</param>
/// <param name="CurrencyId">The Discogs internal id of the currency.</param>
/// <param name="Formatted">The price, formatted for display (e.g. "$120.00").</param>
/// <param name="Value">The price value.</param>
public sealed record MarketplaceOriginalPrice(
    [property: JsonPropertyName("curr_abbr")]
    MarketplaceCurrency? CurrencyAbbreviation,
    [property: JsonPropertyName("curr_id")]
    int? CurrencyId,
    [property: JsonPropertyName("formatted")]
    string? Formatted,
    [property: JsonPropertyName("value")]
    decimal? Value);
