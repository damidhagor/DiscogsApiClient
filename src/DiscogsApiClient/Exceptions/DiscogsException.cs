namespace DiscogsApiClient.Exceptions;

/// <summary>
/// An <see cref="Exception"/> representing that a request to the Discogs Api failed.
/// Expected failures that should be handled are represented by <see cref="Exception"/>s inheriting from this one.
/// </summary>
public class DiscogsException : Exception
{
    public DiscogsException() { }
    public DiscogsException(string? message) : base(message) { }
    public DiscogsException(string? message, Exception? innerException) : base(message, innerException) { }
}
