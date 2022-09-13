namespace DiscogsApiClient.Authentication.UserToken;

/// <summary>
/// This <see cref="IAuthenticationProvider"/> implementation authenticates against the Discogs Api
/// using the user's personal access token as described <a href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">here</a>
/// and should be provided to the <see cref="DiscogsApiClient"/>'s constructor.
/// </summary>
public sealed class UserTokenAuthenticationProvider : IAuthenticationProvider
{
    /// <summary>
    /// The user's personal access token used for authentication.
    /// </summary>
    private string _userToken = "";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsAuthenticated => !String.IsNullOrWhiteSpace(_userToken);


    /// <summary>
    /// Authenticates the client with a user token which is appended to the api requests.
    /// <para/>
    /// This method always returns true since the token is stored for the <see cref="UserTokenAuthenticationProvider.CreateAuthenticatedRequest"/> method
    /// and no actual authentication flow with Discogs is needed.
    /// </summary>
    /// <param name="authenticationRequest">The <see cref="UserTokenAuthenticationRequest"/> providing the user token to the provider.</param>
    /// <returns>The <see cref="UserTokenAuthenticationResponse"/> indicating if the authentication was successful.</returns>
    /// <exception cref="ArgumentException">Fires this exception if the provided <see cref="IAuthenticationRequest"/> is not a <see cref="UserTokenAuthenticationRequest"/>.</exception>
    public Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
    {
        if (authenticationRequest is not UserTokenAuthenticationRequest userTokenAuthenticationRequest)
            throw new ArgumentException($"The provided authentication request must be of type {typeof(UserTokenAuthenticationRequest).Name}", nameof(authenticationRequest));

        _userToken = userTokenAuthenticationRequest.UserToken;

        return Task.FromResult<IAuthenticationResponse>(new UserTokenAuthenticationResponse(true));
    }

    /// <summary>
    /// Creates an authenticated <see cref="HttpRequestMessage"/> with an added authorization header containing the user's personal access token.
    /// </summary>
    /// <param name="httpMethod"><inheritdoc/></param>
    /// <param name="url"><inheritdoc/></param>
    /// <returns><inheritdoc/></returns>
    public HttpRequestMessage CreateAuthenticatedRequest(HttpMethod httpMethod, string url)
    {
        var request = new HttpRequestMessage(httpMethod, url);
        request.Headers.Add("Authorization", $"Discogs token={_userToken}");

        return request;
    }
}
