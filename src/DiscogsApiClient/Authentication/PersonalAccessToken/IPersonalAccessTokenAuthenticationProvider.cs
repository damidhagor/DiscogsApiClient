namespace DiscogsApiClient.Authentication.PersonalAccessToken;

/// <summary>
/// Defines an authentication provider which uses a personal access token to authenticate a user against the Discogs Api.
/// </summary>
internal interface IPersonalAccessTokenAuthenticationProvider
{
    /// <summary>
    /// Indicates if the user is authenticated with a personal access token.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Authenticates the user with a personal access token.
    /// </summary>
    /// <param name="token">The personal access token.</param>
    void Authenticate(string token);

    /// <summary>
    /// Creates an authentication header value to insert into a HttpRequestMessage.
    /// <para />
    /// Discogs expects this header to be named 'Authorization'.
    /// </summary>
    /// <returns>The authorization header value.</returns>
    string CreateAuthenticationHeader();
}
