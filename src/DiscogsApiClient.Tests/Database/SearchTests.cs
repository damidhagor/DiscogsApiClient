using DiscogsApiClient.Contract.Search;

namespace DiscogsApiClient.Tests.Database;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class SearchTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();

    [Test]
    public async Task SearchDatabase_ShouldReturnResults_WhenQueryIsValid(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };

        var response = await _apiClient.SearchDatabase(queryParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.ItemsPerPage).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Results.Count).IsEqualTo(50);
    }

    [Test]
    public async Task SearchDatabase_ShouldReturnResults_WhenNoQueryIsProvided(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters();

        var response = await _apiClient.SearchDatabase(queryParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.ItemsPerPage).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Results.Count).IsEqualTo(50);
    }

    [Test]
    public async Task SearchDatabase_ShouldFilterByType_WhenTypeIsSpecified(CancellationToken cancellationToken)
    {
        // Artist
        var searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "artist" };
        var response = await _apiClient.SearchDatabase(searchParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Results.Count).IsGreaterThan(0);
        await Assert.That(response.Results.All(r => r.ResultType == SearchResultType.Artist)).IsTrue();

        // Master
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "master" };
        response = await _apiClient.SearchDatabase(searchParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Results.Count).IsGreaterThan(0);
        await Assert.That(response.Results.All(r => r.ResultType == SearchResultType.Master)).IsTrue();

        // Release
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "release" };
        response = await _apiClient.SearchDatabase(searchParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Results.Count).IsGreaterThan(0);
        await Assert.That(response.Results.All(r => r.ResultType == SearchResultType.Release)).IsTrue();

        // Label
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "label" };
        response = await _apiClient.SearchDatabase(searchParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Results.Count).IsGreaterThan(0);
        await Assert.That(response.Results.All(r => r.ResultType == SearchResultType.Label)).IsTrue();
    }


    [Test]
    public async Task SearchDatabase_ShouldDefaultToPageOne_WhenPageNumberIsNegative(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await _apiClient.SearchDatabase(queryParams, paginationParams, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(50);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Results.Count).IsEqualTo(50);
    }

    [Test]
    public async Task SearchDatabase_ShouldThrowResourceNotFoundException_WhenPageNumberIsTooLarge(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        // Should fail with 404 but Discord seems to enounter an internal error instead!
        await Assert.That(async () => await _apiClient.SearchDatabase(queryParams, paginationParams, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task SearchDatabase_ShouldClampPageSizeToMinimum_WhenPageSizeIsNegative(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await _apiClient.SearchDatabase(queryParams, paginationParams, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(1);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Results.Count).IsEqualTo(1);
    }

    [Test]
    public async Task SearchDatabase_ShouldClampPageSizeToMaximum_WhenPageSizeIsTooLarge(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await _apiClient.SearchDatabase(queryParams, paginationParams, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(100);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Results.Count).IsEqualTo(100);
    }
}
