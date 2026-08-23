namespace DiscogsApiClient.Contract.User.List;

/// <summary>
/// A minimal reference to the user who owns a list.
/// </summary>
/// <param name="Id">The user's id.</param>
/// <param name="Username">The user's username.</param>
/// <param name="AvatarUrl">The url of the user's avatar image.</param>
/// <param name="ResourceUrl">The url of the user's API endpoint.</param>
public sealed record ListUser(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("username")]
    string Username,
    [property:JsonPropertyName("avatar_url")]
    string AvatarUrl,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl);
