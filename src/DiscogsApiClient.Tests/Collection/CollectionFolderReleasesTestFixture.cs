using static DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;

namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionFolderReleasesTestFixture : ApiBaseTestFixture
{
    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetCollectionFolderReleases_Username_Guard(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 999;

        var exception = await Assert.That(async () => await ApiClient.GetCollectionFolderReleases(username!, folderId, null, null, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task GetCollectionFolderReleases_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;

        await Assert.That(async () => await ApiClient.GetCollectionFolderReleases(username, folderId, null, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task GetCollectionFolderReleases_FolderId_Guard(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        await Assert.That(async () => await ApiClient.GetCollectionFolderReleases(username, -1, null, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task GetCollectionFolderReleases_NotExistingFolderId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        await Assert.That(async () => await ApiClient.GetCollectionFolderReleases(username, folderId, null, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task GetCollectionFolderReleases_Unauthenticated(CancellationToken cancellationToken)
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";
        var folderId = 1;

        await Assert.That(async () => await clients.discogsApiClient.GetCollectionFolderReleases(username, folderId, null, null, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }

    [Test]
    public async Task GetCollectionFolderReleases_Sorted(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 0;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        // Artist
        var sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Artist, SortOrder = SortOrder.Ascending };
        var responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        var sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Artist, SortOrder = SortOrder.Descending };
        var responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Artists.First().Name)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Artists.First().Name)).IsInDescendingOrder();

        // Label
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Labels.First().Name)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Labels.First().Name)).IsInDescendingOrder();

        // Title
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Title)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Title)).IsInDescendingOrder();

        // CatalogNumber
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Labels.First().CatalogNumber)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Labels.First().CatalogNumber)).IsInDescendingOrder();

        // Format
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Formats.First().Name)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Formats.First().Name)).IsInDescendingOrder();

        // Rating
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Rating, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Rating, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Rating)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Rating)).IsInDescendingOrder();

        // AddedAt
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.AddedAt, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.AddedAt, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.AddedAt)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.AddedAt)).IsInDescendingOrder();

        // Year
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Ascending };
        responseAscending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Descending };
        responseDescending = await ApiClient.GetCollectionFolderReleases(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Year)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Year)).IsInDescendingOrder();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task AddReleaseToCollectionFolder_Username_Guard(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 999;
        var releaseId = 5134861;

        var exception = await Assert.That(async () => await ApiClient.AddReleaseToCollectionFolder(username!, folderId, releaseId, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task AddReleaseToCollectionFolder_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var releaseId = 5134861;

        await Assert.That(async () => await ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task AddReleaseToCollectionFolder_FolderId_Guard(int folderId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;

        await Assert.That(async () => await ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task AddReleaseToCollectionFolder_NotExistingFolderId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        await Assert.That(async () => await ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task AddReleaseToCollectionFolder_ReleaseId_Guard(int releaseId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        await Assert.That(async () => await ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task AddReleaseToCollectionFolder_NotExistingReleaseId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = int.MaxValue;

        await Assert.That(async () => await ApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task AddReleaseToCollectionFolder_Unauthenticated(CancellationToken cancellationToken)
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        await Assert.That(async () => await clients.discogsApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task DeleteReleaseFromCollectionFolder_Username_Guard(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        var exception = await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username!, folderId, releaseId, instanceId, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task DeleteReleaseFromCollectionFolder_FolderId_Guard(int folderId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;
        var instanceId = -1;

        await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_NotExistingFolderId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task DeleteReleaseFromCollectionFolder_ReleaseId_Guard(int releaseId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var instanceId = 999;

        await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_NotExistingReleaseId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = int.MaxValue;
        var instanceId = 999;

        await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task DeleteReleaseFromCollectionFolder_InstanceId_Guard(int instanceId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_NotExistingInstanceId(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = int.MaxValue;

        await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_Unauthenticated(CancellationToken cancellationToken)
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        await Assert.That(async () => await clients.discogsApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public async Task CreateDeleteReleaseFromCollectionFolder_Success(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "CreateDeleteReleaseFromCollectionFolder_Success";
        var releaseId = 5134861;

        // Create test folder
        var collectionFolder = await ApiClient.CreateCollectionFolder(username, folderName, cancellationToken);

        // Add release to folder
        var collectionFolderRelease = await ApiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId, cancellationToken);

        // Get release from folder
        var collectionFolderReleaseResponse = await ApiClient.GetCollectionFolderReleases(username, collectionFolder.Id, null, null, cancellationToken);

        // Delete release from folder
        await Assert.That(async () => await ApiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, collectionFolderRelease.Id, collectionFolderRelease.InstanceId, cancellationToken)).ThrowsNothing();

        // Delete folder
        await Assert.That(async () => await ApiClient.DeleteCollectionFolder(username, collectionFolder.Id, cancellationToken)).ThrowsNothing();

        await Assert.That(collectionFolderRelease).IsNotNull();
        await Assert.That(collectionFolderRelease.Id).IsEqualTo(releaseId);
        await Assert.That(collectionFolderReleaseResponse).IsNotNull();
        await Assert.That(collectionFolderReleaseResponse.Pagination.TotalItems).IsEqualTo(1);
        await Assert.That(collectionFolderReleaseResponse.Releases[0].Id).IsEqualTo(releaseId);
    }
}
