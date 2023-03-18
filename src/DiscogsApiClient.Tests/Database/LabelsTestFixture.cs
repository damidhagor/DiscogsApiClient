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
        Assert.DoesNotThrow(() => new Uri(label.ResourceUrl));
        Assert.AreEqual("Nuclear Blast", label.Name);
        Assert.IsFalse(string.IsNullOrWhiteSpace(label.ContactInfo));
        Assert.IsFalse(string.IsNullOrWhiteSpace(label.Profile));
        Assert.DoesNotThrow(() => new Uri(label.Uri));
        Assert.DoesNotThrow(() => new Uri(label.ReleasesUrl));

        Assert.Less(0, label.Images.Count);

        Assert.IsNotNull(label.ParentLabel);
        Assert.AreEqual(222987, label.ParentLabel.Id);
        Assert.AreEqual("Nuclear Blast GmbH", label.ParentLabel.Name);
        Assert.DoesNotThrow(() => new Uri(label.ParentLabel.ResourceUrl));

        Assert.Less(0, label.SubLabels.Count);
        foreach (var subLabel in label.SubLabels)
        {
            Assert.IsNotNull(subLabel);
            Assert.Greater(subLabel.Id, 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(subLabel.Name));
            Assert.DoesNotThrow(() => new Uri(subLabel.ResourceUrl));
        }

        Assert.Less(0, label.Urls.Count);
        foreach (var url in label.Urls)
        {
            Assert.DoesNotThrow(() => new Uri(url));
        }
    }

    [Test]
    public void GetLabel_LabelId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetLabel(-1, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetLabel(0, default));
    }

    [Test]
    public void GetLabel_NotExistingLabelId()
    {
        var labelId = int.MaxValue;

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
        Assert.DoesNotThrow(() => new Uri(response.Pagination.Urls.NextPageUrl));
        Assert.DoesNotThrow(() => new Uri(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(50, response.Releases.Count);

        var release = response.Releases.First();
        Assert.IsNotNull(release);
        Assert.DoesNotThrow(() => new Uri(release.ResourceUrl));
        Assert.DoesNotThrow(() => new Uri(release.ThumbnailUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Title));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Status));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Format));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.CatalogNumber));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Artist));
        Assert.Greater(release.Year, 0);
        Assert.IsNotNull(release.Statistics);
        Assert.IsNotNull(release.Statistics.CommunityStatistics);
        Assert.Greater(release.Statistics.CommunityStatistics.ReleasesInWantlistCount, 0);
        Assert.Greater(release.Statistics.CommunityStatistics.ReleasesInCollectionCount, 0);
        Assert.GreaterOrEqual(release.Statistics.UserStatistics.ReleasesInWantlistCount, 0);
        Assert.GreaterOrEqual(release.Statistics.UserStatistics.ReleasesInCollectionCount, 0);
    }

    [Test]
    public void GetLabelReleases_NotExistingLabelId()
    {
        var labelId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetLabelReleases(labelId, default!, default));
    }

    [Test]
    public void GetLabelReleases_LabelId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetLabelReleases(-1, default!, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetLabelReleases(0, default!, default));
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
    public void GetLabelReleases_InvalidBigPageNumber()
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetLabelReleases(labelId, paginationParams, default));
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

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, default);
        var itemCount = response.Pagination.TotalItems;
        var summedUpItemCount = response.Releases.Count;

        for (var p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await ApiClient.GetLabelReleases(labelId, paginationParams, default);
            summedUpItemCount += response.Releases.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }
}
