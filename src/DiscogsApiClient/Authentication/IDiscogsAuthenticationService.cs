using DiscogsApiClient.Authentication.OAuth;

namespace DiscogsApiClient.Authentication;

/// <summary>
/// A service which enables authentication against the Discogs Api with one of the supported authentication flows.
/// <para/>
/// Currently supported are
/// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Personal Access Tokens</see>
/// and
/// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see>.
/// </summary>
public interface IDiscogsAuthenticationService
{
    /// <summary>
    /// Indicates if the user is authenticated with at least one of the authentication flows.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Authenticates the user with a <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Personal Access Token</see>.
    /// </summary>
    /// <param name="token">Personal Access Token</param>
    void AuthenticateWithPersonalAccessToken(string token);

    /// <summary>
    /// Starts authenticating the user with <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see>.
    /// <para />
    /// The flow is started and returns the <see cref="OAuthAuthenticationSession"/> containing the authorization url and verifier callback url
    /// needed for retrieving the verifier token in the UI.
    /// </summary>
    /// <returns>The session object.</returns>
    Task<OAuthAuthenticationSession> StartOAuthAuthentication(CancellationToken cancellationToken);

    /// <summary>
    /// Completes authenticating the user with <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see>.
    /// <para />
    /// The flow is completed by providing the <see cref="OAuthAuthenticationSession"/> from starting the authentication flow and the retrieved verifier token.
    /// </summary>
    /// <param name="session">The session object returned from starting the authentication.</param>
    /// <param name="verifierToken">The verifier token retrieved from the user in the UI.</param>
    /// <returns>A tuple containing the access token and access token secret.</returns>
    Task<(string AccessToken, string AccessTokenSecret)> CompleteOAuthAuthentication(
        OAuthAuthenticationSession session,
        string verifierToken,
        CancellationToken cancellationToken);

    /// <summary>
    /// Authenticates the user with already existing <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see> access token and secret without triggering the authentication flow.
    /// </summary>
    /// <param name="accessToken">The already obtained access token.</param>
    /// <param name="accessTokenSecret">The already obtained access token secret.</param>
    void AuthenticateWithOAuth(string accessToken, string accessTokenSecret);

    /// <summary>
    /// Creates an authentication header value for HttpRequestMessages.
    /// The header is created from the last method which the user was authenticated with.
    /// <para/>
    /// NOTE: Although this is used for authentication, Discogs expects the header key to be named 'Authorization'.
    /// </summary>
    /// <returns>The authentication header value.</returns>
    string CreateAuthenticationHeader();
}
