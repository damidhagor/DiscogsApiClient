using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Pagination parameters used by paginated requests to the Discogs Api.
/// </summary>
public sealed record PaginationQueryParameters()
{
    /// <summary>
    /// Indicates which page should be returned.
    /// Pages are counted starting with 1.
    /// </summary>
    [AliasAs("page")]
    public int? Page
    {
        get;
        init => field = value is not null ? Math.Max(1, value.Value) : null;
    }

    /// <summary>
    /// Indicates the size of the requested page.
    /// The value must be between 1 and 100. The default value is 50.
    /// </summary>
    [AliasAs("per_page")]
    public int? PageSize
    {
        get;
        init => field = value is not null ? Math.Clamp(value.Value, 1, 100) : value;
    }
}
