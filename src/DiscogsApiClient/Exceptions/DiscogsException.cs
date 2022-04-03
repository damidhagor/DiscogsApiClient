namespace DiscogsApiClient.Exceptions;

public class DiscogsException : Exception
{
    public DiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}