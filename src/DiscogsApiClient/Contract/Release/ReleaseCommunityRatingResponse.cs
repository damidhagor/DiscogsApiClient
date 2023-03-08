namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// Returns the community rating for a release
/// </summary>
/// <param name="ReleaseId">release id</param>
/// <param name="Rating">Community rating</param>
public sealed record ReleaseCommunityRatingResponse(
    [property:JsonPropertyName("release_id")]
    int ReleaseId,
    [property:JsonPropertyName("rating")]
    ReleaseCommunityRating Rating);


/*
{
    "release_id" : 5134861,
    "rating" : {
        "count" : 22,
        "average" : 4.05
    }
}
 */
