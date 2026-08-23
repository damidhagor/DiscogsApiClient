namespace DiscogsApiClient.Tests.Database;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class ReleaseRatingTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetReleaseRating_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetReleaseRating(releaseId, "DamIDhagor", cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetReleaseRating_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetReleaseRating(5134861, username!, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task GetReleaseRating_ShouldThrowResourceNotFoundException_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        var releaseId = int.MaxValue;

        await Assert.That(async () => await _apiClient.GetReleaseRating(releaseId, "DamIDhagor", cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Release does not exist or may have been deleted.");
    }

    [Test]
    public async Task GetReleaseRating_ShouldReturnEmptyRating_WhenReleaseHasNotBeenRatedByUser(CancellationToken cancellationToken)
    {
        var releaseId = 38029494;
        var username = "DamIDhagor";

        var rating = await _apiClient.GetReleaseRating(releaseId, username, cancellationToken);
        await Assert.That(rating).IsNotNull();
        await Assert.That(rating.Username).IsEqualTo(username);
        await Assert.That(rating.ReleaseId).IsEqualTo(releaseId);
        await Assert.That(rating.Rating).IsEqualTo(0);
    }


    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task UpdateReleaseRating_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateReleaseRating(releaseId, "DamIDhagor", 3, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateReleaseRating_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateReleaseRating(5134861, username!, 3, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(6)]
    public async Task UpdateReleaseRating_ShouldThrowArgumentOutOfRangeException_WhenRatingIsInvalid(int rating, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateReleaseRating(5134861, "DamIDhagor", rating, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("rating");
    }

    [Test]
    public async Task UpdateReleaseRating_ShouldThrowResourceNotFoundException_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        var releaseId = int.MaxValue;

        await Assert.That(async () => await _apiClient.UpdateReleaseRating(releaseId, "DamIDhagor", 3, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Release does not exist or may have been deleted.");
    }

    [Test]
    public async Task UpdateReleaseRating_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.UpdateReleaseRating(5134861, "DamIDhagor", 3, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task DeleteReleaseRating_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.DeleteReleaseRating(releaseId, "DamIDhagor", cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task DeleteReleaseRating_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.DeleteReleaseRating(5134861, username!, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task DeleteReleaseRating_ShouldThrowResourceNotFoundException_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        var releaseId = int.MaxValue;

        await Assert.That(async () => await _apiClient.DeleteReleaseRating(releaseId, "DamIDhagor", cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Release does not exist or may have been deleted.");
    }

    [Test]
    public async Task DeleteReleaseRating_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.DeleteReleaseRating(5134861, "DamIDhagor", cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    public async Task UpdateGetAndDeleteReleaseRating_ShouldSucceed_WhenParametersAreValid(CancellationToken cancellationToken)
    {
        var releaseId = 38022159;
        var username = "DamIDhagor";

        try
        {
            var updatedRating = await _apiClient.UpdateReleaseRating(releaseId, username, 4, cancellationToken);
            await Assert.That(updatedRating).IsNotNull();
            await Assert.That(updatedRating.Username).IsEqualTo(username);
            await Assert.That(updatedRating.ReleaseId).IsEqualTo(releaseId);
            await Assert.That(updatedRating.Rating).IsEqualTo(4);

            await Task.Delay(100, cancellationToken);
            var fetchedRating = await _apiClient.GetReleaseRating(releaseId, username, cancellationToken);
            await Assert.That(fetchedRating).IsNotNull();
            await Assert.That(fetchedRating.Username).IsEqualTo(username);
            await Assert.That(fetchedRating.ReleaseId).IsEqualTo(releaseId);
            await Assert.That(fetchedRating.Rating).IsEqualTo(4);

            await Assert.That(async () => await _apiClient.DeleteReleaseRating(releaseId, username, cancellationToken)).ThrowsNothing();

            await Task.Delay(100, cancellationToken);
            var clearedRating = await _apiClient.GetReleaseRating(releaseId, username, cancellationToken);
            await Assert.That(clearedRating).IsNotNull();
            await Assert.That(clearedRating.Username).IsEqualTo(username);
            await Assert.That(clearedRating.ReleaseId).IsEqualTo(releaseId);
            await Assert.That(clearedRating.Rating).IsEqualTo(0);
        }
        finally
        {
            await _apiClient.DeleteReleaseRating(releaseId, username, cancellationToken);
        }
    }
}
