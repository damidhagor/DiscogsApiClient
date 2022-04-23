namespace DiscogsApiClient.Authentication;

public interface IAuthenticationResponse
{
    bool Success { get; }

    string? Error { get; }
}