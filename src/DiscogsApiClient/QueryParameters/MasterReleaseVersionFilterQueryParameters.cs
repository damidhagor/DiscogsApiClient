using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;
using static DiscogsApiClient.QueryParameters.MasterReleaseVersionFilterQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Filter and sorting query parameters used for retrieving master release versions.
/// </summary>
/// <param name="Format">The format to filter the results by.</param>
/// <param name="Label">The label to filter the results by.</param>
/// <param name="Year">The release year to filter the results by.</param>
/// <param name="Country">The country to filter the results by.</param>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record MasterReleaseVersionFilterQueryParameters(
    [property: AliasAs("format")]
    string? Format = default,
    [property: AliasAs("label")]
    string? Label = default,
    [property: AliasAs("released")]
    string? Year = default,
    [property: AliasAs("country")]
    string? Country = default,
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
        [AliasAs("released")]
        Year,
        [AliasAs("title")]
        Title,
        [AliasAs("format")]
        Format,
        [AliasAs("label")]
        Label,
        [AliasAs("catno")]
        CatalogNumber,
        [AliasAs("country")]
        Country
    }
}
