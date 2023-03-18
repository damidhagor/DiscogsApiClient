namespace DiscogsApiClient.Tests.Database;

public sealed class ArtistsTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetArtist_Success()
    {
        var artistId = 287459;

        var artist = await ApiClient.GetArtist(artistId, default);

        Assert.IsNotNull(artist);
        Assert.AreEqual(artistId, artist.Id);
        Assert.AreEqual("HammerFall", artist.Name);
        Assert.AreEqual("https://api.discogs.com/artists/287459", artist.ResourceUrl);
        Assert.AreEqual("https://www.discogs.com/artist/287459-HammerFall", artist.Uri);
        Assert.AreEqual("https://api.discogs.com/artists/287459/releases", artist.ReleasesUrl);
        Assert.IsFalse(string.IsNullOrWhiteSpace(artist.Profile));

        Assert.Less(0, artist.Urls.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(artist.Urls[0]));
        Assert.DoesNotThrow(() => new Uri(artist.Urls[0]));

        Assert.Less(0, artist.NameVariations.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(artist.NameVariations[0]));

        Assert.Less(0, artist.Members.Count);
        var member = artist.Members.FirstOrDefault(m => m.Id == 262015);
        Assert.IsNotNull(member);
        Assert.IsTrue(member!.IsActive);
        Assert.AreEqual("Oscar Dronjak", member.Name);
        Assert.AreEqual("https://api.discogs.com/artists/262015", member.ResourceUrl);
        Assert.IsFalse(string.IsNullOrWhiteSpace(member.ThumbnailUrl));
        Assert.DoesNotThrow(() => new Uri(member.ThumbnailUrl));

        Assert.Less(0, artist.Images.Count);
        var image = artist.Images.FirstOrDefault();
        Assert.IsNotNull(image);
        Assert.IsTrue(image!.Width > 0);
        Assert.IsTrue(image!.Height > 0);
        Assert.IsFalse(string.IsNullOrWhiteSpace(image.ResourceUrl));
        Assert.DoesNotThrow(() => new Uri(image.ResourceUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(image.ImageUri));
        Assert.DoesNotThrow(() => new Uri(image.ImageUri));
        Assert.IsFalse(string.IsNullOrWhiteSpace(image.ImageUri150));
        Assert.DoesNotThrow(() => new Uri(image.ImageUri150));
    }

    [Test]
    public void GetArtist_ArtistId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetArtist(-1, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetArtist(0, default));
    }

    [Test]
    public void GetArtist_NotExistingArtistId()
    {
        var artistId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtist(artistId, default));
    }


    [Test]
    public async Task GetArtistReleases_Success()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await ApiClient.GetArtistReleases(artistId, paginationParams, default!, default);

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

        var release = response.Releases.First();
        Assert.IsNotNull(release);
        Assert.Greater(release.Id, 0);
        Assert.DoesNotThrow(() => new Uri(release.ResourceUrl));
        Assert.DoesNotThrow(() => new Uri(release.ThumbnailUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Type));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Title));
        Assert.Greater(release.MainReleaseId, 0);
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Artist));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Role));
        Assert.Greater(release.Year, 0);
        Assert.IsNotNull(release.Statistics);
        Assert.IsNotNull(release.Statistics.CommunityStatistics);
        Assert.Greater(release.Statistics.CommunityStatistics.ReleasesInWantlistCount, 0);
        Assert.Greater(release.Statistics.CommunityStatistics.ReleasesInCollectionCount, 0);
        Assert.IsNotNull(release.Statistics.UserStatistics);
        Assert.GreaterOrEqual(release.Statistics.UserStatistics.ReleasesInWantlistCount, 0);
        Assert.GreaterOrEqual(release.Statistics.UserStatistics.ReleasesInCollectionCount, 0);
    }

    [Test]
    public void GetArtistReleases_ArtistId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetArtistReleases(-1, default!, default!, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetArtistReleases(0, default!, default!, default));
    }

    [Test]
    public void GetArtistReleases_NotExistingArtistId()
    {
        var artistId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtistReleases(artistId, default!, default!, default));
    }

    [Test]
    public async Task GetArtistReleases_Success_InvalidSmallPageNumber()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = -1, PageSize = 50 };

        var response = await ApiClient.GetArtistReleases(artistId, paginationParams, default!, default);

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
    public void GetArtistReleases_InvalidBigPageNumber()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = int.MaxValue, PageSize = 50 };

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtistReleases(artistId, paginationParams, default!, default));
    }

    [Test]
    public async Task GetArtistReleases_Success_InvalidSmallPageSize()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = -1 };

        var response = await ApiClient.GetArtistReleases(artistId, paginationParams, default!, default);

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
    public async Task GetArtistReleases_Success_InvalidBigPageSize()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = int.MaxValue };

        var response = await ApiClient.GetArtistReleases(artistId, paginationParams, default!, default);

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
    public async Task GetArtistAllReleases_Success()
    {
        var artistId = 287459;

        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var sortParams = new ArtistReleaseSortQueryParameters();

        var response = await ApiClient.GetArtistReleases(artistId, paginationParams, sortParams, default);
        var itemCount = response.Pagination.TotalItems;
        var summedUpItemCount = response.Releases.Count;

        for (var p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await ApiClient.GetArtistReleases(artistId, paginationParams, sortParams, default);
            summedUpItemCount += response.Releases.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }
}
