using System.Globalization;
using System.Net;
using System.Threading.RateLimiting;

namespace DiscogsApiClient.Middleware;

internal sealed class RateLimitedDelegatingHandler : DelegatingHandler, IAsyncDisposable
{
    private readonly RateLimiter _rateLimiter;
    private readonly bool _disposeRateLimiter;

    public RateLimitedDelegatingHandler(RateLimiter limiter, bool disposeRateLimiter = true)
    {
        _rateLimiter = limiter;
        _disposeRateLimiter = disposeRateLimiter;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var lease = await _rateLimiter.AcquireAsync(1, cancellationToken);

        if (lease.IsAcquired)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        if (lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            response.Headers.Add("Retry-After", ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo));
        }

        return response;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_disposeRateLimiter)
        {
            await _rateLimiter.DisposeAsync().ConfigureAwait(false);
        }

        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing && _disposeRateLimiter)
        {
            _rateLimiter.Dispose();
        }
    }
}
