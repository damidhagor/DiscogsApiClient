namespace DiscogsApiClient.Authorization.UserToken;

public class UserTokenAuthorizationResponse : IAuthorizationResponse
{
    public bool Success { get; init; }

    public string? Error { get; init; }


    public UserTokenAuthorizationResponse(bool success, string? error = null)
    {
        Success = success;
        Error = error;
    }
}