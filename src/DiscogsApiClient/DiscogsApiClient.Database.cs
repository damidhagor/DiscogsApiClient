using DiscogsApiClient.Contract;
using DiscogsApiClient.Exceptions;
using DiscogsApiClient.QueryParameters;

namespace DiscogsApiClient;

public sealed partial class DiscogsApiClient
{
    /// <inheritdoc/>
    public async Task<Artist> GetArtistAsync(int artistId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = string.Format(DiscogsApiUrls.ArtistsUrl, artistId);
        return await GetAsync<Artist>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ArtistReleasesResponse> GetArtistReleasesAsync(int artistId, PaginationQueryParameters paginationQueryParameters, ArtistReleaseSortQueryParameters artistReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = string.Format(DiscogsApiUrls.ArtistReleasesUrl, artistId)
            .AppendQueryParameters(paginationQueryParameters, artistReleaseSortQueryParameters);
        return await GetAsync<ArtistReleasesResponse>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<MasterRelease> GetMasterReleaseAsync(int masterReleaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = string.Format(DiscogsApiUrls.MasterReleasesUrl, masterReleaseId);
        return await GetAsync<MasterRelease>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<MasterReleaseVersionsResponse> GetMasterReleaseVersionsAsync(int masterReleaseId, PaginationQueryParameters paginationQueryParameters, MasterReleaseVersionFilterQueryParameters masterReleaseVersionQueryParameters, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        string url = string.Format(DiscogsApiUrls.MasterReleaseVersionsUrl, masterReleaseId)
            .AppendQueryParameters(masterReleaseVersionQueryParameters, paginationQueryParameters);
        return await GetAsync<MasterReleaseVersionsResponse>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Release> GetReleaseAsync(int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = string.Format(DiscogsApiUrls.ReleasesUrl, releaseId);
        return await GetAsync<Release>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRatingAsync(int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = string.Format(DiscogsApiUrls.ReleaseCommunityRatingsUrl, releaseId);
        return await GetAsync<ReleaseCommunityRatingResponse>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ReleaseStatsResponse> GetReleaseStatsAsync(int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = string.Format(DiscogsApiUrls.ReleaseStatsUrl, releaseId);
        return await GetAsync<ReleaseStatsResponse>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Label> GetLabelAsync(int labelId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = string.Format(DiscogsApiUrls.LabelsUrl, labelId);
        return await GetAsync<Label>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<LabelReleasesResponse> GetLabelReleasesAsync(int labelId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = string.Format(DiscogsApiUrls.LabelReleasesUrl, labelId)
            .AppendQueryParameters(paginationQueryParameters);
        return await GetAsync<LabelReleasesResponse>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<SearchResultsResponse> SearchDatabaseAsync(SearchQueryParameters searchQueryParameters, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = DiscogsApiUrls.SearchUrl
            .AppendQueryParameters(searchQueryParameters, paginationQueryParameters);
        return await GetAsync<SearchResultsResponse>(url, cancellationToken);
    }
}
