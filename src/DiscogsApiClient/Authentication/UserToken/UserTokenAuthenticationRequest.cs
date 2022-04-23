namespace DiscogsApiClient.Authentication.UserToken;

public class UserTokenAuthenticationRequest : IAuthenticationRequest
{
    public string UserToken { get; init; }


    public UserTokenAuthenticationRequest(string userToken)
    {
        UserToken = userToken;
    }
}
