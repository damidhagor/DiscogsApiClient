using System.Runtime.Serialization;
using static DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Sorting query parameters used for retrieving collection folder releases.
/// </summary>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record CollectionFolderReleaseSortQueryParameters(
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
        [EnumMember(Value = "label")]
        Label,
        [EnumMember(Value = "artist")]
        Artist,
        [EnumMember(Value = "title")]
        Title,
        [EnumMember(Value = "catno")]
        CatalogNumber,
        [EnumMember(Value = "format")]
        Format,
        [EnumMember(Value = "rating")]
        Rating,
        [EnumMember(Value = "added")]
        AddedAt,
        [EnumMember(Value = "year")]
        Year
    }
}
