using static DiscogsApiClient.QueryParameters.MasterReleaseVersionFilterQueryParameters;

namespace DiscogsApiClient.Tests.Database;

public sealed class MasterReleaseTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetMasterRelease_Success()
    {
        var masterReleaseId = 156551;

        var masterRelease = await ApiClient.GetMasterRelease(masterReleaseId);

        Assert.IsNotNull(masterRelease);
        Assert.AreEqual(masterReleaseId, masterRelease.Id);
        Assert.Less(0, masterRelease.MainReleaseId);
        Assert.Less(0, masterRelease.MostRecentReleaseId);
        Assert.DoesNotThrow(() => new Uri(masterRelease.ResourceUrl));
        Assert.DoesNotThrow(() => new Uri(masterRelease.Uri));
        Assert.DoesNotThrow(() => new Uri(masterRelease.VersionsUrl));
        Assert.DoesNotThrow(() => new Uri(masterRelease.MainReleaseUrl));
        Assert.DoesNotThrow(() => new Uri(masterRelease.MostRecentReleaseUrl));
        Assert.Less(0, masterRelease.NumForSale);
        Assert.Less(0, masterRelease.LowestPrice);
        Assert.AreEqual(1997, masterRelease.Year);
        Assert.AreEqual("Glory To The Brave", masterRelease.Title);

        Assert.Less(0, masterRelease.Images.Count);
        foreach (var image in masterRelease.Images)
        {
            Assert.IsTrue(Enum.IsDefined(image.Type));
            Assert.DoesNotThrow(() => new Uri(image.ResourceUrl));
            Assert.DoesNotThrow(() => new Uri(image.ImageUri));
            Assert.DoesNotThrow(() => new Uri(image.ImageUri150));
            Assert.Less(0, image.Width);
            Assert.Less(0, image.Height);
        }

        Assert.Less(0, masterRelease.Genres.Count);
        foreach (var genre in masterRelease.Genres)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(genre));
        }

        Assert.Less(0, masterRelease.Styles.Count);
        foreach (var style in masterRelease.Styles)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(style));
        }

        Assert.AreEqual(9, masterRelease.Tracklist.Count);
        Assert.AreEqual("1", masterRelease.Tracklist[0].Position);
        Assert.AreEqual("track", masterRelease.Tracklist[0].Type);
        Assert.AreEqual("The Dragon Lies Bleeding", masterRelease.Tracklist[0].Title);
        Assert.AreEqual("4:23", masterRelease.Tracklist[0].Duration);

        Assert.AreEqual(1, masterRelease.Artists.Count);
        Assert.AreEqual(287459, masterRelease.Artists[0].Id);
        Assert.AreEqual("HammerFall", masterRelease.Artists[0].Name);
        Assert.DoesNotThrow(() => new Uri(masterRelease.Artists[0].ResourceUrl));

        Assert.Less(0, masterRelease.Videos.Count);
        foreach (var video in masterRelease.Videos)
        {
            Assert.DoesNotThrow(() => new Uri(video.Uri));
            Assert.IsFalse(string.IsNullOrWhiteSpace(video.Title));
            Assert.IsFalse(string.IsNullOrWhiteSpace(video.Description));
            Assert.Less(0, video.DurationInSeconds);
        }
    }

    [Test]
    public void GetMasterRelease_MasterReleaseId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetMasterRelease(-1));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetMasterRelease(0));
    }

    [Test]
    public void GetMasterRelease_NotExistingReleaseId()
    {
        var masterReleaseId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetMasterRelease(masterReleaseId));
    }


    [Test]
    public async Task GetMasterReleaseVersions_Success()
    {
        var masterReleaseId = 156551;

        var response = await ApiClient.GetMasterReleaseVersions(masterReleaseId);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.DoesNotThrow(() => new Uri(response.Pagination.Urls.NextPageUrl));
        Assert.DoesNotThrow(() => new Uri(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.ReleaseVersions);
        Assert.AreEqual(50, response.ReleaseVersions.Count);

        var version = response.ReleaseVersions.First();
        Assert.Less(0, version.Id);
        Assert.IsFalse(string.IsNullOrWhiteSpace(version.Label));
        Assert.IsFalse(string.IsNullOrWhiteSpace(version.Country));
        Assert.IsFalse(string.IsNullOrWhiteSpace(version.Title));
        Assert.IsFalse(string.IsNullOrWhiteSpace(version.Format));
        Assert.IsFalse(string.IsNullOrWhiteSpace(version.CatalogNumber));
        Assert.IsFalse(string.IsNullOrWhiteSpace(version.Year));
        Assert.IsFalse(string.IsNullOrWhiteSpace(version.Status));
        Assert.DoesNotThrow(() => new Uri(version.ResourceUrl));
        Assert.DoesNotThrow(() => new Uri(version.ThumbnailUrl));

        Assert.IsNotNull(version.MajorFormats);
        Assert.Less(0, version.MajorFormats.Count);
        foreach (var format in version.MajorFormats)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(format));
        }

        Assert.IsNotNull(version.Statistics);
        Assert.IsNotNull(version.Statistics.CommunityStatistics);
        Assert.Less(0, version.Statistics.CommunityStatistics.ReleasesInWantlistCount);
        Assert.Less(0, version.Statistics.CommunityStatistics.ReleasesInCollectionCount);
        Assert.IsNotNull(version.Statistics.UserStatistics);
        Assert.LessOrEqual(0, version.Statistics.UserStatistics.ReleasesInWantlistCount);
        Assert.LessOrEqual(0, version.Statistics.UserStatistics.ReleasesInCollectionCount);
    }

    [Test]
    public void GetMasterReleaseVersions_MasterReleaseId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetMasterReleaseVersions(-1));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetMasterReleaseVersions(0));
    }

    [Test]
    public void GetMasterReleaseVersions_NotExistingMasterReleaseId()
    {
        var masterReleaseId = int.MaxValue;

        // Should Fail!! But Discogs seems to return a list of over 6 million release versions if master release id is invalid
        Assert.DoesNotThrowAsync(() => ApiClient.GetMasterReleaseVersions(masterReleaseId));
    }

    [Test]
    public async Task GetMasterReleaseVersions_Success_InvalidSmallPageNumber()
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await ApiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.DoesNotThrow(() => new Uri(response.Pagination.Urls.NextPageUrl));
        Assert.DoesNotThrow(() => new Uri(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.ReleaseVersions);
        Assert.AreEqual(50, response.ReleaseVersions.Count);
    }

    [Test]
    public void GetMasterReleaseVersions_InvalidBigPageNumber()
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        // Should fail with 404 but Discord seems to enounter an internal error instead!
        Assert.ThrowsAsync<DiscogsException>(() => ApiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams), "internal error");
    }

    [Test]
    public async Task GetMasterReleaseVersions_Success_InvalidSmallPageSize()
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await ApiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(1, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.DoesNotThrow(() => new Uri(response.Pagination.Urls.NextPageUrl));
        Assert.DoesNotThrow(() => new Uri(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.ReleaseVersions);
        Assert.AreEqual(1, response.ReleaseVersions.Count);
    }

    [Test]
    public async Task GetMasterReleaseVersions_Success_InvalidBigPageSize()
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await ApiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.GreaterOrEqual(100, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsTrue(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsTrue(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.ReleaseVersions);
        Assert.GreaterOrEqual(100, response.ReleaseVersions.Count);
    }

    [Test]
    public async Task GetAllMasterReleaseVersions_Success()
    {
        var masterReleaseId = 156551;

        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await ApiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams);
        var itemCount = response.Pagination.TotalItems;
        var summedUpItemCount = response.ReleaseVersions.Count;

        for (var p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await ApiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams);
            summedUpItemCount += response.ReleaseVersions.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }

    [Test]
    public async Task GetMasterReleaseVersions_Sorted()
    {
        var masterReleaseId = 156551;

        // Released
        var sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Ascending };
        var responseAscending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending);
        var sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Descending };
        var responseDescending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending);

        Assert.That(
            responseAscending.ReleaseVersions.Select(r => r.Year),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.ReleaseVersions.Select(r => r.Year),
            Is.Ordered.Descending);

        // Title
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending);

        Assert.That(
            responseAscending.ReleaseVersions.Select(r => r.Title),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.ReleaseVersions.Select(r => r.Title),
            Is.Ordered.Descending);

        // Format
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending);

        // Discogs does only a sudo sort of the format which is not reliably testable
        //Assert.That(
        //    responseAscending.ReleaseVersions.Select(r => r.Format),
        //    Is.Ordered.Ascending);
        //Assert.That(
        //    responseDescending.ReleaseVersions.Select(r => r.Format),
        //    Is.Ordered.Descending);

        // Label
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending);

        // Discogs does only a sudo sort of the label which is not reliably testable
        //Assert.That(
        //    responseAscending.ReleaseVersions.Select(r => r.Label),
        //    Is.Ordered.Ascending);
        //Assert.That(
        //    responseDescending.ReleaseVersions.Select(r => r.Label),
        //    Is.Ordered.Descending);

        // Catalog Number
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending);

        // Discogs does only a sudo sort of the catalog number which is not reliably testable
        //Assert.That(
        //    responseAscending.ReleaseVersions.Select(r => r.CatalogNumber),
        //    Is.Ordered.Ascending);
        //Assert.That(
        //    responseDescending.ReleaseVersions.Select(r => r.CatalogNumber),
        //    Is.Ordered.Descending);

        // Country
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Country, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Country, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending);

        Assert.That(
            responseAscending.ReleaseVersions.Select(r => r.Country),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.ReleaseVersions.Select(r => r.Country),
            Is.Ordered.Descending);
    }

    [Test]
    public async Task GetMasterReleaseVersions_GetFilters()
    {
        var masterReleaseId = 156551;

        var response = await ApiClient.GetMasterReleaseVersions(masterReleaseId);

        Assert.IsNotNull(response.FilterFacets);
        foreach (var facet in response.FilterFacets)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(facet.Id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(facet.Title));

            Assert.IsNotNull(facet.Values);
            foreach (var value in facet.Values)
            {
                Assert.Less(0, value.Count);
                Assert.IsFalse(string.IsNullOrWhiteSpace(value.Title));
                Assert.IsFalse(string.IsNullOrWhiteSpace(value.Value));
            }
        }

        Assert.IsNotNull(response.Filters);
        Assert.IsNotNull(response.Filters.AppliedFilters);
        Assert.IsNull(response.Filters.AppliedFilters.Countries);
        Assert.IsNull(response.Filters.AppliedFilters.Formats);
        Assert.IsNull(response.Filters.AppliedFilters.Labels);
        Assert.IsNull(response.Filters.AppliedFilters.Years);

        Assert.IsNotNull(response.Filters.AvailableFilters);
        Assert.IsNotNull(response.Filters.AvailableFilters.Country);
        foreach (var country in response.Filters.AvailableFilters.Country!)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(country.Key));
            Assert.Less(0, country.Value);
        }
        Assert.IsNotNull(response.Filters.AvailableFilters.Format);
        foreach (var format in response.Filters.AvailableFilters.Format!)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(format.Key));
            Assert.Less(0, format.Value);
        }
        Assert.IsNotNull(response.Filters.AvailableFilters.Label);
        foreach (var label in response.Filters.AvailableFilters.Label!)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(label.Key));
            Assert.Less(0, label.Value);
        }
        Assert.IsNotNull(response.Filters.AvailableFilters.Year);
        foreach (var year in response.Filters.AvailableFilters.Year!)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(year.Key));
            Assert.Less(0, year.Value);
        }
    }

    [Test]
    public async Task GetMasterReleaseVersions_ApplyFilters()
    {
        var masterReleaseId = 156551;

        var response = await ApiClient.GetMasterReleaseVersions(masterReleaseId);

        Assert.AreEqual(50, response.ReleaseVersions.Count);

        var countryFilter = response.Filters.AvailableFilters.Country?.First();
        var filterParams1 = new MasterReleaseVersionFilterQueryParameters
        {
            Country = countryFilter?.Key
        };
        var filteredResponse1 = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, filterParams1);

        Assert.AreEqual(countryFilter?.Value, filteredResponse1.ReleaseVersions.Count);

        var releasedFilter = filteredResponse1.Filters.AvailableFilters.Year?.First();
        var filterParams2 = new MasterReleaseVersionFilterQueryParameters
        {
            Country = countryFilter?.Key,
            Year = releasedFilter?.Key
        };
        var filteredResponse2 = await ApiClient.GetMasterReleaseVersions(masterReleaseId, null, filterParams2);

        Assert.AreEqual(releasedFilter?.Value, filteredResponse2.ReleaseVersions.Count);
    }
}
