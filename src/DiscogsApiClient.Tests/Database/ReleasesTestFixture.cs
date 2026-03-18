namespace DiscogsApiClient.Tests.Database;

public sealed class ReleasesTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetRelease_Success(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var release = await ApiClient.GetRelease(releaseId, cancellationToken);

        await Assert.That(release).IsNotNull();
        await Assert.That(release.Id).IsEqualTo(releaseId);
        await Assert.That(() => new Uri(release.ResourceUrl)).ThrowsNothing();
        await Assert.That(() => new Uri(release.Uri)).ThrowsNothing();
        await Assert.That(release.ArtistsSort).IsEqualTo("HammerFall");


        await Assert.That(release.Artists.Count).IsEqualTo(1);
        await Assert.That(release.Artists[0].Name).IsEqualTo("HammerFall");
        await Assert.That(() => new Uri(release.Artists[0].ThumbnailUrl)).ThrowsNothing();

        await Assert.That(release.Labels.Count).IsEqualTo(1);
        await Assert.That(release.Labels[0].Id).IsEqualTo(11499);
        await Assert.That(release.Labels[0].Name).IsEqualTo("Nuclear Blast");
        await Assert.That(release.Labels[0].CatalogNumber).IsEqualTo("NB 265-2");
        await Assert.That(release.Formats.Count).IsEqualTo(release.FormatCount);
        await Assert.That(release.AddedAt).IsGreaterThan(DateTime.MinValue);
        await Assert.That(release.ChangedAt).IsGreaterThan(DateTime.MinValue);
        await Assert.That(release.NumForSale).IsGreaterThan(0);
        await Assert.That(release.LowestPrice).IsNotNull();
        await Assert.That(release.LowestPrice!.Value).IsGreaterThan(0);
        await Assert.That(release.MasterId).IsGreaterThan(0);
        await Assert.That(() => new Uri(release.MasterUrl)).ThrowsNothing();
        await Assert.That(release.Title).IsEqualTo("Glory To The Brave");
        await Assert.That(release.Country).IsEqualTo("Germany");
        await Assert.That(release.Year).IsEqualTo(1997);
        await Assert.That(release.Released).IsEqualTo("1997");
        await Assert.That(release.Notes).IsNotNullOrWhiteSpace();
        await Assert.That(release.ReleasedFormatted).IsEqualTo("1997");
        await Assert.That(() => new Uri(release.ThumbnailUrl)).ThrowsNothing();
        await Assert.That(release.EstimatedWeight).IsGreaterThan(0);
        await Assert.That(release.IsBlockedFromSale).IsFalse();
        await Assert.That(release.DataQuality).IsNotNullOrWhiteSpace();

        await Assert.That(release.Formats.Count).IsEqualTo(1);
        await Assert.That(release.Formats[0].Name).IsEqualTo("CD");
        await Assert.That(release.Formats[0].Count).IsEqualTo("1");
        await Assert.That(release.Formats[0].Descriptions.Count).IsGreaterThan(0);

        await Assert.That(release.CommunityStatistics).IsNotNull();
        await Assert.That(release.CommunityStatistics.UsersOwningReleaseCount).IsGreaterThan(0);
        await Assert.That(release.CommunityStatistics.UsersWantingReleaseCount).IsGreaterThan(0);
        await Assert.That(release.CommunityStatistics.Rating).IsNotNull();
        await Assert.That(release.CommunityStatistics.Rating.Count).IsGreaterThan(0);
        await Assert.That(release.CommunityStatistics.Rating.Average).IsGreaterThan(0);

        await Assert.That(release.Identifiers.Count).IsGreaterThan(0);
        await Assert.That(release.Identifiers[0].Type).IsEqualTo("Barcode");
        await Assert.That(release.Identifiers[0].Value).IsEqualTo("7 27361 62652 5");

        await Assert.That(release.Videos.Count).IsGreaterThan(0);
        foreach (var video in release.Videos)
        {
            await Assert.That(() => new Uri(video.Uri)).ThrowsNothing();
            await Assert.That(video.Title).IsNotNullOrWhiteSpace();
            await Assert.That(video.Description).IsNotNullOrWhiteSpace();
            await Assert.That(video.DurationInSeconds).IsGreaterThan(0);
        }

        await Assert.That(release.Genres.Count).IsGreaterThan(0);
        foreach (var genre in release.Genres)
        {
            await Assert.That(genre).IsNotNullOrWhiteSpace();
        }

        await Assert.That(release.Styles.Count).IsGreaterThan(0);
        foreach (var style in release.Styles)
        {
            await Assert.That(style).IsNotNullOrWhiteSpace();
        }

        await Assert.That(release.Tracklist.Count).IsEqualTo(9);
        await Assert.That(release.Tracklist[0].Position).IsEqualTo("1");
        await Assert.That(release.Tracklist[0].Title).IsEqualTo("The Dragon Lies Bleeding");
        await Assert.That(release.Tracklist[0].Duration).IsEqualTo("4:23");
        await Assert.That(release.Tracklist[0].Type).IsEqualTo("track");

        await Assert.That(release.ExtraArtists).IsNull();

        await Assert.That(release.Images.Count).IsGreaterThan(0);
        foreach (var image in release.Images)
        {
            await Assert.That(Enum.IsDefined(image.Type)).IsTrue();
            await Assert.That(() => new Uri(image.ResourceUrl)).ThrowsNothing();
            await Assert.That(() => new Uri(image.ImageUri)).ThrowsNothing();
            await Assert.That(() => new Uri(image.ImageUri150)).ThrowsNothing();
            await Assert.That(image.Width).IsGreaterThan(0);
            await Assert.That(image.Height).IsGreaterThan(0);
        }
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetRelease_ReleaseId_Guard(int releaseId, CancellationToken cancellationToken)
    {
        await Assert.That(async () => await ApiClient.GetRelease(releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task GetRelease_NotExistingReleaseId(CancellationToken cancellationToken)
    {
        var releaseId = int.MaxValue;

        await Assert.That(async () => await ApiClient.GetRelease(releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }


    [Test]
    public async Task GetReleaseCommunityRating_Success(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var ratingResponse = await ApiClient.GetReleaseCommunityRating(releaseId, cancellationToken);

        await Assert.That(ratingResponse).IsNotNull();
        await Assert.That(ratingResponse.ReleaseId).IsEqualTo(releaseId);
        await Assert.That(ratingResponse.Rating).IsNotNull();
        await Assert.That(ratingResponse.Rating.Count).IsGreaterThan(0);
        await Assert.That(ratingResponse.Rating.Average).IsGreaterThan(0);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetReleaseCommunityRating_ReleaseId_Guard(int releaseId, CancellationToken cancellationToken)
    {
        await Assert.That(async () => await ApiClient.GetReleaseCommunityRating(releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task GetReleaseCommunityRating_NotExistingReleaseId(CancellationToken cancellationToken)
    {
        var releaseId = int.MaxValue;

        await Assert.That(async () => await ApiClient.GetReleaseCommunityRating(releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }


    [Test]
    public async Task GetReleaseStats_Success(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var stats = await ApiClient.GetReleaseStats(releaseId, cancellationToken);

        await Assert.That(stats).IsNotNull();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetReleaseStats_ReleaseId_Guard(int releaseId, CancellationToken cancellationToken)
    {
        await Assert.That(async () => await ApiClient.GetReleaseStats(releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task GetReleaseStats_NotExistingReleaseId(CancellationToken cancellationToken)
    {
        var releaseId = int.MaxValue;

        await Assert.That(async () => await ApiClient.GetReleaseStats(releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }
}
