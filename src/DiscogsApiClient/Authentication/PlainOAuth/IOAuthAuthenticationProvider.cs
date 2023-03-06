namespace DiscogsApiClient.Authentication.OAuth;

/// <summary>
/// Defines an authentication provider which uses Discogs' OAuth 1.0a authentication flow to authenticate a user against the Discogs Api.
/// </summary>
internal interface IOAuthAuthenticationProvider
{
    /// <summary>
    /// Indicates if the user is authenticated with the OAuth 1.0a flow.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Authenticates the user with the OAuth 1.0a flow.
    /// </summary>
    /// <param name="token">The personal access token.</param>
    Task<OAuthAuthenticationResponse> Authenticate(OAuthAuthenticationRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Creates an authentication header value to insert into a HttpRequestMessage.
    /// <para />
    /// Discogs expects this header to be named 'Authorization'.
    /// </summary>
    /// <returns>The authorization header value.</returns>
    string CreateAuthenticationHeader();
}
