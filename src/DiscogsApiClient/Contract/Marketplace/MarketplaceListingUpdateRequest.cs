namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// The request body for editing an existing Marketplace listing.
/// </summary>
/// <param name="ReleaseId">The id of the release being listed.</param>
/// <param name="Condition">The condition of the item (media) being listed.</param>
/// <param name="Price">The price of the item, in the seller's currency.</param>
/// <param name="Status">
/// The status of the listing. Use <see cref="MarketplaceListingStatus.Draft"/> to unpublish/keep the listing
/// hidden, or <see cref="MarketplaceListingStatus.ForSale"/> to publish it.
/// </param>
/// <param name="SleeveCondition">The condition of the sleeve/cover of the item being listed.</param>
/// <param name="Comments">Remarks about the item that will be displayed to buyers.</param>
/// <param name="AllowOffers">Whether to allow buyers to make offers on the item. Defaults to <c>false</c>.</param>
/// <param name="ExternalId">
/// A freeform field for the seller's own reference. Called "Private Comments" on the Discogs website.
/// </param>
/// <param name="Location">A freeform field describing the item's physical storage location.</param>
/// <param name="Weight">The weight, in grams, of the listing, for the purpose of calculating shipping.</param>
/// <param name="FormatQuantity">
/// The number of items this listing counts as, for the purpose of calculating shipping.
/// Called "Counts As" on the Discogs website.
/// </param>
public sealed record MarketplaceListingUpdateRequest(
    [property: JsonPropertyName("release_id")]
    int ReleaseId,
    [property: JsonPropertyName("condition")]
    MarketplaceListingCondition Condition,
    [property: JsonPropertyName("price")]
    decimal Price,
    [property: JsonPropertyName("status")]
    MarketplaceListingStatus Status,
    [property: JsonPropertyName("sleeve_condition")]
    MarketplaceListingSleeveCondition? SleeveCondition,
    [property: JsonPropertyName("comments")]
    string? Comments,
    [property: JsonPropertyName("allow_offers")]
    bool? AllowOffers,
    [property: JsonPropertyName("external_id")]
    string? ExternalId,
    [property: JsonPropertyName("location")]
    string? Location,
    [property: JsonPropertyName("weight")]
    double? Weight,
    [property: JsonPropertyName("format_quantity")]
    int? FormatQuantity);
