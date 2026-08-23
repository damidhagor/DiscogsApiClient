using System.Net;
using Microsoft.Extensions.DependencyInjection;
using static DiscogsApiClient.QueryParameters.ContributionSortQueryParameters;

namespace DiscogsApiClient.Tests.User;

public sealed class ContributionSortQueryParametersTests
{
    private const string EmptyResponse =
        """
        {"pagination":{"page":1,"pages":1,"per_page":50,"items":0,"urls":{}},"contributions":[]}
        """;

    [Test]
    public async Task GetContributions_ShouldSendNoQueryString_WhenNoSortIsProvided(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(null, null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetContributions_ShouldSendNoQueryString_WhenSortIsEmpty(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(), null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetContributions_ShouldSendSingleParameter_WhenOnlySortPropertyIsSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortProperty: SortableProperty.Rating), null, cancellationToken);

        await Assert.That(query).IsEqualTo("?sort=rating");
    }

    [Test]
    public async Task GetContributions_ShouldSeparateParametersWithAmpersand_WhenBothFieldsAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortProperty: SortableProperty.AddedAt, SortOrder: SortOrder.Ascending), null, cancellationToken);

        await Assert.That(query).IsEqualTo("?sort=added&sort_order=asc");
    }

    [Test]
    [Arguments(SortableProperty.Label, "label")]
    [Arguments(SortableProperty.Artist, "artist")]
    [Arguments(SortableProperty.Title, "title")]
    [Arguments(SortableProperty.CatalogNumber, "catno")]
    [Arguments(SortableProperty.Format, "format")]
    [Arguments(SortableProperty.Rating, "rating")]
    [Arguments(SortableProperty.Year, "year")]
    [Arguments(SortableProperty.AddedAt, "added")]
    public async Task GetContributions_ShouldMapEverySortableProperty_WhenSortPropertyIsSet(SortableProperty sortProperty, string expectedAlias, CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortProperty: sortProperty), null, cancellationToken);

        await Assert.That(query).IsEqualTo($"?sort={expectedAlias}");
    }

    [Test]
    [Arguments(SortOrder.Ascending, "asc")]
    [Arguments(SortOrder.Descending, "desc")]
    public async Task GetContributions_ShouldMapEverySortOrder_WhenSortOrderIsSet(SortOrder sortOrder, string expectedAlias, CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortOrder: sortOrder), null, cancellationToken);

        await Assert.That(query).IsEqualTo($"?sort_order={expectedAlias}");
    }

    [Test]
    public async Task GetContributions_ShouldAppendSortParametersAfterPagination_WhenBothAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(
            new(SortProperty: SortableProperty.Format),
            new() { Page = 2 },
            cancellationToken);

        await Assert.That(query).IsEqualTo("?page=2&sort=format");
    }

    private static async Task<string> CaptureQuery(
        ContributionSortQueryParameters? sortQueryParameters,
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

        await apiClient.GetContributions("tester", paginationQueryParameters, sortQueryParameters, cancellationToken);

        return capturedUri!.Query;
    }
}
