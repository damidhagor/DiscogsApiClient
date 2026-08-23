using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.Contract.User.Collection;

/// <summary>
/// The kind of a user-defined collection notes field.
/// </summary>
[GenerateJsonConverter]
public enum CollectionFieldType
{
    [AliasAs("dropdown")]
    Dropdown,
    [AliasAs("textarea")]
    Textarea
}
