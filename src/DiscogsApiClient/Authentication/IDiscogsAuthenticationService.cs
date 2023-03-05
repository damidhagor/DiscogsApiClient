using DiscogsApiClient.Authentication.OAuth;

namespace DiscogsApiClient.Authentication;

public interface IDiscogsAuthenticationService
{
    bool IsAuthenticated { get; }

    void AuthenticateWithPersonalAccessToken(string token);

    Task<OAuthAuthenticationResponse> AuthenticateWithOAuth(OAuthAuthenticationRequest request, CancellationToken cancellationToken);

    string CreateAuthenticationHeader();
}
