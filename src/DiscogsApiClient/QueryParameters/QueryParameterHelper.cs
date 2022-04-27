namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Contains helper methods for appending query parameters in any combination to a request Url.
/// </summary>
internal static class QueryParameterHelper
{
    /// <summary>
    /// Appends <see cref="PaginationQueryParameters"/> to the Url.
    /// </summary>
    /// <param name="url">The Url top append the parameters to.</param>
    /// <param name="paginationQueryParameters">The <see cref="PaginationQueryParameters"/> to append to the Url.</param>
    /// <returns>The Url with the appended query.</returns>
    /// <exception cref="ArgumentNullException">The passed in Url is null or empty.</exception>
    public static string AppendPaginationQuery(string url, PaginationQueryParameters paginationQueryParameters)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        string query = paginationQueryParameters.CreateQueryParameterString();

        return string.IsNullOrWhiteSpace(query) ? url : $"{url}?{query}";
    }

    /// <summary>
    /// Appends <see cref="SearchQueryParameters"/> to the Url.
    /// </summary>
    /// <param name="url">The Url top append the parameters to.</param>
    /// <param name="searchQueryParameters">The <see cref="SearchQueryParameters"/> to append to the Url.</param>
    /// <returns>The Url with the appended query.</returns>
    /// <exception cref="ArgumentNullException">The passed in Url is null or empty.</exception>
    public static string AppendSearchQuery(string url, SearchQueryParameters searchQueryParameters)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        string query = searchQueryParameters.CreateQueryParameterString();

        return string.IsNullOrWhiteSpace(query) ? url : $"{url}?{query}";
    }

    /// <summary>
    /// Appends both <see cref="SearchQueryParameters"/> and <see cref="PaginationQueryParameters"/> to the Url.
    /// </summary>
    /// <param name="url">The Url top append the parameters to.</param>
    /// <param name="searchQueryParameters">The <see cref="SearchQueryParameters"/> to append to the Url.</param>
    /// <param name="paginationQueryParameters">The <see cref="PaginationQueryParameters"/> to append to the Url.</param>
    /// <returns>The Url with the appended query parameters.</returns>
    /// <exception cref="ArgumentNullException">The passed in Url is null or empty.</exception>
    public static string AppendSearchQueryWithPagination(string url, SearchQueryParameters searchQueryParameters, PaginationQueryParameters paginationQueryParameters)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        string searchQuery = searchQueryParameters.CreateQueryParameterString();
        string paginationQuery = paginationQueryParameters.CreateQueryParameterString();

        if (!String.IsNullOrWhiteSpace(searchQuery) && !String.IsNullOrWhiteSpace(paginationQuery))
            return $"{url}?{searchQuery}&{paginationQuery}";

        if (!String.IsNullOrWhiteSpace(searchQuery) && String.IsNullOrWhiteSpace(paginationQuery))
            return $"{url}?{searchQuery}";

        if (String.IsNullOrWhiteSpace(searchQuery) && !String.IsNullOrWhiteSpace(paginationQuery))
            return $"{url}?{paginationQuery}";

        return url;
    }
}