namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that a request failed because it needed to be authenticated but wasn't.
/// </summary>
public sealed class UnauthorizedDiscogsException : Exception
{
    public UnauthorizedDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}
