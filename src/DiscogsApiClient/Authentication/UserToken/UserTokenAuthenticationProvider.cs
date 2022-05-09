namespace DiscogsApiClient.Authentication.UserToken;

/// <summary>
/// This <see cref="IAuthenticationProvider"/> implementation authenticates against the Discogs Api
/// using the user's personal access token as described <a href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">here</a>
/// and should be provided to the <see cref="DiscogsApiClient"/>'s constructor.
/// </summary>
public class UserTokenAuthenticationProvider : IAuthenticationProvider
{
    /// <summary>
    /// The user's personal access token used for authentication.
    /// </summary>
    private string? _userToken;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsAuthenticated { get; private set; }


    public UserTokenAuthenticationProvider()
    {
        _userToken = null;
        IsAuthenticated = false;
    }


    /// <summary>
    /// </summary>
    /// <param name="authenticationRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
    {
        if (authenticationRequest is not UserTokenAuthenticationRequest userTokenAuthenticationRequest)
            throw new ArgumentException($"The provided authentication request must be of type {typeof(UserTokenAuthenticationRequest).Name}", nameof(authenticationRequest));

        _userToken = userTokenAuthenticationRequest.UserToken;
        IsAuthenticated = true;

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
