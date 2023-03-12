namespace DiscogsApiClient.Tests.Collection;

public sealed class WantlistTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetAllWantlistReleases_Success()
    {
        var username = "damidhagor";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var summedUpItemCount = 0;

        var response = await ApiClient.GetWantlistReleases(username, paginationParams, default);
        var itemCount = response.Pagination.TotalItems;
        summedUpItemCount += response.Releases.Count;

        for (var p = 2; p <= response.Pagination.TotalPages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await ApiClient.GetWantlistReleases(username, paginationParams, default);
            summedUpItemCount += response.Releases.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }

    [Test]
    public void GetWantlist_Username_Guard()
    {
        var paginationParams = new PaginationQueryParameters();

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.GetWantlistReleases(null!, paginationParams, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetWantlistReleases("", paginationParams, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetWantlistReleases("  ", paginationParams, default), "username");
    }

    [Test]
    public void GetWantlist_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var paginationParams = new PaginationQueryParameters();

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetWantlistReleases(username, paginationParams, default));
    }


    [Test]
    public void AddWantlistRelease_Username_Guard()
    {
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.AddReleaseToWantlist(null!, releaseId, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.AddReleaseToWantlist("", releaseId, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.AddReleaseToWantlist("  ", releaseId, default), "username");
    }

    [Test]
    public void AddWantlistRelease_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToWantlist(username, releaseId, default));
    }

    [Test]
    public void AddWantlistRelease_ReleaseId_Guard()
    {
        var username = "damidhagor";

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.AddReleaseToWantlist(username, -1, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.AddReleaseToWantlist(username, 0, default));
    }

    [Test]
    public void AddWantlistRelease_NotExistingReleaseId()
    {
        var username = "damidhagor";
        var releaseId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToWantlist(username, releaseId, default));
    }

    [Test]
    public void AddWantlistRelease_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "damidhagor";
        var releaseId = 5134861;

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(async () => await clients.discogsApiClient.AddReleaseToWantlist(username, releaseId, default));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public void DeleteWantlistRelease_Username_Guard()
    {
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.DeleteReleaseFromWantlist(null!, releaseId, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteReleaseFromWantlist("", releaseId, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteReleaseFromWantlist("  ", releaseId, default), "username");
    }

    [Test]
    public void DeleteWantlistRelease_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromWantlist(username, releaseId, default));
    }

    [Test]
    public void DeleteWantlistRelease_ReleaseId_Guard()
    {
        var username = "damidhagor";

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteReleaseFromWantlist(username, -1, default));
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ApiClient.DeleteReleaseFromWantlist(username, 0, default));
    }

    [Test]
    public void DeleteWantlistRelease_NotExistingReleaseId()
    {
        var username = "damidhagor";
        var releaseId = int.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromWantlist(username, releaseId, default));
    }

    [Test]
    public void DeleteWantlistRelease_Unauthenticated()
    {
        var clients = CreateUnauthenticatedDiscogsApiClient();
        var username = "damidhagor";
        var releaseId = 5134861;

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(async () => await clients.discogsApiClient.DeleteReleaseFromWantlist(username, releaseId, default));

        clients.authHttpClient.Dispose();
        clients.clientHttpClient.Dispose();
    }


    [Test]
    public async Task AddDeleteWantlistRelease_Success()
    {
        var username = "damidhagor";
        var releaseId = 5134861;

        // Add
        var addedRelease = await ApiClient.AddReleaseToWantlist(username, releaseId, default);
        Assert.IsNotNull(addedRelease);
        Assert.AreEqual(releaseId, addedRelease.Id);
        Assert.IsTrue(string.IsNullOrWhiteSpace(addedRelease.Notes));
        Assert.AreEqual(0, addedRelease.Rating);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.ResourceUrl));
        Assert.IsNotNull(addedRelease.Release);
        Assert.AreEqual(releaseId, addedRelease.Release.Id);
        Assert.AreEqual(1997, addedRelease.Release.Year);
        Assert.AreEqual("Glory To The Brave", addedRelease.Release.Title);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.Release.ResourceUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.Release.ThumbnailUrl));
        Assert.AreEqual(156551, addedRelease.Release.MasterId);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.Release.MasterUrl));
        Assert.AreEqual(1, addedRelease.Release.Artists.Count);
        Assert.AreEqual(287459, addedRelease.Release.Artists[0].Id);
        Assert.AreEqual("HammerFall", addedRelease.Release.Artists[0].Name);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.Release.Artists[0].ResourceUrl));
        Assert.IsNotNull(addedRelease.Release.Formats);
        Assert.Less(0, addedRelease.Release.Formats.Count);
        Assert.Less("0", addedRelease.Release.Formats[0].Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.Release.Formats[0].Name));
        Assert.IsNotNull(addedRelease.Release.Formats[0].Descriptions);
        Assert.Less(0, addedRelease.Release.Formats[0].Descriptions.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.Release.Formats[0].Descriptions[0]));
        Assert.IsNotNull(addedRelease.Release.Genres);
        Assert.Less(0, addedRelease.Release.Genres.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.Release.Genres[0]));
        Assert.IsNotNull(addedRelease.Release.Styles);
        Assert.Less(0, addedRelease.Release.Styles.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.Release.Styles[0]));
        Assert.IsNotNull(addedRelease.Release.Labels);
        Assert.Less(0, addedRelease.Release.Labels.Count);

        // Delete
        Assert.DoesNotThrowAsync(() => ApiClient.DeleteReleaseFromWantlist(username, releaseId, default));
    }
}
