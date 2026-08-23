namespace DiscogsApiClient.Tests.User;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class SubmissionsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetSubmissions_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetSubmissions(username!, null, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task GetSubmissions_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetSubmissions(username, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    public async Task GetSubmissions_ShouldReturnEmptySubmissions_WhenUserHasNoSubmissions(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await _apiClient.GetSubmissions(username, paginationParams, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(0);
        await Assert.That(response.Submissions.Artists).IsNull();
        await Assert.That(response.Submissions.Labels).IsNull();
        await Assert.That(response.Submissions.Releases).IsNull();
    }

    [Test]
    public async Task GetSubmissions_ShouldReturnSubmissions_WhenUserHasSubmissions(CancellationToken cancellationToken)
    {
        var username = "Murder.Records";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 100 };

        var response = await _apiClient.GetSubmissions(username, paginationParams, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(111);
        await Assert.That(response.Pagination.TotalPages).IsEqualTo(2);

        await Assert.That(response.Submissions.Releases).IsNotNull();
        await Assert.That(response.Submissions.Releases!.Count).IsEqualTo(88);
        var release = response.Submissions.Releases![0];
        await Assert.That(release.Id).IsEqualTo(37848567);
        await Assert.That(release.Status).IsEqualTo("Accepted");
        await Assert.That(release.Title).IsEqualTo("Consultations Of The Obscure In The Shadow Of Antique Darkness");
        await Assert.That(release.Year).IsEqualTo(2026);
        await Assert.That(release.Country).IsEqualTo("USA & Europe");
        await Assert.That(release.DataQuality).IsEqualTo("Needs Vote");
        await Assert.That(release.FormatCount).IsEqualTo(1);
        await Assert.That(release.NumForSale).IsEqualTo(2);

        await Assert.That(response.Submissions.Labels).IsNotNull();
        await Assert.That(response.Submissions.Labels!.Count).IsEqualTo(2);
        var label = response.Submissions.Labels![0];
        await Assert.That(label.Id).IsEqualTo(1742496);
        await Assert.That(label.Name).IsEqualTo("Echoes Of Death Prods");
        await Assert.That(label.Uri).IsEqualTo("https://www.discogs.com/label/1742496-Echoes-Of-Death-Prods");
        await Assert.That(label.Profile).IsEqualTo("ECHOES OF DEATH RECORDS\r\nDeath Black Metal label from Brazil\r\nsatanic666war@hotmail.com\r\nWhatsapp: (55) 75 99969-3147");
        await Assert.That(label.DataQuality).IsEqualTo("Needs Vote");

        await Assert.That(response.Submissions.Artists).IsNotNull();
        await Assert.That(response.Submissions.Artists!.Count).IsEqualTo(10);
        var artist = response.Submissions.Artists![0];
        await Assert.That(artist.Id).IsEqualTo(15870707);
        await Assert.That(artist.Name).IsEqualTo("Hessian...");
        await Assert.That(artist.Uri).IsEqualTo("https://www.discogs.com/artist/15870707-Hessian");
        await Assert.That(artist.Urls).IsEquivalentTo(["https://hessian666.bandcamp.com", "https://www.instagram.com/hessian.666"]);
        await Assert.That(artist.DataQuality).IsEqualTo("Needs Vote");
    }
}
