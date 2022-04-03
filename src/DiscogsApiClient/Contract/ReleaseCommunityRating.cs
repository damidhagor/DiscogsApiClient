namespace DiscogsApiClient.Contract;

/*
{
    "release_id" : 5134861,
    "rating" : {
        "count" : 22,
        "average" : 4.05
    }
}
 */


public record ReleaseCommunityRating(
    int Count,
    float Average);

public record ReleaseCommunityRatingResponse(
    int ReleaseId,
    ReleaseCommunityRating Rating);