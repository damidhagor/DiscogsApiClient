namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// Statistics of a release
/// </summary>
/// <param name="IsOffensive">If the release is marked as offensive</param>
public sealed record ReleaseStatsResponse(
    [property:JsonPropertyName("is_offensive")]
    bool IsOffensive);


/*
{
    "is_offensive": false
}
 */
