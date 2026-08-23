namespace DiscogsApiClient.Contract.User.Collection;

internal sealed record CollectionFolderCreateRequest(
    [property:JsonPropertyName("name")]
    string Name);
