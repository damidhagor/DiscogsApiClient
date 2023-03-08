namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// Community rating for a release
/// </summary>
/// <param name="Count">Number of ratings</param>
/// <param name="Average">Average rating</param>
public sealed record ReleaseCommunityRating(
    [property:JsonPropertyName("count")]
    int Count,
    [property:JsonPropertyName("average")]
    float Average);


/*
{
    "release_id" : 5134861,
    "rating" : {
        "count" : 22,
        "average" : 4.05
    }
}
 */
