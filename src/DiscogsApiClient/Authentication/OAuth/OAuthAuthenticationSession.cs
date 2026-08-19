namespace DiscogsApiClient.Authentication.OAuth;

/// <summary>
/// Holds the necessary information from a started OAuth authentication flow.
/// </summary>
/// <param name="AuthorizeUrl">The url needed to be opened in a browser to let the user login with.</param>
/// <param name="VerifierCallbackUrl">The url Discogs will redirect to when returning the verifier token.</param>
/// <param name="RequestToken">The returned request token from starting the authentication flow.</param>
/// <param name="RequestTokenSecret">The returned request token secret from starting the authentication flow.</param>
public sealed record OAuthAuthenticationSession(
    Uri AuthorizeUrl,
    Uri VerifierCallbackUrl,
    string RequestToken,
    string RequestTokenSecret);
