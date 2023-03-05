using System.Threading.Tasks;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.Database;

public sealed class SearchTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task Search_Success()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams, default);

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
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams, default);

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
    public async Task Search_Success_InvalidSmallPageNumber()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams, default);

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
    public async Task Search_Success_InvalidSmallPageSize()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams, default);

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
    public async Task Search_Success_InvalidBigPageSize()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await ApiClient.SearchDatabase(queryParams, paginationParams, default);

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
