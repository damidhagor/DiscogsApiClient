using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionFoldersTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetCollectionFolders_Success()
    {
        var username = "damidhagor";

        var foldersResponse = await ApiClient.GetCollectionFolders(username, default);

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
    public void GetCollectionFolders_EmptyUsername()
    {
        var username = "";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolders(username, default), "username");
    }

    [Test]
    public void GetCollectionFolders_InvalidUsername()
    {
        var username = "awrbaerhnqw54";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolders(username, default));
    }


    [Test]
    public async Task GetCollectionFolder_Success()
    {
        var username = "damidhagor";
        var folderId = 0;

        var folder = await ApiClient.GetCollectionFolder(username, folderId, default);

        Assert.IsNotNull(folder);
        Assert.AreEqual(0, folder!.Id);
        Assert.AreEqual("All", folder!.Name);
    }

    [Test]
    public void GetCollectionFolder_EmptyUsername()
    {
        var username = "";
        var folderId = 0;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolder(username, folderId, default), "username");
    }

    [Test]
    public void GetCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 0;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolder(username, folderId, default));
    }

    [Test]
    public void GetCollectionFolder_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolder(username, folderId, default));
    }

    [Test]
    public void GetCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolder(username, folderId, default));
    }


    [Test]
    public void CreateCollectionFolder_EmptyUsername()
    {
        var username = "";
        var folderName = "API_TEST_CREATE_EMPTY_USERNAME";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.CreateCollectionFolder(username, folderName, default), "username");
    }

    [Test]
    public void CreateCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderName = "API_TEST_CREATE_INVALID_USERNAME";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.CreateCollectionFolder(username, folderName, default));
    }

    [Test]
    public void CreateCollectionFolder_EmptyFolderName()
    {
        var username = "damidhagor";
        var folderName = "";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.CreateCollectionFolder(username, folderName, default), "createFolderRequest");
    }


    [Test]
    public void UpdateCollectionFolder_EmptyUsername()
    {
        var username = "";
        var folderId = 0;
        var folderName = "API_TEST_UPDATE_EMPTY_USERNAME";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.UpdateCollectionFolder(username, folderId, folderName, default), "username");
    }

    [Test]
    public void UpdateCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 0;
        var folderName = "API_TEST_UPDATE_INVALID_USERNAME";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.UpdateCollectionFolder(username, folderId, folderName, default));
    }

    [Test]
    public void UpdateCollectionFolder_EmptyFolderName()
    {
        var username = "damidhagor";
        var folderId = 0;
        var folderName = "";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.UpdateCollectionFolder(username, folderId, folderName, default), "createFolderRequest");
    }

    [Test]
    public void UpdateCollectionFolder_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;
        var folderName = "API_TEST_UPDATE_INVALID_ID";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.UpdateCollectionFolder(username, folderId, folderName, default));
    }

    [Test]
    public void UpdateCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;
        var folderName = "API_TEST_UPDATE_NOT_EXISTING_ID";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.UpdateCollectionFolder(username, folderId, folderName, default));
    }


    [Test]
    public void DeleteCollectionFolder_EmptyUsername()
    {
        var username = "";
        var folderId = -1;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteCollectionFolder(username, folderId, default), "username");
    }

    [Test]
    public void DeleteCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 0;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteCollectionFolder(username, folderId, default));
    }

    [Test]
    public void DeleteCollectionFolder_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteCollectionFolder(username, folderId, default));
    }

    [Test]
    public void DeleteCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteCollectionFolder(username, folderId, default));
    }


    [Test]
    public async Task CreateUpdateDeleteCollectionFolder_Success()
    {
        var username = "damidhagor";
        var folderName1 = "API_TEST_WORKFLOW_CREATE";
        var folderName2 = "API_TEST_WORKFLOW_UPDATE";

        // Add
        var createdFolder = await ApiClient.CreateCollectionFolder(username, folderName1, default);
        Assert.IsNotNull(createdFolder);
        Assert.AreEqual(folderName1, createdFolder!.Name);

        // Update
        var updatedFolder = await ApiClient.UpdateCollectionFolder(username, createdFolder.Id, folderName2, default);
        Assert.IsNotNull(updatedFolder);
        Assert.AreEqual(folderName2, updatedFolder!.Name);
        Assert.AreEqual(createdFolder.Id, updatedFolder!.Id);

        // Delete
        Assert.DoesNotThrowAsync(() => ApiClient.DeleteCollectionFolder(username, createdFolder.Id, default));
    }


    [Test]
    public async Task GetCollectionValue_Success()
    {
        var username = "damidhagor";

        var collectionValue = await ApiClient.GetCollectionValue(username, default);

        Assert.IsNotNull(collectionValue);
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Minimum));
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Median));
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Maximum));
    }


    [Test]
    public void GetCollectionValue_EmptyUsername()
    {
        var username = "";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionValue(username, default), "username");
    }

    [Test]
    public void GetCollectionValue_InvalidUsername()
    {
        var username = "awrbaerhnqw54";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionValue(username, default));
    }
}
