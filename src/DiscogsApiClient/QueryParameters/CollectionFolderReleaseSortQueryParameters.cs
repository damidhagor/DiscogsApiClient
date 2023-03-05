using static DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Sorting query parameters used for retrieving collection folder releases.
/// </summary>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record CollectionFolderReleaseSortQueryParameters(
    [AliasAs("sort")]
    SortableProperty? SortProperty = default,
    [AliasAs("sort_order")]
    SortOrder? SortOrder = default)
{
    /// <summary>
    /// Release properties which can be used to sort the results with.
    /// </summary>
    public enum SortableProperty
    {
        Label,
        Artist,
        Title,
        Catno,
        Format,
        Rating,
        Added,
        Year
    }
}
