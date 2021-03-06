using System;
using System.Threading.Tasks;
using DiscogsApiClient.QueryParameters;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.Database;

public class SearchTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task Search_Success()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters(1, 50);

        var response = await ApiClient.SearchDatabaseAsync(queryParams, paginationParams, default);

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.Less(0, response.Pagination.PerPage);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(50, response.Results.Count);
    }

    [Test]
    public async Task Search_NoQuery_Success()
    {
        var queryParams = new SearchQueryParameters();
        var paginationParams = new PaginationQueryParameters(1, 50);

        var response = await ApiClient.SearchDatabaseAsync(queryParams, paginationParams, default);

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.Less(0, response.Pagination.PerPage);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(50, response.Results.Count);
    }


    [Test]
    public async Task Search_Success_InvalidSmallPageNumber()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters(-1, 50);

        var response = await ApiClient.SearchDatabaseAsync(queryParams, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(50, response.Results.Count);
    }

    [Test]
    public async Task Search_Success_InvalidSmallPageSize()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters(1, -1);

        var response = await ApiClient.SearchDatabaseAsync(queryParams, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(1, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(1, response.Results.Count);
    }

    [Test]
    public async Task Search_Success_InvalidBigPageSize()
    {
        var queryParams = new SearchQueryParameters { Query = "hammerfall" };
        var paginationParams = new PaginationQueryParameters(1, int.MaxValue);

        var response = await ApiClient.SearchDatabaseAsync(queryParams, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(100, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(100, response.Results.Count);
    }
}