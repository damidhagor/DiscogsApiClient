using System.Net;
using Microsoft.Extensions.DependencyInjection;
using static DiscogsApiClient.QueryParameters.MasterReleaseVersionFilterQueryParameters;

namespace DiscogsApiClient.Tests.Database;

public sealed class MasterReleaseVersionFilterQueryParametersTests
{
    private const string EmptyResponse =
        """
        {"pagination":{"page":1,"pages":1,"per_page":50,"items":0,"urls":{"next":"","last":""}},"filters":null,"filter_facets":[],"versions":[]}
        """;

    [Test]
    public async Task GetMasterReleaseVersions_ShouldSendNoQueryString_WhenNoFilterIsProvided(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(null, null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldSendNoQueryString_WhenFilterIsEmpty(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(), null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldSendSingleParameter_WhenOnlyOneFieldIsSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(Format: "Vinyl"), null, cancellationToken);

        await Assert.That(query).IsEqualTo("?format=Vinyl");
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldSeparateParametersWithAmpersand_WhenMultipleFieldsAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(Format: "Vinyl", Country: "US"), null, cancellationToken);

        await Assert.That(query).IsEqualTo("?format=Vinyl&country=US");
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldIncludeEveryParameter_WhenAllFieldsAreSet(CancellationToken cancellationToken)
    {
        var filter = new MasterReleaseVersionFilterQueryParameters(
            Format: "Vinyl",
            Label: "Bastard Loud Records",
            Year: "2005",
            Country: "US",
            SortProperty: SortableProperty.CatalogNumber,
            SortOrder: SortOrder.Descending);

        var query = await CaptureQuery(filter, null, cancellationToken);

        await Assert.That(query).IsEqualTo("?format=Vinyl&label=Bastard%20Loud%20Records&released=2005&country=US&sort=catno&sort_order=desc");
    }

    [Test]
    [Arguments(SortableProperty.Year, "released")]
    [Arguments(SortableProperty.Title, "title")]
    [Arguments(SortableProperty.Format, "format")]
    [Arguments(SortableProperty.Label, "label")]
    [Arguments(SortableProperty.CatalogNumber, "catno")]
    [Arguments(SortableProperty.Country, "country")]
    public async Task GetMasterReleaseVersions_ShouldMapEverySortableProperty_WhenSortPropertyIsSet(SortableProperty sortProperty, string expectedAlias, CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortProperty: sortProperty), null, cancellationToken);

        await Assert.That(query).IsEqualTo($"?sort={expectedAlias}");
    }

    [Test]
    [Arguments(SortOrder.Ascending, "asc")]
    [Arguments(SortOrder.Descending, "desc")]
    public async Task GetMasterReleaseVersions_ShouldMapEverySortOrder_WhenSortOrderIsSet(SortOrder sortOrder, string expectedAlias, CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortOrder: sortOrder), null, cancellationToken);

        await Assert.That(query).IsEqualTo($"?sort_order={expectedAlias}");
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldAppendFilterParametersAfterPagination_WhenBothAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(
            new(Format: "Vinyl"),
            new() { Page = 2 },
            cancellationToken);

        await Assert.That(query).IsEqualTo("?page=2&format=Vinyl");
    }

    private static async Task<string> CaptureQuery(
        MasterReleaseVersionFilterQueryParameters? filterQueryParameters,
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

        await apiClient.GetMasterReleaseVersions(1, paginationQueryParameters, filterQueryParameters, cancellationToken);

        return capturedUri!.Query;
    }
}
