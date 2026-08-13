namespace DiscogsApiClient.Authentication.OAuth;

/// <summary>
/// Options for authenticating against the Discogs Api with the
/// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see> flow.
/// <para/>
/// These are the static, app-identity credentials issued when registering your application with Discogs.
/// The dynamic, per-user access token/secret obtained through the interactive flow are <b>not</b> part of
/// these options — see <see cref="IDiscogsOAuthAuthenticationProvider.Authenticate(string, string)"/>.
/// </summary>
public sealed class DiscogsOAuthOptions
{
    /// <summary>
    /// The default configuration section name (<c>"Discogs:OAuth"</c>) bound by
    /// <c>WithOAuthAuthentication</c>.
    /// </summary>
    public const string SectionName = "Discogs:OAuth";

    /// <summary>
    /// Consumer Key of the Discogs application.
    /// </summary>
    public string ConsumerKey { get; set; } = "";

    /// <summary>
    /// Consumer Secret of the Discogs application.
    /// </summary>
    public string ConsumerSecret { get; set; } = "";

    /// <summary>
    /// The url provided to the Discogs Api when initiating the OAuth flow to where the browser should be redirected to return the verifier token.
    /// </summary>
    public string? VerifierCallbackUrl { get; set; }
}
