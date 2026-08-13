namespace DiscogsApiClient.Authentication.Pat;

/// <summary>
/// Options for authenticating against the Discogs Api with a
/// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Personal Access Token</see>.
/// </summary>
public sealed class DiscogsPatOptions
{
    /// <summary>
    /// The default configuration section name (<c>"Discogs:Pat"</c>) bound by
    /// <c>WithPatAuthentication</c>.
    /// </summary>
    public const string SectionName = "Discogs:Pat";

    /// <summary>
    /// The personal access token, as generated in your Discogs account settings.
    /// <para/>
    /// Optional at registration time — it can also be supplied later at runtime via
    /// <see cref="IDiscogsPatAuthenticationProvider.Authenticate(string)"/> (e.g. for apps that let the
    /// user type in their own token interactively).
    /// </summary>
    public string? Token { get; set; }
}
