namespace DiscogsApiClient.Authentication.UserToken;

/// <summary>
/// Parameters needed by the <see cref="UserTokenAuthenticationProvider"/> to authenticate against the Discogs Api with the user's personal access token.
/// If the <see cref="UserTokenAuthenticationProvider"/> is used then this <see cref="UserTokenAuthenticationRequest"/> must be passed to the <see cref="DiscogsApiClient.AuthenticateAsync"/> method.
/// </summary>
public sealed class UserTokenAuthenticationRequest : IAuthenticationRequest
{
    /// <summary>
    /// The user's personal access token.
    /// </summary>
    public string UserToken { get; init; }


    /// <param name="userToken">The user's personal access token.</param>
    public UserTokenAuthenticationRequest(string userToken)
    {
        UserToken = userToken;
    }
}
