namespace DiscogsApiClient.Authentication;

public interface IAuthenticationProvider
{
    bool IsAuthenticated { get; }

    Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken);

    HttpRequestMessage CreateAuthenticatedRequest(HttpMethod httpMethod, string url);
}
