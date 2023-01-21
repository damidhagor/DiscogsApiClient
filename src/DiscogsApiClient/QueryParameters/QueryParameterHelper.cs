namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Contains helper methods for appending query parameters in any combination to a request Url.
/// </summary>
internal static class QueryParameterHelper
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
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        var queryParts = queryParameters.Select(p => p.CreateQueryParameterString());
        var query = string.Join("&", queryParts);

        return string.IsNullOrWhiteSpace(query) ? url : $"{url}?{query}";
    }
}