namespace DiscogsApiClient.Contract.User.List;

/// <summary>
/// The details and items of a user's list.
/// </summary>
/// <param name="Id">The list's id.</param>
/// <param name="User">The user who owns the list.</param>
/// <param name="Name">The list's name.</param>
/// <param name="Description">The list's description.</param>
/// <param name="Public">Whether the list is publicly visible.</param>
/// <param name="DateAdded">The date the list was created.</param>
/// <param name="DateChanged">The date the list was last changed.</param>
/// <param name="Uri">The url of the list's webpage.</param>
/// <param name="ResourceUrl">The url of the list's API endpoint.</param>
/// <param name="ImageUrl">The url of the list's cover image.</param>
/// <param name="Items">The items contained in the list.</param>
public sealed record ListDetails(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("user")]
    ListUser User,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("description")]
    string Description,
    [property:JsonPropertyName("public")]
    bool Public,
    [property:JsonPropertyName("date_added")]
    DateTimeOffset DateAdded,
    [property:JsonPropertyName("date_changed")]
    DateTimeOffset DateChanged,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("image_url")]
    string ImageUrl,
    [property:JsonPropertyName("items")]
    IReadOnlyList<ListItem> Items);
