using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace DiscogsApiClient.Tests.Marketplace;

public sealed class MarketplaceCurrencyQueryParametersTests
{
    private const string ListingResponse =
        """
        {
          "id": 1,
          "status": "For Sale",
          "condition": "Mint (M)",
          "price": {"currency":"USD","value":10},
          "allow_offers": false,
          "posted": "2014-07-15T12:55:01-07:00",
          "uri": "https://www.discogs.com/sell/item/1",
          "seller": {"id":1,"username":"seller","resource_url":"https://api.discogs.com/users/seller"},
          "release": {"id":1,"description":"desc","year":2020,"resource_url":"https://api.discogs.com/releases/1"},
          "resource_url": "https://api.discogs.com/marketplace/listings/1",
          "audio": false
        }
        """;

    [Test]
    public async Task GetMarketplaceListing_ShouldSendNoQueryString_WhenNoCurrencyIsProvided(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetMarketplaceListing_ShouldSendNoQueryString_WhenCurrencyIsEmpty(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(), cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    [Arguments(MarketplaceCurrency.Usd, "USD")]
    [Arguments(MarketplaceCurrency.Gbp, "GBP")]
    [Arguments(MarketplaceCurrency.Eur, "EUR")]
    [Arguments(MarketplaceCurrency.Cad, "CAD")]
    [Arguments(MarketplaceCurrency.Aud, "AUD")]
    [Arguments(MarketplaceCurrency.Jpy, "JPY")]
    [Arguments(MarketplaceCurrency.Chf, "CHF")]
    [Arguments(MarketplaceCurrency.Mxn, "MXN")]
    [Arguments(MarketplaceCurrency.Brl, "BRL")]
    [Arguments(MarketplaceCurrency.Nzd, "NZD")]
    [Arguments(MarketplaceCurrency.Sek, "SEK")]
    [Arguments(MarketplaceCurrency.Zar, "ZAR")]
    public async Task GetMarketplaceListing_ShouldMapEveryCurrency_WhenCurrencyIsSet(MarketplaceCurrency currency, string expectedAlias, CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(CurrencyAbbreviation: currency), cancellationToken);

        await Assert.That(query).IsEqualTo($"?curr_abbr={expectedAlias}");
    }

    private static async Task<string> CaptureQuery(
        MarketplaceCurrencyQueryParameters? currencyQueryParameters,
        CancellationToken cancellationToken)
    {
        Uri? capturedUri = null;

        using var serviceProvider = new ServiceCollection()
            .AddDiscogsApiClient(
                options =>
                {
                    options.BaseUrl = "https://api.discogs.com";
                    options.UserAgent = "TestUserAgent";
                },
                configureClient: builder => builder.ConfigurePrimaryHttpMessageHandler(
                    () => new HttpMessageHandlerFixture(request =>
                    {
                        capturedUri = request.RequestUri;
                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(ListingResponse),
                        };
                    })))
            .BuildServiceProvider();

        var apiClient = serviceProvider.GetRequiredService<IDiscogsApiClient>();

        await apiClient.GetMarketplaceListing(1, currencyQueryParameters, cancellationToken);

        return capturedUri!.Query;
    }
}
