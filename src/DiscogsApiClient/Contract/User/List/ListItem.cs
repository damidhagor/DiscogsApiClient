namespace DiscogsApiClient.Contract.User.List;

/// <summary>
/// An item contained in a user's list.
/// </summary>
/// <param name="Id">The id of the referenced artist, label or release.</param>
/// <param name="Type">The type of the referenced item.</param>
/// <param name="DisplayTitle">The display title of the referenced item.</param>
/// <param name="Comment">The comment added to the item in the list.</param>
/// <param name="Uri">The url of the referenced item's webpage.</param>
/// <param name="ResourceUrl">The url of the referenced item's API endpoint.</param>
/// <param name="ImageUrl">The url of the referenced item's image.</param>
/// <param name="Stats">The want/collection statistics of the referenced item. Absent (<see langword="null"/>) for artist and label items.</param>
public sealed record ListItem(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("type")]
    ListItemType Type,
    [property:JsonPropertyName("display_title")]
    string DisplayTitle,
    [property:JsonPropertyName("comment")]
    string Comment,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("image_url")]
    string ImageUrl,
    [property:JsonPropertyName("stats")]
    ListItemStats? Stats = null);
