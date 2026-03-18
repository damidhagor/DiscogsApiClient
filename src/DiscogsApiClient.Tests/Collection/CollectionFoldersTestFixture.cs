namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionFoldersTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetCollectionFolders_Success(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var foldersResponse = await ApiClient.GetCollectionFolders(username, cancellationToken);

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
    public async Task GetCollectionFolders_Unauthenticated_Success(CancellationToken cancellationToken)
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";

        var foldersResponse = await clients.discogsApiClient.GetCollectionFolders(username, cancellationToken);

        await Assert.That(foldersResponse?.Folders).IsNotNull();
        await Assert.That(foldersResponse!.Folders.Count).IsEqualTo(1);

        var allFolder = foldersResponse.Folders.FirstOrDefault(f => f.Id == 0);

        await Assert.That(allFolder).IsNotNull();
        await Assert.That(allFolder!.Id).IsEqualTo(0);
        await Assert.That(allFolder!.Name).IsEqualTo("All");
        await Assert.That(allFolder!.ResourceUrl).IsNotNullOrWhiteSpace();

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetCollectionFolders_Username_Guard(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await ApiClient.GetCollectionFolders(username!, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task GetCollectionFolders_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await ApiClient.GetCollectionFolders(username, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }


    [Test]
    public async Task GetCollectionFolder_Success(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 1;

        var folder = await ApiClient.GetCollectionFolder(username, folderId, cancellationToken);

        await Assert.That(folder).IsNotNull();
        await Assert.That(folder!.Id).IsEqualTo(1);
        await Assert.That(folder!.Name).IsEqualTo("Uncategorized");
    }

    [Test]
    public async Task GetCollectionFolder_Unauthenticated(CancellationToken cancellationToken)
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();

        var username = "DamIDhagor";
        var folderId = 1;

        await Assert.That(async () => await clients.discogsApiClient.GetCollectionFolder(username, folderId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetCollectionFolder_Username_Guard(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 0;

        var exception = await Assert.That(async () => await ApiClient.GetCollectionFolder(username!, folderId, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task GetCollectionFolder_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 0;

        await Assert.That(async () => await ApiClient.GetCollectionFolder(username, folderId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task GetCollectionFolder_FolderId_Guard(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = -1;

        await Assert.That(async () => await ApiClient.GetCollectionFolder(username, folderId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task GetCollectionFolder_NotExistingFolderId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 42;

        await Assert.That(async () => await ApiClient.GetCollectionFolder(username, folderId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task CreateCollectionFolder_Username_Guard(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderName = "API_TEST_CREATE_EMPTY_USERNAME";

        var exception = await Assert.That(async () => await ApiClient.CreateCollectionFolder(username!, folderName, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task CreateCollectionFolder_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderName = "API_TEST_CREATE_INVALID_USERNAME";

        await Assert.That(async () => await ApiClient.CreateCollectionFolder(username, folderName, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task CreateCollectionFolder_FolderName_Guard(string? folderName, Type expectedException, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var exception = await Assert.That(async () => await ApiClient.CreateCollectionFolder(username, folderName!, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("folderName");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task CreateCollectionFolder_Unauthenticated(CancellationToken cancellationToken)
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";

        await Assert.That(async () => await clients.discogsApiClient.CreateCollectionFolder(username, "API_TEST_CREATE_INVALID_USERNAME", cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateCollectionFolder_Username_Guard(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_EMPTY_USERNAME";

        var exception = await Assert.That(async () => await ApiClient.UpdateCollectionFolder(username!, folderId, folderName, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task UpdateCollectionFolder_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_INVALID_USERNAME";

        await Assert.That(async () => await ApiClient.UpdateCollectionFolder(username, folderId, folderName, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateCollectionFolder_FolderName_Guard(string? folderName, Type expectedException, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        var exception = await Assert.That(async () => await ApiClient.UpdateCollectionFolder(username, folderId, folderName!, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("folderName");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    public async Task UpdateCollectionFolder_FolderId_Guard(int folderId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "API_TEST_UPDATE_INVALID_ID";

        await Assert.That(async () => await ApiClient.UpdateCollectionFolder(username, folderId, folderName, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task UpdateCollectionFolder_NotExistingFolderId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_NOT_EXISTING_ID";

        await Assert.That(async () => await ApiClient.UpdateCollectionFolder(username, folderId, folderName, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task UpdateCollectionFolder_Unauthenticated(CancellationToken cancellationToken)
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_NOT_EXISTING_ID";

        await Assert.That(async () => await clients.discogsApiClient.UpdateCollectionFolder(username, folderId, folderName, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task DeleteCollectionFolder_Username_Guard(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = -1;

        var exception = await Assert.That(async () => await ApiClient.DeleteCollectionFolder(username!, folderId, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task DeleteCollectionFolder_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;

        await Assert.That(async () => await ApiClient.DeleteCollectionFolder(username, folderId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(1)]
    public async Task DeleteCollectionFolder_FolderId_Guard(int folderId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        await Assert.That(async () => await ApiClient.DeleteCollectionFolder(username, folderId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task DeleteCollectionFolder_NotExistingFolderId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        await Assert.That(async () => await ApiClient.DeleteCollectionFolder(username, folderId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task DeleteCollectionFolder_Unauthenticated(CancellationToken cancellationToken)
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";
        var folderId = 999;

        await Assert.That(async () => await clients.discogsApiClient.DeleteCollectionFolder(username, folderId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public async Task CreateUpdateDeleteCollectionFolder_Success(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName1 = "API_TEST_WORKFLOW_CREATE";
        var folderName2 = "API_TEST_WORKFLOW_UPDATE";

        // Add
        var createdFolder = await ApiClient.CreateCollectionFolder(username, folderName1, cancellationToken);
        await Assert.That(createdFolder).IsNotNull();
        await Assert.That(createdFolder!.Name).IsEqualTo(folderName1);

        // Update
        var updatedFolder = await ApiClient.UpdateCollectionFolder(username, createdFolder.Id, folderName2, cancellationToken);
        await Assert.That(updatedFolder).IsNotNull();
        await Assert.That(updatedFolder!.Name).IsEqualTo(folderName2);
        await Assert.That(updatedFolder!.Id).IsEqualTo(createdFolder.Id);

        // Delete
        await Assert.That(async () => await ApiClient.DeleteCollectionFolder(username, createdFolder.Id, cancellationToken)).ThrowsNothing();
    }
}
