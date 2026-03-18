namespace DiscogsApiClient.Tests.Database;

public sealed class LabelsTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetLabel_Success(CancellationToken cancellationToken)
    {
        var labelId = 11499;

        var label = await ApiClient.GetLabel(labelId, cancellationToken);

        await Assert.That(label).IsNotNull();
        await Assert.That(label.Id).IsEqualTo(labelId);
        await Assert.That(() => new Uri(label.ResourceUrl)).ThrowsNothing();
        await Assert.That(label.Name).IsEqualTo("Nuclear Blast");
        await Assert.That(label.ContactInfo).IsNotNullOrWhiteSpace();
        await Assert.That(label.Profile).IsNotNullOrWhiteSpace();
        await Assert.That(() => new Uri(label.Uri)).ThrowsNothing();
        await Assert.That(() => new Uri(label.ReleasesUrl)).ThrowsNothing();

        await Assert.That(label.Images.Count).IsGreaterThan(0);

        await Assert.That(label.ParentLabel).IsNotNull();
        await Assert.That(label.ParentLabel.Id).IsEqualTo(222987);
        await Assert.That(label.ParentLabel.Name).IsEqualTo("Nuclear Blast GmbH");
        await Assert.That(() => new Uri(label.ParentLabel.ResourceUrl)).ThrowsNothing();

        await Assert.That(label.SubLabels.Count).IsGreaterThan(0);
        foreach (var subLabel in label.SubLabels)
        {
            await Assert.That(subLabel).IsNotNull();
            await Assert.That(subLabel.Id).IsGreaterThan(0);
            await Assert.That(subLabel.Name).IsNotNullOrWhiteSpace();
            await Assert.That(() => new Uri(subLabel.ResourceUrl)).ThrowsNothing();
        }

        await Assert.That(label.Urls.Count).IsGreaterThan(0);
        foreach (var url in label.Urls)
        {
            await Assert.That(() => new Uri(url)).ThrowsNothing();
        }
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetLabel_LabelId_Guard(int labelId, CancellationToken cancellationToken)
    {
        await Assert.That(async () => await ApiClient.GetLabel(labelId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task GetLabel_NotExistingLabelId(CancellationToken cancellationToken)
    {
        var labelId = int.MaxValue;

        await Assert.That(async () => await ApiClient.GetLabel(labelId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }


    [Test]
    public async Task GetLabelReleases_Success(CancellationToken cancellationToken)
    {
        var labelId = 11499;

        var response = await ApiClient.GetLabelReleases(labelId, null, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(50);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        await Assert.That(() => new Uri(response.Pagination.Urls.NextPageUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(response.Pagination.Urls.LastPageUrl)).ThrowsNothing();

        await Assert.That(response.Releases).IsNotNull();
        await Assert.That(response.Releases.Count).IsEqualTo(50);

        var release = response.Releases.First();
        await Assert.That(release).IsNotNull();
        await Assert.That(() => new Uri(release.ResourceUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(release.ThumbnailUrl)).ThrowsNothing();
        await Assert.That(release.Title).IsNotNullOrWhiteSpace();
        await Assert.That(release.Status).IsNotNullOrWhiteSpace();
        await Assert.That(release.Format).IsNotNullOrWhiteSpace();
        await Assert.That(release.CatalogNumber).IsNotNullOrWhiteSpace();
        await Assert.That(release.Artist).IsNotNullOrWhiteSpace();
        await Assert.That(release.Year).IsGreaterThan(0);
        await Assert.That(release.Statistics).IsNotNull();
        await Assert.That(release.Statistics.CommunityStatistics).IsNotNull();
        await Assert.That(release.Statistics.CommunityStatistics.ReleasesInWantlistCount).IsGreaterThan(0);
        await Assert.That(release.Statistics.CommunityStatistics.ReleasesInCollectionCount).IsGreaterThan(0);
        await Assert.That(release.Statistics.UserStatistics.ReleasesInWantlistCount).IsGreaterThanOrEqualTo(0);
        await Assert.That(release.Statistics.UserStatistics.ReleasesInCollectionCount).IsGreaterThanOrEqualTo(0);
    }

    [Test]
    public async Task GetLabelReleases_NotExistingLabelId(CancellationToken cancellationToken)
    {
        var labelId = int.MaxValue;

        await Assert.That(async () => await ApiClient.GetLabelReleases(labelId, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetLabelReleases_LabelId_Guard(int labelId, CancellationToken cancellationToken)
    {
        await Assert.That(async () => await ApiClient.GetLabelReleases(labelId, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task GetLabelReleases_Success_InvalidSmallPageNumber(CancellationToken cancellationToken)
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(50);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNullOrWhiteSpace();

        await Assert.That(response.Releases).IsNotNull();
        await Assert.That(response.Releases.Count).IsEqualTo(50);
    }

    [Test]
    public async Task GetLabelReleases_InvalidBigPageNumber(CancellationToken cancellationToken)
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        await Assert.That(async () => await ApiClient.GetLabelReleases(labelId, paginationParams, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task GetLabelReleases_Success_InvalidSmallPageSize(CancellationToken cancellationToken)
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(1);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNullOrWhiteSpace();

        await Assert.That(response.Releases).IsNotNull();
        await Assert.That(response.Releases.Count).IsEqualTo(1);
    }

    [Test]
    public async Task GetLabelReleases_Success_InvalidBigPageSize(CancellationToken cancellationToken)
    {
        var labelId = 11499;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(100);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNullOrWhiteSpace();

        await Assert.That(response.Releases).IsNotNull();
        await Assert.That(response.Releases.Count).IsEqualTo(100);
    }

    [Test]
    public async Task GetLabelAllReleases_Success(CancellationToken cancellationToken)
    {
        var labelId = 34650;

        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await ApiClient.GetLabelReleases(labelId, paginationParams, cancellationToken);
        var itemCount = response.Pagination.TotalItems;
        var summedUpItemCount = response.Releases.Count;

        for (var p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await ApiClient.GetLabelReleases(labelId, paginationParams, cancellationToken);
            summedUpItemCount += response.Releases.Count;
        }

        await Assert.That(itemCount).IsEqualTo(summedUpItemCount);
    }
}
