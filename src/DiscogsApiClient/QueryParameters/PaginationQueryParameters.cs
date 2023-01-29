namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Pagination parameters used by paginated requests to the Discogs Api.
/// </summary>
public sealed record PaginationQueryParameters() : IQueryParameters
{
    private int? _page = default;
    /// <summary>
    /// Indicates which page should be returned.
    /// Pages are counted starting with 1.
    /// </summary>
    public int? Page
    {
        get => _page;
        init => _page = value != null ? Math.Max(1, value.Value) : null;
    }

    private int? _pageSize = default;
    /// <summary>
    /// Indicates the size of the requested page.
    /// The value must be between 1 and 100. The default value is 50.
    /// </summary>
    public int? PageSize
    {
        get => _pageSize;
        init => _pageSize = value != null ? Math.Clamp(value.Value, 1, 100) : value;
    }


    /// <inheritdoc/>
    public string CreateQueryParameterString()
    {
        if (Page is not null && PageSize is not null)
            return $"page={Page}&per_page={PageSize}";

        if (Page is not null)
            return $"page={Page}";

        if (PageSize is not null)
            return $"per_page={PageSize}";

        return "";
    }
}
