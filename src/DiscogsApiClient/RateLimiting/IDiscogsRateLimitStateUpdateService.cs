namespace DiscogsApiClient.RateLimiting;

/// <summary>
/// Internal interface for updating the Discogs API rate limit state.
/// Not exposed to library consumers.
/// </summary>
internal interface IDiscogsRateLimitStateUpdateService
{
    /// <summary>
    /// Updates the Discogs API rate limit state.
    /// </summary>
    /// <param name="limit">The total number of requests allowed per minute (from <c>x-discogs-ratelimit</c> header).</param>
    /// <param name="remaining">The number of remaining requests in the current window (from <c>x-discogs-ratelimit-remaining</c> header).</param>
    /// <param name="used">The number of requests used in the current window (from <c>x-discogs-ratelimit-used</c> header).</param>
    void UpdateState(int limit, int remaining, int used);
}
