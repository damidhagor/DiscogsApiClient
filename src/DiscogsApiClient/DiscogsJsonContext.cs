namespace DiscogsApiClient;

public static partial class ServiceCollectionExtensions
{
    // General
    [JsonSerializable(typeof(ErrorMessage))]
    [JsonSerializable(typeof(Image))]
    [JsonSerializable(typeof(Pagination))]
    // User
    [JsonSerializable(typeof(Identity))]
    [JsonSerializable(typeof(User))]
    [JsonSerializable(typeof(UserListReleaseInformation))]
    // Collection
    [JsonSerializable(typeof(CollectionFolder))]
    [JsonSerializable(typeof(CollectionFolderCreateRequest))]
    [JsonSerializable(typeof(CollectionFolderRelease))]
    [JsonSerializable(typeof(CollectionFolderReleasesResponse))]
    [JsonSerializable(typeof(CollectionFoldersResponse))]
    [JsonSerializable(typeof(CollectionValue))]
    // Wantlist
    [JsonSerializable(typeof(WantlistRelease))]
    [JsonSerializable(typeof(WantlistReleasesResponse))]
    // Artist
    [JsonSerializable(typeof(Artist))]
    [JsonSerializable(typeof(ArtistMember))]
    [JsonSerializable(typeof(ArtistRelease))]
    [JsonSerializable(typeof(ArtistReleasesResponse))]
    // Label
    [JsonSerializable(typeof(Label))]
    [JsonSerializable(typeof(LabelRelease))]
    [JsonSerializable(typeof(LabelReleasesResponse))]
    [JsonSerializable(typeof(LabelShortInfo))]
    // Release
    [JsonSerializable(typeof(MasterRelease))]
    [JsonSerializable(typeof(MasterReleaseVersion))]
    [JsonSerializable(typeof(MasterReleaseVersionAppliedFilter))]
    [JsonSerializable(typeof(MasterReleaseVersionFilter))]
    [JsonSerializable(typeof(MasterReleaseVersionFilterFacet))]
    [JsonSerializable(typeof(MasterReleaseVersionFilterFacetValue))]
    [JsonSerializable(typeof(MasterReleaseVersionFilters))]
    [JsonSerializable(typeof(MasterReleaseVersionsResponse))]
    [JsonSerializable(typeof(Release))]
    [JsonSerializable(typeof(ReleaseArtist))]
    [JsonSerializable(typeof(ReleaseCommunity))]
    [JsonSerializable(typeof(ReleaseCommunityRating))]
    [JsonSerializable(typeof(ReleaseCommunityRatingResponse))]
    [JsonSerializable(typeof(ReleaseFormat))]
    [JsonSerializable(typeof(ReleaseIdentifier))]
    [JsonSerializable(typeof(ReleaseLabel))]
    [JsonSerializable(typeof(ReleaseStats))]
    [JsonSerializable(typeof(ReleaseStatsResponse))]
    [JsonSerializable(typeof(ReleaseStatValues))]
    [JsonSerializable(typeof(TracklistItem))]
    [JsonSerializable(typeof(Video))]
    // Search
    [JsonSerializable(typeof(SearchResult))]
    [JsonSerializable(typeof(SearchResultCommunityStats))]
    [JsonSerializable(typeof(SearchResultsResponse))]
    [JsonSerializable(typeof(SearchResultType))]
    [JsonSerializable(typeof(SearchResultUserData))]
    internal partial class DiscogsJsonContext : JsonSerializerContext
    {

    }
}
