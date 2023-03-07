namespace DiscogsApiClient.Contract;

/// <summary>
/// Represents an error message returned from the Discogs Api.
/// </summary>
/// <param name="Message">The error message.</param>
public sealed record ErrorMessage(
    [property:JsonPropertyName("message")]
    string Message);
