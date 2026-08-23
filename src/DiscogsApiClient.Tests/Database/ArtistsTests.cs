using static DiscogsApiClient.QueryParameters.ArtistReleaseSortQueryParameters;

namespace DiscogsApiClient.Tests.Database;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class ArtistsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();

    [Test]
    public async Task GetArtist_ShouldReturnArtist_WhenIdIsValid(CancellationToken cancellationToken)
    {
        var artistId = 287459;

        var artist = await _apiClient.GetArtist(artistId, cancellationToken);

        await Assert.That(artist).IsNotNull();
        await Assert.That(artist.Id).IsEqualTo(artistId);
        await Assert.That(artist.Name).IsEqualTo("HammerFall");
        await Assert.That(artist.ResourceUrl).IsEqualTo("https://api.discogs.com/artists/287459");
        await Assert.That(artist.Uri).IsEqualTo("https://www.discogs.com/artist/287459-HammerFall");
        await Assert.That(artist.ReleasesUrl).IsEqualTo("https://api.discogs.com/artists/287459/releases");
        await Assert.That(artist.Profile).IsNotNullOrWhiteSpace();

        await Assert.That(artist.Urls.Count).IsGreaterThan(0);
        await Assert.That(artist.Urls[0]).IsNotNullOrWhiteSpace();
        await Assert.That(() => new Uri(artist.Urls[0])).ThrowsNothing();

        await Assert.That(artist.NameVariations.Count).IsGreaterThan(0);
        await Assert.That(artist.NameVariations[0]).IsNotNullOrWhiteSpace();

        await Assert.That(artist.Members.Count).IsGreaterThan(0);
        var member = artist.Members.FirstOrDefault(m => m.Id == 262015);
        await Assert.That(member).IsNotNull();
        await Assert.That(member!.IsActive).IsTrue();
        await Assert.That(member.Name).IsEqualTo("Oscar Dronjak");
        await Assert.That(member.ResourceUrl).IsEqualTo("https://api.discogs.com/artists/262015");
        await Assert.That(member.ThumbnailUrl).IsNotNullOrWhiteSpace();
        await Assert.That(() => new Uri(member.ThumbnailUrl)).ThrowsNothing();

        await Assert.That(artist.Images.Count).IsGreaterThan(0);
        var image = artist.Images[0];
        await Assert.That(image.Width).IsGreaterThan(0);
        await Assert.That(image.Height).IsGreaterThan(0);
        await Assert.That(image.ResourceUrl).IsNotNullOrWhiteSpace();
        await Assert.That(() => new Uri(image.ResourceUrl)).ThrowsNothing();
        await Assert.That(image.ImageUri).IsNotNullOrWhiteSpace();
        await Assert.That(() => new Uri(image.ImageUri)).ThrowsNothing();
        await Assert.That(image.ImageUri150).IsNotNullOrWhiteSpace();
        await Assert.That(() => new Uri(image.ImageUri150)).ThrowsNothing();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetArtist_ShouldThrowArgumentOutOfRangeException_WhenIdIsInvalid(int artistId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetArtist(artistId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("artistId");
    }

    [Test]
    public async Task GetArtist_ShouldThrowResourceNotFoundException_WhenArtistDoesNotExist(CancellationToken cancellationToken)
    {
        var artistId = int.MaxValue;

        await Assert.That(async () => await _apiClient.GetArtist(artistId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Artist not found.");
    }


    [Test]
    public async Task GetArtistReleases_ShouldReturnReleases_WhenIdIsValid(CancellationToken cancellationToken)
    {
        var artistId = 287459;

        var response = await _apiClient.GetArtistReleases(artistId, null, null, cancellationToken);

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

        var release = response.Releases[0];
        await Assert.That(release).IsNotNull();
        await Assert.That(release.Id).IsGreaterThan(0);
        await Assert.That(() => new Uri(release.ResourceUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(release.ThumbnailUrl)).ThrowsNothing();
        await Assert.That(release.Type).IsNotNullOrWhiteSpace();
        await Assert.That(release.Title).IsNotNullOrWhiteSpace();
        await Assert.That(release.MainReleaseId).IsGreaterThan(0);
        await Assert.That(release.Artist).IsNotNullOrWhiteSpace();
        await Assert.That(release.Role).IsNotNullOrWhiteSpace();
        await Assert.That(release.Year).IsGreaterThan(0);
        await Assert.That(release.Statistics).IsNotNull();
        await Assert.That(release.Statistics.CommunityStatistics).IsNotNull();
        await Assert.That(release.Statistics.CommunityStatistics.ReleasesInWantlistCount).IsGreaterThan(0);
        await Assert.That(release.Statistics.CommunityStatistics.ReleasesInCollectionCount).IsGreaterThan(0);
        await Assert.That(release.Statistics.UserStatistics).IsNotNull();
        await Assert.That(release.Statistics.UserStatistics.ReleasesInWantlistCount).IsGreaterThanOrEqualTo(0);
        await Assert.That(release.Statistics.UserStatistics.ReleasesInCollectionCount).IsGreaterThanOrEqualTo(0);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetArtistReleases_ShouldThrowArgumentOutOfRangeException_WhenIdIsInvalid(int artistId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetArtistReleases(artistId, null, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("artistId");
    }

    [Test]
    public async Task GetArtistReleases_ShouldThrowResourceNotFoundException_WhenArtistDoesNotExist(CancellationToken cancellationToken)
    {
        var artistId = int.MaxValue;

        await Assert.That(async () => await _apiClient.GetArtistReleases(artistId, null, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Artist not found.");
    }

    [Test]
    public async Task GetArtistReleases_ShouldReturnFirstPage_WhenPageNumberIsTooSmall(CancellationToken cancellationToken)
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await _apiClient.GetArtistReleases(artistId, paginationParams, null, cancellationToken);

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
    public async Task GetArtistReleases_ShouldThrowResourceNotFoundException_WhenPageNumberIsTooLarge(CancellationToken cancellationToken)
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        await Assert.That(async () => await _apiClient.GetArtistReleases(artistId, paginationParams, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessageContaining("is outside of valid range");
    }

    [Test]
    public async Task GetArtistReleases_ShouldReturnSingleItemPerPage_WhenPageSizeIsTooSmall(CancellationToken cancellationToken)
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await _apiClient.GetArtistReleases(artistId, paginationParams, null, cancellationToken);

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
    public async Task GetArtistReleases_ShouldReturnOneHundredItemsPerPage_WhenPageSizeIsTooLarge(CancellationToken cancellationToken)
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await _apiClient.GetArtistReleases(artistId, paginationParams, null, cancellationToken);

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
    public async Task GetAllArtistReleases_ShouldReturnAllReleases_WhenIteratingAllPages(CancellationToken cancellationToken)
    {
        var artistId = 287459;

        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await _apiClient.GetArtistReleases(artistId, paginationParams, null, cancellationToken);
        var itemCount = response.Pagination.TotalItems;
        var summedUpItemCount = response.Releases.Count;

        for (var p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await _apiClient.GetArtistReleases(artistId, paginationParams, null, cancellationToken);
            summedUpItemCount += response.Releases.Count;
        }

        await Assert.That(itemCount).IsEqualTo(summedUpItemCount);
    }

    [Test]
    public async Task GetArtistReleases_ShouldReturnSortedReleases_WhenSortParametersAreProvided(CancellationToken cancellationToken)
    {
        var artistId = 253729;

        // Title
        var sortParametersAscending = new ArtistReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Ascending };
        var responseAscending = await _apiClient.GetArtistReleases(artistId, null, sortParametersAscending, cancellationToken);
        var sortParametersDescending = new ArtistReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Descending };
        var responseDescending = await _apiClient.GetArtistReleases(artistId, null, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Title)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Title)).IsInDescendingOrder();

        // Year
        sortParametersAscending = new ArtistReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetArtistReleases(artistId, null, sortParametersAscending, cancellationToken);
        sortParametersDescending = new ArtistReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetArtistReleases(artistId, null, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Year)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Year)).IsInDescendingOrder();
    }
}
