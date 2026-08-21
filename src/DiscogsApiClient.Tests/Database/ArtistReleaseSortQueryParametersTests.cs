using System.Net;
using Microsoft.Extensions.DependencyInjection;
using static DiscogsApiClient.QueryParameters.ArtistReleaseSortQueryParameters;

namespace DiscogsApiClient.Tests.Database;

public sealed class ArtistReleaseSortQueryParametersTests
{
    private const string EmptyResponse =
        """
        {"pagination":{"page":1,"pages":1,"per_page":50,"items":0,"urls":{"next":"","last":""}},"releases":[]}
        """;

    [Test]
    public async Task GetArtistReleases_ShouldSendNoQueryString_WhenNoSortIsProvided(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(null, null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetArtistReleases_ShouldSendNoQueryString_WhenSortIsEmpty(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(), null, cancellationToken);

        await Assert.That(query).IsEqualTo("");
    }

    [Test]
    public async Task GetArtistReleases_ShouldSendSingleParameter_WhenOnlySortPropertyIsSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortProperty: SortableProperty.Title), null, cancellationToken);

        await Assert.That(query).IsEqualTo("?sort=title");
    }

    [Test]
    public async Task GetArtistReleases_ShouldSeparateParametersWithAmpersand_WhenBothFieldsAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortProperty: SortableProperty.Year, SortOrder: SortOrder.Descending), null, cancellationToken);

        await Assert.That(query).IsEqualTo("?sort=year&sort_order=desc");
    }

    [Test]
    [Arguments(SortableProperty.Year, "year")]
    [Arguments(SortableProperty.Title, "title")]
    [Arguments(SortableProperty.Format, "format")]
    public async Task GetArtistReleases_ShouldMapEverySortableProperty_WhenSortPropertyIsSet(SortableProperty sortProperty, string expectedAlias, CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortProperty: sortProperty), null, cancellationToken);

        await Assert.That(query).IsEqualTo($"?sort={expectedAlias}");
    }

    [Test]
    [Arguments(SortOrder.Ascending, "asc")]
    [Arguments(SortOrder.Descending, "desc")]
    public async Task GetArtistReleases_ShouldMapEverySortOrder_WhenSortOrderIsSet(SortOrder sortOrder, string expectedAlias, CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(new(SortOrder: sortOrder), null, cancellationToken);

        await Assert.That(query).IsEqualTo($"?sort_order={expectedAlias}");
    }

    [Test]
    public async Task GetArtistReleases_ShouldAppendSortParametersAfterPagination_WhenBothAreSet(CancellationToken cancellationToken)
    {
        var query = await CaptureQuery(
            new(SortProperty: SortableProperty.Format),
            new() { Page = 2 },
            cancellationToken);

        await Assert.That(query).IsEqualTo("?page=2&sort=format");
    }

    private static async Task<string> CaptureQuery(
        ArtistReleaseSortQueryParameters? sortQueryParameters,
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

        await apiClient.GetArtistReleases(1, paginationQueryParameters, sortQueryParameters, cancellationToken);

        return capturedUri!.Query;
    }
}
