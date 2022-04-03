namespace DiscogsApiClient.QueryParameters;

internal static class QueryParameterHelper
{
    public static string AppendPaginationQuery(string url, PaginationQueryParameters paginationQueryParameters)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        string query = paginationQueryParameters.CreateQueryParameterString();

        return string.IsNullOrWhiteSpace(query) ? url : $"{url}?{query}";
    }

    public static string AppendSearchQuery(string url, SearchQueryParameters searchQueryParameters)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        string query = searchQueryParameters.CreateQueryParameterString();

        return string.IsNullOrWhiteSpace(query) ? url : $"{url}?{query}";
    }

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