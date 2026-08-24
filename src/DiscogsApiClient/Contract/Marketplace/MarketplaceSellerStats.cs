namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// A Marketplace seller's feedback statistics.
/// </summary>
/// <param name="Rating">The seller's feedback rating percentage.</param>
/// <param name="Stars">The seller's star rating.</param>
/// <param name="Total">The total number of feedback ratings the seller has received.</param>
public sealed record MarketplaceSellerStats(
    [property: JsonPropertyName("rating")]
    string Rating,
    [property: JsonPropertyName("stars")]
    double Stars,
    [property: JsonPropertyName("total")]
    int Total);
