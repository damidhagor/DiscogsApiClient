namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// User & community statistics for <see langword="abstract"/>release
/// </summary>
/// <param name="CommunityStatistics">Community statistics</param>
/// <param name="UserStatistics">User statistics</param>
public sealed record ReleaseStats(
    [property:JsonPropertyName("community")]
    ReleaseStatValues CommunityStatistics,
    [property:JsonPropertyName("user")]
    ReleaseStatValues UserStatistics);


/*
"stats":{
    "community":{
        "in_wantlist":29,
        "in_collection":280
    },
    "user":{
        "in_wantlist":0,
        "in_collection":0
    }
} 
*/
