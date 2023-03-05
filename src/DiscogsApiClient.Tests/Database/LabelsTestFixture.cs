using System.Threading.Tasks;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.Database;

public sealed class LabelsTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetLabel_Success()
    {
        var labelId = 11499;

        var label = await ApiClient.GetLabel(labelId, default);

        Assert.IsNotNull(label);
        Assert.AreEqual(labelId, label.Id);
        Assert.AreEqual("Nuclear Blast", label.Name);
        Assert.IsFalse(string.IsNullOrWhiteSpace(label.ContactInfo));
        Assert.IsFalse(string.IsNullOrWhiteSpace(label.Profile));
        Assert.IsFalse(string.IsNullOrWhiteSpace(label.ResourceUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(label.Uri));
        Assert.IsFalse(string.IsNullOrWhiteSpace(label.ReleasesUrl));
        Assert.Less(0, label.Images.Count);
        Assert.IsNotNull(label.ParentLabel);
        Assert.AreEqual(222987, label.ParentLabel.Id);
        Assert.AreEqual("Nuclear Blast GmbH", label.ParentLabel.Name);
        Assert.IsFalse(string.IsNullOrWhiteSpace(label.ParentLabel.ResourceUrl));
        Assert.Less(0, label.Sublabels.Count);
        Assert.Less(0, label.Urls.Count);
    }

    [Test]
    public void GetLabel_NotExistingLabelId()
    {
        var labelId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetLabel(labelId, default));
    }

    [Test]
    public async Task GetLabelReleases_Success()
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(50, response.Releases.Count);
    }

    [Test]
    public async Task GetLabelReleases_Success_InvalidSmallPageNumber()
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(50, response.Releases.Count);
    }

    [Test]
    public async Task GetLabelReleases_Success_InvalidSmallPageSize()
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(1, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(1, response.Releases.Count);
    }

    [Test]
    public async Task GetLabelReleases_Success_InvalidBigPageSize()
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(100, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(100, response.Releases.Count);
    }

    [Test]
    public async Task GetLabelAllReleases_Success()
    {
        var labelId = 34650;

        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var itemCount = 0;
        var summedUpItemCount = 0;

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, default);
        itemCount = response.Pagination.TotalItems;
        summedUpItemCount += response.Releases.Count;

        for (int p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await ApiClient.GetLabelReleases(labelId, paginationParams, default);
            summedUpItemCount += response.Releases.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }

    [Test]
    public void GetLabelReleases_NotExistingLabelId()
    {
        var labelId = -1;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetLabelReleases(labelId, paginationParams, default));
    }

    [Test]
    public void GetLabelReleases_InvalidBigPageNumber()
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetLabelReleases(labelId, paginationParams, default));
    }
}
