namespace DiscogsApiClient;

/// <summary>
/// Options for initializing the library with Dependency Injection.
/// </summary>
public sealed class DiscogsApiClientOptions
{
    /// <summary>
    /// Base url of the Discogs Api.
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.discogs.com";

    /// <summary>
    /// User-Agent header value to identify your app.
    /// </summary>
    public string UserAgent { get; set; } = "";

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
