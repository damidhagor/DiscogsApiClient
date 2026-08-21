using System.Diagnostics.CodeAnalysis;

namespace DiscogsApiClient.RateLimiting;

/// <summary>
/// Provides access to the current Discogs API rate limit state.
/// </summary>
/// <remarks>
/// Discogs API rate limit state values are automatically extracted from Discogs API response headers
/// (<c>x-discogs-ratelimit</c>, <c>x-discogs-ratelimit-remaining</c>, <c>x-discogs-ratelimit-used</c>)
/// and updated after each successful API call.
/// </remarks>
public interface IDiscogsRateLimitStateService
{
    /// <summary>
    /// Gets the current Discogs API rate limit state from the most recent API response.
    /// </summary>
    /// <returns>
    /// A <see cref="DiscogsRateLimitState"/> containing the current Discogs API rate limit state values,
    /// or <see langword="null"/> if no API response has been received yet.
    /// </returns>
    DiscogsRateLimitState? GetCurrentState();

    /// <summary>
    /// Tries to get the current Discogs API rate limit state from the most recent API response.
    /// </summary>
    /// <param name="state">
    /// When this method returns <see langword="true"/>, contains the current Discogs API rate limit state values.
    /// When this method returns <see langword="false"/>, contains <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a Discogs API rate limit state is available;
    /// <see langword="false"/> if no API response has been received yet.
    /// </returns>
    bool TryGetCurrentState([NotNullWhen(true)] out DiscogsRateLimitState? state);
}
