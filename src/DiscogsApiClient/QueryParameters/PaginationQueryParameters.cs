namespace DiscogsApiClient.QueryParameters;


/// <summary>
/// Pagination parameters used by paginated requests to the Discogs Api.
/// </summary>
public record PaginationQueryParameters()
{
    private int? _page;
    /// <summary>
    /// Indicates which page should be returned.
    /// Pages are counted starting with 1.
    /// </summary>
    public int? Page
    {
        get => _page;
        set => _page = value != null ? Math.Max(1, value.Value) : null;
    }

    private int? _pageSize;
    /// <summary>
    /// Indicates the size of the requested page.
    /// The value must be between 1 and 100. The default value is 50.
    /// </summary>
    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = value != null ? Math.Clamp(value.Value, 1, 100) : value;
    }


    /// <summary>
    /// Creates a new set of pagination parameters.
    /// </summary>
    /// <param name="page">The number of the page to request. The value must greater than 1. The default, if left empty, is 1.</param>
    /// <param name="pageSize">The size of the page to request. The value must be between 1 and 100. The default, if left empty, is 50.</param>
    public PaginationQueryParameters(int? page, int? pageSize)
        : this()
    {
        Page = page;
        PageSize = pageSize;
    }

    /// <summary>
    /// Creates the Url query representation of the pagination parameters.
    /// The starting '?' query indicator is not included.
    /// </summary>
    /// <returns>Url query representation of the pagination parameters without '?'.</returns>
    public string CreateQueryParameterString()
    {
        if (Page is not null && PageSize is not null)
        {
            return $"page={Page}&per_page={PageSize}";
        }
        else if (Page is not null)
        {
            return $"page={Page}";
        }
        else if (PageSize is not null)
        {
            return $"per_page={PageSize}";
        }

        return "";
    }
}
