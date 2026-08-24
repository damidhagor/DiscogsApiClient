using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Currency query parameters used for retrieving or mutating Marketplace listings.
/// </summary>
/// <param name="CurrencyAbbreviation">
/// The currency for marketplace data. Defaults to the authenticated user's currency.
/// </param>
public sealed record MarketplaceCurrencyQueryParameters(
    [property: AliasAs("curr_abbr")]
    MarketplaceCurrency? CurrencyAbbreviation = default);
