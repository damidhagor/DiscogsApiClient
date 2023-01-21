using static DiscogsApiClient.QueryParameters.ArtistReleaseSortQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Sorting query parameters used for retrieving artist releases.
/// </summary>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record ArtistReleaseSortQueryParameters(
    SortableProperty? SortProperty = default,
    SortOrder? SortOrder = default)
    : IQueryParameters
{
    /// <summary>
    /// Release properties which can be used to sort the results with.
    /// </summary>
    public enum SortableProperty
    {
        Year,
        Title,
        Format
    }

    /// <inheritdoc/>
    public string CreateQueryParameterString()
    {
        var parameters = new List<string>();

        if (SortProperty is not null)
            parameters.Add($"sort={SortProperty switch
            {
                SortableProperty.Year => "year",
                SortableProperty.Title => "title",
                SortableProperty.Format => "format",
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
