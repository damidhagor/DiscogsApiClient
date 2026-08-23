namespace DiscogsApiClient.Contract.Release;

internal sealed record ReleaseRatingUpdateRequest(
    [property: JsonPropertyName("rating")]
    int Rating);
