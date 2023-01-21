namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Contains helper methods for appending query parameters in any combination to a request Url.
/// </summary>
internal static class QueryParameterHelper
{
    public static string AppendQueryParameters(this string url, params IQueryParameters[] queryParameters)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        var queryParts = queryParameters.Select(p => p.CreateQueryParameterString());
        var query = string.Join("&", queryParts);

        return string.IsNullOrWhiteSpace(query) ? url : $"{url}?{query}";
    }
}