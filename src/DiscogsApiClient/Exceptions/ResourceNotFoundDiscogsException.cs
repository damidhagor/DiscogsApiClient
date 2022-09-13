namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that the resource accessed by the request was not found or does not exist.
/// </summary>
public sealed class ResourceNotFoundDiscogsException : Exception
{
    public ResourceNotFoundDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}
