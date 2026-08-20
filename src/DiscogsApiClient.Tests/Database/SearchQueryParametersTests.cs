using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace DiscogsApiClient.Tests.Database;

public sealed class SearchQueryParametersTests
{
    private const string EmptyResponse =
        """
        {"pagination":{"page":1,"pages":1,"per_page":50,"items":0,"urls":{"next":"","last":""}},"results":[]}
        """;

    [Test]
    public async Task SearchDatabase_ShouldSendNoQueryString_WhenNoParametersAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(), null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task SearchDatabase_ShouldSendSingleParameter_WhenOnlyOneFieldIsSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new() { Query = "hammerfall" }, null, cancellationToken);

        await Assert.That(query).IsEqualTo("?q=hammerfall");
    }

    [Test]
    public async Task SearchDatabase_ShouldSeparateParametersWithAmpersand_WhenMultipleFieldsAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new() { Query = "hammerfall", Type = "release" }, null, cancellationToken);

        await Assert.That(query).IsEqualTo("?q=hammerfall&type=release");
    }

    [Test]
    public async Task SearchDatabase_ShouldIncludeEveryParameter_WhenAllFieldsAreSet(CancellationToken cancellationToken)
    {
        var searchParams = new SearchQueryParameters(
            Query: "hammerfall",
            Type: "release",
            Title: "Jensen - Hammerfall",
            ReleaseTitle: "Hammerfall",
            Artist: "Jensen",
            Country: "US",
            Year: "2005",
            Format: "Vinyl",
            Barcode: "1234567890");

        var query = await CaptureQuery(searchParams, null, cancellationToken);

        await Assert.That(query).IsEqualTo(
            "?q=hammerfall&type=release&title=Jensen%20-%20Hammerfall&release_title=Hammerfall&artist=Jensen&country=US&year=2005&format=Vinyl&barcode=1234567890");
    }

    [Test]
    public async Task SearchDatabase_ShouldAppendPaginationAfterSearchParameters_WhenBothAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(
            new() { Query = "hammerfall" },
            new() { Page = 2, PageSize = 25 },
            cancellationToken);

        await Assert.That(query).IsEqualTo("?q=hammerfall&page=2&per_page=25");
    }

    [Test]
    public async Task SearchDatabase_ShouldSendOnlyPagination_WhenSearchParametersAreEmpty(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(), new() { Page = 3 }, cancellationToken);

        await Assert.That(query).IsEqualTo("?page=3");
    }

    [Test]
    public async Task SearchDatabase_ShouldClampPageSize_WhenPageSizeExceedsMaximum(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(), new() { PageSize = 500 }, cancellationToken);

        await Assert.That(query).IsEqualTo("?per_page=100");
    }

    private static async Task<string> CaptureQuery(
        SearchQueryParameters searchQueryParameters,
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

        await apiClient.SearchDatabase(searchQueryParameters, paginationQueryParameters, cancellationToken);

        return capturedUri!.Query;
    }
}
