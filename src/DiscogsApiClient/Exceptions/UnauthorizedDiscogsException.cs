namespace DiscogsApiClient.Exceptions;

public class UnauthorizedDiscogsException : Exception
{
    public UnauthorizedDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}