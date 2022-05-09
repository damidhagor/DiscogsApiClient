namespace DiscogsApiClient.Authentication.UserToken;

/// <summary>
/// The response of the <see cref="UserTokenAuthenticationProvider.AuthenticateAsync"/> method.
/// If the <see cref="UserTokenAuthenticationProvider"/> is used then this <see cref="UserTokenAuthenticationResponse"/> is returned by the <see cref="DiscogsApiClient.AuthenticateAsync"/> method.
/// 
/// <inheritdoc/>
/// </summary>
public class UserTokenAuthenticationResponse : IAuthenticationResponse
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string? Error { get; init; }


    /// <param name="success">If the authentication was successful.</param>
    /// <param name="error">The error if the authentication was not successful.</param>
    public UserTokenAuthenticationResponse(bool success, string? error = null)
    {
        Success = success;
        Error = error;
    }
}