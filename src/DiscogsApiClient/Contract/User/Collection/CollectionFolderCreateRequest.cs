namespace DiscogsApiClient.Contract.User.Collection;

/// <summary>
/// Request for creating or updating a collection folder.
/// </summary>
/// <param name="Name">The new folder name</param>
public sealed record CollectionFolderCreateRequest(
    [property:JsonPropertyName("name")]
    string Name);
