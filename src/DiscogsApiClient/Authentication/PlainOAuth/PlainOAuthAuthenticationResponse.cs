namespace DiscogsApiClient.Authentication.PlainOAuth;

/// <summary>
/// The response of the <see cref="PlainOAuthAuthenticationProvider.AuthenticateAsync"/> method.
/// If the <see cref="PlainOAuthAuthenticationProvider"/> is used then this <see cref="PlainOAuthAuthenticationResponse"/> is returned by the <see cref="DiscogsApiClient.AuthenticateAsync"/> method.
/// <para/>
/// It also returns the access token and secret if authentication was successful so they can be stored and reused by the client.
/// </summary>
public class PlainOAuthAuthenticationResponse : IAuthenticationResponse
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// The obtained access token if authentication was successful.
    /// <para/>
    /// Store this token with the access secret to pass it to the <see cref="PlainOAuthAuthenticationProvider"/>
    /// next time a <see cref="DiscogsApiClient"/> with a <see cref="PlainOAuthAuthenticationProvider"/> is constructed be authenticated immediately.
    /// </summary>
    public string? AccessToken { get; init; }

    /// <summary>
    /// The obtained access secret if authentication was successful.
    /// <para/>
    /// Store this secret with the access token to pass it to the <see cref="PlainOAuthAuthenticationProvider"/>
    /// next time a <see cref="DiscogsApiClient"/> with a <see cref="PlainOAuthAuthenticationProvider"/> is constructed be authenticated immediately.
    /// </summary>
    public string? AccessSecret { get; init; }


    /// <summary>
    /// Constructor for when authentication was successful.
    /// </summary>
    public PlainOAuthAuthenticationResponse(string accessToken, string accessSecret)
    {
        Success = true;
        Error = null;
        AccessToken = accessToken;
        AccessSecret = accessSecret;
    }


    /// <summary>
    /// Constructor for when authentication failed.
    /// </summary>
    public PlainOAuthAuthenticationResponse(string error)
    {
        Success = false;
        Error = error;
        AccessToken = null;
        AccessSecret = null;
    }
}
