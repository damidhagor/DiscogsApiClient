using System.Diagnostics.CodeAnalysis;

namespace DiscogsApiClient.Authentication.OAuth;

/// <summary>
/// The response of the <see cref="PlainOAuthAuthenticationProvider.AuthenticateAsync"/> method.
/// If the <see cref="PlainOAuthAuthenticationProvider"/> is used then this <see cref="PlainOAuthAuthenticationResponse"/> is returned by the <see cref="DiscogsApiClient.AuthenticateAsync"/> method.
/// <para/>
/// It also returns the access token and secret if authentication was successful so they can be stored and reused by the client.
/// </summary>
public sealed class OAuthAuthenticationResponse
{

    /// <summary>
    /// Indicates if the authentication was successful.
    /// <para/>
    /// If the authentication was successful, the <see cref="AccessToken"/> and <see cref="AccessTokenSecret"/> are returned.
    /// <para/>
    /// If the authentication failed the <see cref="Error"/> message contains details about the error.
    /// </summary>
#if NET7_0
    required
#endif
    public bool Success
    { get; init; }

    /// <summary>
    /// Contains the error if the authentication failed.
    /// </summary>
    public string? Error { get; init; }

    /// <summary>
    /// The obtained access token if authentication was successful.
    /// <para/>
    /// Store this token with the access secret to pass it to the <see cref="OAuthAuthenticationProvider"/>
    /// next time a <see cref="DiscogsApiClient"/> with a <see cref="OAuthAuthenticationProvider"/> is constructed be authenticated immediately.
    /// </summary>
    public string? AccessToken { get; init; }

    /// <summary>
    /// The obtained access token secret if authentication was successful.
    /// <para/>
    /// Store this secret with the access token to pass it to the <see cref="OAuthAuthenticationProvider"/>
    /// next time a <see cref="DiscogsApiClient"/> with a <see cref="PlainOAuthAuthenticationProvider"/> is constructed be authenticated immediately.
    /// </summary>
    public string? AccessTokenSecret { get; init; }


    /// <summary>
    /// Constructor for when authentication was successful.
    /// </summary>
#if NET7_0
    [SetsRequiredMembers]
#endif
    public OAuthAuthenticationResponse(string accessToken, string accessTokenSecret)
    {
        Success = true;
        Error = null;
        AccessToken = accessToken;
        AccessTokenSecret = accessTokenSecret;
    }


    /// <summary>
    /// Constructor for when authentication failed.
    /// </summary>
#if NET7_0
    [SetsRequiredMembers]
#endif
    public OAuthAuthenticationResponse(string error)
    {
        Success = false;
        Error = error;
        AccessToken = null;
        AccessTokenSecret = null;
    }
}
