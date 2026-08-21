namespace DiscogsApiClient.RateLimiting;

/// <summary>
/// Represents the current Discogs API rate limit state from response headers.
/// </summary>
/// <param name="Limit">The total number of requests allowed per minute.</param>
/// <param name="Remaining">The number of remaining requests in the current window.</param>
/// <param name="Used">The number of requests used in the current window.</param>
public sealed record class DiscogsRateLimitState(int Limit, int Remaining, int Used);
