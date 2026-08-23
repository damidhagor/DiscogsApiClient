namespace DiscogsApiClient.Contract.User.Collection;

/// <summary>
/// Represents a user-defined collection notes field. These fields are available on every release in the collection.
/// </summary>
/// <param name="Id">The field's id.</param>
/// <param name="Name">The field's name.</param>
/// <param name="Position">The field's display position.</param>
/// <param name="Type">The field's type.</param>
/// <param name="Public">Whether the field is visible to users other than the collection owner.</param>
/// <param name="Options">The selectable values, only present for <see cref="CollectionFieldType.Dropdown"/> fields.</param>
/// <param name="Lines">The number of text lines, only present for <see cref="CollectionFieldType.Textarea"/> fields.</param>
public sealed record CollectionField(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("position")]
    int Position,
    [property: JsonPropertyName("type")]
    CollectionFieldType Type,
    [property: JsonPropertyName("public")]
    bool Public,
    [property: JsonPropertyName("options")]
    IReadOnlyList<string>? Options = null,
    [property: JsonPropertyName("lines")]
    int? Lines = null);
