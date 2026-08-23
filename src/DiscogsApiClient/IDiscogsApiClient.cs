namespace DiscogsApiClient;

/// <summary>
/// A client for accessing the Discogs API.
/// </summary>
public interface IDiscogsApiClient
{
    /// <summary>
    /// Queries the <see cref="Identity"/> of the currently authenticated user.
    /// </summary>
    Task<Identity> GetIdentity(CancellationToken cancellationToken);

    /// <summary>
    /// Queries the <see cref="User"/> object of the currently authenticated user.
    /// </summary>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<User> GetUser(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Edits the profile of a user. Authentication as that user is required.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="editUserProfileRequest">The profile fields to change. Omitted (<see langword="null"/>) properties are left unchanged.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<User> UpdateUser(string username, UserProfileEditRequest editUserProfileRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the collection folders of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<CollectionFoldersResponse> GetCollectionFolders(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a specific collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The id of the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    Task<CollectionFolder> GetCollectionFolder(string username, int folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderName">The new folder's name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    Task<CollectionFolder> CreateCollectionFolder(string username, string folderName, CancellationToken cancellationToken);

    /// <summary>
    /// Changes a collection folder's name for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="folderName">The new folder name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    Task<CollectionFolder> UpdateCollectionFolder(string username, int folderId, string folderName, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    Task DeleteCollectionFolder(string username, int folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases in a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    Task<CollectionFolderReleasesResponse> GetCollectionFolderReleases(string username, int folderId, PaginationQueryParameters? paginationQueryParameters, CollectionFolderReleaseSortQueryParameters? collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a release to the collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder or release id is invalid.</exception>
    Task<CollectionFolderRelease> AddReleaseToCollectionFolder(string username, int folderId, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a release from a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="instanceId">The release's instance id in the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder, release or instance id is invalid.</exception>
    Task DeleteReleaseFromCollectionFolder(string username, int folderId, int releaseId, long instanceId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the estimated value of the user's collection.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<CollectionValue> GetCollectionValue(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases on the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<WantlistReleasesResponse> GetWantlistReleases(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a release to the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<WantlistRelease> AddReleaseToWantlist(string username, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a release from the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task DeleteReleaseFromWantlist(string username, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an artist from the Discogs database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the artist id is invalid.</exception>
    Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases of an artist from the Discogs database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the artist id is invalid.</exception>
    Task<ArtistReleasesResponse> GetArtistReleases(int artistId, PaginationQueryParameters? paginationQueryParameters, ArtistReleaseSortQueryParameters? artistReleaseSortQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a label from the Discogs database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the label id is invalid.</exception>
    Task<Label> GetLabel(int labelId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases of a label from the Discogs database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the label id is invalid.</exception>
    Task<LabelReleasesResponse> GetLabelReleases(int labelId, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a master release from the Discogs database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<MasterRelease> GetMasterRelease(int masterReleaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the release versions of a master release from the Discogs database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<MasterReleaseVersionsResponse> GetMasterReleaseVersions(int masterReleaseId, PaginationQueryParameters? paginationQueryParameters, MasterReleaseVersionFilterQueryParameters? masterReleaseVersionQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<Release> GetRelease(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the community rating of a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRating(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets statistics of a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<ReleaseStatsResponse> GetReleaseStats(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Queries the Discogs database for entries.
    /// </summary>
    /// <param name="searchQueryParameters">The search parameters to query for.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    Task<SearchResultsResponse> SearchDatabase(SearchQueryParameters searchQueryParameters, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);
}
