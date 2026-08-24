using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// Currency types supported by the Discogs Marketplace.
/// </summary>
[GenerateJsonConverter]
public enum MarketplaceCurrency
{
    [AliasAs("USD")]
    Usd,
    [AliasAs("GBP")]
    Gbp,
    [AliasAs("EUR")]
    Eur,
    [AliasAs("CAD")]
    Cad,
    [AliasAs("AUD")]
    Aud,
    [AliasAs("JPY")]
    Jpy,
    [AliasAs("CHF")]
    Chf,
    [AliasAs("MXN")]
    Mxn,
    [AliasAs("BRL")]
    Brl,
    [AliasAs("NZD")]
    Nzd,
    [AliasAs("SEK")]
    Sek,
    [AliasAs("ZAR")]
    Zar
}
