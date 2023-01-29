using DiscogsApiClient.Contract;
using DiscogsApiClient.Exceptions;
using DiscogsApiClient.QueryParameters;

namespace DiscogsApiClient;

public sealed partial class DiscogsApiClient
{
    /// <inheritdoc/>
    public async Task<WantlistReleasesResponse> GetWantlistReleasesAsync(string username, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.WantlistUrl, username)
            .AppendQueryParameters(paginationQueryParameters);
        return await GetAsync<WantlistReleasesResponse>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<WantlistRelease> AddReleaseToWantlistAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId);
        return await PutAsync<WantlistRelease>(url, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteReleaseFromWantlistAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId);
        return await DeleteAsync(url, cancellationToken);
    }
}
