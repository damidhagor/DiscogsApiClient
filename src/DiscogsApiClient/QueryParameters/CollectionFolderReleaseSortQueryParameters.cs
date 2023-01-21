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
    /// Release's properties which can be used to sort the results with.
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
            parameters.Add($"sort={SortProperty switch
            {
                SortableProperty.Label => "label",
                SortableProperty.Artist => "artist",
                SortableProperty.Title => "title",
                SortableProperty.Catno => "catno",
                SortableProperty.Format => "format",
                SortableProperty.Rating => "rating",
                SortableProperty.Added => "added",
                SortableProperty.Year => "year",
                _ => throw new InvalidOperationException()
            }}");

        if (SortOrder is not null)
            parameters.Add($"sort_order={SortOrder switch
            {
                QueryParameters.SortOrder.Ascending => "asc",
                QueryParameters.SortOrder.Descending => "desc",
                _ => throw new InvalidOperationException()
            }}");

        return string.Join("&", parameters);
    }
}
