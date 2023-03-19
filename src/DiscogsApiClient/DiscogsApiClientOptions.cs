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
    /// If the <see cref="IDiscogsApiClient"/> should be rate limited.
    /// <para/>
    /// The rate limiter uses the <see href="https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-8.0#slide"> sliding window algorithm</see>.
    /// </summary>
    public bool UseRateLimiting { get; set; } = false;

    /// <summary>
    /// The length of the sliding window.
    /// </summary>
    public TimeSpan RateLimitingWindow { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// In how many segments the window is split up.
    /// </summary>
    public int RateLimitingWindowSegments { get; set; } = 12;

    /// <summary>
    /// How many permits the window allows in total.
    /// </summary>
    public int RateLimitingPermits { get; set; } = 40;

    /// <summary>
    /// How many requests will be allowed to be waiting for leases.
    /// </summary>
    public int RateLimitingQueueSize { get; set; } = 100;
}
