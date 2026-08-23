namespace DiscogsApiClient.Contract.User.Submission;

/// <summary>
/// Label, company or series entry credited on a release within a user's submissions.
/// </summary>
/// <param name="Id">Label id</param>
/// <param name="Name">Label name</param>
/// <param name="CatalogNumber">Label catalog number</param>
/// <param name="ResourceUrl">The Api url for the label</param>
/// <param name="EntityType"></param>
/// <param name="EntityTypeName"></param>
public sealed record SubmissionReleaseLabel(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("catno")]
    string CatalogNumber,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("entity_type")]
    string EntityType,
    [property:JsonPropertyName("entity_type_name")]
    string EntityTypeName);


/**
{
    "name":"Nuclear Blast",
    "catno":"NB 265-2",
    "entity_type":"1",
    "entity_type_name":"Label",
    "id":11499,
    "resource_url":"https://api.discogs.com/labels/11499"
}
*/
