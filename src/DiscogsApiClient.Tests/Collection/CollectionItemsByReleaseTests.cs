namespace DiscogsApiClient.Tests.Collection;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class CollectionItemsByReleaseTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetCollectionItemsByRelease_ShouldReturnReleases_WhenReleaseIsInCollection(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "GetCollectionItemsByRelease_Success";
        var releaseId = 5134861;

        var collectionFolder = await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken);

        try
        {
            var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId, cancellationToken);

            var response = await _apiClient.GetCollectionItemsByRelease(username, releaseId, null, cancellationToken);

            await Assert.That(response).IsNotNull();
            await Assert.That(response.Releases).IsNotEmpty();
            await Assert.That(response.Releases.Select(r => r.Id)).Contains(releaseId);
            await Assert.That(response.Releases.Select(r => r.FolderId)).Contains(collectionFolder.Id);

            await _apiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
        }
        finally
        {
            await _apiClient.DeleteCollectionFolder(username, collectionFolder.Id, cancellationToken);
        }
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetCollectionItemsByRelease_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var exception = await Assert.That(async () => await _apiClient.GetCollectionItemsByRelease(username!, releaseId, null, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task GetCollectionItemsByRelease_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var releaseId = 5134861;

        await Assert.That(async () => await _apiClient.GetCollectionItemsByRelease(username, releaseId, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task GetCollectionItemsByRelease_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var exception = await Assert.That(async () => await _apiClient.GetCollectionItemsByRelease(username, releaseId, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    public async Task GetCollectionItemsByRelease_ShouldReturnEmptyReleases_WhenReleaseIsNotInCollection(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = int.MaxValue;

        var response = await _apiClient.GetCollectionItemsByRelease(username, releaseId, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Releases).IsEmpty();
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(0);
    }

    [Test]
    public async Task GetCollectionItemsByRelease_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;

        await Assert.That(async () => await _unauthenticatedApiClient.GetCollectionItemsByRelease(username, releaseId, null, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }
}
