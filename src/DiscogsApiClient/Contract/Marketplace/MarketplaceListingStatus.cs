using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// The status to set when creating or editing a Marketplace listing.
/// </summary>
[GenerateJsonConverter]
public enum MarketplaceListingStatus
{
    /// <summary>
    /// The listing is ready to be shown on the Marketplace.
    /// </summary>
    [AliasAs("For Sale")]
    ForSale,

    /// <summary>
    /// The listing is not ready for public display. Draft listings are never publicly visible or orderable.
    /// </summary>
    [AliasAs("Draft")]
    Draft
}
