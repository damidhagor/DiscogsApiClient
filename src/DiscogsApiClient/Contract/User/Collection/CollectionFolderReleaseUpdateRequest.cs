namespace DiscogsApiClient.Contract.User.Collection;

// Discogs rejects an explicit "null" for these fields with a 422 - they must be omitted entirely to mean "leave unchanged".
internal sealed record CollectionFolderReleaseUpdateRequest(
    [property: JsonPropertyName("rating")]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? Rating = null,
    [property: JsonPropertyName("folder_id")]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? FolderId = null);
