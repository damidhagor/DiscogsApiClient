using DiscogsApiClient.SourceGenerator.ApiClient;

namespace DiscogsApiClient;

// CA1822: Mark members as static
// CA1823: Avoid unused private fields
#pragma warning disable CA1822, CA1823
[ApiClient(typeof(DiscogsJsonSerializerContext))]
internal sealed partial class DiscogsApiClient(HttpClient httpClient, DiscogsJsonSerializerContext jsonSerializerContext) : IDiscogsApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly DiscogsJsonSerializerContext _jsonSerializerContext = jsonSerializerContext;

    [HttpGet("/oauth/identity")]
    public partial Task<Identity> GetIdentity(CancellationToken cancellationToken);


    [HttpGet("/users/{username}")]
    private partial Task<User> GetUserInternal(string username, CancellationToken cancellationToken);

    public async Task<User> GetUser(string username, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetUserInternal(username, cancellationToken).ConfigureAwait(false);
    }


    [HttpPost("/users/{username}")]
    private partial Task<User> UpdateUserInternal(string username, [Body] UserProfileUpdateRequest request, CancellationToken cancellationToken);

    public async Task<User> UpdateUser(string username, UserProfileUpdateRequest request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentNullException.ThrowIfNull(request);
        return await UpdateUserInternal(username, request, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/collection/folders")]
    private partial Task<CollectionFoldersResponse> GetCollectionFoldersInternal(string username, CancellationToken cancellationToken);

    public async Task<CollectionFoldersResponse> GetCollectionFolders(string username, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetCollectionFoldersInternal(username, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/collection/folders/{folderId}")]
    private partial Task<CollectionFolder> GetCollectionFolderInternal(string username, int folderId, CancellationToken cancellationToken);

    public async Task<CollectionFolder> GetCollectionFolder(string username, int folderId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThan(folderId, 0);
        return await GetCollectionFolderInternal(username, folderId, cancellationToken).ConfigureAwait(false);
    }


    [HttpPost("/users/{username}/collection/folders")]
    private partial Task<CollectionFolder> CreateCollectionFolderInternal(string username, [Body] CollectionFolderCreateRequest createRequest, CancellationToken cancellationToken);

    public async Task<CollectionFolder> CreateCollectionFolder(string username, string folderName, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(folderName);
        return await CreateCollectionFolderInternal(username, new(folderName), cancellationToken).ConfigureAwait(false);
    }


    [HttpPost("/users/{username}/collection/folders/{folderId}")]
    private partial Task<CollectionFolder> UpdateCollectionFolderInternal(string username, int folderId, [Body] CollectionFolderUpdateRequest request, CancellationToken cancellationToken);

    public async Task<CollectionFolder> UpdateCollectionFolder(string username, int folderId, string folderName, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(folderName);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(folderId, 1);
        return await UpdateCollectionFolderInternal(username, folderId, new(folderName), cancellationToken).ConfigureAwait(false);
    }


    [HttpDelete("/users/{username}/collection/folders/{folderId}")]
    private partial Task DeleteCollectionFolderInternal(string username, int folderId, CancellationToken cancellationToken);

    public async Task DeleteCollectionFolder(string username, int folderId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(folderId, 1);
        await DeleteCollectionFolderInternal(username, folderId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/collection/folders/{folderId}/releases")]
    private partial Task<CollectionFolderReleasesResponse> GetCollectionItemsByFolderInternal(string username, int folderId, PaginationQueryParameters? paginationQueryParameters, CollectionFolderReleaseSortQueryParameters? collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken);

    public async Task<CollectionFolderReleasesResponse> GetCollectionItemsByFolder(string username, int folderId, PaginationQueryParameters? paginationQueryParameters, CollectionFolderReleaseSortQueryParameters? collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThan(folderId, 0);
        return await GetCollectionItemsByFolderInternal(username, folderId, paginationQueryParameters, collectionFolderReleaseSortQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/collection/releases/{releaseId}")]
    private partial Task<CollectionFolderReleasesResponse> GetCollectionItemsByReleaseInternal(string username, int releaseId, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    public async Task<CollectionFolderReleasesResponse> GetCollectionItemsByRelease(string username, int releaseId, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        return await GetCollectionItemsByReleaseInternal(username, releaseId, paginationQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpPost("/users/{username}/collection/folders/{folderId}/releases/{releaseId}")]
    private partial Task<CollectionFolderRelease> AddReleaseToCollectionFolderInternal(string username, int folderId, int releaseId, CancellationToken cancellationToken);

    public async Task<CollectionFolderRelease> AddReleaseToCollectionFolder(string username, int folderId, int releaseId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThan(folderId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(releaseId, 1);
        return await AddReleaseToCollectionFolderInternal(username, folderId, releaseId, cancellationToken).ConfigureAwait(false);
    }


    [HttpDelete("/users/{username}/collection/folders/{folderId}/releases/{releaseId}/instances/{instanceId}")]
    private partial Task DeleteReleaseFromCollectionFolderInternal(string username, int folderId, int releaseId, long instanceId, CancellationToken cancellationToken);

    public async Task DeleteReleaseFromCollectionFolder(string username, int folderId, int releaseId, long instanceId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThan(folderId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(releaseId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(instanceId, 1);
        await DeleteReleaseFromCollectionFolderInternal(username, folderId, releaseId, instanceId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/collection/value")]
    private partial Task<CollectionValue> GetCollectionValueInternal(string username, CancellationToken cancellationToken);

    public async Task<CollectionValue> GetCollectionValue(string username, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetCollectionValueInternal(username, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/collection/fields")]
    private partial Task<CollectionFieldsResponse> GetCollectionFieldsInternal(string username, CancellationToken cancellationToken);

    public async Task<CollectionFieldsResponse> GetCollectionFields(string username, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetCollectionFieldsInternal(username, cancellationToken).ConfigureAwait(false);
    }


    [HttpPost("/users/{username}/collection/folders/{folderId}/releases/{releaseId}/instances/{instanceId}")]
    private partial Task UpdateCollectionFolderReleaseInternal(string username, int folderId, int releaseId, long instanceId, [Body] CollectionFolderReleaseUpdateRequest request, CancellationToken cancellationToken);

    public async Task UpdateCollectionFolderRelease(string username, int folderId, int releaseId, long instanceId, int? rating, int? targetFolderId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThan(folderId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(releaseId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(instanceId, 1);
        if (targetFolderId is not null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(targetFolderId.Value, 1);
        }

        await UpdateCollectionFolderReleaseInternal(username, folderId, releaseId, instanceId, new(rating, targetFolderId), cancellationToken).ConfigureAwait(false);
    }


    [HttpPost("/users/{username}/collection/folders/{folderId}/releases/{releaseId}/instances/{instanceId}/fields/{fieldId}")]
    private partial Task UpdateCollectionFolderReleaseFieldInternal(string username, int folderId, int releaseId, long instanceId, int fieldId, [Body] CollectionFieldValueUpdateRequest request, CancellationToken cancellationToken);

    public async Task UpdateCollectionFolderReleaseField(string username, int folderId, int releaseId, long instanceId, int fieldId, string value, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfLessThan(folderId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(releaseId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(instanceId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(fieldId, 1);
        await UpdateCollectionFolderReleaseFieldInternal(username, folderId, releaseId, instanceId, fieldId, new(value), cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/wants")]
    private partial Task<WantlistReleasesResponse> GetWantlistReleasesInternal(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    public async Task<WantlistReleasesResponse> GetWantlistReleases(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetWantlistReleasesInternal(username, paginationQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpPut("/users/{username}/wants/{releaseId}")]
    private partial Task<WantlistRelease> AddReleaseToWantlistInternal(string username, int releaseId, CancellationToken cancellationToken);

    public async Task<WantlistRelease> AddReleaseToWantlist(string username, int releaseId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        return await AddReleaseToWantlistInternal(username, releaseId, cancellationToken).ConfigureAwait(false);
    }


    [HttpDelete("/users/{username}/wants/{releaseId}")]
    private partial Task DeleteReleaseFromWantlistInternal(string username, int releaseId, CancellationToken cancellationToken);

    public async Task DeleteReleaseFromWantlist(string username, int releaseId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        await DeleteReleaseFromWantlistInternal(username, releaseId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/submissions")]
    private partial Task<SubmissionsResponse> GetSubmissionsInternal(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    public async Task<SubmissionsResponse> GetSubmissions(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetSubmissionsInternal(username, paginationQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/contributions")]
    private partial Task<ContributionsResponse> GetContributionsInternal(string username, PaginationQueryParameters? paginationQueryParameters, ContributionSortQueryParameters? contributionSortQueryParameters, CancellationToken cancellationToken);

    public async Task<ContributionsResponse> GetContributions(string username, PaginationQueryParameters? paginationQueryParameters, ContributionSortQueryParameters? contributionSortQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetContributionsInternal(username, paginationQueryParameters, contributionSortQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/users/{username}/lists")]
    private partial Task<UserListsResponse> GetUserListsInternal(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    public async Task<UserListsResponse> GetUserLists(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetUserListsInternal(username, paginationQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/lists/{listId}")]
    private partial Task<ListDetails> GetListInternal(int listId, CancellationToken cancellationToken);

    public async Task<ListDetails> GetList(int listId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(listId, 0);
        return await GetListInternal(listId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/artists/{artistId}")]
    private partial Task<Artist> GetArtistInternal(int artistId, CancellationToken cancellationToken);

    public async Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(artistId, 0);
        return await GetArtistInternal(artistId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/artists/{artistId}/releases")]
    private partial Task<ArtistReleasesResponse> GetArtistReleasesInternal(int artistId, PaginationQueryParameters? paginationQueryParameters, ArtistReleaseSortQueryParameters? artistReleaseSortQueryParameters, CancellationToken cancellationToken);

    public async Task<ArtistReleasesResponse> GetArtistReleases(int artistId, PaginationQueryParameters? paginationQueryParameters, ArtistReleaseSortQueryParameters? artistReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(artistId, 0);
        return await GetArtistReleasesInternal(artistId, paginationQueryParameters, artistReleaseSortQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/labels/{labelId}")]
    private partial Task<Label> GetLabelInternal(int labelId, CancellationToken cancellationToken);

    public async Task<Label> GetLabel(int labelId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(labelId, 0);
        return await GetLabelInternal(labelId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/labels/{labelId}/releases")]
    private partial Task<LabelReleasesResponse> GetLabelReleasesInternal(int labelId, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    public async Task<LabelReleasesResponse> GetLabelReleases(int labelId, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(labelId, 0);
        return await GetLabelReleasesInternal(labelId, paginationQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/masters/{masterReleaseId}")]
    private partial Task<MasterRelease> GetMasterReleaseInternal(int masterReleaseId, CancellationToken cancellationToken);

    public async Task<MasterRelease> GetMasterRelease(int masterReleaseId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(masterReleaseId, 0);
        return await GetMasterReleaseInternal(masterReleaseId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/masters/{masterReleaseId}/versions")]
    private partial Task<MasterReleaseVersionsResponse> GetMasterReleaseVersionsInternal(int masterReleaseId, PaginationQueryParameters? paginationQueryParameters, MasterReleaseVersionFilterQueryParameters? masterReleaseVersionQueryParameters, CancellationToken cancellationToken);

    public async Task<MasterReleaseVersionsResponse> GetMasterReleaseVersions(int masterReleaseId, PaginationQueryParameters? paginationQueryParameters, MasterReleaseVersionFilterQueryParameters? masterReleaseVersionQueryParameters, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(masterReleaseId, 0);
        return await GetMasterReleaseVersionsInternal(masterReleaseId, paginationQueryParameters, masterReleaseVersionQueryParameters, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/releases/{releaseId}")]
    private partial Task<Release> GetReleaseInternal(int releaseId, CancellationToken cancellationToken);

    public async Task<Release> GetRelease(int releaseId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        return await GetReleaseInternal(releaseId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/releases/{releaseId}/rating/{username}")]
    private partial Task<ReleaseRatingResponse> GetReleaseRatingInternal(int releaseId, string username, CancellationToken cancellationToken);

    public async Task<ReleaseRatingResponse> GetReleaseRating(int releaseId, string username, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        return await GetReleaseRatingInternal(releaseId, username, cancellationToken).ConfigureAwait(false);
    }


    [HttpPut("/releases/{releaseId}/rating/{username}")]
    private partial Task<ReleaseRatingResponse> UpdateReleaseRatingInternal(int releaseId, string username, [Body] ReleaseRatingUpdateRequest request, CancellationToken cancellationToken);

    public async Task<ReleaseRatingResponse> UpdateReleaseRating(int releaseId, string username, int rating, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentOutOfRangeException.ThrowIfLessThan(rating, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rating, 5);
        return await UpdateReleaseRatingInternal(releaseId, username, new(rating), cancellationToken).ConfigureAwait(false);
    }


    [HttpDelete("/releases/{releaseId}/rating/{username}")]
    private partial Task DeleteReleaseRatingInternal(int releaseId, string username, CancellationToken cancellationToken);

    public async Task DeleteReleaseRating(int releaseId, string username, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        await DeleteReleaseRatingInternal(releaseId, username, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/releases/{releaseId}/rating")]
    private partial Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRatingInternal(int releaseId, CancellationToken cancellationToken);

    public async Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRating(int releaseId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        return await GetReleaseCommunityRatingInternal(releaseId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/releases/{releaseId}/stats")]
    private partial Task<ReleaseStatsResponse> GetReleaseStatsInternal(int releaseId, CancellationToken cancellationToken);

    public async Task<ReleaseStatsResponse> GetReleaseStats(int releaseId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(releaseId, 0);
        return await GetReleaseStatsInternal(releaseId, cancellationToken).ConfigureAwait(false);
    }


    [HttpGet("/database/search")]
    public partial Task<SearchResultsResponse> SearchDatabase(SearchQueryParameters searchQueryParameters, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);
}
