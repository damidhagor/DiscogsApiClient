using DiscogsApiClient.Contract.Search;

namespace DiscogsApiClient.Tests.Database;

public sealed class SearchTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task Search_Success(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };

        var response = await ApiClient.SearchDatabase(queryParams, null, cancellationToken);

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
    public async Task Search_NoQuery_Success(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters();

        var response = await ApiClient.SearchDatabase(queryParams, null, cancellationToken);

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
    public async Task Search_Type_Success(CancellationToken cancellationToken)
    {
        // Artist
        var searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "artist" };
        var response = await ApiClient.SearchDatabase(searchParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Results.Count).IsGreaterThan(0);
        await Assert.That(response.Results.All(r => r.ResultType == SearchResultType.Artist)).IsTrue();

        // Master
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "master" };
        response = await ApiClient.SearchDatabase(searchParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Results.Count).IsGreaterThan(0);
        await Assert.That(response.Results.All(r => r.ResultType == SearchResultType.Master)).IsTrue();

        // Release
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "release" };
        response = await ApiClient.SearchDatabase(searchParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Results.Count).IsGreaterThan(0);
        await Assert.That(response.Results.All(r => r.ResultType == SearchResultType.Release)).IsTrue();

        // Label
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "label" };
        response = await ApiClient.SearchDatabase(searchParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Results.Count).IsGreaterThan(0);
        await Assert.That(response.Results.All(r => r.ResultType == SearchResultType.Label)).IsTrue();
    }


    [Test]
    public async Task Search_InvalidSmallPageNumber(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams, cancellationToken);

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
    public async Task Search_InvalidBigPageNumber(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        // Should fail with 404 but Discord seems to enounter an internal error instead!
        await Assert.That(async () => await ApiClient.SearchDatabase(queryParams, paginationParams, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task Search_InvalidSmallPageSize(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams, cancellationToken);

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
    public async Task Search_InvalidBigPageSize(CancellationToken cancellationToken)
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams, cancellationToken);

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
