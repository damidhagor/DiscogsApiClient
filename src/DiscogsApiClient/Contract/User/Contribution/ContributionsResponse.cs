namespace DiscogsApiClient.Contract.User.Contribution;

/// <summary>
/// The releases a user has contributed (added or edited) to the Discogs database.
/// </summary>
/// <param name="Pagination">Pagination information.</param>
/// <param name="Contributions">The user's contributed releases.</param>
public sealed record ContributionsResponse(
    [property:JsonPropertyName("pagination")]
    Pagination Pagination,
    [property:JsonPropertyName("contributions")]
    IReadOnlyList<ContributionRelease> Contributions);
