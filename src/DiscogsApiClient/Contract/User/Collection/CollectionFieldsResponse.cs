namespace DiscogsApiClient.Contract.User.Collection;

/// <summary>
/// The list of a user's collection custom fields.
/// </summary>
/// <param name="Fields">The custom fields.</param>
public sealed record CollectionFieldsResponse(
    [property: JsonPropertyName("fields")]
    IReadOnlyList<CollectionField> Fields);
