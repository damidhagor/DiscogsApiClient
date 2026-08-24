using System.Net;
using Microsoft.Extensions.DependencyInjection;
using static DiscogsApiClient.QueryParameters.MarketplaceInventoryQueryParameters;

namespace DiscogsApiClient.Tests.Marketplace;

public sealed class MarketplaceInventoryQueryParametersTests
{
    private const string EmptyResponse =
        """
        {"pagination":{"page":1,"pages":1,"per_page":50,"items":0,"urls":{}},"listings":[]}
        """;

    [Test]
    public async Task GetInventory_ShouldSendNoQueryString_WhenNoParametersAreProvided(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(null, null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetInventory_ShouldSendNoQueryString_WhenQueryParametersAreEmpty(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(), null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetInventory_ShouldSendStatus_WhenOnlyStatusIsSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(Status: "for sale"), null, cancellationToken);

        await Assert.That(query).IsEqualTo("?status=for%20sale");
    }

    [Test]
    public async Task GetInventory_ShouldSeparateParametersWithAmpersand_WhenAllFieldsAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(
            new(Status: "for sale", SortProperty: SortableProperty.Price, SortOrder: SortOrder.Descending),
            null,
            cancellationToken);

        await Assert.That(query).IsEqualTo("?status=for%20sale&sort=price&sort_order=desc");
    }

    [Test]
    [Arguments(SortableProperty.Listed, "listed")]
    [Arguments(SortableProperty.Price, "price")]
    [Arguments(SortableProperty.Item, "item")]
    [Arguments(SortableProperty.Artist, "artist")]
    [Arguments(SortableProperty.Label, "label")]
    [Arguments(SortableProperty.CatalogNumber, "catno")]
    [Arguments(SortableProperty.Audio, "audio")]
    [Arguments(SortableProperty.Status, "status")]
    public async Task GetInventory_ShouldMapEverySortableProperty_WhenSortPropertyIsSet(SortableProperty sortProperty, string expectedAlias, CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortProperty: sortProperty), null, cancellationToken);

        await Assert.That(query).IsEqualTo($"?sort={expectedAlias}");
    }

    [Test]
    public async Task GetInventory_ShouldAppendInventoryParametersAfterPagination_WhenBothAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(
            new(SortProperty: SortableProperty.Artist),
            new() { Page = 2 },
            cancellationToken);

        await Assert.That(query).IsEqualTo("?page=2&sort=artist");
    }

    private static async Task<string> CaptureQuery(
        MarketplaceInventoryQueryParameters? inventoryQueryParameters,
        PaginationQueryParameters? paginationQueryParameters,
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
                            Content = new StringContent(EmptyResponse),
                        };
                    })))
            .BuildServiceProvider();

        var apiClient = serviceProvider.GetRequiredService<IDiscogsApiClient>();

        await apiClient.GetInventory("someuser", paginationQueryParameters, inventoryQueryParameters, cancellationToken);

        return capturedUri!.Query;
    }
}
