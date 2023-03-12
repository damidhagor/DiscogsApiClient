using static DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;

namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionFolderReleasesTestFixture : ApiBaseTestFixture
{
    [Test]
    public void GetCollectionFolderReleases_Username_Guard()
    {
        var folderId = 999;
        var paginationParams = new PaginationQueryParameters();
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.GetCollectionFolderReleases(null!, folderId, paginationParams, sortParameters, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolderReleases("", folderId, paginationParams, sortParameters, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolderReleases("  ", folderId, paginationParams, sortParameters, default), "username");
    }

    [Test]
    public void GetCollectionFolderReleases_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var paginationParams = new PaginationQueryParameters();
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParameters, default));
    }

    [Test]
    public void GetCollectionFolderReleases_FolderId_Guard()
    {
        var username = "damidhagor";
        var paginationParams = new PaginationQueryParameters();
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetCollectionFolderReleases(username, -1, paginationParams, sortParameters, default));
    }

    [Test]
    public void GetCollectionFolderReleases_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 999;
        var paginationParams = new PaginationQueryParameters();
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParameters, default));
    }

    [Test]
    public void GetCollectionFolderReleases_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "damidhagor";
        var folderId = 1;
        var paginationParams = new PaginationQueryParameters();
        var sortParameters = new CollectionFolderReleaseSortQueryParameters();

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => clients.discogsApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParameters, default));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }

    [Test]
    public async Task GetCollectionFolderReleases_Sorted()
    {
        var username = "damidhagor";
        var folderId = 0;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        // Artist
        var sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Artist, SortOrder = SortOrder.Ascending };
        var responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, default);
        var sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Artist, SortOrder = SortOrder.Descending };
        var responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, default);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Artists.First().Name),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Artists.First().Name),
            Is.Ordered.Descending);

        // Label
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, default);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, default);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Labels.First().Name),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Labels.First().Name),
            Is.Ordered.Descending);

        // Title
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, default);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, default);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Title),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Title),
            Is.Ordered.Descending);

        // CatalogNumber
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, default);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, default);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Labels.First().CatalogNumber),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Labels.First().CatalogNumber),
            Is.Ordered.Descending);

        // Format
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, default);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, default);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Formats.First().Name),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Formats.First().Name),
            Is.Ordered.Descending);

        // Rating
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Rating, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, default);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Rating, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, default);

        Assert.That(
            responseAscending.Releases.Select(r => r.Rating),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Rating),
            Is.Ordered.Descending);

        // AddedAt
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.AddedAt, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, default);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.AddedAt, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, default);

        Assert.That(
            responseAscending.Releases.Select(r => r.AddedAt),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.AddedAt),
            Is.Ordered.Descending);

        // Year
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, default);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, default);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Year),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Year),
            Is.Ordered.Descending);
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
        Assert.AreEqual(1, collectionFolderReleaseResponse.Pagination.TotalItems);
        Assert.AreEqual(releaseId, collectionFolderReleaseResponse.Releases[0].Id);
    }
}
