namespace DiscogsApiClient;

public interface IDiscogsApiClient
{
    /// <summary>
    /// Queries the <see cref="Identity"/> of the currently authenticated user.
    /// </summary>
    [Get("/oauth/identity")]
    Task<Identity> GetIdentity(CancellationToken cancellationToken);

    [Get("/users/{username}")]
    internal Task<User> GetUserInternal(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Queries the <see cref="User"/> object of the currently authenticated user.
    /// </summary>
    public async Task<User> GetUser(string username, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        return await GetUserInternal(username, cancellationToken);
    }


    [Get("/users/{username}/collection/folders")]
    internal Task<CollectionFoldersResponse> GetCollectionFoldersInternal(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the collection folders of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionFoldersResponse> GetCollectionFolders(string username, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        return await GetCollectionFoldersInternal(username, cancellationToken);
    }

    [Get("/users/{username}/collection/folders/{folderId}")]
    internal Task<CollectionFolder> GetCollectionFolderInternal(string username, int folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a specific collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The id of the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionFolder> GetCollectionFolder(string username, int folderId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThanOrEqualTo(folderId, 0);
        return await GetCollectionFolderInternal(username, folderId, cancellationToken);
    }

    [Post("/users/{username}/collection/folders")]
    internal Task<CollectionFolder> CreateCollectionFolderInternal(string username, [Body] object folderNameObject, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderName">The new folder's name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    public async Task<CollectionFolder> CreateCollectionFolder(string username, string folderName, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(folderName);
        return await CreateCollectionFolderInternal(username, new { Name = folderName }, cancellationToken);
    }

    [Post("/users/{username}/collection/folders/{folderId}")]
    internal Task<CollectionFolder> UpdateCollectionFolderInternal(string username, int folderId, [Body] object folderNameObject, CancellationToken cancellationToken);

    /// <summary>
    /// Changes a collection folder's name for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="folderName">The new folder name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    public async Task<CollectionFolder> UpdateCollectionFolder(string username, int folderId, string folderName, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(folderName);
        Guard.IsGreaterThan(folderId, 1);
        return await UpdateCollectionFolderInternal(username, folderId, new { Name = folderName }, cancellationToken);
    }

    [Delete("/users/{username}/collection/folders/{folderId}")]
    internal Task DeleteCollectionFolderInternal(string username, int folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task DeleteCollectionFolder(string username, int folderId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThan(folderId, 1);
        await DeleteCollectionFolderInternal(username, folderId, cancellationToken);
    }

    [Get("/users/{username}/collection/folders/{folderId}/releases")]
    internal Task<CollectionFolderReleasesResponse> GetCollectionFolderReleasesInternal(string username, int folderId, PaginationQueryParameters paginationQueryParameters, CollectionFolderReleaseSortQueryParameters collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases in a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionFolderReleasesResponse> GetCollectionFolderReleases(string username, int folderId, PaginationQueryParameters paginationQueryParameters, CollectionFolderReleaseSortQueryParameters collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThanOrEqualTo(folderId, 0);
        return await GetCollectionFolderReleasesInternal(username, folderId, paginationQueryParameters, collectionFolderReleaseSortQueryParameters, cancellationToken);
    }

    [Post("/users/{username}/collection/folders/{folderId}/releases/{releaseId}")]
    internal Task<CollectionFolderRelease> AddReleaseToCollectionFolderInternal(string username, int folderId, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a release to the collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionFolderRelease> AddReleaseToCollectionFolder(string username, int folderId, int releaseId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThanOrEqualTo(folderId, 1);
        Guard.IsGreaterThanOrEqualTo(releaseId, 1);
        return await AddReleaseToCollectionFolderInternal(username, folderId, releaseId, cancellationToken);
    }

    [Delete("/users/{username}/collection/folders/{folderId}/releases/{releaseId}/instances/{instanceId}")]
    internal Task DeleteReleaseFromCollectionFolderInternal(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a release from a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="instanceId">The release's instance id in the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task DeleteReleaseFromCollectionFolder(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThanOrEqualTo(folderId, 1);
        Guard.IsGreaterThanOrEqualTo(releaseId, 1);
        Guard.IsGreaterThanOrEqualTo(instanceId, 1);
        await DeleteReleaseFromCollectionFolderInternal(username, folderId, releaseId, instanceId, cancellationToken);
    }

    [Get("/users/{username}/collection/value")]
    internal Task<CollectionValue> GetCollectionValueInternal(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the estimated value of the user's collection.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionValue> GetCollectionValue(string username, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        return await GetCollectionValueInternal(username, cancellationToken);
    }


    [Get("/users/{username}/wants")]
    internal Task<WantlistReleasesResponse> GetWantlistReleasesInternal(string username, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases on the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<WantlistReleasesResponse> GetWantlistReleases(string username, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        return await GetWantlistReleasesInternal(username, paginationQueryParameters, cancellationToken);
    }

    [Put("/users/{username}/wants/{releaseId}")]
    internal Task<WantlistRelease> AddReleaseToWantlistInternal(string username, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a release to the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<WantlistRelease> AddReleaseToWantlist(string username, int releaseId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThan(releaseId, 0);
        return await AddReleaseToWantlistInternal(username, releaseId, cancellationToken);
    }

    [Delete("/users/{username}/wants/{releaseId}")]
    internal Task DeleteReleaseFromWantlistInternal(string username, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a release from the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task DeleteReleaseFromWantlist(string username, int releaseId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsGreaterThan(releaseId, 0);
        await DeleteReleaseFromWantlistInternal(username, releaseId, cancellationToken);
    }


    [Get("/artists/{artistId}")]
    internal Task<Artist> GetArtistInternal(int artistId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an artist from the Discog database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    public async Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken)
    {
        Guard.IsGreaterThan(artistId, 0);
        return await GetArtistInternal(artistId, cancellationToken);
    }

    [Get("/artists/{artistId}/releases")]
    internal Task<ArtistReleasesResponse> GetArtistReleasesInternal(int artistId, PaginationQueryParameters paginationQueryParameters, ArtistReleaseSortQueryParameters artistReleaseSortQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases of an artist from the Discog database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    async Task<ArtistReleasesResponse> GetArtistReleases(int artistId, PaginationQueryParameters paginationQueryParameters, ArtistReleaseSortQueryParameters artistReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        Guard.IsGreaterThan(artistId, 0);
        return await GetArtistReleasesInternal(artistId, paginationQueryParameters, artistReleaseSortQueryParameters, cancellationToken);
    }

    /// <summary>
    /// Gets a label from the Discog database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    [Get("/labels/{labelId}")]
    Task<Label> GetLabel(int labelId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases of a label from the Discog database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    [Get("/labels/{labelId}/releases")]
    Task<LabelReleasesResponse> GetLabelReleases(int labelId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a master release from the Discog database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    [Get("/masters/{masterReleaseId}")]
    Task<MasterRelease> GetMasterRelease(int masterReleaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the release versions of a master release from the Discog database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    [Get("/masters/{masterReleaseId}/versions")]
    Task<MasterReleaseVersionsResponse> GetMasterReleaseVersions(int masterReleaseId, PaginationQueryParameters paginationQueryParameters, MasterReleaseVersionFilterQueryParameters masterReleaseVersionQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a release from the Discog database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    [Get("/releases/{releaseId}")]
    Task<Release> GetRelease(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the community rating of a release from the Discog database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    [Get("/releases/{releaseId}/rating")]
    Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRating(int releaseId, CancellationToken cancellationToken);

    [Get("/releases/{releaseId}/stats")]
    Task<ReleaseStatsResponse> GetReleaseStats(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Queries the Discogs database for entries.
    /// </summary>
    /// <param name="searchQueryParameters">The search parameters to query for.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    [Get("/database/search")]
    Task<SearchResultsResponse> SearchDatabase(SearchQueryParameters searchQueryParameters, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);
}
