using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Authentication;

/// <summary>
/// A service which enables authentication against the Discogs Api with one of the supported authentication flows.
/// <para/>
/// Currently supported are
/// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Personal Access Tokens</see>
/// and
/// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see>.
/// </summary>
internal sealed class DiscogsAuthenticationService : IDiscogsAuthenticationService
{
    private readonly IPersonalAccessTokenAuthenticationProvider _personalAccessTokenAuthenticationProvider;
    private readonly IOAuthAuthenticationProvider _oAuthAuthenticationProvider;
    private bool _lastAuthenticatedWithPersonalAccessToken = false;
    private bool _lastAuthenticatedWithOauth = false;

    public bool IsAuthenticated => _personalAccessTokenAuthenticationProvider.IsAuthenticated || _oAuthAuthenticationProvider.IsAuthenticated;

    public DiscogsAuthenticationService(
        IPersonalAccessTokenAuthenticationProvider personalAccessTokenAuthenticationProvider,
        IOAuthAuthenticationProvider oAuthAuthenticationProvider)
    {
        _personalAccessTokenAuthenticationProvider = personalAccessTokenAuthenticationProvider;
        _oAuthAuthenticationProvider = oAuthAuthenticationProvider;
    }

    public void AuthenticateWithPersonalAccessToken(string token)
    {
        _lastAuthenticatedWithPersonalAccessToken = false;

        _personalAccessTokenAuthenticationProvider.Authenticate(token);

        _lastAuthenticatedWithPersonalAccessToken = true;
        _lastAuthenticatedWithOauth = false;
    }

    /// <inheritdoc />
    /// <exception cref="AuthenticationFailedDiscogsException" />
    public async Task<(string accessToken, string accessTokenSecret)> AuthenticateWithOAuth(
        string consumerKey,
        string consumerSecret,
        string? accessToken,
        string? accessTokenSecret,
        string verifierCallbackUrl,
        GetVerifierCallback getVerifierCallback,
        CancellationToken cancellationToken)
    {
        _lastAuthenticatedWithOauth = false;

        var tokens = await _oAuthAuthenticationProvider.Authenticate(
            consumerKey,
            consumerSecret,
            accessToken,
            accessTokenSecret,
            verifierCallbackUrl,
            getVerifierCallback,
            cancellationToken)
            .ConfigureAwait(false);

        _lastAuthenticatedWithOauth = true;
        _lastAuthenticatedWithPersonalAccessToken = false;

        return tokens;
    }

    /// <inheritdoc/>
    /// <exception cref="UnauthenticatedDiscogsException"></exception>
    public string CreateAuthenticationHeader()
    {
        if (_lastAuthenticatedWithPersonalAccessToken
            && _personalAccessTokenAuthenticationProvider.IsAuthenticated)
            return _personalAccessTokenAuthenticationProvider.CreateAuthenticationHeader();

        if (_lastAuthenticatedWithOauth
            && _oAuthAuthenticationProvider.IsAuthenticated)
            return _oAuthAuthenticationProvider.CreateAuthenticationHeader();

        throw new UnauthenticatedDiscogsException("The authentication provider is not authenticated with any of the available authentication flows.");
    }
}
