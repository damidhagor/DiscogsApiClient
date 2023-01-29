using System.Diagnostics.CodeAnalysis;

namespace DiscogsApiClient.Authentication.UserToken;

/// <summary>
/// The response of the <see cref="UserTokenAuthenticationProvider.AuthenticateAsync"/> method.
/// If the <see cref="UserTokenAuthenticationProvider"/> is used then this <see cref="UserTokenAuthenticationResponse"/> is returned by the <see cref="DiscogsApiClient.AuthenticateAsync"/> method.
/// </summary>
public sealed class UserTokenAuthenticationResponse : IAuthenticationResponse
{
    /// <inheritdoc/>
#if NET7_0
    required
#endif
    public bool Success { get; init; }

    /// <inheritdoc/>
    public string? Error { get; init; }


    /// <param name="success">If the authentication was successful.</param>
    /// <param name="error">The error if the authentication was not successful.</param>
#if NET7_0
    [SetsRequiredMembers]
#endif
    public UserTokenAuthenticationResponse(bool success, string? error = null)
    {
        Success = success;
        Error = error;
    }
}