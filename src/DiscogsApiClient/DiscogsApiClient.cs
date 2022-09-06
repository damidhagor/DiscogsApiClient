using System.Net;
using DiscogsApiClient.Authentication;
using DiscogsApiClient.Contract;
using DiscogsApiClient.Exceptions;
using DiscogsApiClient.QueryParameters;
using DiscogsApiClient.Serialization;

namespace DiscogsApiClient;

/// <summary>
/// The Discogs api client for making requests to the Discogs api.
/// <para/>
/// It needs an <see cref="IAuthenticationProvider"/> and an initial call to <see cref="DiscogsApiClient.AuthenticateAsync"/>
/// with the corresponding <see cref="IAuthenticationRequest"/> to make the authenticated requests.
/// </summary>
public class DiscogsApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationProvider _authenticationProvider;
    private readonly string _userAgent;

    /// <summary>
    /// Indicates if the <see cref="DiscogsApiClient"/> has been authenticated and is ready to use.
    /// </summary>
    public bool IsAuthenticated => _authenticationProvider.IsAuthenticated;

    /// <summary>
    /// Creates a new <see cref="DiscogsApiClient"/>.
    /// </summary>
    /// <param name="authenticationProvider">An implementation of the <see cref="IAuthenticationProvider"/> for the authentication method to be used.</param>
    /// <param name="userAgent">The user agent string used by the client to identify itself to the Discogs api.</param>
    public DiscogsApiClient(IAuthenticationProvider authenticationProvider, string userAgent)
    {
        _userAgent = userAgent;

        _authenticationProvider = authenticationProvider;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
    }


    /// <summary>
    /// Authenticates the <see cref="DiscogsApiClient"/> against the Discogs api by using the chosen <see cref="IAuthenticationProvider"/>.
    /// </summary>
    /// <param name="authenticationRequest">The <see cref="IAuthenticationRequest"/> implementation corresponding to the chosen <see cref="IAuthenticationProvider"/>.</param>
    /// <returns>The <see cref="IAuthenticationResponse"/> implementation corresponding to the chosen <see cref="IAuthenticationProvider"/>.</returns>
    public async Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
    {
        return await _authenticationProvider.AuthenticateAsync(authenticationRequest, cancellationToken);
    }



    #region User
    /// <summary>
    /// Queries the <see cref="Identity"/> of the currently authenticated user.
    /// </summary>
    public async Task<Identity> GetIdentityAsync(CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, DiscogsApiUrls.OAuthIdentityUrl);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var identity = await response.Content.DeserializeAsJsonAsync<Identity>(cancellationToken);

        return identity;
    }

    /// <summary>
    /// Queries the <see cref="User"/> object of the currently authenticated user.
    /// </summary>
    public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.UsersUrl, username));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var user = await response.Content.DeserializeAsJsonAsync<User>(cancellationToken);

        return user;
    }
    #endregion


    #region Collection Folders
    /// <summary>
    /// Gets the collection folders of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<List<CollectionFolder>> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.CollectionFoldersUrl, username));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFoldersResponse = await response.Content.DeserializeAsJsonAsync<CollectionFoldersResponse>(cancellationToken);

        return collectionFoldersResponse.Folders;
    }

    /// <summary>
    /// Gets a specific collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The id of the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionFolder> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, $"{String.Format(DiscogsApiUrls.CollectionFoldersUrl, username)}/{folderId}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    /// <summary>
    /// Creates a new collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderName">The new folder's name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    public async Task<CollectionFolder> CreateCollectionFolderAsync(string username, string folderName, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (String.IsNullOrWhiteSpace(folderName))
            throw new ArgumentException(nameof(folderName));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Post, String.Format(DiscogsApiUrls.CollectionFoldersUrl, username));

        request.Content = CreateJsonContent(new { Name = folderName });

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    /// <summary>
    /// Changes a collection folder's name for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="folderName">The new folder name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    public async Task<CollectionFolder> UpdateCollectionFolderAsync(string username, int folderId, string folderName, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (String.IsNullOrWhiteSpace(folderName))
            throw new ArgumentException(nameof(folderName));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Post, $"{String.Format(DiscogsApiUrls.CollectionFoldersUrl, username)}/{folderId}");

        request.Content = CreateJsonContent(new { Name = folderName });

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    /// <summary>
    /// Deletes a collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Delete, $"{String.Format(DiscogsApiUrls.CollectionFoldersUrl, username)}/{folderId}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Collection Items
    /// <summary>
    /// Gets the releases in a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionFolderReleasesResponse> GetCollectionFolderReleasesByFolderIdAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.CollectionFolderReleasesUrl, username, folderId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolderReleasesResponse = await response.Content.DeserializeAsJsonAsync<CollectionFolderReleasesResponse>(cancellationToken);

        return collectionFolderReleasesResponse;
    }

    /// <summary>
    /// Adds a release to the collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<CollectionFolderRelease> AddReleaseToCollectionFolderAsync(string username, int folderId, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Post, String.Format(DiscogsApiUrls.CollectionFolderAddReleaseUrl, username, folderId, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolderRelease = await response.Content.DeserializeAsJsonAsync<CollectionFolderRelease>(cancellationToken);

        return collectionFolderRelease;
    }

    /// <summary>
    /// Removes a release from a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="instanceId">The release's instance id in the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<bool> DeleteReleaseFromCollectionFolderAsync(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Delete, String.Format(DiscogsApiUrls.CollectionFolderDeleteReleaseUrl, username, folderId, releaseId, instanceId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Wantlist
    /// <summary>
    /// Gets the releases on the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<WantlistReleasesResponse> GetWantlistReleasesAsync(string username, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        string url = QueryParameterHelper.AppendPaginationQuery(String.Format(DiscogsApiUrls.WantlistUrl, username), paginationQueryParameters);

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var releasesResponse = await response.Content.DeserializeAsJsonAsync<WantlistReleasesResponse>(cancellationToken);

        return releasesResponse;
    }

    /// <summary>
    /// Adds a release to the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<WantlistRelease> AddWantlistReleaseAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Put, String.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var release = await response.Content.DeserializeAsJsonAsync<WantlistRelease>(cancellationToken);

        return release;
    }

    /// <summary>
    /// Removes a release from the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    public async Task<bool> DeleteWantlistReleaseAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Delete, String.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Database
    /// <summary>
    /// Gets an artist from the Discog database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    public async Task<Artist> GetArtistAsync(int artistId, CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.ArtistsUrl, artistId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var artist = await response.Content.DeserializeAsJsonAsync<Artist>(cancellationToken);

        return artist;
    }

    /// <summary>
    /// Gets the releases of an artist from the Discog database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    public async Task<ArtistReleasesResponse> GetArtistReleasesAsync(int artistId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        string url = QueryParameterHelper.AppendPaginationQuery(String.Format(DiscogsApiUrls.ArtistReleasesUrl, artistId), paginationQueryParameters);

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var releasesResponse = await response.Content.DeserializeAsJsonAsync<ArtistReleasesResponse>(cancellationToken);

        return releasesResponse;
    }

    /// <summary>
    /// Gets a master release from the Discog database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    public async Task<MasterRelease> GetMasterReleaseAsync(int masterReleaseId, CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.MasterReleasesUrl, masterReleaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var masterRelease = await response.Content.DeserializeAsJsonAsync<MasterRelease>(cancellationToken);

        return masterRelease;
    }

    /// <summary>
    /// Gets a release from the Discog database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    public async Task<Release> GetReleaseAsync(int releaseId, CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.ReleasesUrl, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var release = await response.Content.DeserializeAsJsonAsync<Release>(cancellationToken);

        return release;
    }

    /// <summary>
    /// Gets the community rating of a release from the Discog database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    public async Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRatingAsync(int releaseId, CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.ReleaseCommunityRatingsUrl, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var rating = await response.Content.DeserializeAsJsonAsync<ReleaseCommunityRatingResponse>(cancellationToken);

        return rating;
    }

    /// <summary>
    /// Gets a label from the Discog database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    public async Task<Label> GetLabelAsync(int labelId, CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.LabelsUrl, labelId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var label = await response.Content.DeserializeAsJsonAsync<Label>(cancellationToken);

        return label;
    }

    /// <summary>
    /// Gets the releases of a label from the Discog database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    public async Task<LabelReleasesResponse> GetLabelReleasesAsync(int labelId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        string url = QueryParameterHelper.AppendPaginationQuery(String.Format(DiscogsApiUrls.LabelReleasesUrl, labelId), paginationQueryParameters);

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var releasesResponse = await response.Content.DeserializeAsJsonAsync<LabelReleasesResponse>(cancellationToken);

        return releasesResponse;
    }

    /// <summary>
    /// Queries the Discogs database for entries.
    /// </summary>
    /// <param name="searchQueryParameters">The search parameters to query for.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    public async Task<SearchResultsResponse> SearchDatabaseAsync(SearchQueryParameters searchQueryParameters, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        string url = QueryParameterHelper.AppendSearchQueryWithPagination(DiscogsApiUrls.SearchUrl, searchQueryParameters, paginationQueryParameters);

        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var content = await response.Content.ReadAsStringAsync();
        var searchResultsResponse = await response.Content.DeserializeAsJsonAsync<SearchResultsResponse>(cancellationToken);

        return searchResultsResponse;
    }
    #endregion


    public void Dispose()
    {
        _httpClient?.Dispose();
    }


    /// <summary>
    /// Helper method to create a Json payload for a request.
    /// </summary>
    /// <typeparam name="T">The type of the serialized payload.</typeparam>
    /// <param name="payload">The payload to serialize.</param>
    /// <returns>the serialized payload in a <see cref="StringContent"/>.</returns>
    private StringContent CreateJsonContent<T>(T payload)
    {
        string json = payload.SerializeAsJson<T>();

        var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        return stringContent;
    }
}
