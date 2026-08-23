namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// A user's rating for a release.
/// </summary>
/// <param name="Username">The name of the user who owns the rating.</param>
/// <param name="ReleaseId">The release's id.</param>
/// <param name="Rating">The rating between 1 and 5.</param>
public sealed record ReleaseRatingResponse(
    [property: JsonPropertyName("username")]
    string Username,
    [property: JsonPropertyName("release_id")]
    int ReleaseId,
    [property: JsonPropertyName("rating")]
    int Rating);


/*
{
    "username": "memory",
    "release_id": 249504,
    "rating": 5
}
 */
