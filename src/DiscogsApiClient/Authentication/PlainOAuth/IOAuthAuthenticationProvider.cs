namespace DiscogsApiClient.Authentication.OAuth;

/// <summary>
/// Defines an authentication provider which uses Discogs' OAuth 1.0a authentication flow to authenticate a user against the Discogs Api.
/// </summary>
public interface IOAuthAuthenticationProvider
{
    /// <summary>
    /// Indicates if the user is authenticated with the OAuth 1.0a flow.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Starts authenticating the user with <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see>.
    /// <para />
    /// The flow is started and returns the <see cref="OAuthAuthenticationSession"/> containing the authorization url and verifier callback url
    /// needed for retrieving the verifier token in the UI.
    /// </summary>
    /// <returns>The session object.</returns>
    Task<OAuthAuthenticationSession> StartAuthentication(CancellationToken cancellationToken);

    /// <summary>
    /// Completes authenticating the user with <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see>.
    /// <para />
    /// The flow is completed by providing the <see cref="OAuthAuthenticationSession"/> from starting the authentication flow and retrieved verifier token.
    /// </summary>
    /// <param name="session">The session object returned from starting the authentication.</param>
    /// <param name="verifierToken">The verifier token retrieved from the user in the UI.</param>
    /// <returns>A tuple containing the access token and access token secret.</returns>
    Task<(string AccessToken, string AccessTokenSecret)> CompleteAuthentication(OAuthAuthenticationSession session, string verifierToken, CancellationToken cancellationToken);

    /// <summary>
    /// Authenticates the user with already existing <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see> access token and secret without triggering the authentication flow.
    /// </summary>
    /// <param name="accessToken">The already obtained access token.</param>
    /// <param name="accessTokenSecret">The already obtained access token secret.</param>
    void Authenticate(string accessToken, string accessTokenSecret);

    /// <summary>
    /// Creates an authentication header value to insert into a HttpRequestMessage.
    /// <para />
    /// Discogs expects this header to be named 'Authorization'.
    /// </summary>
    /// <returns>The authorization header value.</returns>
    string CreateAuthenticationHeader();
}
