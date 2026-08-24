namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// The seller of a Marketplace listing.
/// </summary>
/// <param name="Id">The seller's user id.</param>
/// <param name="Username">The seller's username.</param>
/// <param name="ResourceUrl">The Api url to the seller's user profile.</param>
/// <param name="AvatarUrl">The Url to the seller's avatar image.</param>
/// <param name="Url">The Api url to the seller's user profile.</param>
/// <param name="HtmlUrl">The Url to the seller's user profile on the Discogs website.</param>
/// <param name="UserId">The seller's user id. Duplicates <paramref name="Id"/>.</param>
/// <param name="MinOrderTotal">The seller's minimum order total, in the seller's currency.</param>
/// <param name="Shipping">The seller's shipping terms, as free text.</param>
/// <param name="Payment">The seller's accepted payment methods, as free text.</param>
/// <param name="Stats">The seller's feedback statistics.</param>
public sealed record MarketplaceSeller(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("username")]
    string Username,
    [property: JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property: JsonPropertyName("avatar_url")]
    string? AvatarUrl,
    [property: JsonPropertyName("url")]
    string? Url,
    [property: JsonPropertyName("html_url")]
    string? HtmlUrl,
    [property: JsonPropertyName("uid")]
    int? UserId,
    [property: JsonPropertyName("min_order_total")]
    decimal? MinOrderTotal,
    [property: JsonPropertyName("shipping")]
    string? Shipping,
    [property: JsonPropertyName("payment")]
    string? Payment,
    [property: JsonPropertyName("stats")]
    MarketplaceSellerStats? Stats);
