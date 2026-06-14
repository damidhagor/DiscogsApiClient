using DiscogsApiClient.RateLimiting;

namespace DiscogsApiClient.Middleware;

/// <summary>
/// A delegating handler that extracts Discogs API rate limit information from response headers
/// and updates the <see cref="IDiscogsRateLimitStateService"/>.
/// </summary>
/// <remarks>
/// This handler reads the <c>x-discogs-ratelimit</c>, <c>x-discogs-ratelimit-remaining</c>,
/// and <c>x-discogs-ratelimit-used</c> headers from each API response and updates the shared
/// rate limit state that can be accessed via <see cref="IDiscogsRateLimitStateService"/>.
/// </remarks>
internal sealed class RateLimitStateDelegatingHandler(IDiscogsRateLimitStateUpdateService rateLimitStateUpdateService) : DelegatingHandler
{
    private const string LimitHeaderName = "x-discogs-ratelimit";
    private const string RemainingHeaderName = "x-discogs-ratelimit-remaining";
    private const string UsedHeaderName = "x-discogs-ratelimit-used";

    private readonly IDiscogsRateLimitStateUpdateService _rateLimitStateUpdateService = rateLimitStateUpdateService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (TryParseHeader(response, LimitHeaderName, out var limit)
         && TryParseHeader(response, RemainingHeaderName, out var remaining)
         && TryParseHeader(response, UsedHeaderName, out var used))
        {
            _rateLimitStateUpdateService.UpdateState(limit, remaining, used);
        }

        return response;
    }

    private static bool TryParseHeader(HttpResponseMessage response, string headerName, out int value)
    {
        if (response.Headers.TryGetValues(headerName, out var values))
        {
            var firstValue = values.FirstOrDefault();
            if (firstValue is not null && int.TryParse(firstValue, out value))
            {
                return true;
            }
        }

        value = default;
        return false;
    }
}
