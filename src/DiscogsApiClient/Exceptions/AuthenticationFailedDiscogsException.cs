namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that the authentication failed.
/// </summary>
public sealed class AuthenticationFailedDiscogsException : Exception
{
    public AuthenticationFailedDiscogsException() { }
    public AuthenticationFailedDiscogsException(string? message) : base(message) { }
    public AuthenticationFailedDiscogsException(string? message, Exception? innerException) : base(message, innerException) { }
}
