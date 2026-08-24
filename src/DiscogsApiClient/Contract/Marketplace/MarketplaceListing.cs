namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// A Marketplace listing.
/// </summary>
/// <param name="Id">The listing id.</param>
/// <param name="Status">
/// The status of the listing (e.g. <c>For Sale</c>, <c>Draft</c>, <c>Sold</c>, <c>Expired</c>).
/// </param>
/// <param name="Condition">The condition of the item (media).</param>
/// <param name="SleeveCondition">The condition of the sleeve/cover.</param>
/// <param name="Price">The price of the item.</param>
/// <param name="OriginalPrice">The price of the item, converted into the requesting user's own currency.</param>
/// <param name="ShippingPrice">The shipping price of the item.</param>
/// <param name="OriginalShippingPrice">The shipping price, converted into the requesting user's own currency.</param>
/// <param name="AllowOffers">Whether buyers can make offers on the item.</param>
/// <param name="OfferSubmitted">Whether the authenticated user has already submitted an offer on the item.</param>
/// <param name="Posted">The date and time the listing was posted.</param>
/// <param name="ShipsFrom">The location the item ships from.</param>
/// <param name="ShippingIsBlocked">Whether the seller is unable to ship to the requesting user's country.</param>
/// <param name="Uri">The Url to the listing on the Discogs website.</param>
/// <param name="Comments">Remarks about the item that are displayed to buyers.</param>
/// <param name="Seller">The seller of the item.</param>
/// <param name="Release">Summary information about the release the listing is for.</param>
/// <param name="ResourceUrl">The Api url to the listing.</param>
/// <param name="Audio">Whether the release has audio available on the Discogs website.</param>
/// <param name="InCart">
/// Whether this listing is in the authenticated user's cart. Only present for authenticated requests.
/// </param>
/// <param name="Weight">
/// The weight, in grams, of the listing. Only present if the authenticated user is the listing owner.
/// </param>
/// <param name="EstimatedWeight">
/// Discogs' own estimated weight, in grams, of the listing, used as a fallback for shipping calculations
/// when <paramref name="Weight"/> is not set. Only present if the authenticated user is the listing owner.
/// </param>
/// <param name="FormatQuantity">
/// The number of items this listing counts as, for the purpose of calculating shipping.
/// Only present if the authenticated user is the listing owner.
/// </param>
/// <param name="ExternalId">
/// A freeform field for the seller's own reference. Only present if the authenticated user is the listing owner.
/// </param>
/// <param name="Location">
/// A freeform field describing the item's physical storage location.
/// Only present if the authenticated user is the listing owner.
/// </param>
/// <param name="Quantity">
/// The quantity of the item available. Only present if the authenticated user is the listing owner.
/// Read-only for NearMint users, who will always see <c>1</c> regardless of their actual count.
/// </param>
public sealed record MarketplaceListing(
    [property: JsonPropertyName("id")]
    long Id,
    [property: JsonPropertyName("status")]
    string Status,
    [property: JsonPropertyName("condition")]
    MarketplaceListingCondition Condition,
    [property: JsonPropertyName("sleeve_condition")]
    MarketplaceListingSleeveCondition? SleeveCondition,
    [property: JsonPropertyName("price")]
    MarketplacePrice Price,
    [property: JsonPropertyName("original_price")]
    MarketplaceOriginalPrice? OriginalPrice,
    [property: JsonPropertyName("shipping_price")]
    MarketplacePrice? ShippingPrice,
    [property: JsonPropertyName("original_shipping_price")]
    MarketplaceOriginalPrice? OriginalShippingPrice,
    [property: JsonPropertyName("allow_offers")]
    bool AllowOffers,
    [property: JsonPropertyName("offer_submitted")]
    bool? OfferSubmitted,
    [property: JsonPropertyName("posted")]
    DateTimeOffset Posted,
    [property: JsonPropertyName("ships_from")]
    string? ShipsFrom,
    [property: JsonPropertyName("shipping_is_blocked")]
    bool? ShippingIsBlocked,
    [property: JsonPropertyName("uri")]
    string Uri,
    [property: JsonPropertyName("comments")]
    string? Comments,
    [property: JsonPropertyName("seller")]
    MarketplaceSeller Seller,
    [property: JsonPropertyName("release")]
    MarketplaceListingRelease Release,
    [property: JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property: JsonPropertyName("audio")]
    bool Audio,
    [property: JsonPropertyName("in_cart")]
    bool? InCart,
    [property: JsonPropertyName("weight")]
    double? Weight,
    [property: JsonPropertyName("estimated_weight")]
    double? EstimatedWeight,
    [property: JsonPropertyName("format_quantity")]
    int? FormatQuantity,
    [property: JsonPropertyName("external_id")]
    string? ExternalId,
    [property: JsonPropertyName("location")]
    string? Location,
    [property: JsonPropertyName("quantity")]
    int? Quantity);
