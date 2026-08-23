namespace DiscogsApiClient.Contract.User.List;

/// <summary>
/// The number of users who want or have an item, split by community-wide and requesting-user-specific counts.
/// </summary>
/// <param name="Community">The community-wide statistics for the item.</param>
/// <param name="User">The requesting user's statistics for the item. Absent (<see langword="null"/>) when unauthenticated.</param>
public sealed record ListItemStats(
    [property:JsonPropertyName("community")]
    ListItemStatsEntry Community,
    [property:JsonPropertyName("user")]
    ListItemStatsEntry? User = null);

/// <summary>
/// The want/collection counts for a list item.
/// </summary>
/// <param name="InWantlist">The number of wantlists the item is contained in.</param>
/// <param name="InCollection">The number of collections the item is contained in.</param>
public sealed record ListItemStatsEntry(
    [property:JsonPropertyName("in_wantlist")]
    int InWantlist,
    [property:JsonPropertyName("in_collection")]
    int InCollection);
