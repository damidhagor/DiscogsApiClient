using System;
using System.Threading.Tasks;
using DiscogsApiClient.Exceptions;
using DiscogsApiClient.QueryParameters;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionFolderReleasesTestFixture : ApiBaseTestFixture
{
    [Test]
    public void GetCollectionFolderReleases_EmptyUsername()
    {
        var username = "";
        var folderId = 0;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParameters, default), "username");
    }

    [Test]
    public void GetCollectionFolderReleases_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 0;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParameters, default));
    }

    [Test]
    public void GetCollectionFolderReleases_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParameters, default));
    }

    [Test]
    public void GetCollectionFolderReleases_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParameters, default));
    }


    [Test]
    public void AddReleaseToCollectionFolder_EmptyUsername()
    {
        var username = "";
        var folderId = 1;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, default), "username");
    }

    [Test]
    public void AddReleaseToCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 1;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, default));
    }

    [Test]
    public void AddReleaseToCollectionFolder_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, default));
    }

    [Test]
    public void AddReleaseToCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, default));
    }

    [Test]
    public void AddReleaseToCollectionFolder_AllFolderId()
    {
        var username = "damidhagor";
        var folderId = 0;
        var releaseId = 5134861;

        Assert.ThrowsAsync<DiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, default),
            "Invalid folder_id: cannot add releases to the 'All' folder.");
    }

    [Test]
    public void AddReleaseToCollectionFolder_InvalidReleaseId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, default));
    }

    [Test]
    public void AddReleaseToCollectionFolder_NotExistingReleaseId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, default));
    }


    [Test]
    public void DeleteReleaseFromCollectionFolder_EmptyUsername()
    {
        var username = "";
        var folderId = 1;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, default), "username");
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 1;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidReleaseId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = -1;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingReleaseId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = int.MaxValue;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidInstanceId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingInstanceId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = 5134861;
        var instanceId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, default));
    }


    [Test]
    public async Task CreateDeleteReleaseFromCollectionFolder_Success()
    {
        var username = "damidhagor";
        var folderName = "CreateDeleteReleaseFromCollectionFolder_Success";
        var releaseId = 5134861;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        // Create test folder
        var collectionFolder = await ApiClient.CreateCollectionFolder(username, folderName, default);

        // Add release to folder
        var collectionFolderRelease = await ApiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId, default);

        // Get release from folder
        var collectionFolderReleaseResponse = await ApiClient.GetCollectionFolderReleases(username, collectionFolder.Id, paginationParams, sortParameters, default);

        // Delete release from folder
        Assert.DoesNotThrowAsync(() => ApiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, collectionFolderRelease.Id, collectionFolderRelease.InstanceId, default));

        // Delete folder
        Assert.DoesNotThrowAsync(() => ApiClient.DeleteCollectionFolder(username, collectionFolder.Id, default));


        Assert.IsNotNull(collectionFolderRelease);
        Assert.AreEqual(releaseId, collectionFolderRelease.Id);
        Assert.IsNotNull(collectionFolderReleaseResponse);
        Assert.AreEqual(1, collectionFolderReleaseResponse.Pagination.Items);
        Assert.AreEqual(releaseId, collectionFolderReleaseResponse.Releases[0].Id);
    }
}
