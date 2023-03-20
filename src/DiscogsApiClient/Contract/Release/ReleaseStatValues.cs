namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// User statistics for a release
/// </summary>
/// <param name="ReleasesInWantlistCount">How many times the release is on the wantlist</param>
/// <param name="ReleasesInCollectionCount">How many times the release is in the collection</param>
public sealed record ReleaseStatValues(
    [property:JsonPropertyName("in_wantlist")]
    int ReleasesInWantlistCount,
    [property:JsonPropertyName("in_collection")]
    int ReleasesInCollectionCount);


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
