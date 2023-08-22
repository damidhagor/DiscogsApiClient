using DiscogsApiClient.SourceGenerator.ApiClient;

namespace DiscogsApiClient;

[ApiClient(Name = "DiscogsApiClient", Namespace = "DiscogsApiClient.Generated")]
public interface IDiscogsApiClient
{
    /// <summary>
    /// Queries the <see cref="Identity"/> of the currently authenticated user.
    /// </summary>
    [HttpGet("/oauth/identity")]
    Task<Identity> GetIdentity(CancellationToken cancellationToken = default);

    [HttpGet("/users/{username}")]
    internal Task<User> GetUserInternal(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Queries the <see cref="User"/> object of the currently authenticated user.
    /// </summary>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<User> GetUser(string username, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        return await GetUserInternal(username, cancellationToken);
    }


    [HttpGet("/users/{username}/collection/folders")]
    internal Task<CollectionFoldersResponse> GetCollectionFoldersInternal(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the collection folders of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionFoldersResponse> GetCollectionFolders(string username, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        return await GetCollectionFoldersInternal(username, cancellationToken);
    }

    [HttpGet("/users/{username}/collection/folders/{folderId}")]
    internal Task<CollectionFolder> GetCollectionFolderInternal(string username, int folderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The id of the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    public async Task<CollectionFolder> GetCollectionFolder(string username, int folderId, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThanOrEqualTo(folderId, 0);
        return await GetCollectionFolderInternal(username, folderId, cancellationToken);
    }

    [HttpPost("/users/{username}/collection/folders")]
    internal Task<CollectionFolder> CreateCollectionFolderInternal(string username, [Body] CollectionFolderCreateRequest createRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderName">The new folder's name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    public async Task<CollectionFolder> CreateCollectionFolder(string username, string folderName, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(folderName);
        return await CreateCollectionFolderInternal(username, new(folderName), cancellationToken);
    }

    [HttpPost("/users/{username}/collection/folders/{folderId}")]
    internal Task<CollectionFolder> UpdateCollectionFolderInternal(string username, int folderId, [Body] CollectionFolderCreateRequest createRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a collection folder's name for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="folderName">The new folder name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    public async Task<CollectionFolder> UpdateCollectionFolder(string username, int folderId, string folderName, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(folderName);
        Guard.IsGreaterThan(folderId, 1);
        return await UpdateCollectionFolderInternal(username, folderId, new(folderName), cancellationToken);
    }

    [HttpDelete("/users/{username}/collection/folders/{folderId}")]
    internal Task DeleteCollectionFolderInternal(string username, int folderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    public async Task DeleteCollectionFolder(string username, int folderId, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThan(folderId, 1);
        await DeleteCollectionFolderInternal(username, folderId, cancellationToken);
    }

    [HttpGet("/users/{username}/collection/folders/{folderId}/releases")]
    internal Task<CollectionFolderReleasesResponse> GetCollectionFolderReleasesInternal(string username, int folderId, PaginationQueryParameters? paginationQueryParameters = null, CollectionFolderReleaseSortQueryParameters? collectionFolderReleaseSortQueryParameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the releases in a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    public async Task<CollectionFolderReleasesResponse> GetCollectionFolderReleases(string username, int folderId, PaginationQueryParameters? paginationQueryParameters = null, CollectionFolderReleaseSortQueryParameters? collectionFolderReleaseSortQueryParameters = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThanOrEqualTo(folderId, 0);
        return await GetCollectionFolderReleasesInternal(username, folderId, paginationQueryParameters, collectionFolderReleaseSortQueryParameters, cancellationToken);
    }

    [HttpPost("/users/{username}/collection/folders/{folderId}/releases/{releaseId}")]
    internal Task<CollectionFolderRelease> AddReleaseToCollectionFolderInternal(string username, int folderId, int releaseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a release to the collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder or release id is invalid.</exception>
    public async Task<CollectionFolderRelease> AddReleaseToCollectionFolder(string username, int folderId, int releaseId, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThanOrEqualTo(folderId, 1);
        Guard.IsGreaterThanOrEqualTo(releaseId, 1);
        return await AddReleaseToCollectionFolderInternal(username, folderId, releaseId, cancellationToken);
    }

    [HttpDelete("/users/{username}/collection/folders/{folderId}/releases/{releaseId}/instances/{instanceId}")]
    internal Task DeleteReleaseFromCollectionFolderInternal(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a release from a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="instanceId">The release's instance id in the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder, release or instance id is invalid.</exception>
    public async Task DeleteReleaseFromCollectionFolder(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThanOrEqualTo(folderId, 1);
        Guard.IsGreaterThanOrEqualTo(releaseId, 1);
        Guard.IsGreaterThanOrEqualTo(instanceId, 1);
        await DeleteReleaseFromCollectionFolderInternal(username, folderId, releaseId, instanceId, cancellationToken);
    }

    [HttpGet("/users/{username}/collection/value")]
    internal Task<CollectionValue> GetCollectionValueInternal(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the estimated value of the user's collection.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionValue> GetCollectionValue(string username, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        return await GetCollectionValueInternal(username, cancellationToken);
    }


    [HttpGet("/users/{username}/wants")]
    internal Task<WantlistReleasesResponse> GetWantlistReleasesInternal(string username, PaginationQueryParameters? paginationQueryParameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the releases on the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<WantlistReleasesResponse> GetWantlistReleases(string username, PaginationQueryParameters? paginationQueryParameters = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        return await GetWantlistReleasesInternal(username, paginationQueryParameters, cancellationToken);
    }

    [HttpPut("/users/{username}/wants/{releaseId}")]
    internal Task<WantlistRelease> AddReleaseToWantlistInternal(string username, int releaseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a release to the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    public async Task<WantlistRelease> AddReleaseToWantlist(string username, int releaseId, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThan(releaseId, 0);
        return await AddReleaseToWantlistInternal(username, releaseId, cancellationToken);
    }

    [HttpDelete("/users/{username}/wants/{releaseId}")]
    internal Task DeleteReleaseFromWantlistInternal(string username, int releaseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a release from the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    public async Task DeleteReleaseFromWantlist(string username, int releaseId, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThan(releaseId, 0);
        await DeleteReleaseFromWantlistInternal(username, releaseId, cancellationToken);
    }


    [HttpGet("/artists/{artistId}")]
    internal Task<Artist> GetArtistInternal(int artistId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an artist from the Discogs database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the artist id is invalid.</exception>
    public async Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(artistId, 0);
        return await GetArtistInternal(artistId, cancellationToken);
    }

    [HttpGet("/artists/{artistId}/releases")]
    internal Task<ArtistReleasesResponse> GetArtistReleasesInternal(int artistId, PaginationQueryParameters? paginationQueryParameters = null, ArtistReleaseSortQueryParameters? artistReleaseSortQueryParameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the releases of an artist from the Discogs database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the artist id is invalid.</exception>
    public async Task<ArtistReleasesResponse> GetArtistReleases(int artistId, PaginationQueryParameters? paginationQueryParameters = null, ArtistReleaseSortQueryParameters? artistReleaseSortQueryParameters = null, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(artistId, 0);
        return await GetArtistReleasesInternal(artistId, paginationQueryParameters, artistReleaseSortQueryParameters, cancellationToken);
    }


    [HttpGet("/labels/{labelId}")]
    internal Task<Label> GetLabelInternal(int labelId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a label from the Discogs database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the label id is invalid.</exception>
    public async Task<Label> GetLabel(int labelId, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(labelId, 0);
        return await GetLabelInternal(labelId, cancellationToken);
    }

    [HttpGet("/labels/{labelId}/releases")]
    internal Task<LabelReleasesResponse> GetLabelReleasesInternal(int labelId, PaginationQueryParameters? paginationQueryParameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the releases of a label from the Discogs database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the label id is invalid.</exception>
    public async Task<LabelReleasesResponse> GetLabelReleases(int labelId, PaginationQueryParameters? paginationQueryParameters = null, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(labelId, 0);
        return await GetLabelReleasesInternal(labelId, paginationQueryParameters, cancellationToken);
    }


    [HttpGet("/masters/{masterReleaseId}")]
    internal Task<MasterRelease> GetMasterReleaseInternal(int masterReleaseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a master release from the Discogs database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    public async Task<MasterRelease> GetMasterRelease(int masterReleaseId, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(masterReleaseId, 0);
        return await GetMasterReleaseInternal(masterReleaseId, cancellationToken);
    }

    [HttpGet("/masters/{masterReleaseId}/versions")]
    internal Task<MasterReleaseVersionsResponse> GetMasterReleaseVersionsInternal(int masterReleaseId, PaginationQueryParameters? paginationQueryParameters = null, MasterReleaseVersionFilterQueryParameters? masterReleaseVersionQueryParameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the release versions of a master release from the Discogs database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    public async Task<MasterReleaseVersionsResponse> GetMasterReleaseVersions(int masterReleaseId, PaginationQueryParameters? paginationQueryParameters = null, MasterReleaseVersionFilterQueryParameters? masterReleaseVersionQueryParameters = null, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(masterReleaseId, 0);
        return await GetMasterReleaseVersionsInternal(masterReleaseId, paginationQueryParameters, masterReleaseVersionQueryParameters, cancellationToken);
    }


    [HttpGet("/releases/{releaseId}")]
    internal Task<Release> GetReleaseInternal(int releaseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    public async Task<Release> GetRelease(int releaseId, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(releaseId, 0);
        return await GetReleaseInternal(releaseId, cancellationToken);
    }

    [HttpGet("/releases/{releaseId}/rating")]
    internal Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRatingInternal(int releaseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the community rating of a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    public async Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRating(int releaseId, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(releaseId, 0);
        return await GetReleaseCommunityRatingInternal(releaseId, cancellationToken);
    }

    [HttpGet("/releases/{releaseId}/stats")]
    internal Task<ReleaseStatsResponse> GetReleaseStatsInternal(int releaseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets statistics of a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    public async Task<ReleaseStatsResponse> GetReleaseStats(int releaseId, CancellationToken cancellationToken = default)
    {
        Guard.IsGreaterThan(releaseId, 0);
        return await GetReleaseStatsInternal(releaseId, cancellationToken);
    }


    /// <summary>
    /// Queries the Discogs database for entries.
    /// </summary>
    /// <param name="searchQueryParameters">The search parameters to query for.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    [HttpGet("/database/search")]
    Task<SearchResultsResponse> SearchDatabase(SearchQueryParameters searchQueryParameters, PaginationQueryParameters? paginationQueryParameters = null, CancellationToken cancellationToken = default);
}
