namespace DiscogsApiClient.Exceptions;

/// <summary>
/// Single place for all used exception messages.
/// </summary>
internal static class ExceptionMessages
{
    public static string GetUserAgentMissingMessage() => "The user agent string must not be empty.";

    public static string GetSerializationNoResultMessage() => "Serialization returned no result.";

    public static string GetSerializationFailedMessage() => "Serialization failed.";

    public static string GetDeserializationNoResultMessage() => "Deserialization returned no result.";

    public static string GetDeserializationFailedMessage() => "Deserialization failed.";

    public static string GetRequestUriMissingMessage() => "The request uri must be set.";

    public static string GetWrongAuthenticationRequestTypeMessage(string type) => $"The provided authentication request must be of type {type}";

    public static string GetRequestNotDeserializedMessage() => "Request deserialization gave no result.";

    public static string GetNoUsernameProvidedMessage() => "No username was provided for the request.";

    public static string GetNoFolderNameProvidedMessage() => "No folder name was provided for the request.";
}
