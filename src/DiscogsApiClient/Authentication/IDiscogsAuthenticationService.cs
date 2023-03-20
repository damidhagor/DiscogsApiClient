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
    /// Authenticates the user with a <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Personal Access Tokens</see>.
    /// </summary>
    /// <param name="token">Personal Access Token</param>
    void AuthenticateWithPersonalAccessToken(string token);

    /// <summary>
    /// Authenticates the user with <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see>.
    /// </summary>
    /// <param name="consumerKey">The consumer key</param>
    /// <param name="consumerSecret">The consumer secret</param>
    /// <param name="accessToken">The previously obtained access token, if the user was already authenticated with this method.</param>
    /// <param name="accessTokenSecret">The previously obtained access token secret, if the user was already authenticated with this method.</param>
    /// <param name="verifierCallbackUrl">The url to which the Discogs Api will redirect to provide the verifier token back to the application.</param>
    /// <param name="getVerifierCallback">The callback to the application which the provider will invoke to let the application handle the login for the user on the Discogs website.</param>
    /// <returns>A tuple containing the access token and access token secret.</returns>
    Task<(string accessToken, string accessTokenSecret)> AuthenticateWithOAuth(
        string consumerKey,
        string consumerSecret,
        string? accessToken,
        string? accessTokenSecret,
        string verifierCallbackUrl,
        GetVerifierCallback getVerifierCallback,
        CancellationToken cancellationToken);

    /// <summary>
    /// Creates an authentication header value for HttpRequestMessages.
    /// The header is created from the last method which the user was authenticated with.
    /// <para/>
    /// NOTE: Although this is used for authentication, Discogs expects the header key to be named 'Authorization'.
    /// </summary>
    /// <returns>The authentication header value.</returns>
    string CreateAuthenticationHeader();
}
