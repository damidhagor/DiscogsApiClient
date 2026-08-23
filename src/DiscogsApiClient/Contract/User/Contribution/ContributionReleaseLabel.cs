namespace DiscogsApiClient.Contract.User.Contribution;

/// <summary>
/// Label, company or series entry credited on a release within a user's contributions.
/// </summary>
/// <param name="Id">Label id</param>
/// <param name="Name">Label name</param>
/// <param name="CatalogNumber">Label catalog number</param>
/// <param name="ResourceUrl">The Api url for the label</param>
/// <param name="EntityType"></param>
/// <param name="EntityTypeName"></param>
public sealed record ContributionReleaseLabel(
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
