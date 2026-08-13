namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that either the serialization of a request's payload or deserialization of a response failed.
/// </summary>
public sealed class SerializationDiscogsException : Exception
{
    public SerializationDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}
