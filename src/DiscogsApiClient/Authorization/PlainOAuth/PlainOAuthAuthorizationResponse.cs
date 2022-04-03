namespace DiscogsApiClient.Authorization.PlainOAuth;

public class PlainOAuthAuthorizationResponse : IAuthorizationResponse
{
    public bool Success { get; init; }

    public string? Error { get; init; }

    public string? AccessToken { get; init; }

    public string? AccessSecret { get; init; }


    public PlainOAuthAuthorizationResponse(string accessToken, string accessSecret)
    {
        Success = true;
        Error = null;
        AccessToken = accessToken;
        AccessSecret = accessSecret;
    }

    public PlainOAuthAuthorizationResponse(string error)
    {
        Success = false;
        Error = error;
        AccessToken = null;
        AccessSecret = null;
    }
}
