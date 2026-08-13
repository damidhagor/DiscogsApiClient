namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that the rate limit of the Discogs Api was exceeded.
/// </summary>
public sealed class RateLimitExceededDiscogsException : Exception
{
    public RateLimitExceededDiscogsException() { }
    public RateLimitExceededDiscogsException(string? message) : base(message) { }
    public RateLimitExceededDiscogsException(string? message, Exception? innerException) : base(message, innerException) { }
}
