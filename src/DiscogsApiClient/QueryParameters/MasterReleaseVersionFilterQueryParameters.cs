﻿using System.Runtime.Serialization;
using static DiscogsApiClient.QueryParameters.MasterReleaseVersionFilterQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Filter and sorting query parameters used for retrieving master release versions.
/// </summary>
/// <param name="Format">The format to filter the results by.</param>
/// <param name="Label">The label to filter the results by.</param>
/// <param name="Released">The release year to filter the results by.</param>
/// <param name="Country">The country to filter the results by.</param>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record MasterReleaseVersionFilterQueryParameters(
    [property: AliasAs("format")]
    string? Format = default,
    [property: AliasAs("label")]
    string? Label = default,
    [property: AliasAs("released")]
    string? Released = default,
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
    public enum SortableProperty
    {
        [EnumMember(Value = "released")]
        Released,
        [EnumMember(Value = "title")]
        Title,
        [EnumMember(Value = "format")]
        Format,
        [EnumMember(Value = "label")]
        Label,
        [EnumMember(Value = "catno")]
        CatalogNumber,
        [EnumMember(Value = "country")]
        Country
    }
}
