using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;
using static DiscogsApiClient.QueryParameters.ArtistReleaseSortQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Sorting query parameters used for retrieving artist releases.
/// </summary>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record ArtistReleaseSortQueryParameters(
    [property: AliasAs("sort")]
    SortableProperty? SortProperty = default,
    [property: AliasAs("sort_order")]
    SortOrder? SortOrder = default)
{
    /// <summary>
    /// Release properties which can be used to sort the results with.
    /// </summary>
    [GenerateJsonConverter]
    public enum SortableProperty
    {
        [AliasAs("year")]
        Year,
        [AliasAs("title")]
        Title,
        [AliasAs("format")]
        Format
    }
}
