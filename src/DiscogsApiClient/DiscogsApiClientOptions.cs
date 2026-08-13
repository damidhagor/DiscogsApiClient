namespace DiscogsApiClient;

/// <summary>
/// Options for initializing the library with Dependency Injection.
/// </summary>
public sealed class DiscogsApiClientOptions
{
    /// <summary>
    /// The default configuration section name (<c>"Discogs"</c>) bound by the
    /// delegate-based <c>AddDiscogsApiClient</c> overloads.
    /// </summary>
    public const string SectionName = "Discogs";

    /// <summary>
    /// Base url of the Discogs Api.
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.discogs.com";

    /// <summary>
    /// User-Agent header value to identify your app.
    /// </summary>
    public string UserAgent { get; set; } = "";
}
