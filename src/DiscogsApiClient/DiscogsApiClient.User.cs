using DiscogsApiClient.Contract;
using DiscogsApiClient.Exceptions;

namespace DiscogsApiClient;

public sealed partial class DiscogsApiClient
{
    /// <inheritdoc/>
    public async Task<Identity> GetIdentityAsync(CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();

        var url = DiscogsApiUrls.OAuthIdentityUrl;
        return await GetAsync<Identity>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.UsersUrl, username);
        return await GetAsync<User>(url, cancellationToken);
    }
}
