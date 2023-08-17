using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Defines the sorting order for applicable requests.
/// </summary>
public enum SortOrder
{
    [AliasAs("asc")]
    Ascending,
    [AliasAs("desc")]
    Descending
}
