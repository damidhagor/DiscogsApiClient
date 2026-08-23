namespace DiscogsApiClient.Contract.User.Submission;

/// <summary>
/// A label submitted by a user, as returned in a user's submissions.
/// </summary>
/// <param name="Id">Label id</param>
/// <param name="ResourceUrl">The Api url to the label</param>
/// <param name="Name">Label name</param>
/// <param name="ContactInfo">Label's contact information, if any</param>
/// <param name="Profile">Profile text of the label</param>
/// <param name="Uri">Url to the label's profile on the Discogs website</param>
/// <param name="ReleasesUrl">The Api url to the list of releases of the label</param>
/// <param name="Images">List of images of the label</param>
/// <param name="DataQuality"></param>
public sealed record SubmissionLabel(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("contactinfo")]
    string? ContactInfo,
    [property:JsonPropertyName("profile")]
    string Profile,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("releases_url")]
    string ReleasesUrl,
    [property:JsonPropertyName("images")]
    IReadOnlyList<Image> Images,
    [property:JsonPropertyName("data_quality")]
    string DataQuality);


/**
{
    "id":1742496,
    "name":"Echoes Of Death Prods",
    "resource_url":"https://api.discogs.com/labels/1742496",
    "uri":"https://www.discogs.com/label/1742496-Echoes-Of-Death-Prods",
    "releases_url":"https://api.discogs.com/labels/1742496/releases",
    "images":[],
    "contactinfo":"http://rxdxcxmxprods.blogspot.com/",
    "profile":"...",
    "data_quality":"Needs Vote"
}
*/
