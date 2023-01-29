using System.Diagnostics.CodeAnalysis;

namespace DiscogsApiClient.Authentication;

/// <summary>
/// Interface representing a class that handles a certain type of authentication for the <see cref="DiscogsApiClient"/>.
/// An implementation of this interface needs to be provided to the <see cref="DiscogsApiClient"/>'s constructor.
/// <para/>
/// Available implementations are <see cref="UserToken.UserTokenAuthenticationProvider"/> and <see cref="PlainOAuth.PlainOAuthAuthenticationProvider"/>.
/// </summary>
public interface IAuthenticationProvider
{
    /// <summary>
    /// Indicates if the provider authenticated successfully with the Discogs Api and can create authenticated requests.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Authenticates against the Discogs Api with the implementations type of authentication.
    /// Requires the corresponding <see cref="IAuthenticationRequest"/> of the implemented <see cref="IAuthenticationProvider"/>.
    /// </summary>
    /// <param name="authenticationRequest">The corresponding <see cref="IAuthenticationRequest"/> to the implemented <see cref="IAuthenticationProvider"/>.</param>
    /// <returns><see cref="IAuthenticationResponse"/> of the implemented <see cref="IAuthenticationProvider"/>.</returns>
    Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a <see cref="HttpRequestMessage"/> with an added authorization header for the implemented authentication type.
    /// </summary>
    /// <param name="httpMethod">The <see cref="HttpMethod"/> for the created <see cref="HttpRequestMessage"/>.</param>
    /// <param name="url">The Url of the created <see cref="HttpRequestMessage"/>.</param>
    /// <returns>An authenticated <see cref="HttpRequestMessage"/>.</returns>
    HttpRequestMessage CreateAuthenticatedRequest(
        HttpMethod httpMethod,
#if NET7_0
        [StringSyntax(StringSyntaxAttribute.Uri)] string url);
#else
        string url);
#endif
}
