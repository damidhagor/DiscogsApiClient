namespace DiscogsApiClient.Exceptions;

/// <summary>
/// An <see cref="Exception"/> representing that a request to the Discogs Api failed.
/// Expected failures that should be handled are represented by <see cref="Exception"/>s inherited from this one.
/// </summary>
public class DiscogsException : Exception
{
    public DiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}