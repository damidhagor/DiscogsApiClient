using DiscogsApiClient.Authentication;
using DiscogsApiClient.Contract;
using DiscogsApiClient.QueryParameters;

namespace DiscogsApiClient;

public interface IDiscogsApiClient
{
    /// <summary>
    /// Indicates if the <see cref="DiscogsApiClient"/> has been authenticated and is ready to use.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Authenticates the <see cref="DiscogsApiClient"/> against the Discogs api by using the chosen <see cref="IAuthenticationProvider"/>.
    /// </summary>
    /// <param name="authenticationRequest">The <see cref="IAuthenticationRequest"/> implementation corresponding to the chosen <see cref="IAuthenticationProvider"/>.</param>
    /// <returns>The <see cref="IAuthenticationResponse"/> implementation corresponding to the chosen <see cref="IAuthenticationProvider"/>.</returns>
    Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken);


    /// <summary>
    /// Queries the <see cref="Identity"/> of the currently authenticated user.
    /// </summary>
    Task<Identity> GetIdentityAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Queries the <see cref="User"/> object of the currently authenticated user.
    /// </summary>
    Task<User> GetUserAsync(string username, CancellationToken cancellationToken);


    /// <summary>
    /// Gets the collection folders of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<List<CollectionFolder>> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a specific collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The id of the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<CollectionFolder> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderName">The new folder's name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    Task<CollectionFolder> CreateCollectionFolderAsync(string username, string folderName, CancellationToken cancellationToken);

    /// <summary>
    /// Changes a collection folder's name for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="folderName">The new folder name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    Task<CollectionFolder> UpdateCollectionFolderAsync(string username, int folderId, string folderName, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases in a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<CollectionFolderReleasesResponse> GetCollectionFolderReleasesByFolderIdAsync(string username, int folderId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a release to the collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<CollectionFolderRelease> AddReleaseToCollectionFolderAsync(string username, int folderId, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a release from a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="instanceId">The release's instance id in the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<bool> DeleteReleaseFromCollectionFolderAsync(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken);


    /// <summary>
    /// Gets the releases on the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<WantlistReleasesResponse> GetWantlistReleasesAsync(string username, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a release to the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<WantlistRelease> AddWantlistReleaseAsync(string username, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a release from the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<bool> DeleteWantlistReleaseAsync(string username, int releaseId, CancellationToken cancellationToken);


    /// <summary>
    /// Gets an artist from the Discog database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    Task<Artist> GetArtistAsync(int artistId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases of an artist from the Discog database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    Task<ArtistReleasesResponse> GetArtistReleasesAsync(int artistId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a label from the Discog database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    Task<Label> GetLabelAsync(int labelId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases of a label from the Discog database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    Task<LabelReleasesResponse> GetLabelReleasesAsync(int labelId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a master release from the Discog database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    Task<MasterRelease> GetMasterReleaseAsync(int masterReleaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the release versions of a master release from the Discog database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    Task<MasterReleaseVersionsResponse> GetMasterReleaseVersionsAsync(int masterReleaseId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a release from the Discog database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    Task<Release> GetReleaseAsync(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the community rating of a release from the Discog database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRatingAsync(int releaseId, CancellationToken cancellationToken);


    /// <summary>
    /// Queries the Discogs database for entries.
    /// </summary>
    /// <param name="searchQueryParameters">The search parameters to query for.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    Task<SearchResultsResponse> SearchDatabaseAsync(SearchQueryParameters searchQueryParameters, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken);
}