using System;
using System.Threading.Tasks;
using DiscogsApiClient.Exceptions;
using DiscogsApiClient.QueryParameters;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.Collection;

public sealed class WantlistTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetAllWantlistReleases_Success()
    {
        var username = "damidhagor";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var itemCount = 0;
        var summedUpItemCount = 0;

        var response = await ApiClient.GetWantlistReleasesAsync(username, paginationParams, default);
        itemCount = response.Pagination.Items;
        summedUpItemCount += response.Wants.Count;

        for (int p = 2; p <= response.Pagination.Pages; p++)
        {
            paginationParams = paginationParams with { Page = p };
            response = await ApiClient.GetWantlistReleasesAsync(username, paginationParams, default);
            summedUpItemCount += response.Wants.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }

    [Test]
    public void GetWantlist_EmptyUsername()
    {
        var username = "";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetWantlistReleasesAsync(username, paginationParams, default), "username");
    }

    [Test]
    public void GetWantlist_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetWantlistReleasesAsync(username, paginationParams, default));
    }


    [Test]
    public void AddWantlistRelease_EmptyUsername()
    {
        var username = "";
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.AddReleaseToWantlistAsync(username, releaseId, default), "username");
    }

    [Test]
    public void AddWantlistRelease_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToWantlistAsync(username, releaseId, default));
    }

    [Test]
    public void AddWantlistRelease_InvalidReleaseId()
    {
        var username = "damidhagor";
        var releaseId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToWantlistAsync(username, releaseId, default));
    }

    [Test]
    public void AddWantlistRelease_NotExistingReleaseId()
    {
        var username = "damidhagor";
        var releaseId = 99999999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToWantlistAsync(username, releaseId, default));
    }


    [Test]
    public void DeleteWantlistRelease_EmptyUsername()
    {
        var username = "";
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteReleaseFromWantlistAsync(username, releaseId, default), "username");
    }

    [Test]
    public void DeleteWantlistRelease_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromWantlistAsync(username, releaseId, default));
    }

    [Test]
    public void DeleteWantlistRelease_InvalidReleaseId()
    {
        var username = "damidhagor";
        var releaseId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromWantlistAsync(username, releaseId, default));
    }

    [Test]
    public void DeleteWantlistRelease_NotExistingReleaseId()
    {
        var username = "damidhagor";
        var releaseId = 99999999;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromWantlistAsync(username, releaseId, default));
    }


    [Test]
    public async Task AddDeleteWantlistRelease_Success()
    {
        var username = "damidhagor";
        var releaseId = 5134861;

        // Add
        var addedRelease = await ApiClient.AddReleaseToWantlistAsync(username, releaseId, default);
        Assert.IsNotNull(addedRelease);
        Assert.AreEqual(releaseId, addedRelease.Id);
        Assert.IsTrue(string.IsNullOrWhiteSpace(addedRelease.Notes));
        Assert.AreEqual(0, addedRelease.Rating);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.ResourceUrl));
        Assert.IsNotNull(addedRelease.BasicInformation);
        Assert.AreEqual(releaseId, addedRelease.BasicInformation.Id);
        Assert.AreEqual(1997, addedRelease.BasicInformation.Year);
        Assert.AreEqual("Glory To The Brave", addedRelease.BasicInformation.Title);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.BasicInformation.ResourceUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.BasicInformation.Thumb));
        Assert.AreEqual(156551, addedRelease.BasicInformation.MasterId);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.BasicInformation.MasterUrl));
        Assert.AreEqual(1, addedRelease.BasicInformation.Artists.Count);
        Assert.AreEqual(287459, addedRelease.BasicInformation.Artists[0].Id);
        Assert.AreEqual("HammerFall", addedRelease.BasicInformation.Artists[0].Name);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.BasicInformation.Artists[0].ResourceUrl));
        Assert.IsNotNull(addedRelease.BasicInformation.Formats);
        Assert.Less(0, addedRelease.BasicInformation.Formats.Count);
        Assert.Less("0", addedRelease.BasicInformation.Formats[0].Qty);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.BasicInformation.Formats[0].Name));
        Assert.IsNotNull(addedRelease.BasicInformation.Formats[0].Descriptions);
        Assert.Less(0, addedRelease.BasicInformation.Formats[0].Descriptions.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.BasicInformation.Formats[0].Descriptions[0]));
        Assert.IsNotNull(addedRelease.BasicInformation.Genres);
        Assert.Less(0, addedRelease.BasicInformation.Genres.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.BasicInformation.Genres[0]));
        Assert.IsNotNull(addedRelease.BasicInformation.Styles);
        Assert.Less(0, addedRelease.BasicInformation.Styles.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(addedRelease.BasicInformation.Styles[0]));
        Assert.IsNotNull(addedRelease.BasicInformation.Labels);
        Assert.Less(0, addedRelease.BasicInformation.Labels.Count);

        // Delete
        var result = await ApiClient.DeleteReleaseFromWantlistAsync(username, releaseId, default);
        Assert.IsTrue(result);
    }
}