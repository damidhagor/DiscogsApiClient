namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionFoldersTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetCollectionFolders_Success()
    {
        var username = "damidhagor";

        var foldersResponse = await ApiClient.GetCollectionFolders(username);

        Assert.IsNotNull(foldersResponse?.Folders);
        Assert.LessOrEqual(2, foldersResponse!.Folders.Count);

        var allFolder = foldersResponse.Folders.FirstOrDefault(f => f.Id == 0);
        var uncategorizedFolder = foldersResponse.Folders.FirstOrDefault(f => f.Id == 1);

        Assert.IsNotNull(allFolder);
        Assert.AreEqual(0, allFolder!.Id);
        Assert.AreEqual("All", allFolder!.Name);
        Assert.IsFalse(string.IsNullOrWhiteSpace(allFolder!.ResourceUrl));

        Assert.IsNotNull(uncategorizedFolder);
        Assert.AreEqual(1, uncategorizedFolder!.Id);
        Assert.AreEqual("Uncategorized", uncategorizedFolder!.Name);
        Assert.IsFalse(string.IsNullOrWhiteSpace(uncategorizedFolder!.ResourceUrl));
    }

    [Test]
    public async Task GetCollectionFolders_Unauthenticated_Success()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "damidhagor";

        var foldersResponse = await clients.discogsApiClient.GetCollectionFolders(username);

        Assert.IsNotNull(foldersResponse?.Folders);
        Assert.AreEqual(1, foldersResponse!.Folders.Count);

        var allFolder = foldersResponse.Folders.FirstOrDefault(f => f.Id == 0);

        Assert.IsNotNull(allFolder);
        Assert.AreEqual(0, allFolder!.Id);
        Assert.AreEqual("All", allFolder!.Name);
        Assert.IsFalse(string.IsNullOrWhiteSpace(allFolder!.ResourceUrl));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }

    [Test]
    public void GetCollectionFolders_Username_Guard()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.GetCollectionFolders(null!), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolders(""), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolders("  "), "username");
    }

    [Test]
    public void GetCollectionFolders_InvalidUsername()
    {
        var username = "awrbaerhnqw54";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolders(username));
    }


    [Test]
    public async Task GetCollectionFolder_Success()
    {
        var username = "damidhagor";
        var folderId = 1;

        var folder = await ApiClient.GetCollectionFolder(username, folderId);

        Assert.IsNotNull(folder);
        Assert.AreEqual(1, folder!.Id);
        Assert.AreEqual("Uncategorized", folder!.Name);
    }

    [Test]
    public void GetCollectionFolder_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();

        var username = "damidhagor";
        var folderId = 1;

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => clients.discogsApiClient.GetCollectionFolder(username, folderId));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }

    [Test]
    public void GetCollectionFolder_Username_Guard()
    {
        var folderId = 0;

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.GetCollectionFolder(null!, folderId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolder("", folderId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolder("  ", folderId), "username");
    }

    [Test]
    public void GetCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 0;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolder(username, folderId));
    }

    [Test]
    public void GetCollectionFolder_FolderId_Guard()
    {
        var username = "damidhagor";
        var folderId = -1;

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetCollectionFolder(username, folderId), "folderId");
    }

    [Test]
    public void GetCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolder(username, folderId));
    }


    [Test]
    public void CreateCollectionFolder_Username_Guard()
    {
        var folderName = "API_TEST_CREATE_EMPTY_USERNAME";

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.CreateCollectionFolder(null!, folderName), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.CreateCollectionFolder("", folderName), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.CreateCollectionFolder("  ", folderName), "username");
    }

    [Test]
    public void CreateCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderName = "API_TEST_CREATE_INVALID_USERNAME";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.CreateCollectionFolder(username, folderName));
    }

    [Test]
    public void CreateCollectionFolder_FolderName_Guard()
    {
        var username = "damidhagor";

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.CreateCollectionFolder(username, null!), "folderName");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.CreateCollectionFolder(username, ""), "folderName");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.CreateCollectionFolder(username, "  "), "folderName");
    }

    [Test]
    public void CreateCollectionFolder_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "damidhagor";

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => clients.discogsApiClient.CreateCollectionFolder(username, "API_TEST_CREATE_INVALID_USERNAME"));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public void UpdateCollectionFolder_Username_Guard()
    {
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_EMPTY_USERNAME";

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.UpdateCollectionFolder(null!, folderId, folderName), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.UpdateCollectionFolder("", folderId, folderName), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.UpdateCollectionFolder("  ", folderId, folderName), "username");
    }

    [Test]
    public void UpdateCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_INVALID_USERNAME";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.UpdateCollectionFolder(username, folderId, folderName));
    }

    [Test]
    public void UpdateCollectionFolder_FolderName_Guard()
    {
        var username = "damidhagor";
        var folderId = 999;

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.UpdateCollectionFolder(username, folderId, null!), "folderName");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.UpdateCollectionFolder(username, folderId, ""), "folderName");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.UpdateCollectionFolder(username, folderId, "  "), "folderName");
    }

    [Test]
    public void UpdateCollectionFolder_FolderId_Guard()
    {
        var username = "damidhagor";
        var folderName = "API_TEST_UPDATE_INVALID_ID";

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.UpdateCollectionFolder(username, -1, folderName));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.UpdateCollectionFolder(username, 0, folderName));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.UpdateCollectionFolder(username, 1, folderName));
    }

    [Test]
    public void UpdateCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_NOT_EXISTING_ID";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.UpdateCollectionFolder(username, folderId, folderName));
    }

    [Test]
    public void UpdateCollectionFolder_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "damidhagor";
        var folderId = 999;
        var folderName = "API_TEST_UPDATE_NOT_EXISTING_ID";

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => clients.discogsApiClient.UpdateCollectionFolder(username, folderId, folderName));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public void DeleteCollectionFolder_Username_Guard()
    {
        var folderId = -1;

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.DeleteCollectionFolder(null!, folderId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteCollectionFolder("", folderId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteCollectionFolder("  ", folderId), "username");
    }

    [Test]
    public void DeleteCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteCollectionFolder(username, folderId));
    }

    [Test]
    public void DeleteCollectionFolder_FolderId_Guard()
    {
        var username = "damidhagor";

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteCollectionFolder(username, -1), "folderId");
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteCollectionFolder(username, 0), "folderId");
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteCollectionFolder(username, 1), "folderId");
    }

    [Test]
    public void DeleteCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteCollectionFolder(username, folderId));
    }

    [Test]
    public void DeleteCollectionFolder_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "damidhagor";
        var folderId = 999;

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => clients.discogsApiClient.DeleteCollectionFolder(username, folderId));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public async Task CreateUpdateDeleteCollectionFolder_Success()
    {
        var username = "damidhagor";
        var folderName1 = "API_TEST_WORKFLOW_CREATE";
        var folderName2 = "API_TEST_WORKFLOW_UPDATE";

        // Add
        var createdFolder = await ApiClient.CreateCollectionFolder(username, folderName1);
        Assert.IsNotNull(createdFolder);
        Assert.AreEqual(folderName1, createdFolder!.Name);

        // Update
        var updatedFolder = await ApiClient.UpdateCollectionFolder(username, createdFolder.Id, folderName2);
        Assert.IsNotNull(updatedFolder);
        Assert.AreEqual(folderName2, updatedFolder!.Name);
        Assert.AreEqual(createdFolder.Id, updatedFolder!.Id);

        // Delete
        Assert.DoesNotThrowAsync(() => ApiClient.DeleteCollectionFolder(username, createdFolder.Id));
    }
}
