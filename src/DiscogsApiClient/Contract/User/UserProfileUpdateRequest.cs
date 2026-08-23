namespace DiscogsApiClient.Contract.User;

/// <summary>
/// Request for updating a user's profile. All properties are optional. <see langword="null"/> leaves the
/// corresponding profile field unchanged; an empty string clears it, except for <see cref="CurrencyAbbreviation"/>,
/// which is left unchanged by both <see langword="null"/> and an empty string and can only be updated to
/// another valid currency code.
/// </summary>
/// <param name="Name">The real name of the user.</param>
/// <param name="HomePage">The user's website.</param>
/// <param name="Location">The geographical location of the user.</param>
/// <param name="Profile">Biographical information about the user.</param>
/// <param name="CurrencyAbbreviation">
/// The three-letter currency abbreviation for marketplace data (e.g. "USD", "EUR"). An unsupported value
/// causes the request to fail with a <see cref="ResourceNotFoundDiscogsException"/>.
/// </param>
public sealed record UserProfileUpdateRequest(
    [property:JsonPropertyName("name")]
    string? Name = null,
    [property:JsonPropertyName("home_page")]
    string? HomePage = null,
    [property:JsonPropertyName("location")]
    string? Location = null,
    [property:JsonPropertyName("profile")]
    string? Profile = null,
    [property:JsonPropertyName("curr_abbr")]
    string? CurrencyAbbreviation = null);
