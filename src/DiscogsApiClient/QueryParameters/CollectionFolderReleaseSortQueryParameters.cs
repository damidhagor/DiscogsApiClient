using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;
using static DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Sorting query parameters used for retrieving collection folder releases.
/// </summary>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record CollectionFolderReleaseSortQueryParameters(
    [property: AliasAs("sort")]
    SortableProperty? SortProperty = default,
    [property: AliasAs("sort_order")]
    SortOrder? SortOrder = default)
{
    /// <summary>
    /// Release properties which can be used to sort the results with.
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
        [AliasAs("added")]
        AddedAt,
        [AliasAs("year")]
        Year
    }
}
