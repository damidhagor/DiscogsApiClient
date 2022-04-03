namespace DiscogsApiClient.Exceptions;

public class ResourceNotFoundDiscogsException : Exception
{
    public ResourceNotFoundDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}
