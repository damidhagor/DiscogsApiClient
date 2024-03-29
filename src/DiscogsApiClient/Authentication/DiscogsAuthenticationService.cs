﻿using DiscogsApiClient.Authentication.OAuth;
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
public sealed class DiscogsAuthenticationService : IDiscogsAuthenticationService
{
    private readonly IPersonalAccessTokenAuthenticationProvider _personalAccessTokenAuthenticationProvider;
    private readonly IOAuthAuthenticationProvider _oAuthAuthenticationProvider;
    private bool _lastAuthenticatedWithPersonalAccessToken = false;
    private bool _lastAuthenticatedWithOAuth = false;

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
        _lastAuthenticatedWithOAuth = false;
    }

    /// <inheritdoc />
    /// <exception cref="AuthenticationFailedDiscogsException" />
    public async Task<OAuthAuthenticationSession> StartOAuthAuthentication(CancellationToken cancellationToken)
        => await _oAuthAuthenticationProvider.StartAuthentication(cancellationToken);

    /// <inheritdoc />
    /// <exception cref="AuthenticationFailedDiscogsException" />
    public async Task<(string AccessToken, string AccessTokenSecret)> CompleteOAuthAuthentication(
        OAuthAuthenticationSession session,
        string verifierToken,
        CancellationToken cancellationToken)
    {
        var tokens = await _oAuthAuthenticationProvider.CompleteAuthentication(session, verifierToken, cancellationToken);

        _lastAuthenticatedWithOAuth = true;
        _lastAuthenticatedWithPersonalAccessToken = false;

        return tokens;
    }

    /// <inheritdoc />
    public void AuthenticateWithOAuth(string accessToken, string accessTokenSecret)
    {
        _lastAuthenticatedWithOAuth = false;

        _oAuthAuthenticationProvider.Authenticate(accessToken, accessTokenSecret);

        _lastAuthenticatedWithOAuth = true;
        _lastAuthenticatedWithPersonalAccessToken = false;
    }

    /// <inheritdoc/>
    /// <exception cref="UnauthenticatedDiscogsException"></exception>
    public string CreateAuthenticationHeader()
    {
        if (_lastAuthenticatedWithPersonalAccessToken
            && _personalAccessTokenAuthenticationProvider.IsAuthenticated)
            return _personalAccessTokenAuthenticationProvider.CreateAuthenticationHeader();

        if (_lastAuthenticatedWithOAuth
            && _oAuthAuthenticationProvider.IsAuthenticated)
            return _oAuthAuthenticationProvider.CreateAuthenticationHeader();

        throw new UnauthenticatedDiscogsException("The authentication provider is not authenticated with any of the available authentication flows.");
    }
}
