using System.Globalization;
using System.Net;
using System.Threading.RateLimiting;

namespace DiscogsApiClient.Middleware;

/// <summary>
/// An optional handler which rate-limits the <see cref="IDiscogsApiClient"/> to ensure that the Discogs Api is not overloading and rejecting requests.
/// <para/>
/// This uses the <see href="https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-8.0#slide"> sliding window algorithm</see>
/// which is configured at startup with the options provided by the <see cref="ServiceCollectionExtensions.DiscogsApiClientOptions"/> method.
/// <para/>
/// For use in the test project the handler can optionally be configured to not dispose its <see cref="RateLimiter"/> so all unit tests are rate limited and run successfully.
/// </summary>
public sealed class RateLimitedDelegatingHandler : DelegatingHandler, IAsyncDisposable
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
