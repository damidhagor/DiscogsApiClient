namespace DiscogsApiClient.Exceptions;

public class SerializationDiscogsException : Exception
{
    public SerializationDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}
