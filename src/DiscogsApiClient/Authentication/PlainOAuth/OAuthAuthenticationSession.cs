namespace DiscogsApiClient.Authentication.OAuth;

public sealed record OAuthAuthenticationSession(
    string AuthorizeUrl,
    string VerifierCallbackUrl,
    string RequestToken,
    string RequestTokenSecret);
