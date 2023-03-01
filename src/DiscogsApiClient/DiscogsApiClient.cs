using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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
    private readonly JsonSerializerOptions _jsonSerializerOptions;
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
        _jsonSerializerOptions = DiscogsSerializerOptions.Options;
    }

    public async Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
        => await _authenticationProvider.AuthenticateAsync(authenticationRequest, cancellationToken);


    #region User
    public async Task<Identity> GetIdentityAsync(CancellationToken cancellationToken)
    {
        var url = DiscogsApiUrls.OAuthIdentityUrl;
        return await _httpClient.GetFromJsonAsync<Identity>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.UsersUrl, username);
        return await _httpClient.GetFromJsonAsync<User>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }
    #endregion


    #region Wantlist
    public async Task<WantlistReleasesResponse> GetWantlistReleasesAsync(string username, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.WantlistUrl, username)
            .AppendQueryParameters(paginationQueryParameters);
        return await _httpClient.GetFromJsonAsync<WantlistReleasesResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<WantlistRelease> AddReleaseToWantlistAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId);
        using var response = await _httpClient.PutAsJsonAsync(url, new { }, _jsonSerializerOptions, cancellationToken);
        return await response.Content.DeserializeAsJsonAsync<WantlistRelease>(cancellationToken);
    }

    public async Task<bool> DeleteReleaseFromWantlistAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId);
        using var response = await _httpClient.DeleteAsync(url, cancellationToken);
        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Collection
    public async Task<CollectionFoldersResponse> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.CollectionFoldersUrl, username);
        return await _httpClient.GetFromJsonAsync<CollectionFoldersResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<CollectionFolder> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        return await _httpClient.GetFromJsonAsync<CollectionFolder>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<CollectionFolder> CreateCollectionFolderAsync(string username, string folderName, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(folderName);

        var url = string.Format(DiscogsApiUrls.CollectionFoldersUrl, username);
        var content = new { Name = folderName };
        using var response = await _httpClient.PostAsJsonAsync(url, content, _jsonSerializerOptions, cancellationToken);
        return await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);
    }

    public async Task<CollectionFolder> UpdateCollectionFolderAsync(string username, int folderId, string folderName, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);
        Guard.IsNotNullOrWhiteSpace(folderName);

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        var content = new { Name = folderName };
        using var response = await _httpClient.PostAsJsonAsync(url, content, _jsonSerializerOptions, cancellationToken);
        return await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);
    }

    public async Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        using var response = await _httpClient.DeleteAsync(url, cancellationToken);
        return response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<CollectionFolderReleasesResponse> GetCollectionFolderReleasesAsync(string username, int folderId, PaginationQueryParameters paginationQueryParameters, CollectionFolderReleaseSortQueryParameters collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.CollectionFolderReleasesUrl, username, folderId)
            .AppendQueryParameters(paginationQueryParameters, collectionFolderReleaseSortQueryParameters);
        return await _httpClient.GetFromJsonAsync<CollectionFolderReleasesResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<CollectionFolderRelease> AddReleaseToCollectionFolderAsync(string username, int folderId, int releaseId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.CollectionFolderAddReleaseUrl, username, folderId, releaseId);
        using var response = await _httpClient.PostAsJsonAsync(url, new { }, _jsonSerializerOptions, cancellationToken);
        return await response.Content.DeserializeAsJsonAsync<CollectionFolderRelease>(cancellationToken);
    }

    public async Task<bool> DeleteReleaseFromCollectionFolderAsync(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.CollectionFolderDeleteReleaseUrl, username, folderId, releaseId, instanceId);
        using var response = await _httpClient.DeleteAsync(url, cancellationToken);
        return response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<CollectionValue> GetCollectionValueAsync(string username, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(username);

        var url = string.Format(DiscogsApiUrls.CollectionValueUrl, username);
        return await _httpClient.GetFromJsonAsync<CollectionValue>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }
    #endregion


    #region Database
    public async Task<Artist> GetArtistAsync(int artistId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ArtistsUrl, artistId);
        return await _httpClient.GetFromJsonAsync<Artist>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<ArtistReleasesResponse> GetArtistReleasesAsync(int artistId, PaginationQueryParameters paginationQueryParameters, ArtistReleaseSortQueryParameters artistReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ArtistReleasesUrl, artistId)
            .AppendQueryParameters(paginationQueryParameters, artistReleaseSortQueryParameters);
        return await _httpClient.GetFromJsonAsync<ArtistReleasesResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<MasterRelease> GetMasterReleaseAsync(int masterReleaseId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.MasterReleasesUrl, masterReleaseId);
        return await _httpClient.GetFromJsonAsync<MasterRelease>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<MasterReleaseVersionsResponse> GetMasterReleaseVersionsAsync(int masterReleaseId, PaginationQueryParameters paginationQueryParameters, MasterReleaseVersionFilterQueryParameters masterReleaseVersionQueryParameters, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.MasterReleaseVersionsUrl, masterReleaseId)
            .AppendQueryParameters(masterReleaseVersionQueryParameters, paginationQueryParameters);
        return await _httpClient.GetFromJsonAsync<MasterReleaseVersionsResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<Release> GetReleaseAsync(int releaseId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ReleasesUrl, releaseId);
        return await _httpClient.GetFromJsonAsync<Release>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRatingAsync(int releaseId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ReleaseCommunityRatingsUrl, releaseId);
        return await _httpClient.GetFromJsonAsync<ReleaseCommunityRatingResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<ReleaseStatsResponse> GetReleaseStatsAsync(int releaseId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.ReleaseStatsUrl, releaseId);
        return await _httpClient.GetFromJsonAsync<ReleaseStatsResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<Label> GetLabelAsync(int labelId, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.LabelsUrl, labelId);
        return await _httpClient.GetFromJsonAsync<Label>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<LabelReleasesResponse> GetLabelReleasesAsync(int labelId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        var url = string.Format(DiscogsApiUrls.LabelReleasesUrl, labelId)
            .AppendQueryParameters(paginationQueryParameters);
        return await _httpClient.GetFromJsonAsync<LabelReleasesResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }

    public async Task<SearchResultsResponse> SearchDatabaseAsync(SearchQueryParameters searchQueryParameters, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        var url = DiscogsApiUrls.SearchUrl
            .AppendQueryParameters(searchQueryParameters, paginationQueryParameters);
        return await _httpClient.GetFromJsonAsync<SearchResultsResponse>(url, _jsonSerializerOptions, cancellationToken)
            ?? throw new DiscogsException(ExceptionMessages.GetRequestNotDeserializedMessage());
    }
    #endregion
}
