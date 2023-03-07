namespace DiscogsApiClient.Exceptions;

/// <summary>
/// A <see cref="DiscogsException"/> representing that the authentication failed.
/// </summary>
public sealed class AuthenticationFailedDiscogsException : Exception
{
    public AuthenticationFailedDiscogsException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    { }
}
