namespace DiscogsApiClient.Contract.User.Collection;

internal sealed record CollectionFieldValueUpdateRequest(
    [property: JsonPropertyName("value")]
    string Value);
