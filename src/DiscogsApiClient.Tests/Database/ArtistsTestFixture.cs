using System;
using System.Threading.Tasks;
using DiscogsApiClient.Exceptions;
using DiscogsApiClient.QueryParameters;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.Database;

public sealed class ArtistsTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetArtist_Success()
    {
        var artistId = 287459;

        var artist = await ApiClient.GetArtistAsync(artistId, default);

        Assert.IsNotNull(artist);
        Assert.AreEqual(artistId, artist.Id);
        Assert.AreEqual("HammerFall", artist.Name);
        Assert.IsFalse(String.IsNullOrWhiteSpace(artist.ResourceUrl));
        Assert.IsFalse(String.IsNullOrWhiteSpace(artist.Uri));
        Assert.IsFalse(String.IsNullOrWhiteSpace(artist.ReleasesUrl));
        Assert.IsFalse(String.IsNullOrWhiteSpace(artist.Profile));

        Assert.Less(0, artist.Urls.Count);
        Assert.Less(0, artist.Namevariations.Count);
        Assert.Less(0, artist.Members.Count);
        Assert.Less(0, artist.Images.Count);
    }

    [Test]
    public void GetArtist_NotExistingArtistId()
    {
        var artistId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtistAsync(artistId, default));
    }

    [Test]
    public async Task GetArtistReleases_Success()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters(1, 50);

        var response = await ApiClient.GetArtistReleasesAsync(artistId, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(50, response.Releases.Count);
    }

    [Test]
    public async Task GetArtistReleases_Success_InvalidSmallPageNumber()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters(-1, 50);

        var response = await ApiClient.GetArtistReleasesAsync(artistId, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(50, response.Releases.Count);
    }

    [Test]
    public async Task GetArtistReleases_Success_InvalidSmallPageSize()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters(1, -1);

        var response = await ApiClient.GetArtistReleasesAsync(artistId, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(1, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(1, response.Releases.Count);
    }

    [Test]
    public async Task GetArtistReleases_Success_InvalidBigPageSize()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters(1, int.MaxValue);

        var response = await ApiClient.GetArtistReleasesAsync(artistId, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(100, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(100, response.Releases.Count);
    }

    [Test]
    public async Task GetArtistAllReleases_Success()
    {
        var artistId = 287459;

        var paginationParams = new PaginationQueryParameters(1, 50);
        var itemCount = 0;
        var summedUpItemCount = 0;

        var response = await ApiClient.GetArtistReleasesAsync(artistId, paginationParams, default);
        itemCount = response.Pagination.Items;
        summedUpItemCount += response.Releases.Count;

        for (int p = 2; p <= response.Pagination.Pages; p++)
        {
            paginationParams.Page = p;
            response = await ApiClient.GetArtistReleasesAsync(artistId, paginationParams, default);
            summedUpItemCount += response.Releases.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }

    [Test]
    public void GetArtistReleases_NotExistingArtistId()
    {
        var artistId = -1;
        var paginationParams = new PaginationQueryParameters(1, 50);

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtistReleasesAsync(artistId, paginationParams, default));
    }

    [Test]
    public void GetArtistReleases_InvalidBigPageNumber()
    {
        var artistId = 287459;
        var paginationParams = new PaginationQueryParameters(int.MaxValue, 50);

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtistReleasesAsync(artistId, paginationParams, default));
    }
}