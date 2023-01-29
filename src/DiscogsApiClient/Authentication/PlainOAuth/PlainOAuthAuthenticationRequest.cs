using System.Diagnostics.CodeAnalysis;

namespace DiscogsApiClient.Authentication.PlainOAuth;

/// <summary>
/// Defines the method signature for the callback the <see cref="PlainOAuthAuthenticationProvider.AuthenticateAsync"/> method invokes
/// when it needs the client to open the Discogs login page to obtain the verifier key for the OAuth 1.0a flow.
/// </summary>
/// <param name="authorizeUrl">The Discogs login Url to open in a browser.</param>
/// <param name="verifierCallbackUrl">The Url the login page will redirect after the user logged in which will provide the verifier key.</param>
/// <returns>The verifier key obtained from the redirected login page.</returns>
public delegate Task<string> GetVerifierCallback(string authorizeUrl, string verifierCallbackUrl, CancellationToken cancellationToken);

/// <summary>
/// Parameters needed by the <see cref="PlainOAuthAuthenticationProvider"/> to authenticate against the Discogs Api with the OAuth 1.0a flow.
/// If the <see cref="PlainOAuthAuthenticationProvider"/> is used then this <see cref="PlainOAuthAuthenticationRequest"/> must be passed to the <see cref="DiscogsApiClient.AuthenticateAsync"/> method.
/// </summary>
public sealed class PlainOAuthAuthenticationRequest : IAuthenticationRequest
{
    /// <summary>
    /// The user agent used by the DiscogsApiClient.
    /// </summary>
    public string UserAgent { get; init; }

    /// <summary>
    /// The app's consumer key from Discogs.
    /// </summary>
    public string ConsumerKey { get; init; }

    /// <summary>
    /// The app's consumer secret from Discogs.
    /// </summary>
    public string ConsumerSecret { get; init; }

    /// <summary>
    /// The user's access token.
    /// The token should be persisted after the first successful authentication
    /// and provided/reused here to keep the user logged in when the application restarts.
    /// </summary>
    public string AccessToken { get; init; }

    /// <summary>
    /// The user's access token secret.
    /// The secret should be persisted after the first successful authentication
    /// and provided/reused here to keep the user logged in when the application restarts.
    /// </summary>
    public string AccessTokenSecret { get; init; }

    /// <summary>
    /// The Url the login page will be redirected to to pass the OAuth verifier key back to the client.
    /// </summary>
#if NET7_0
    [StringSyntax(StringSyntaxAttribute.Uri)]
#endif
    public string VerifierCallbackUrl { get; init; }

    /// <summary>
    /// The method which will be called by the <see cref="PlainOAuthAuthenticationProvider"/> to let the client open the Discogs login page and obtain the verifier key.
    /// </summary>
    public GetVerifierCallback GetVerifierCallback { get; init; }


    /// <param name="userAgent">The app's consumer secret from Discogs.</param>
    /// <param name="consumerKey">The app's consumer key from Discogs.</param>
    /// <param name="consumerSecret">The app's consumer secret from Discogs.</param>
    /// <param name="verifierCallbackUrl">The Url the login page will be redirected to to pass the OAuth verifier key back to the client.</param>
    /// <param name="getVerifierCallback">The method which will be called by the <see cref="PlainOAuthAuthenticationProvider"/> to let the client open the Discogs login page and obtain the verifier key.</param>
    public PlainOAuthAuthenticationRequest(
        string userAgent,
        string consumerKey,
        string consumerSecret,
#if NET7_0
        [StringSyntax(StringSyntaxAttribute.Uri)] string verifierCallbackUrl,
#else
        string verifierCallbackUrl,
#endif
        GetVerifierCallback getVerifierCallback)
    {
        UserAgent = userAgent;
        ConsumerKey = consumerKey;
        ConsumerSecret = consumerSecret;
        AccessToken = "";
        AccessTokenSecret = "";
        VerifierCallbackUrl = verifierCallbackUrl;
        GetVerifierCallback = getVerifierCallback;
    }
}
