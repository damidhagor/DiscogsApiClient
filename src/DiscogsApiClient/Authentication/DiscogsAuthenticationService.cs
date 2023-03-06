using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Authentication;

internal sealed class DiscogsAuthenticationService : IDiscogsAuthenticationService
{
    private readonly IPersonalAccessTokenAuthenticationProvider _personalAccessTokenAuthenticationProvider;
    private readonly IOAuthAuthenticationProvider _oAuthAuthenticationProvider;
    private bool _lastAuthenticatedWithPersonalAccessToken = false;
    private bool _lastAuthenticatedWithOauth = false;

    public bool IsAuthenticated => _personalAccessTokenAuthenticationProvider.IsAuthenticated || _oAuthAuthenticationProvider.IsAuthenticated;

    public DiscogsAuthenticationService(
        IPersonalAccessTokenAuthenticationProvider personalAccessTokenAuthenticationProvider,
        IOAuthAuthenticationProvider oAuthAuthenticationProvider)
    {
        _personalAccessTokenAuthenticationProvider = personalAccessTokenAuthenticationProvider;
        _oAuthAuthenticationProvider = oAuthAuthenticationProvider;
    }

    public void AuthenticateWithPersonalAccessToken(string token)
    {
        _lastAuthenticatedWithPersonalAccessToken = false;
        _personalAccessTokenAuthenticationProvider.Authenticate(token);
        _lastAuthenticatedWithPersonalAccessToken = true;
    }

    public async Task<OAuthAuthenticationResponse> AuthenticateWithOAuth(OAuthAuthenticationRequest request, CancellationToken cancellationToken)
    {
        _lastAuthenticatedWithOauth = false;
        var response = await _oAuthAuthenticationProvider.Authenticate(request, cancellationToken);
        _lastAuthenticatedWithOauth = response.Success;
        return response;
    }

    public string CreateAuthenticationHeader()
    {
        if (_lastAuthenticatedWithPersonalAccessToken
            && _personalAccessTokenAuthenticationProvider.IsAuthenticated)
            return _personalAccessTokenAuthenticationProvider.CreateAuthenticationHeader();

        if (_lastAuthenticatedWithOauth
            && _oAuthAuthenticationProvider.IsAuthenticated)
            return _oAuthAuthenticationProvider.CreateAuthenticationHeader();

        throw new InvalidOperationException();
    }
}
