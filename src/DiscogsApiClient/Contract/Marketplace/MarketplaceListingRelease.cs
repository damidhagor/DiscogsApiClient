namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// Summary information about the release a Marketplace listing is for.
/// </summary>
/// <param name="Id">The release id.</param>
/// <param name="Description">The combined artist/title/format description of the release.</param>
/// <param name="CatalogNumber">The catalog number(s) of the release.</param>
/// <param name="Year">The release year.</param>
/// <param name="Thumbnail">The Url to a thumbnail image of the release.</param>
/// <param name="ResourceUrl">The Api url to the full release.</param>
/// <param name="Title">The release's title.</param>
/// <param name="Artist">The release's artist name(s), as free text.</param>
/// <param name="Format">The release's format description, as free text (e.g. "Vinyl, 7\", 45 RPM, Single").</param>
/// <param name="Label">The release's label name(s), as free text.</param>
/// <param name="Images">The release's images.</param>
/// <param name="Stats">The release's community want/collection statistics.</param>
public sealed record MarketplaceListingRelease(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("catalog_number")]
    string? CatalogNumber,
    [property: JsonPropertyName("year")]
    int Year,
    [property: JsonPropertyName("thumbnail")]
    string? Thumbnail,
    [property: JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property: JsonPropertyName("title")]
    string? Title,
    [property: JsonPropertyName("artist")]
    string? Artist,
    [property: JsonPropertyName("format")]
    string? Format,
    [property: JsonPropertyName("label")]
    string? Label,
    [property: JsonPropertyName("images")]
    IReadOnlyList<Image>? Images,
    [property: JsonPropertyName("stats")]
    MarketplaceReleaseStats? Stats);
