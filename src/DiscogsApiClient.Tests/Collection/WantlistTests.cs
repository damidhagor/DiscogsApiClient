namespace DiscogsApiClient.Tests.Collection;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class WantlistTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetWantlistReleases_ShouldReturnReleases_WhenUsernameIsValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var summedUpItemCount = 0;

        var response = await _apiClient.GetWantlistReleases(username, paginationParams, cancellationToken);
        var itemCount = response.Pagination.TotalItems;
        summedUpItemCount += response.Releases.Count;

        for (var p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await _apiClient.GetWantlistReleases(username, paginationParams, cancellationToken);
            summedUpItemCount += response.Releases.Count;
        }

        await Assert.That(itemCount).IsEqualTo(summedUpItemCount);
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetWantlistReleases_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetWantlistReleases(username!, null, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task GetWantlistReleases_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetWantlistReleases(username, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task AddReleaseToWantlist_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var exception = await Assert.That(async () => await _apiClient.AddReleaseToWantlist(username!, releaseId, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task AddReleaseToWantlist_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var releaseId = 5134861;

        await Assert.That(async () => await _apiClient.AddReleaseToWantlist(username, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task AddReleaseToWantlist_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        await Assert.That(async () => await _apiClient.AddReleaseToWantlist(username, releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task AddReleaseToWantlist_ShouldThrowResourceNotFoundException_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = int.MaxValue;

        await Assert.That(async () => await _apiClient.AddReleaseToWantlist(username, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task AddReleaseToWantlist_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;

        await Assert.That(async () => await _unauthenticatedApiClient.AddReleaseToWantlist(username, releaseId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task DeleteReleaseFromWantlist_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var exception = await Assert.That(async () => await _apiClient.DeleteReleaseFromWantlist(username!, releaseId, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task DeleteReleaseFromWantlist_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var releaseId = 5134861;

        await Assert.That(async () => await _apiClient.DeleteReleaseFromWantlist(username, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task DeleteReleaseFromWantlist_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        await Assert.That(async () => await _apiClient.DeleteReleaseFromWantlist(username, releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task DeleteReleaseFromWantlist_ShouldSucceed_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = int.MaxValue;

        await Assert.That(async () => await _apiClient.DeleteReleaseFromWantlist(username, releaseId, cancellationToken))
            .ThrowsNothing();
    }

    [Test]
    public async Task DeleteReleaseFromWantlist_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;

        await Assert.That(async () => await _unauthenticatedApiClient.DeleteReleaseFromWantlist(username, releaseId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    public async Task AddAndRemoveReleaseFromWantlist_ShouldSucceed_WhenParametersAreValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;

        // Add
        var addedRelease = await _apiClient.AddReleaseToWantlist(username, releaseId, cancellationToken);
        await Assert.That(addedRelease).IsNotNull();
        await Assert.That(addedRelease.Id).IsEqualTo(releaseId);
        await Assert.That(addedRelease.Notes).IsNullOrWhiteSpace();
        await Assert.That(addedRelease.Rating).IsEqualTo(0);
        await Assert.That(addedRelease.ResourceUrl).IsNotNullOrWhiteSpace();
        await Assert.That(addedRelease.Release).IsNotNull();
        await Assert.That(addedRelease.Release.Id).IsEqualTo(releaseId);
        await Assert.That(addedRelease.Release.Year).IsEqualTo(1997);
        await Assert.That(addedRelease.Release.Title).IsEqualTo("Glory To The Brave");
        await Assert.That(addedRelease.Release.ResourceUrl).IsNotNullOrWhiteSpace();
        await Assert.That(addedRelease.Release.ThumbnailUrl).IsNotNullOrWhiteSpace();
        await Assert.That(addedRelease.Release.MasterId).IsEqualTo(156551);
        await Assert.That(addedRelease.Release.MasterUrl).IsNotNullOrWhiteSpace();
        await Assert.That(addedRelease.Release.Artists.Count).IsEqualTo(1);
        await Assert.That(addedRelease.Release.Artists[0].Id).IsEqualTo(287459);
        await Assert.That(addedRelease.Release.Artists[0].Name).IsEqualTo("HammerFall");
        await Assert.That(() => new Uri(addedRelease.Release.Artists[0].ResourceUrl)).ThrowsNothing();
        await Assert.That(addedRelease.Release.Artists[0].ThumbnailUrl).IsNull();
        await Assert.That(addedRelease.Release.Formats).IsNotNull();
        await Assert.That(addedRelease.Release.Formats.Count).IsGreaterThan(0);
        await Assert.That(addedRelease.Release.Formats[0].Count).IsGreaterThan("0");
        await Assert.That(addedRelease.Release.Formats[0].Name).IsNotNullOrWhiteSpace();
        await Assert.That(addedRelease.Release.Formats[0].Descriptions).IsNotNull();
        await Assert.That(addedRelease.Release.Formats[0].Descriptions.Count).IsGreaterThan(0);
        await Assert.That(addedRelease.Release.Formats[0].Descriptions[0]).IsNotNullOrWhiteSpace();
        await Assert.That(addedRelease.Release.Genres).IsNotNull();
        await Assert.That(addedRelease.Release.Genres.Count).IsGreaterThan(0);
        await Assert.That(addedRelease.Release.Genres[0]).IsNotNullOrWhiteSpace();
        await Assert.That(addedRelease.Release.Styles).IsNotNull();
        await Assert.That(addedRelease.Release.Styles.Count).IsGreaterThan(0);
        await Assert.That(addedRelease.Release.Styles[0]).IsNotNullOrWhiteSpace();
        await Assert.That(addedRelease.Release.Labels).IsNotNull();
        await Assert.That(addedRelease.Release.Labels.Count).IsGreaterThan(0);

        // Delete
        await Assert.That(async () => await _apiClient.DeleteReleaseFromWantlist(username, releaseId, cancellationToken)).ThrowsNothing();
    }
}
