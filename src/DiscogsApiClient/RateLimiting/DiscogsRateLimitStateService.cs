using System.Diagnostics.CodeAnalysis;

namespace DiscogsApiClient.RateLimiting;

/// <summary>
/// Internal service that tracks the current Discogs API rate limit state.
/// Provides thread-safe access to rate limit values extracted from API response headers.
/// </summary>
internal sealed class DiscogsRateLimitStateService : IDiscogsRateLimitStateService, IDiscogsRateLimitStateUpdateService
{
    private volatile DiscogsRateLimitState? _currentState;

    /// <inheritdoc/>
    public DiscogsRateLimitState? GetCurrentState() => _currentState;

    /// <inheritdoc/>
    public bool TryGetCurrentState([NotNullWhen(true)] out DiscogsRateLimitState? state)
    {
        state = _currentState;
        return state is not null;
    }

    /// <inheritdoc/>
    public void UpdateState(int limit, int remaining, int used)
        => _currentState = new(limit, remaining, used);
}
