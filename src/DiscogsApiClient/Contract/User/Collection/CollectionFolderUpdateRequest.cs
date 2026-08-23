namespace DiscogsApiClient.Contract.User.Collection;

internal sealed record CollectionFolderUpdateRequest(
    [property:JsonPropertyName("name")]
    string Name);
