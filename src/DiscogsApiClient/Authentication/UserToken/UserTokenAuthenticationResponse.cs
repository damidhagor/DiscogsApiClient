namespace DiscogsApiClient.Authentication.UserToken;

public class UserTokenAuthenticationResponse : IAuthenticationResponse
{
    public bool Success { get; init; }

    public string? Error { get; init; }


    public UserTokenAuthenticationResponse(bool success, string? error = null)
    {
        Success = success;
        Error = error;
    }
}