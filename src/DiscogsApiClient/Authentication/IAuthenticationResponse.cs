namespace DiscogsApiClient.Authentication;

/// <summary>
/// The response of the <see cref="IAuthenticationProvider.AuthenticateAsync"/> method.
/// Specific implementations may return more response properties depending on the authentication type.
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