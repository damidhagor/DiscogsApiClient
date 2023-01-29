using static DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Sorting query parameters used for retrieving collection folder releases.
/// </summary>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record CollectionFolderReleaseSortQueryParameters(
    SortableProperty? SortProperty = default,
    SortOrder? SortOrder = default)
    : IQueryParameters
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

    /// <inheritdoc/>
    public string CreateQueryParameterString()
    {
        var parameters = new List<string>();

        if (SortProperty is not null)
            parameters.Add($"sort={SortProperty.Value.GetSortablePropertyString()}");

        if (SortOrder is not null)
            parameters.Add($"sort_order={SortOrder.Value.GetSortOrderString()}");

        return string.Join("&", parameters);
    }
}
