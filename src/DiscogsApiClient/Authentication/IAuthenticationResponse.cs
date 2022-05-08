namespace DiscogsApiClient.Authentication;

/// <summary>
/// The response of the <see cref="IAuthenticationProvider"/>'s AuthenticateAsync method.
/// Specific implementations my return more response properties depending on the authentication type.
/// </summary>
public interface IAuthenticationResponse
{
    /// <summary>
    /// Indicates if the authentication was successful.
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// The error message if the authentication failed.
    /// </summary>
    string? Error { get; }
}