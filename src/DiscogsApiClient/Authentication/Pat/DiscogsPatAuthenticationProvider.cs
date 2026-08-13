using Microsoft.Extensions.Options;

namespace DiscogsApiClient.Authentication.Pat;

internal sealed class DiscogsPatAuthenticationProvider(IOptions<DiscogsPatOptions> options)
    : IDiscogsPatAuthenticationProvider,
      IDiscogsAuthenticationProvider
{
    private volatile string? _token = string.IsNullOrWhiteSpace(options.Value.Token) ? null : options.Value.Token;

    public bool IsAuthenticated => _token is not null;

    public void Authenticate(string token)
    {
        _token = null;
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        _token = token;
    }

    public string CreateAuthenticationHeader()
    {
        var token = _token
            ?? throw new UnauthenticatedDiscogsException($"The {nameof(DiscogsPatAuthenticationProvider)} must be authenticated before creating an authentication header.");

        return $"Discogs token={token}";
    }
}
