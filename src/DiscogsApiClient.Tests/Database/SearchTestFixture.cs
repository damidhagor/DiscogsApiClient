using DiscogsApiClient.Contract.Search;

namespace DiscogsApiClient.Tests.Database;

public sealed class SearchTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task Search_Success()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };

        var response = await ApiClient.SearchDatabase(queryParams);

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.Less(0, response.Pagination.ItemsPerPage);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));
        Assert.AreEqual(50, response.Results.Count);
    }

    [Test]
    public async Task Search_NoQuery_Success()
    {
        var queryParams = new SearchQueryParameters();

        var response = await ApiClient.SearchDatabase(queryParams);

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.Less(0, response.Pagination.ItemsPerPage);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));
        Assert.AreEqual(50, response.Results.Count);
    }

    [Test]
    public async Task Search_Type_Success()
    {
        // Artist
        var searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "artist" };
        var response = await ApiClient.SearchDatabase(searchParams);

        Assert.IsNotNull(response);
        Assert.Less(0, response.Results.Count);
        Assert.IsTrue(response.Results.All(r => r.ResultType == SearchResultType.Artist));

        // Master
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "master" };
        response = await ApiClient.SearchDatabase(searchParams);

        Assert.IsNotNull(response);
        Assert.Less(0, response.Results.Count);
        Assert.IsTrue(response.Results.All(r => r.ResultType == SearchResultType.Master));

        // Release
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "release" };
        response = await ApiClient.SearchDatabase(searchParams);

        Assert.IsNotNull(response);
        Assert.Less(0, response.Results.Count);
        Assert.IsTrue(response.Results.All(r => r.ResultType == SearchResultType.Release));

        // Label
        searchParams = new SearchQueryParameters { Query = "hammerfall", Type = "label" };
        response = await ApiClient.SearchDatabase(searchParams);

        Assert.IsNotNull(response);
        Assert.Less(0, response.Results.Count);
        Assert.IsTrue(response.Results.All(r => r.ResultType == SearchResultType.Label));
    }


    [Test]
    public async Task Search_InvalidSmallPageNumber()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));
        Assert.AreEqual(50, response.Results.Count);
    }

    [Test]
    public void Search_InvalidBigPageNumber()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        // Should fail with 404 but Discord seems to enounter an internal error instead!
        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.SearchDatabase(queryParams, paginationParams));
    }

    [Test]
    public async Task Search_InvalidSmallPageSize()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(1, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));
        Assert.AreEqual(1, response.Results.Count);
    }

    [Test]
    public async Task Search_InvalidBigPageSize()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(100, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));
        Assert.AreEqual(100, response.Results.Count);
    }
}
