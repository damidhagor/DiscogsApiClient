namespace DiscogsApiClient.Contract;

public sealed record ErrorMessage(
    [property:JsonPropertyName("message")]
    string Message);
