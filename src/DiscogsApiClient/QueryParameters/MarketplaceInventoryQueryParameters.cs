using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;
using static DiscogsApiClient.QueryParameters.MarketplaceInventoryQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Filtering and sorting query parameters used for retrieving a user's Marketplace inventory.
/// </summary>
/// <param name="Status">Only show items with this status.</param>
/// <param name="SortProperty">The listing property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record MarketplaceInventoryQueryParameters(
    [property: AliasAs("status")]
    string? Status = default,
    [property: AliasAs("sort")]
    SortableProperty? SortProperty = default,
    [property: AliasAs("sort_order")]
    SortOrder? SortOrder = default)
{
    /// <summary>
    /// Listing properties which can be used to sort the results with.
    /// </summary>
    [GenerateJsonConverter]
    public enum SortableProperty
    {
        [AliasAs("listed")]
        Listed,
        [AliasAs("price")]
        Price,
        [AliasAs("item")]
        Item,
        [AliasAs("artist")]
        Artist,
        [AliasAs("label")]
        Label,
        [AliasAs("catno")]
        CatalogNumber,
        [AliasAs("audio")]
        Audio,
        [AliasAs("status")]
        Status
    }
}
