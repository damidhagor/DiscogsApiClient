using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Defines the sorting order for applicable requests.
/// </summary>

[GenerateJsonConverter]
public enum SortOrder
{
    [AliasAs("asc")]
    Ascending,
    [AliasAs("desc")]
    Descending
}
