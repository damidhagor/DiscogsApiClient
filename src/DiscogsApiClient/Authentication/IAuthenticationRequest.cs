namespace DiscogsApiClient.Authentication;

/// <summary>
/// Parameters needed by the <see cref="IAuthenticationProvider"/> to authenticate against the Discogs Api.
/// Implementations of the <see cref="IAuthenticationProvider"/> require the corresponding implementation of the <see cref="IAuthenticationRequest"/>.
/// </summary>
public interface IAuthenticationRequest { }
