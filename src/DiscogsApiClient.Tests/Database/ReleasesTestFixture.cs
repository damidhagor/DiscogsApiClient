namespace DiscogsApiClient.Tests.Database;

public sealed class ReleasesTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetRelease_Success()
    {
        var releaseId = 5134861;

        var release = await ApiClient.GetRelease(releaseId, default);

        Assert.IsNotNull(release);
        Assert.AreEqual(releaseId, release.Id);
        Assert.DoesNotThrow(() => new Uri(release.ResourceUrl));
        Assert.DoesNotThrow(() => new Uri(release.Uri));
        Assert.AreEqual("HammerFall", release.ArtistsSort);


        Assert.AreEqual(1, release.Artists.Count);
        Assert.AreEqual("HammerFall", release.Artists[0].Name);

        Assert.AreEqual(1, release.Labels.Count);
        Assert.AreEqual(11499, release.Labels[0].Id);
        Assert.AreEqual("Nuclear Blast", release.Labels[0].Name);
        Assert.AreEqual("NB 265-2", release.Labels[0].CatalogNumber);
        Assert.AreEqual(release.FormatCount, release.Formats.Count);
        Assert.Less(DateTime.MinValue, release.AddedAt);
        Assert.Less(DateTime.MinValue, release.ChangedAt);
        Assert.Less(0, release.NumForSale);
        Assert.Less(0, release.LowestPrice);
        Assert.Less(0, release.MasterId);
        Assert.DoesNotThrow(() => new Uri(release.MasterUrl));
        Assert.AreEqual("Glory To The Brave", release.Title);
        Assert.AreEqual("Germany", release.Country);
        Assert.AreEqual("1997", release.Year);
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Notes));
        Assert.AreEqual("1997", release.YearFormatted);
        Assert.DoesNotThrow(() => new Uri(release.ThumbnailUrl));
        Assert.Less(0, release.EstimatedWeight);
        Assert.IsFalse(release.IsBlockedFromSale);
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.DataQuality));

        Assert.AreEqual(1, release.Formats.Count);
        Assert.AreEqual("CD", release.Formats[0].Name);
        Assert.AreEqual("1", release.Formats[0].Count);
        Assert.Less(0, release.Formats[0].Descriptions.Count);

        Assert.IsNotNull(release.CommunityStatistics);
        Assert.Less(0, release.CommunityStatistics.UsersOwningReleaseCount);
        Assert.Less(0, release.CommunityStatistics.UsersWantingReleaseCount);
        Assert.IsNotNull(release.CommunityStatistics.Rating);
        Assert.Less(0, release.CommunityStatistics.Rating.Count);
        Assert.Less(0, release.CommunityStatistics.Rating.Average);

        Assert.Less(0, release.Identifiers.Count);
        Assert.AreEqual("Barcode", release.Identifiers[0].Type);
        Assert.AreEqual("7 27361 62652 5", release.Identifiers[0].Value);

        Assert.Less(0, release.Videos.Count);
        foreach (var video in release.Videos)
        {
            Assert.DoesNotThrow(() => new Uri(video.Uri));
            Assert.IsFalse(string.IsNullOrWhiteSpace(video.Title));
            Assert.IsFalse(string.IsNullOrWhiteSpace(video.Description));
            Assert.Less(0, video.DurationInSeconds);
        }

        Assert.Less(0, release.Genres.Count);
        foreach (var genre in release.Genres)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(genre));
        }

        Assert.Less(0, release.Styles.Count);
        foreach (var style in release.Styles)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(style));
        }

        Assert.AreEqual(9, release.Tracklist.Count);
        Assert.AreEqual("1", release.Tracklist[0].Position);
        Assert.AreEqual("The Dragon Lies Bleeding", release.Tracklist[0].Title);
        Assert.AreEqual("4:23", release.Tracklist[0].Duration);
        Assert.AreEqual("track", release.Tracklist[0].Type);

        Assert.AreEqual(0, release.ExtraArtists.Count);

        Assert.Less(0, release.Images.Count);
        foreach (var image in release.Images)
        {
            Assert.IsTrue(Enum.IsDefined(image.Type));
            Assert.DoesNotThrow(() => new Uri(image.ResourceUrl));
            Assert.DoesNotThrow(() => new Uri(image.ImageUri));
            Assert.DoesNotThrow(() => new Uri(image.ImageUri150));
            Assert.Less(0, image.Width);
            Assert.Less(0, image.Height);
        }
    }

    [Test]
    public void GetRelease_ReleaseId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetRelease(-1, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetRelease(0, default));
    }

    [Test]
    public void GetRelease_NotExistingReleaseId()
    {
        var releaseId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetRelease(releaseId, default));
    }


    [Test]
    public async Task GetReleaseCommunityRating_Success()
    {
        var releaseId = 5134861;

        var ratingResponse = await ApiClient.GetReleaseCommunityRating(releaseId, default);

        Assert.IsNotNull(ratingResponse);
        Assert.AreEqual(ratingResponse.ReleaseId, releaseId);
        Assert.IsNotNull(ratingResponse.Rating);
        Assert.Less(0, ratingResponse.Rating.Count);
        Assert.Less(0, ratingResponse.Rating.Average);
    }

    [Test]
    public void GetReleaseCommunityRating_ReleaseId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetReleaseCommunityRating(-1, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetReleaseCommunityRating(0, default));
    }

    [Test]
    public void GetReleaseCommunityRating_NotExistingReleaseId()
    {
        var releaseId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetReleaseCommunityRating(releaseId, default));
    }


    [Test]
    public async Task GetReleaseStats_Success()
    {
        var releaseId = 5134861;

        var stats = await ApiClient.GetReleaseStats(releaseId, default);

        Assert.IsNotNull(stats);
    }

    [Test]
    public void GetReleaseStats_ReleaseId_Guard()
    {
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetReleaseStats(-1, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetReleaseStats(0, default));
    }

    [Test]
    public void GetReleaseStats_NotExistingReleaseId()
    {
        var releaseId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetReleaseStats(releaseId, default));
    }
}
