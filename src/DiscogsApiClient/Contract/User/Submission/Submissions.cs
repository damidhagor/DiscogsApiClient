namespace DiscogsApiClient.Contract.User.Submission;

/// <summary>
/// The artists, labels and releases a user has submitted to the Discogs database.
/// </summary>
/// <param name="Artists">List of submitted artists. Absent (<see langword="null"/>) if the user has not submitted any artists.</param>
/// <param name="Labels">List of submitted labels. Absent (<see langword="null"/>) if the user has not submitted any labels.</param>
/// <param name="Releases">List of submitted releases. Absent (<see langword="null"/>) if the user has not submitted any releases.</param>
public sealed record Submissions(
    [property:JsonPropertyName("artists")]
    IReadOnlyList<SubmissionArtist>? Artists = null,
    [property:JsonPropertyName("labels")]
    IReadOnlyList<SubmissionLabel>? Labels = null,
    [property:JsonPropertyName("releases")]
    IReadOnlyList<SubmissionRelease>? Releases = null);
