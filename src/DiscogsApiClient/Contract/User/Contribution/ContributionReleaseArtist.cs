namespace DiscogsApiClient.Contract.User.Contribution;

/// <summary>
/// Artist credited on a release within a user's contributions.
/// </summary>
/// <param name="Id">Artist id</param>
/// <param name="Name">Artist name</param>
/// <param name="ResourceUrl">The Api url for this artist</param>
/// <param name="Anv"></param>
/// <param name="Join"></param>
/// <param name="Role"></param>
/// <param name="Tracks">The tracks this artist participated in on the release</param>
public sealed record ContributionReleaseArtist(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("anv")]
    string Anv,
    [property:JsonPropertyName("join")]
    string Join,
    [property:JsonPropertyName("role")]
    string Role,
    [property:JsonPropertyName("tracks")]
    string Tracks);
