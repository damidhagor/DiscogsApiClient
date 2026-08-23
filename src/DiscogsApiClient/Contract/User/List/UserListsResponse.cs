namespace DiscogsApiClient.Contract.User.List;

/// <summary>
/// Contains paged lists created by a user.
/// </summary>
/// <param name="Pagination">Pagination information.</param>
/// <param name="Lists">The user's lists.</param>
public sealed record UserListsResponse(
    [property:JsonPropertyName("pagination")]
    Pagination Pagination,
    [property:JsonPropertyName("lists")]
    IReadOnlyList<UserList> Lists);
