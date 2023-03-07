namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that a request failed because it needed to be authenticated but wasn't.
/// </summary>
public sealed class UnauthenticatedDiscogsException : Exception
{
    public UnauthenticatedDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}
