using System.Runtime.Serialization;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Defines the sorting order for applicable requests.
/// </summary>
public enum SortOrder
{
    [EnumMember(Value = "asc")]
    Ascending,
    [EnumMember(Value = "desc")]
    Descending
}
