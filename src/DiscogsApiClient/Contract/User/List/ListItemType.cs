using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.Contract.User.List;

/// <summary>
/// The type of entity a list item refers to.
/// </summary>
[GenerateJsonConverter]
public enum ListItemType
{
    [AliasAs("artist")]
    Artist,
    [AliasAs("master")]
    Master,
    [AliasAs("release")]
    Release,
    [AliasAs("label")]
    Label
}
