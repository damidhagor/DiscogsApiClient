namespace DiscogsApiClient.Authentication.UserToken;

public class UserTokenAuthenticationProvider : IAuthenticationProvider
{
    private string? _userToken;

    public bool IsAuthenticated { get; private set; }


    public UserTokenAuthenticationProvider()
    {
        _userToken = null;
        IsAuthenticated = false;
    }


    public Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
    {
        if (authenticationRequest is not UserTokenAuthenticationRequest userTokenAuthenticationRequest)
            throw new ArgumentException($"The provided authentication request must be of type {typeof(UserTokenAuthenticationRequest).Name}", nameof(authenticationRequest));

        _userToken = userTokenAuthenticationRequest.UserToken;
        IsAuthenticated = true;

        return Task.FromResult<IAuthenticationResponse>(new UserTokenAuthenticationResponse(true));
    }


    public HttpRequestMessage CreateAuthenticatedRequest(HttpMethod httpMethod, string url)
    {
        var request = new HttpRequestMessage(httpMethod, url);
        request.Headers.Add("Authorization", $"Discogs token={_userToken}");

        return request;
    }

}
