using static DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;

namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionFolderReleasesTestFixture : ApiBaseTestFixture
{
    [Test]
    public void GetCollectionFolderReleases_Username_Guard()
    {
        var folderId = 999;

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.GetCollectionFolderReleases(null!, folderId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolderReleases("", folderId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolderReleases("  ", folderId), "username");
    }

    [Test]
    public void GetCollectionFolderReleases_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleases(username, folderId));
    }

    [Test]
    public void GetCollectionFolderReleases_FolderId_Guard()
    {
        var username = "DamIDhagor";

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.GetCollectionFolderReleases(username, -1));
    }

    [Test]
    public void GetCollectionFolderReleases_NotExistingFolderId()
    {
        var username = "DamIDhagor";
        var folderId = 999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleases(username, folderId));
    }

    [Test]
    public void GetCollectionFolderReleases_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";
        var folderId = 1;

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => clients.discogsApiClient.GetCollectionFolderReleases(username, folderId));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }

    [Test]
    public async Task GetCollectionFolderReleases_Sorted()
    {
        var username = "DamIDhagor";
        var folderId = 0;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        // Artist
        var sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Artist, SortOrder = SortOrder.Ascending };
        var responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending);
        var sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Artist, SortOrder = SortOrder.Descending };
        var responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Artists.First().Name),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Artists.First().Name),
            Is.Ordered.Descending);

        // Label
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Labels.First().Name),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Labels.First().Name),
            Is.Ordered.Descending);

        // Title
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Title),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Title),
            Is.Ordered.Descending);

        // CatalogNumber
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Labels.First().CatalogNumber),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Labels.First().CatalogNumber),
            Is.Ordered.Descending);

        // Format
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Formats.First().Name),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Formats.First().Name),
            Is.Ordered.Descending);

        // Rating
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Rating, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Rating, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending);

        Assert.That(
            responseAscending.Releases.Select(r => r.Rating),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Rating),
            Is.Ordered.Descending);

        // AddedAt
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.AddedAt, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.AddedAt, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending);

        Assert.That(
            responseAscending.Releases.Select(r => r.AddedAt),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.AddedAt),
            Is.Ordered.Descending);

        // Year
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending);

        Assert.That(
            responseAscending.Releases.Select(r => r.Release.Year),
            Is.Ordered.Ascending);
        Assert.That(
            responseDescending.Releases.Select(r => r.Release.Year),
            Is.Ordered.Descending);
    }


    [Test]
    public void AddReleaseToCollectionFolder_Username_Guard()
    {
        var folderId = 999;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.AddReleaseToCollectionFolder(null!, folderId, releaseId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.AddReleaseToCollectionFolder("", folderId, releaseId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.AddReleaseToCollectionFolder("  ", folderId, releaseId), "username");
    }

    [Test]
    public void AddReleaseToCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId));
    }

    [Test]
    public void AddReleaseToCollectionFolder_FolderId_Guard()
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.AddReleaseToCollectionFolder(username, -1, releaseId));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.AddReleaseToCollectionFolder(username, 0, releaseId));
    }

    [Test]
    public void AddReleaseToCollectionFolder_NotExistingFolderId()
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId));
    }

    [Test]
    public void AddReleaseToCollectionFolder_ReleaseId_Guard()
    {
        var username = "DamIDhagor";
        var folderId = 999;

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, -1));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, 0));
    }

    [Test]
    public void AddReleaseToCollectionFolder_NotExistingReleaseId()
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId));
    }

    [Test]
    public void AddReleaseToCollectionFolder_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => clients.discogsApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public void DeleteReleaseFromCollectionFolder_Username_Guard()
    {
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.DeleteReleaseFromCollectionFolder(null!, folderId, releaseId, instanceId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteReleaseFromCollectionFolder("", folderId, releaseId, instanceId), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteReleaseFromCollectionFolder("  ", folderId, releaseId, instanceId), "username");
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_FolderId_Guard()
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, -1, releaseId, instanceId));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, 0, releaseId, instanceId));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingFolderId()
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_ReleaseId_Guard()
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var instanceId = 999;

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, -1, instanceId));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, 0, instanceId));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingReleaseId()
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = int.MaxValue;
        var instanceId = 999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InstanceId_Guard()
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, -1));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, 0));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingInstanceId()
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => clients.discogsApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public async Task CreateDeleteReleaseFromCollectionFolder_Success()
    {
        var username = "DamIDhagor";
        var folderName = "CreateDeleteReleaseFromCollectionFolder_Success";
        var releaseId = 5134861;

        // Create test folder
        var collectionFolder = await ApiClient.CreateCollectionFolder(username, folderName);

        // Add release to folder
        var collectionFolderRelease = await ApiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId);

        // Get release from folder
        var collectionFolderReleaseResponse = await ApiClient.GetCollectionFolderReleases(username, collectionFolder.Id);

        // Delete release from folder
        Assert.DoesNotThrowAsync(() => ApiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, collectionFolderRelease.Id, collectionFolderRelease.InstanceId));

        // Delete folder
        Assert.DoesNotThrowAsync(() => ApiClient.DeleteCollectionFolder(username, collectionFolder.Id));

        Assert.IsNotNull(collectionFolderRelease);
        Assert.AreEqual(releaseId, collectionFolderRelease.Id);
        Assert.IsNotNull(collectionFolderReleaseResponse);
        Assert.AreEqual(1, collectionFolderReleaseResponse.Pagination.TotalItems);
        Assert.AreEqual(releaseId, collectionFolderReleaseResponse.Releases[0].Id);
    }
}
