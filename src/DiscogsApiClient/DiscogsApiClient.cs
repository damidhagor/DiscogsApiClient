using System.Net;
using System.Net.Http.Json;

namespace DiscogsApiClient;

/// <summary>
/// The Discogs api client for making requests to the Discogs api.
/// <para/>
/// It needs an <see cref="IAuthenticationProvider"/> and an initial call to <see cref="DiscogsApiClient.AuthenticateAsync"/>
/// with the corresponding <see cref="IAuthenticationRequest"/> to make the authenticated requests.
/// </summary>
public sealed partial class DiscogsApiClient : IDiscogsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationProvider _authenticationProvider;

    public bool IsAuthenticated => _authenticationProvider.IsAuthenticated;


    /// <summary>
    /// Creates a new <see cref="DiscogsApiClient"/> with injectable options.
    /// </summary>
    /// <param name="httpClient">The HttpClient to be used for the requests.</param>
    /// <param name="authenticationProvider">An implementation of the <see cref="IAuthenticationProvider"/> for the authentication method to be used.</param>
    public DiscogsApiClient(HttpClient httpClient, IAuthenticationProvider authenticationProvider)
    {
        _httpClient = httpClient;
        _authenticationProvider = authenticationProvider;
    }

    public async Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
        => await _authenticationProvider.AuthenticateAsync(authenticationRequest, cancellationToken);


    #region User
    public async Task<Identity> GetIdentityAsync(CancellationToken cancellationToken)
    {
        var url = DiscogsApiUrls.OAuthIdentityUrl;
        return await _httpClient.GetFromJsonAsync<Identity>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.UsersUrl, username);
        return await _httpClient.GetFromJsonAsync<User>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }
    #endregion


    #region Wantlist
    public async Task<WantlistReleasesResponse> GetWantlistReleasesAsync(string username, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.WantlistUrl, username)
            .AppendQueryParameters(paginationQueryParameters);
        return await _httpClient.GetFromJsonAsync<WantlistReleasesResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<WantlistRelease> AddReleaseToWantlistAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId);
        using var response = await _httpClient.PutAsJsonAsync(url, new { }, cancellationToken);
        return await response.Content.DeserializeAsJsonAsync<WantlistRelease>(cancellationToken);
    }

    public async Task<bool> DeleteReleaseFromWantlistAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId);
        using var response = await _httpClient.DeleteAsync(url, cancellationToken);
        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Collection
    public async Task<CollectionFoldersResponse> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFoldersUrl, username);
        return await _httpClient.GetFromJsonAsync<CollectionFoldersResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<CollectionFolder> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        return await _httpClient.GetFromJsonAsync<CollectionFolder>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<CollectionFolder> CreateCollectionFolderAsync(string username, string folderName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));
        if (string.IsNullOrWhiteSpace(folderName))
            throw new ArgumentException(ExceptionMessages.GetNoFolderNameProvidedMessage(), nameof(folderName));

        var url = string.Format(DiscogsApiUrls.CollectionFoldersUrl, username);
        var content = new { Name = folderName };
        using var response = await _httpClient.PostAsJsonAsync(url, content, cancellationToken);
        return await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);
    }

    public async Task<CollectionFolder> UpdateCollectionFolderAsync(string username, int folderId, string folderName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));
        if (string.IsNullOrWhiteSpace(folderName))
            throw new ArgumentException(ExceptionMessages.GetNoFolderNameProvidedMessage(), nameof(folderName));

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        var content = new { Name = folderName };
        using var response = await _httpClient.PostAsJsonAsync(url, content, cancellationToken);
        return await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);
    }

    public async Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        using var response = await _httpClient.DeleteAsync(url, cancellationToken);
        return response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<CollectionFolderReleasesResponse> GetCollectionFolderReleasesAsync(string username, int folderId, PaginationQueryParameters paginationQueryParameters, CollectionFolderReleaseSortQueryParameters collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderReleasesUrl, username, folderId)
            .AppendQueryParameters(paginationQueryParameters, collectionFolderReleaseSortQueryParameters);
        return await _httpClient.GetFromJsonAsync<CollectionFolderReleasesResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<CollectionFolderRelease> AddReleaseToCollectionFolderAsync(string username, int folderId, int releaseId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderAddReleaseUrl, username, folderId, releaseId);
        using var response = await _httpClient.PostAsJsonAsync(url, new { }, cancellationToken);
        return await response.Content.DeserializeAsJsonAsync<CollectionFolderRelease>(cancellationToken);
    }

    public async Task<bool> DeleteReleaseFromCollectionFolderAsync(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderDeleteReleaseUrl, username, folderId, releaseId, instanceId);
        using var response = await _httpClient.DeleteAsync(url, cancellationToken);
        return response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<CollectionValue> GetCollectionValueAsync(string username, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(ExceptionMessages.GetNoUsernameProvidedMessage(), nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionValueUrl, username);
        return await _httpClient.GetFromJsonAsync<CollectionValue>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }
    #endregion


    #region Database
    public async Task<Artist> GetArtistAsync(int artistId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ArtistsUrl, artistId);
        return await _httpClient.GetFromJsonAsync<Artist>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<ArtistReleasesResponse> GetArtistReleasesAsync(int artistId, PaginationQueryParameters paginationQueryParameters, ArtistReleaseSortQueryParameters artistReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ArtistReleasesUrl, artistId)
            .AppendQueryParameters(paginationQueryParameters, artistReleaseSortQueryParameters);
        return await _httpClient.GetFromJsonAsync<ArtistReleasesResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<MasterRelease> GetMasterReleaseAsync(int masterReleaseId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.MasterReleasesUrl, masterReleaseId);
        return await _httpClient.GetFromJsonAsync<MasterRelease>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<MasterReleaseVersionsResponse> GetMasterReleaseVersionsAsync(int masterReleaseId, PaginationQueryParameters paginationQueryParameters, MasterReleaseVersionFilterQueryParameters masterReleaseVersionQueryParameters, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.MasterReleaseVersionsUrl, masterReleaseId)
            .AppendQueryParameters(masterReleaseVersionQueryParameters, paginationQueryParameters);
        return await _httpClient.GetFromJsonAsync<MasterReleaseVersionsResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<Release> GetReleaseAsync(int releaseId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ReleasesUrl, releaseId);
        return await _httpClient.GetFromJsonAsync<Release>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRatingAsync(int releaseId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ReleaseCommunityRatingsUrl, releaseId);
        return await _httpClient.GetFromJsonAsync<ReleaseCommunityRatingResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<ReleaseStatsResponse> GetReleaseStatsAsync(int releaseId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ReleaseStatsUrl, releaseId);
        return await _httpClient.GetFromJsonAsync<ReleaseStatsResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<Label> GetLabelAsync(int labelId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.LabelsUrl, labelId);
        return await _httpClient.GetFromJsonAsync<Label>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<LabelReleasesResponse> GetLabelReleasesAsync(int labelId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.LabelReleasesUrl, labelId)
            .AppendQueryParameters(paginationQueryParameters);
        return await _httpClient.GetFromJsonAsync<LabelReleasesResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<SearchResultsResponse> SearchDatabaseAsync(SearchQueryParameters searchQueryParameters, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        var url = DiscogsApiUrls.SearchUrl
            .AppendQueryParameters(searchQueryParameters, paginationQueryParameters);
        return await _httpClient.GetFromJsonAsync<SearchResultsResponse>(url, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }
    #endregion
}
