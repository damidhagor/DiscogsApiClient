namespace DiscogsApiClient.Authorization.UserToken;

public class UserTokenAuthorizationRequest : IAuthorizationRequest
{
    public string UserToken { get; init; }


    public UserTokenAuthorizationRequest(string userToken)
    {
        UserToken = userToken;
    }
}
