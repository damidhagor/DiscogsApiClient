using ArtistQP = DiscogsApiClient.QueryParameters.ArtistReleaseSortQueryParameters;
using CollectionQP = DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;
using MasterQP = DiscogsApiClient.QueryParameters.MasterReleaseVersionFilterQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Contains helper methods for appending query parameters in any combination to a request Url.
/// </summary>
internal static class QueryParameterExtensions
{
    /// <summary>
    /// Appends one or more <see cref="IQueryParameters"/> to the given url string.
    /// </summary>
    /// <param name="url">The url to append to.</param>
    /// <param name="queryParameters">The query parameters to append.</param>
    /// <returns>The url with appended query parameters.</returns>
    /// <exception cref="ArgumentNullException">If the url is null or whitespace.</exception>
    public static string AppendQueryParameters(this string url, params IQueryParameters[] queryParameters)
    {
        Guard.IsNotNullOrWhiteSpace(url);

        var queryParts = queryParameters.Select(p => p.CreateQueryParameterString());
        var query = string.Join("&", queryParts);

        return string.IsNullOrWhiteSpace(query) ? url : $"{url}?{query}";
    }

    /// <summary>
    /// Converts a <see cref="ArtistReleaseSortQueryParameters.SortableProperty"/> into its Discogs API representation.
    /// </summary>
    /// <returns>Discogs API string representation</returns>
    /// <exception cref="InvalidOperationException">If value is not in the enum's range</exception>
    public static string GetSortablePropertyString(this ArtistQP.SortableProperty sortableProperty)
        => sortableProperty switch
        {
            ArtistQP.SortableProperty.Year => "year",
            ArtistQP.SortableProperty.Title => "title",
            ArtistQP.SortableProperty.Format => "format",
            _ => throw new InvalidOperationException()
        };

    /// <summary>
    /// Converts a <see cref="MasterReleaseVersionFilterQueryParameters.SortableProperty"/> into its Discogs API representation.
    /// </summary>
    /// <returns>Discogs API string representation</returns>
    /// <exception cref="InvalidOperationException">If value is not in the enum's range</exception>
    public static string GetSortablePropertyString(this MasterQP.SortableProperty sortableProperty)
        => sortableProperty switch
        {
            MasterQP.SortableProperty.Released => "released",
            MasterQP.SortableProperty.Title => "title",
            MasterQP.SortableProperty.Format => "format",
            MasterQP.SortableProperty.Label => "label",
            MasterQP.SortableProperty.Catno => "catno",
            MasterQP.SortableProperty.Country => "country",
            _ => throw new NotImplementedException()
        };

    /// <summary>
    /// Converts a <see cref="CollectionFolderReleaseSortQueryParameters.SortableProperty"/> into its Discogs API representation.
    /// </summary>
    /// <returns>Discogs API string representation</returns>
    /// <exception cref="InvalidOperationException">If value is not in the enum's range</exception>
    public static string GetSortablePropertyString(this CollectionQP.SortableProperty sortableProperty)
        => sortableProperty switch
        {
            CollectionQP.SortableProperty.Label => "label",
            CollectionQP.SortableProperty.Artist => "artist",
            CollectionQP.SortableProperty.Title => "title",
            CollectionQP.SortableProperty.Catno => "catno",
            CollectionQP.SortableProperty.Format => "format",
            CollectionQP.SortableProperty.Rating => "rating",
            CollectionQP.SortableProperty.Added => "added",
            CollectionQP.SortableProperty.Year => "year",
            _ => throw new InvalidOperationException()
        };

    /// <summary>
    /// Converts a <see cref="SortOrder"/> into its Discogs API representation.
    /// </summary>
    /// <returns>Discogs API string representation</returns>
    /// <exception cref="InvalidOperationException">If value is not in the enum's range</exception>
    public static string GetSortOrderString(this SortOrder sortOrder)
        => sortOrder switch
        {
            QueryParameters.SortOrder.Ascending => "asc",
            QueryParameters.SortOrder.Descending => "desc",
            _ => throw new InvalidOperationException()
        };
}
