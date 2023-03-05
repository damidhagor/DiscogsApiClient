using Refit;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Pagination parameters used by paginated requests to the Discogs Api.
/// </summary>
public sealed record PaginationQueryParameters()
{
    private int? _page = default;
    /// <summary>
    /// Indicates which page should be returned.
    /// Pages are counted starting with 1.
    /// </summary>
    [AliasAs("page")]
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
    [AliasAs("per_page")]
    public int? PageSize
    {
        get => _pageSize;
        init => _pageSize = value != null ? Math.Clamp(value.Value, 1, 100) : value;
    }
}
