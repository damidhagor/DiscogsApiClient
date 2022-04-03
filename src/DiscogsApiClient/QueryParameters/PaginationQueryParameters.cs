namespace DiscogsApiClient.QueryParameters;

public record PaginationQueryParameters()
{
    private int? _page;
    public int? Page
    {
        get => _page;
        set => _page = value != null ? Math.Max(1, value.Value) : null;
    }

    private int? _pageSize;
    public int? PageSize
    {
        get => _pageSize;
        set => _pageSize = value != null ? Math.Clamp(value.Value, 1, 100) : value;
    }


    public PaginationQueryParameters(int? page, int? pageSize)
        : this()
    {
        Page = page;
        PageSize = pageSize;
    }


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
