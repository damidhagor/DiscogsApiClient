namespace DiscogsApiClient.Authentication.PlainOAuth;

public class PlainOAuthAuthenticationResponse : IAuthenticationResponse
{
    public bool Success { get; init; }

    public string? Error { get; init; }

    public string? AccessToken { get; init; }

    public string? AccessSecret { get; init; }


    public PlainOAuthAuthenticationResponse(string accessToken, string accessSecret)
    {
        Success = true;
        Error = null;
        AccessToken = accessToken;
        AccessSecret = accessSecret;
    }

    public PlainOAuthAuthenticationResponse(string error)
    {
        Success = false;
        Error = error;
        AccessToken = null;
        AccessSecret = null;
    }
}
