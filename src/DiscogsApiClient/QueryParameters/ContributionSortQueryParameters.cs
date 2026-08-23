using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;
using static DiscogsApiClient.QueryParameters.ContributionSortQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Sorting query parameters used for retrieving a user's contributions.
/// </summary>
/// <param name="SortProperty">The property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record ContributionSortQueryParameters(
    [property: AliasAs("sort")]
    SortableProperty? SortProperty = default,
    [property: AliasAs("sort_order")]
    SortOrder? SortOrder = default)
{
    /// <summary>
    /// Properties which can be used to sort the results with.
    /// </summary>
    [GenerateJsonConverter]
    public enum SortableProperty
    {
        [AliasAs("label")]
        Label,
        [AliasAs("artist")]
        Artist,
        [AliasAs("title")]
        Title,
        [AliasAs("catno")]
        CatalogNumber,
        [AliasAs("format")]
        Format,
        [AliasAs("rating")]
        Rating,
        [AliasAs("year")]
        Year,
        [AliasAs("added")]
        AddedAt
    }
}
