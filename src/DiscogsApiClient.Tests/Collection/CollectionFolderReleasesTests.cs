using static DiscogsApiClient.QueryParameters.CollectionFolderReleaseSortQueryParameters;

namespace DiscogsApiClient.Tests.Collection;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class CollectionFolderReleasesTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetCollectionFolderReleases_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 999;

        var exception = await Assert.That(async () => await _apiClient.GetCollectionItemsByFolder(username!, folderId, null, null, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task GetCollectionFolderReleases_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;

        await Assert.That(async () => await _apiClient.GetCollectionItemsByFolder(username, folderId, null, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    public async Task GetCollectionFolderReleases_ShouldThrowArgumentOutOfRangeException_WhenFolderIdIsInvalid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var exception = await Assert.That(async () => await _apiClient.GetCollectionItemsByFolder(username, -1, null, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("folderId");
    }

    [Test]
    public async Task GetCollectionFolderReleases_ShouldThrowResourceNotFoundException_WhenFolderDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        await Assert.That(async () => await _apiClient.GetCollectionItemsByFolder(username, folderId, null, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested folder does not exist.");
    }

    [Test]
    public async Task GetCollectionFolderReleases_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 1;

        await Assert.That(async () => await _unauthenticatedApiClient.GetCollectionItemsByFolder(username, folderId, null, null, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task GetCollectionFolderReleases_ShouldReturnSortedReleases_WhenSortParametersAreValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 0;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        // Artist
        var sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Artist, SortOrder = SortOrder.Ascending };
        var responseAscending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        var sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Artist, SortOrder = SortOrder.Descending };
        var responseDescending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Artists[0].Name)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Artists[0].Name)).IsInDescendingOrder();

        // Label
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Label, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Labels[0].Name)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Labels[0].Name)).IsInDescendingOrder();

        // Title
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Title, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Title)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Title)).IsInDescendingOrder();

        // CatalogNumber
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.CatalogNumber, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Labels[0].CatalogNumber)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Labels[0].CatalogNumber)).IsInDescendingOrder();

        // Format
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Format, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Formats[0].Name)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Formats[0].Name)).IsInDescendingOrder();

        // Rating
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Rating, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Rating, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Rating)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Rating)).IsInDescendingOrder();

        // AddedAt
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.AddedAt, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.AddedAt, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.AddedAt)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.AddedAt)).IsInDescendingOrder();

        // Year
        sortParametersAscending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Ascending };
        responseAscending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersAscending, cancellationToken);
        sortParametersDescending = new CollectionFolderReleaseSortQueryParameters { SortProperty = SortableProperty.Year, SortOrder = SortOrder.Descending };
        responseDescending = await _apiClient.GetCollectionItemsByFolder(username, folderId, paginationParams, sortParametersDescending, cancellationToken);

        await Assert.That(responseAscending.Releases.Select(r => r.Release.Year)).IsInOrder();
        await Assert.That(responseDescending.Releases.Select(r => r.Release.Year)).IsInDescendingOrder();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task AddReleaseToCollectionFolder_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 999;
        var releaseId = 5134861;

        var exception = await Assert.That(async () => await _apiClient.AddReleaseToCollectionFolder(username!, folderId, releaseId, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task AddReleaseToCollectionFolder_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var releaseId = 5134861;

        await Assert.That(async () => await _apiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task AddReleaseToCollectionFolder_ShouldThrowArgumentOutOfRangeException_WhenFolderIdIsInvalid(int folderId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;

        var exception = await Assert.That(async () => await _apiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("folderId");
    }

    [Test]
    public async Task AddReleaseToCollectionFolder_ShouldThrowResourceNotFoundException_WhenFolderDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        await Assert.That(async () => await _apiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested folder does not exist.");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task AddReleaseToCollectionFolder_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;

        var exception = await Assert.That(async () => await _apiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    public async Task AddReleaseToCollectionFolder_ShouldThrowResourceNotFoundException_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = int.MaxValue;

        await Assert.That(async () => await _apiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested folder does not exist.");
    }

    [Test]
    public async Task AddReleaseToCollectionFolder_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        await Assert.That(async () => await _unauthenticatedApiClient.AddReleaseToCollectionFolder(username, folderId, releaseId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        var exception = await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username!, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowArgumentOutOfRangeException_WhenFolderIdIsInvalid(int folderId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;
        var instanceId = -1;

        var exception = await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("folderId");
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowResourceNotFoundException_WhenFolderDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested collection item does not exist in this user's collection.");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var instanceId = 999;

        var exception = await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowResourceNotFoundException_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = int.MaxValue;
        var instanceId = 999;

        await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested collection item does not exist in this user's collection.");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowArgumentOutOfRangeException_WhenInstanceIdIsInvalid(long instanceId, CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;

        var exception = await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("instanceId");
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowResourceNotFoundException_WhenInstanceIdDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = long.MaxValue;

        await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested collection item does not exist in this user's collection.");
    }

    [Test]
    public async Task DeleteReleaseFromCollectionFolder_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderId = 999;
        var releaseId = 5134861;
        var instanceId = 999;

        await Assert.That(async () => await _unauthenticatedApiClient.DeleteReleaseFromCollectionFolder(username, folderId, releaseId, instanceId, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }


    [Test]
    public async Task AddAndRemoveReleaseFromCollectionFolder_ShouldSucceed_WhenParametersAreValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "CreateDeleteReleaseFromCollectionFolder_Success";
        var releaseId = 5134861;

        // Create test folder
        var collectionFolder = await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken);

        // Add release to folder
        var collectionFolderRelease = await _apiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId, cancellationToken);

        // Get release from folder
        var collectionFolderReleaseResponse = await _apiClient.GetCollectionItemsByFolder(username, collectionFolder.Id, null, null, cancellationToken);

        // Delete release from folder
        await Assert.That(async () => await _apiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, collectionFolderRelease.Id, collectionFolderRelease.InstanceId, cancellationToken)).ThrowsNothing();

        // Delete folder
        await Assert.That(async () => await _apiClient.DeleteCollectionFolder(username, collectionFolder.Id, cancellationToken)).ThrowsNothing();

        await Assert.That(collectionFolderRelease).IsNotNull();
        await Assert.That(collectionFolderRelease.Id).IsEqualTo(releaseId);
        await Assert.That(collectionFolderRelease.ResourceUrl).IsNotNullOrWhiteSpace();
        await Assert.That(collectionFolderReleaseResponse).IsNotNull();
        await Assert.That(collectionFolderReleaseResponse.Pagination.TotalItems).IsEqualTo(1);
        await Assert.That(collectionFolderReleaseResponse.Releases[0].Id).IsEqualTo(releaseId);
        await Assert.That(collectionFolderReleaseResponse.Releases[0].ResourceUrl).IsNull();
    }
}
