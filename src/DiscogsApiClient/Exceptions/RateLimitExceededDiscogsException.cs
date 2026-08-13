namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that the rate limit of the Discogs Api was exceeded.
/// </summary>
public sealed class RateLimitExceededDiscogsException : Exception
{
    public RateLimitExceededDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}
