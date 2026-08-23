namespace DiscogsApiClient.Contract.User.Submission;

/// <summary>
/// The submissions of a user, containing all artists, labels and releases they've submitted to the Discogs database.
/// </summary>
/// <param name="Pagination">Pagination information.</param>
/// <param name="Submissions">The user's submissions.</param>
public sealed record SubmissionsResponse(
    [property:JsonPropertyName("pagination")]
    Pagination Pagination,
    [property:JsonPropertyName("submissions")]
    Submissions Submissions);
