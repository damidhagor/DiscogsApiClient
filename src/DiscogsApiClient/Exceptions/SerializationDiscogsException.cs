namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that either the serialization of a request's payload or deserialization of a response failed.
/// </summary>
public sealed class SerializationDiscogsException : Exception
{
    public SerializationDiscogsException() { }
    public SerializationDiscogsException(string? message) : base(message) { }
    public SerializationDiscogsException(string? message, Exception? innerException) : base(message, innerException) { }
}
