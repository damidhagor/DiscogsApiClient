namespace DiscogsApiClient.Authorization.UserToken;

public class UserTokenAuthorizationProvider : IAuthorizationProvider
{
    private string? _userToken;

    public bool IsAuthorized { get; private set; }


    public UserTokenAuthorizationProvider()
    {
        _userToken = null;
        IsAuthorized = false;
    }


    public Task<IAuthorizationResponse> AuthorizeAsync(IAuthorizationRequest authorizationRequest, CancellationToken cancellationToken)
    {
        if (authorizationRequest is not UserTokenAuthorizationRequest userTokenAuthorizationRequest)
            throw new ArgumentException($"The provided authorization request must be of type {typeof(UserTokenAuthorizationRequest).Name}", nameof(authorizationRequest));

        _userToken = userTokenAuthorizationRequest.UserToken;
        IsAuthorized = true;

        return Task.FromResult<IAuthorizationResponse>(new UserTokenAuthorizationResponse(true));
    }


    public HttpRequestMessage CreateAuthorizedRequest(HttpMethod httpMethod, string url)
    {
        var request = new HttpRequestMessage(httpMethod, url);
        request.Headers.Add("Authorization", $"Discogs token={_userToken}");

        return request;
    }

}
