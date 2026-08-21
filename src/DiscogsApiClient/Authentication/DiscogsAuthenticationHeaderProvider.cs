using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.Pat;

namespace DiscogsApiClient.Authentication;

internal sealed class DiscogsAuthenticationHeaderProvider(
    IDiscogsPatAuthenticationProvider? patProvider = null,
    IDiscogsOAuthAuthenticationProvider? oAuthProvider = null)
    : IDiscogsAuthenticationHeaderProvider
{
    private readonly IDiscogsAuthenticationProvider? _activeProvider = (patProvider as IDiscogsAuthenticationProvider)
                                                                    ?? (oAuthProvider as IDiscogsAuthenticationProvider);

    public bool IsAuthenticated => _activeProvider?.IsAuthenticated is true;

    public string GetHeader()
        => _activeProvider?.CreateAuthenticationHeader()
            ?? throw new UnauthenticatedDiscogsException("The authentication provider is not authenticated with any of the available authentication flows.");
}
