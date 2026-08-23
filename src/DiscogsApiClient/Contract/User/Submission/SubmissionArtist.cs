namespace DiscogsApiClient.Contract.User.Submission;

/// <summary>
/// An artist submitted by a user, as returned in a user's submissions.
/// </summary>
/// <param name="Id">Artist id</param>
/// <param name="ResourceUrl">The Api url for this artist</param>
/// <param name="Uri">The url to the artist's page on Discogs</param>
/// <param name="Name">Artist's name</param>
/// <param name="RealName">The artist's real name, if known and applicable (e.g. for solo artists)</param>
/// <param name="Profile">Profile text of the artist</param>
/// <param name="ReleasesUrl">The Api url to query the releases of this artist</param>
/// <param name="Images">List of images from this artist</param>
/// <param name="Urls">List of urls of this artist, if any</param>
/// <param name="Members">Names of the artist's members, if this artist is a group</param>
/// <param name="DataQuality"></param>
public sealed record SubmissionArtist(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("realname")]
    string? RealName,
    [property:JsonPropertyName("profile")]
    string Profile,
    [property:JsonPropertyName("releases_url")]
    string ReleasesUrl,
    [property:JsonPropertyName("images")]
    IReadOnlyList<Image> Images,
    [property:JsonPropertyName("urls")]
    IReadOnlyList<string>? Urls,
    [property:JsonPropertyName("members")]
    IReadOnlyList<string>? Members,
    [property:JsonPropertyName("data_quality")]
    string DataQuality);


/**
{
    "name":"Disfigured Human Mind",
    "id":6772850,
    "resource_url":"https://api.discogs.com/artists/6772850",
    "uri":"https://www.discogs.com/artist/6772850-Disfigured-Human-Mind",
    "releases_url":"https://api.discogs.com/artists/6772850/releases",
    "images":[],
    "profile":"...",
    "urls":["https://..."],
    "members":["Legion (39)", "Panafernalicus", "Pedronazi", "Pestilence (7)", "Ze Nalhas"],
    "data_quality":"Needs Vote"
}
*/
