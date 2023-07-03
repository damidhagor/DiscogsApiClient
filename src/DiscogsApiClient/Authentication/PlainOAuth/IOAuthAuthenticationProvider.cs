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
    /// Authenticates the user with <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see>.
    /// </summary>
    /// <param name="consumerKey">The consumer key</param>
    /// <param name="consumerSecret">The consumer secret</param>
    /// <param name="verifierCallbackUrl">The url to which the Discogs Api will redirect to provide the verifier token back to the application.</param>
    /// <param name="getVerifierCallback">The callback to the application which the provider will invoke to let the application handle the login for the user on the Discogs website.</param>
    /// <returns>A tuple containing the access token and access token secret.</returns>
    Task<(string accessToken, string accessTokenSecret)> Authenticate(
        string consumerKey,
        string consumerSecret,
        string verifierCallbackUrl,
        GetVerifierCallback getVerifierCallback,
        CancellationToken cancellationToken);

    /// <summary>
    /// Authenticates the user with already existing access token and secret without triggering the authentication flow.
    /// </summary>
    /// <param name="consumerKey">The consumer key</param>
    /// <param name="consumerSecret">The consumer secret</param>
    /// <param name="accessToken">The already obtained access token.</param>
    /// <param name="accessTokenSecret">The already obtained access token secret.</param>
    void Authenticate(
        string consumerKey,
        string consumerSecret,
        string accessToken,
        string accessTokenSecret);

    /// <summary>
    /// Creates an authentication header value to insert into a HttpRequestMessage.
    /// <para />
    /// Discogs expects this header to be named 'Authorization'.
    /// </summary>
    /// <returns>The authorization header value.</returns>
    string CreateAuthenticationHeader();
}

/// <summary>
/// Defines the method signature for the callback the <see cref="OAuthAuthenticationProvider.AuthenticateAsync"/> method invokes
/// when it needs the client to open the Discogs login page to obtain the verifier key for the OAuth 1.0a flow.
/// </summary>
/// <param name="authorizeUrl">The Discogs login Url to open in a browser.</param>
/// <param name="verifierCallbackUrl">The Url the login page will redirect to after the user logged in which will provide the verifier key.</param>
/// <returns>The verifier key obtained from the redirected login page.</returns>
public delegate Task<string> GetVerifierCallback(string authorizeUrl, string verifierCallbackUrl, CancellationToken cancellationToken);
