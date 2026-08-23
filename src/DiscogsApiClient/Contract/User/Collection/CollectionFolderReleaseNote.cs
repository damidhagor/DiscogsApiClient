namespace DiscogsApiClient.Contract.User.Collection;

/// <summary>
/// Represents the value of a user-defined custom collection field on a release instance.
/// </summary>
/// <param name="FieldId">The id of the custom field.</param>
/// <param name="Value">The field's value on the release instance.</param>
public sealed record CollectionFolderReleaseNote(
    [property: JsonPropertyName("field_id")]
    int FieldId,
    [property: JsonPropertyName("value")]
    string Value);
