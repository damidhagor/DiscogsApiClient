namespace DiscogsApiClient.Tests.Collection;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class CollectionFoldersTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetCollectionFolders_ShouldReturnFolders_WhenUsernameIsValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var foldersResponse = await _apiClient.GetCollectionFolders(username, cancellationToken);

        await Assert.That(foldersResponse?.Folders).IsNotNull();
        await Assert.That(foldersResponse!.Folders.Count).IsGreaterThanOrEqualTo(2);

        var allFolder = foldersResponse.Folders.FirstOrDefault(f => f.Id == 0);
        var uncategorizedFolder = foldersResponse.Folders.FirstOrDefault(f => f.Id == 1);

        await Assert.That(allFolder).IsNotNull();
        await Assert.That(allFolder!.Id).IsEqualTo(0);
        await Assert.That(allFolder!.Name).IsEqualTo("All");
        await Assert.That(allFolder!.ResourceUrl).IsNotNullOrWhiteSpace();

        await Assert.That(uncategorizedFolder).IsNotNull();
        await Assert.That(uncategorizedFolder!.Id).IsEqualTo(1);
        await Assert.That(uncategorizedFolder!.Name).IsEqualTo("Uncategorized");
        await Assert.That(uncategorizedFolder!.ResourceUrl).IsNotNullOrWhiteSpace();
    }

    [Test]
    public async Task GetCollectionFolders_ShouldReturnPublicFolders_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var foldersResponse = await _unauthenticatedApiClient.GetCollectionFolders(username, cancellationToken);

        await Assert.That(foldersResponse?.Folders).IsNotNull();
        await Assert.That(foldersResponse!.Folders.Count).IsEqualTo(1);

        var allFolder = foldersResponse.Folders.FirstOrDefault(f => f.Id == 0);

        await Assert.That(allFolder).IsNotNull();
        await Assert.That(allFolder!.Id).IsEqualTo(0);
        await Assert.That(allFolder!.Name).IsEqualTo("All");
        await Assert.That(allFolder!.ResourceUrl).IsNotNullOrWhiteSpace();
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetCollectionFolders_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetCollectionFolders(username!, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task GetCollectionFolders_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetCollectionFolders(username, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }


    [Test]
    public async Task GetCollectionFolder_ShouldReturnFolder_WhenFolderExists(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 1;

        var folder = await _apiClient.GetCollectionFolder(username, folderId, cancellationToken);

        await Assert.That(folder).IsNotNull();
        await Assert.That(folder!.Id).IsEqualTo(1);
        await Assert.That(folder!.Name).IsEqualTo("Uncategorized");
    }

    [Test]
    public async Task GetCollectionFolder_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 1;

        await Assert.That(async () => await _unauthenticatedApiClient.GetCollectionFolder(username, folderId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetCollectionFolder_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 0;

        var exception = await Assert.That(async () => await _apiClient.GetCollectionFolder(username!, folderId, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task GetCollectionFolder_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 0;

        await Assert.That(async () => await _apiClient.GetCollectionFolder(username, folderId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task GetCollectionFolder_ShouldThrowArgumentOutOfRangeException_WhenFolderIdIsInvalid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = -1;

        await Assert.That(async () => await _apiClient.GetCollectionFolder(username, folderId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task GetCollectionFolder_ShouldThrowResourceNotFoundException_WhenFolderDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 42;

        await Assert.That(async () => await _apiClient.GetCollectionFolder(username, folderId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task CreateCollectionFolder_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderName = "API_TEST_CREATE_EMPTY_USERNAME";

        var exception = await Assert.That(async () => await _apiClient.CreateCollectionFolder(username!, folderName, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task CreateCollectionFolder_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderName = "API_TEST_CREATE_INVALID_USERNAME";

        await Assert.That(async () => await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task CreateCollectionFolder_ShouldThrowException_WhenFolderNameIsInvalid(string? folderName, Type expectedException, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var exception = await Assert.That(async () => await _apiClient.CreateCollectionFolder(username, folderName!, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("folderName");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task CreateCollectionFolder_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        await Assert.That(async () => await _unauthenticatedApiClient.CreateCollectionFolder(username, "API_TEST_CREATE_INVALID_USERNAME", cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateCollectionFolder_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_EMPTY_USERNAME";

        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolder(username!, folderId, folderName, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task UpdateCollectionFolder_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_INVALID_USERNAME";

        await Assert.That(async () => await _apiClient.UpdateCollectionFolder(username, folderId, folderName, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateCollectionFolder_ShouldThrowException_WhenFolderNameIsInvalid(string? folderName, Type expectedException, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolder(username, folderId, folderName!, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("folderName");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    public async Task UpdateCollectionFolder_ShouldThrowArgumentOutOfRangeException_WhenFolderIdIsInvalid(int folderId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "API_TEST_UPDATE_INVALID_ID";

        await Assert.That(async () => await _apiClient.UpdateCollectionFolder(username, folderId, folderName, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task UpdateCollectionFolder_ShouldThrowResourceNotFoundException_WhenFolderDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_NOT_EXISTING_ID";

        await Assert.That(async () => await _apiClient.UpdateCollectionFolder(username, folderId, folderName, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task UpdateCollectionFolder_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_NOT_EXISTING_ID";

        await Assert.That(async () => await _unauthenticatedApiClient.UpdateCollectionFolder(username, folderId, folderName, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task DeleteCollectionFolder_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = -1;

        var exception = await Assert.That(async () => await _apiClient.DeleteCollectionFolder(username!, folderId, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task DeleteCollectionFolder_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;

        await Assert.That(async () => await _apiClient.DeleteCollectionFolder(username, folderId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    public async Task DeleteCollectionFolder_ShouldThrowArgumentOutOfRangeException_WhenFolderIdIsInvalid(int folderId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        await Assert.That(async () => await _apiClient.DeleteCollectionFolder(username, folderId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task DeleteCollectionFolder_ShouldThrowResourceNotFoundException_WhenFolderDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        await Assert.That(async () => await _apiClient.DeleteCollectionFolder(username, folderId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task DeleteCollectionFolder_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        await Assert.That(async () => await _unauthenticatedApiClient.DeleteCollectionFolder(username, folderId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    public async Task CreateUpdateDeleteCollectionFolder_ShouldSucceed_WhenParametersAreValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName1 = "API_TEST_WORKFLOW_CREATE";
        var folderName2 = "API_TEST_WORKFLOW_UPDATE";

        // Add
        var createdFolder = await _apiClient.CreateCollectionFolder(username, folderName1, cancellationToken);
        await Assert.That(createdFolder).IsNotNull();
        await Assert.That(createdFolder!.Name).IsEqualTo(folderName1);

        // Update
        var updatedFolder = await _apiClient.UpdateCollectionFolder(username, createdFolder.Id, folderName2, cancellationToken);
        await Assert.That(updatedFolder).IsNotNull();
        await Assert.That(updatedFolder!.Name).IsEqualTo(folderName2);
        await Assert.That(updatedFolder!.Id).IsEqualTo(createdFolder.Id);

        // Delete
        await Assert.That(async () => await _apiClient.DeleteCollectionFolder(username, createdFolder.Id, cancellationToken)).ThrowsNothing();
    }
}
