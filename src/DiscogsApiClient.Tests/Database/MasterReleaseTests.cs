using static DiscogsApiClient.QueryParameters.MasterReleaseVersionFilterQueryParameters;

namespace DiscogsApiClient.Tests.Database;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class MasterReleaseTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();

    [Test]
    public async Task GetMasterRelease_ShouldReturnMasterRelease_WhenIdIsValid(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;

        var masterRelease = await _apiClient.GetMasterRelease(masterReleaseId, cancellationToken);

        await Assert.That(masterRelease).IsNotNull();
        await Assert.That(masterRelease.Id).IsEqualTo(masterReleaseId);
        await Assert.That(masterRelease.MainReleaseId).IsGreaterThan(0);
        await Assert.That(masterRelease.MostRecentReleaseId).IsGreaterThan(0);
        await Assert.That(() => new Uri(masterRelease.ResourceUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(masterRelease.Uri)).ThrowsNothing();
        await Assert.That(() => new Uri(masterRelease.VersionsUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(masterRelease.MainReleaseUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(masterRelease.MostRecentReleaseUrl)).ThrowsNothing();
        await Assert.That(masterRelease.NumForSale).IsGreaterThan(0);
        await Assert.That(masterRelease.LowestPrice).IsNotNull();
        await Assert.That(masterRelease.LowestPrice!.Value).IsGreaterThan(0);
        await Assert.That(masterRelease.Year).IsEqualTo(1997);
        await Assert.That(masterRelease.Title).IsEqualTo("Glory To The Brave");

        await Assert.That(masterRelease.Images.Count).IsGreaterThan(0);
        foreach (var image in masterRelease.Images)
        {
            await Assert.That(Enum.IsDefined(image.Type)).IsTrue();
            await Assert.That(() => new Uri(image.ResourceUrl)).ThrowsNothing();
            await Assert.That(() => new Uri(image.ImageUri)).ThrowsNothing();
            await Assert.That(() => new Uri(image.ImageUri150)).ThrowsNothing();
            await Assert.That(image.Width).IsGreaterThan(0);
            await Assert.That(image.Height).IsGreaterThan(0);
        }

        await Assert.That(masterRelease.Genres.Count).IsGreaterThan(0);
        foreach (var genre in masterRelease.Genres)
        {
            await Assert.That(genre).IsNotNullOrWhiteSpace();
        }

        await Assert.That(masterRelease.Styles.Count).IsGreaterThan(0);
        foreach (var style in masterRelease.Styles)
        {
            await Assert.That(style).IsNotNullOrWhiteSpace();
        }

        await Assert.That(masterRelease.Tracklist.Count).IsEqualTo(9);
        await Assert.That(masterRelease.Tracklist[0].Position).IsEqualTo("1");
        await Assert.That(masterRelease.Tracklist[0].Type).IsEqualTo("track");
        await Assert.That(masterRelease.Tracklist[0].Title).IsEqualTo("The Dragon Lies Bleeding");
        await Assert.That(masterRelease.Tracklist[0].Duration).IsEqualTo("4:22");

        await Assert.That(masterRelease.Artists.Count).IsEqualTo(1);
        await Assert.That(masterRelease.Artists[0].Id).IsEqualTo(287459);
        await Assert.That(masterRelease.Artists[0].Name).IsEqualTo("HammerFall");
        await Assert.That(() => new Uri(masterRelease.Artists[0].ResourceUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(masterRelease.Artists[0].ThumbnailUrl)).ThrowsNothing();

        await Assert.That(masterRelease.Videos.Count).IsGreaterThan(0);
        foreach (var video in masterRelease.Videos)
        {
            await Assert.That(() => new Uri(video.Uri)).ThrowsNothing();
            await Assert.That(video.Title).IsNotNullOrWhiteSpace();
            await Assert.That(video.Description).IsNotNullOrWhiteSpace();
            await Assert.That(video.DurationInSeconds).IsGreaterThan(0);
        }
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetMasterRelease_ShouldThrowArgumentOutOfRangeException_WhenIdIsInvalid(int masterReleaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetMasterRelease(masterReleaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("masterReleaseId");
    }

    [Test]
    public async Task GetMasterRelease_ShouldThrowResourceNotFoundException_WhenReleaseDoesNotExist(CancellationToken cancellationToken)
    {
        var masterReleaseId = int.MaxValue;

        await Assert.That(async () => await _apiClient.GetMasterRelease(masterReleaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("That master release does not exist or may have been deleted.");
    }


    [Test]
    public async Task GetMasterReleaseVersions_ShouldReturnVersions_WhenIdIsValid(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;

        var response = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, null, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(50);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        var nextPageUrl = await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNull();
        var lastPageUrl = await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNull();
        await Assert.That(() => new Uri(nextPageUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(lastPageUrl)).ThrowsNothing();

        await Assert.That(response.ReleaseVersions).IsNotNull();
        await Assert.That(response.ReleaseVersions.Count).IsEqualTo(50);

        var version = response.ReleaseVersions[0];
        await Assert.That(version.Id).IsGreaterThan(0);
        await Assert.That(version.Label).IsNotNullOrWhiteSpace();
        await Assert.That(version.Country).IsNotNullOrWhiteSpace();
        await Assert.That(version.Title).IsNotNullOrWhiteSpace();
        await Assert.That(version.Format).IsNotNullOrWhiteSpace();
        await Assert.That(version.CatalogNumber).IsNotNullOrWhiteSpace();
        await Assert.That(version.Released).IsNotNullOrWhiteSpace();
        await Assert.That(version.Status).IsNotNullOrWhiteSpace();
        await Assert.That(() => new Uri(version.ResourceUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(version.ThumbnailUrl)).ThrowsNothing();

        await Assert.That(version.MajorFormats).IsNotNull();
        await Assert.That(version.MajorFormats.Count).IsGreaterThan(0);
        foreach (var format in version.MajorFormats)
        {
            await Assert.That(format).IsNotNullOrWhiteSpace();
        }

        await Assert.That(version.Statistics).IsNotNull();
        await Assert.That(version.Statistics.CommunityStatistics).IsNotNull();
        await Assert.That(version.Statistics.CommunityStatistics.ReleasesInWantlistCount).IsGreaterThan(0);
        await Assert.That(version.Statistics.CommunityStatistics.ReleasesInCollectionCount).IsGreaterThan(0);
        await Assert.That(version.Statistics.UserStatistics).IsNotNull();
        await Assert.That(version.Statistics.UserStatistics.ReleasesInWantlistCount).IsGreaterThanOrEqualTo(0);
        await Assert.That(version.Statistics.UserStatistics.ReleasesInCollectionCount).IsGreaterThanOrEqualTo(0);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetMasterReleaseVersions_ShouldThrowArgumentOutOfRangeException_WhenIdIsInvalid(int masterReleaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("masterReleaseId");
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldSucceed_WhenMasterReleaseDoesNotExist(CancellationToken cancellationToken)
    {
        var masterReleaseId = int.MaxValue;

        // Should Fail!! But Discogs seems to return a list of over 6 million release versions if master release id is invalid
        await Assert.That(async () => await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, null, cancellationToken)).ThrowsNothing();
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldReturnFirstPage_WhenPageNumberIsTooSmall(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await _apiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams, null, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(50);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        var nextPageUrl = await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNull();
        var lastPageUrl = await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNull();
        await Assert.That(() => new Uri(nextPageUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(lastPageUrl)).ThrowsNothing();

        await Assert.That(response.ReleaseVersions).IsNotNull();
        await Assert.That(response.ReleaseVersions.Count).IsEqualTo(50);
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldThrowDiscogsException_WhenPageNumberIsTooLarge(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        // Should fail with 404 but Discord seems to enounter an internal error instead!
        var exception = await Assert.That(async () => await _apiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams, null, cancellationToken))
            .Throws<DiscogsException>()
            .WithMessageContaining("An internal server error occurred");

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception.Message).Contains("internal server error");
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldReturnSingleItemPerPage_WhenPageSizeIsTooSmall(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await _apiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams, null, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(1);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        var nextPageUrl = await Assert.That(response.Pagination.Urls.NextPageUrl).IsNotNull();
        var lastPageUrl = await Assert.That(response.Pagination.Urls.LastPageUrl).IsNotNull();
        await Assert.That(() => new Uri(nextPageUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(lastPageUrl)).ThrowsNothing();

        await Assert.That(response.ReleaseVersions).IsNotNull();
        await Assert.That(response.ReleaseVersions.Count).IsEqualTo(1);
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldReturnUpToOneHundredItemsPerPage_WhenPageSizeIsTooLarge(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await _apiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams, null, cancellationToken);

        await Assert.That(response.Pagination).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsLessThanOrEqualTo(100);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Pagination.TotalPages).IsGreaterThan(0);
        await Assert.That(response.Pagination.Urls).IsNotNull();
        await Assert.That(response.Pagination.Urls.NextPageUrl).IsNullOrWhiteSpace();
        await Assert.That(response.Pagination.Urls.LastPageUrl).IsNullOrWhiteSpace();

        await Assert.That(response.ReleaseVersions).IsNotNull();
        await Assert.That(response.ReleaseVersions.Count).IsLessThanOrEqualTo(100);
    }

    [Test]
    public async Task GetAllMasterReleaseVersions_ShouldReturnAllVersions_WhenIteratingAllPages(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;

        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await _apiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams, null, cancellationToken);
        var itemCount = response.Pagination.TotalItems;
        var summedUpItemCount = response.ReleaseVersions.Count;

        for (var p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await _apiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams, null, cancellationToken);
            summedUpItemCount += response.ReleaseVersions.Count;
        }

        await Assert.That(itemCount).IsEqualTo(summedUpItemCount);
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldReturnSortedVersions_WhenSortParametersAreProvided(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;

        // Released
        var sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Ascending };
        var responseAscending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending, cancellationToken);
        var sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Descending };
        var responseDescending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.ReleaseVersions.Select(r => r.Released)).IsInOrder();
        await Assert.That(responseDescending.ReleaseVersions.Select(r => r.Released)).IsInDescendingOrder();

        // Title
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending, cancellationToken);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.ReleaseVersions.Select(r => r.Title)).IsInOrder();
        await Assert.That(responseDescending.ReleaseVersions.Select(r => r.Title)).IsInDescendingOrder();

        // Format
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending, cancellationToken);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending, cancellationToken);

        // Discogs does only a sudo sort of the format which is not reliably testable
        //Assert.That(
        //    responseAscending.ReleaseVersions.Select(r => r.Format),
        //    Is.Ordered.Ascending);
        //Assert.That(
        //    responseDescending.ReleaseVersions.Select(r => r.Format),
        //    Is.Ordered.Descending);

        // Label
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending, cancellationToken);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending, cancellationToken);

        // Discogs does only a sudo sort of the label which is not reliably testable
        //Assert.That(
        //    responseAscending.ReleaseVersions.Select(r => r.Label),
        //    Is.Ordered.Ascending);
        //Assert.That(
        //    responseDescending.ReleaseVersions.Select(r => r.Label),
        //    Is.Ordered.Descending);

        // Catalog Number
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending, cancellationToken);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending, cancellationToken);

        // Discogs does only a sudo sort of the catalog number which is not reliably testable
        //Assert.That(
        //    responseAscending.ReleaseVersions.Select(r => r.CatalogNumber),
        //    Is.Ordered.Ascending);
        //Assert.That(
        //    responseDescending.ReleaseVersions.Select(r => r.CatalogNumber),
        //    Is.Ordered.Descending);

        // Country
        sortParametersAscending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Country, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersAscending, cancellationToken);
        sortParametersDescending = new MasterReleaseVersionFilterQueryParameters { SortProperty = SortableProperty.Country, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, sortParametersDescending, cancellationToken);

        // Discogs does only a sudo sort of the format which is not reliably testable
        //Assert.That(
        //    responseAscending.ReleaseVersions.Select(r => r.Country),
        //    Is.Ordered.Ascending);
        //Assert.That(
        //    responseDescending.ReleaseVersions.Select(r => r.Country),
        //    Is.Ordered.Descending);
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldReturnFilterFacets_WhenRequestingVersions(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;

        var response = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, null, cancellationToken);

        await Assert.That(response.FilterFacets).IsNotNull();
        foreach (var facet in response.FilterFacets)
        {
            await Assert.That(facet.Id).IsNotNullOrWhiteSpace();
            await Assert.That(facet.Title).IsNotNullOrWhiteSpace();

            await Assert.That(facet.Values).IsNotNull();
            foreach (var value in facet.Values)
            {
                await Assert.That(value.Count).IsGreaterThan(0);
                await Assert.That(value.Title).IsNotNullOrWhiteSpace();
                await Assert.That(value.Value).IsNotNullOrWhiteSpace();
            }
        }

        await Assert.That(response.Filters).IsNotNull();
        await Assert.That(response.Filters.AppliedFilters).IsNotNull();
        await Assert.That(response.Filters.AppliedFilters.Countries).IsNull();
        await Assert.That(response.Filters.AppliedFilters.Formats).IsNull();
        await Assert.That(response.Filters.AppliedFilters.Labels).IsNull();
        await Assert.That(response.Filters.AppliedFilters.Years).IsNull();

        await Assert.That(response.Filters.AvailableFilters).IsNotNull();
        await Assert.That(response.Filters.AvailableFilters.Country).IsNotNull();
        foreach (var country in response.Filters.AvailableFilters.Country!)
        {
            await Assert.That(country.Key).IsNotNullOrWhiteSpace();
            await Assert.That(country.Value).IsGreaterThan(0);
        }

        await Assert.That(response.Filters.AvailableFilters.Format).IsNotNull();
        foreach (var format in response.Filters.AvailableFilters.Format!)
        {
            await Assert.That(format.Key).IsNotNullOrWhiteSpace();
            await Assert.That(format.Value).IsGreaterThan(0);
        }

        await Assert.That(response.Filters.AvailableFilters.Label).IsNotNull();
        foreach (var label in response.Filters.AvailableFilters.Label!)
        {
            await Assert.That(label.Key).IsNotNullOrWhiteSpace();
            await Assert.That(label.Value).IsGreaterThan(0);
        }

        await Assert.That(response.Filters.AvailableFilters.Year).IsNotNull();
        foreach (var year in response.Filters.AvailableFilters.Year!)
        {
            await Assert.That(year.Key).IsNotNullOrWhiteSpace();
            await Assert.That(year.Value).IsGreaterThan(0);
        }
    }

    [Test]
    public async Task GetMasterReleaseVersions_ShouldReturnFilteredVersions_WhenFiltersAreApplied(CancellationToken cancellationToken)
    {
        var masterReleaseId = 156551;

        var response = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, null, cancellationToken);

        await Assert.That(response.ReleaseVersions.Count).IsEqualTo(50);

        var countryFilter = response.Filters.AvailableFilters.Country?.First();
        var filterParams1 = new MasterReleaseVersionFilterQueryParameters
        {
            Country = countryFilter?.Key
        };
        var filteredResponse1 = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, filterParams1, cancellationToken);

        await Assert.That(countryFilter).IsNotNull();
        await Assert.That(filteredResponse1.ReleaseVersions.Count).IsEqualTo(countryFilter!.Value.Value);

        var releasedFilter = filteredResponse1.Filters.AvailableFilters.Year?.First();
        var filterParams2 = new MasterReleaseVersionFilterQueryParameters
        {
            Country = countryFilter?.Key,
            Year = releasedFilter?.Key
        };
        var filteredResponse2 = await _apiClient.GetMasterReleaseVersions(masterReleaseId, null, filterParams2, cancellationToken);

        await Assert.That(releasedFilter).IsNotNull();
        await Assert.That(filteredResponse2.ReleaseVersions.Count).IsEqualTo(releasedFilter!.Value.Value);
    }
}
