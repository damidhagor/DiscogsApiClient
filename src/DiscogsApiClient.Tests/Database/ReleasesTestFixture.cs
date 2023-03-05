﻿using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.Database;

public sealed class ReleasesTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetMasterRelease_Success()
    {
        var masterReleaseId = 156551;

        var masterRelease = await ApiClient.GetMasterRelease(masterReleaseId, default);

        Assert.IsNotNull(masterRelease);
        Assert.AreEqual(masterReleaseId, masterRelease.Id);
        Assert.AreEqual(1, masterRelease.Artists.Count);
        Assert.AreEqual("HammerFall", masterRelease.Artists[0].Name);
        Assert.Less(0, masterRelease.Genres.Count);
        Assert.Less(0, masterRelease.Images.Count);
        Assert.Less(0, masterRelease.LowestPrice);
        Assert.Less(0, masterRelease.MainReleaseId);
        Assert.IsFalse(string.IsNullOrWhiteSpace(masterRelease.MainReleaseUrl));
        Assert.Less(0, masterRelease.MostRecentReleaseId);
        Assert.IsFalse(string.IsNullOrWhiteSpace(masterRelease.MostRecentReleaseUrl));
        Assert.Less(0, masterRelease.NumForSale);
        Assert.IsFalse(string.IsNullOrWhiteSpace(masterRelease.ResourceUrl));
        Assert.AreEqual("Glory To The Brave", masterRelease.Title);
        Assert.AreEqual(9, masterRelease.Tracklist.Count);
        Assert.AreEqual("1", masterRelease.Tracklist[0].Position);
        Assert.AreEqual("The Dragon Lies Bleeding", masterRelease.Tracklist[0].Title);
        Assert.AreEqual("4:23", masterRelease.Tracklist[0].Duration);
        Assert.AreEqual("track", masterRelease.Tracklist[0].Type);
        Assert.IsFalse(string.IsNullOrWhiteSpace(masterRelease.Uri));
        Assert.IsFalse(string.IsNullOrWhiteSpace(masterRelease.VersionsUrl));
        Assert.Less(0, masterRelease.Videos.Count);
        Assert.AreEqual(1997, masterRelease.Year);
        Assert.Less(0, masterRelease.Styles.Count);
    }

    [Test]
    public void GetMasterRelease_NotExistingReleaseId()
    {
        var masterReleaseId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetRelease(masterReleaseId, default));
    }

    [Test]
    public async Task GetMasterReleaseVersions_Success()
    {
        var masterReleaseId = 156551;
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };
        var filterParams = new MasterReleaseVersionFilterQueryParameters();

        var response = await ApiClient.GetMasterReleaseVersions(masterReleaseId, paginationParams, filterParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.ItemsPerPage);
        Assert.Less(0, response.Pagination.TotalItems);
        Assert.Less(0, response.Pagination.TotalPages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.NextPageUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Pagination.Urls.LastPageUrl));

        Assert.IsNotNull(response.ReleaseVersions);
    }


    [Test]
    public async Task GetRelease_Success()
    {
        var releaseId = 5134861;

        var release = await ApiClient.GetRelease(releaseId, default);

        Assert.IsNotNull(release);
        Assert.AreEqual(releaseId, release.Id);
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.ResourceUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Uri));
        Assert.AreEqual(1, release.Artists.Count);
        Assert.AreEqual("HammerFall", release.Artists[0].Name);
        Assert.AreEqual("HammerFall", release.ArtistsSort);
        Assert.AreEqual(1, release.Labels.Count);
        Assert.AreEqual(11499, release.Labels[0].Id);
        Assert.AreEqual("Nuclear Blast", release.Labels[0].Name);
        Assert.AreEqual("NB 265-2", release.Labels[0].CatalogNumber);
        Assert.AreEqual(1, release.Formats.Count);
        Assert.AreEqual("CD", release.Formats[0].Name);
        Assert.AreEqual("1", release.Formats[0].Count);
        Assert.Less(0, release.Formats[0].Descriptions.Count);
        Assert.AreEqual(release.FormatCount, release.Formats.Count);
        Assert.Less(0, release.CommunityStatistics.UsersOwningReleaseCount);
        Assert.Less(0, release.CommunityStatistics.UsersWantingReleaseCount);
        Assert.Less(0, release.CommunityStatistics.Rating.Count);
        Assert.Less(0, release.CommunityStatistics.Rating.Average);
        Assert.AreNotEqual(DateTime.MinValue, release.AddedAt);
        Assert.AreNotEqual(DateTime.MinValue, release.ChangedAt);
        Assert.Less(0, release.NumForSale);
        Assert.Less(0, release.LowestPrice);
        Assert.Less(0, release.MasterId);
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.MasterUrl));
        Assert.AreEqual("Glory To The Brave", release.Title);
        Assert.AreEqual("Germany", release.Country);
        Assert.AreEqual("1997", release.Year);
        Assert.AreEqual("1997", release.YearFormatted);
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.Notes));
        Assert.Less(0, release.Identifiers.Count);
        Assert.AreEqual("Barcode", release.Identifiers[0].Type);
        Assert.AreEqual("7 27361 62652 5", release.Identifiers[0].Value);
        Assert.Less(0, release.Videos.Count);
        Assert.Less(0, release.Genres.Count);
        Assert.Less(0, release.Styles.Count);
        Assert.AreEqual(9, release.Tracklist.Count);
        Assert.AreEqual("1", release.Tracklist[0].Position);
        Assert.AreEqual("The Dragon Lies Bleeding", release.Tracklist[0].Title);
        Assert.AreEqual("4:23", release.Tracklist[0].Duration);
        Assert.AreEqual("track", release.Tracklist[0].Type);
        Assert.AreEqual(0, release.ExtraArtists.Count);
        Assert.Less(0, release.Images.Count);
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.ThumbnailUrl));
        Assert.Less(0, release.EstimatedWeight);
        Assert.IsFalse(release.IsBlockedFromSale);
        Assert.IsFalse(string.IsNullOrWhiteSpace(release.DataQuality));
    }

    [Test]
    public void GetRelease_NotExistingReleaseId()
    {
        var releaseId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetRelease(releaseId, default));
    }


    [Test]
    public async Task GetReleaseCommunityRating_Success()
    {
        var releaseId = 5134861;

        var ratingResponse = await ApiClient.GetReleaseCommunityRating(releaseId, default);

        Assert.IsNotNull(ratingResponse);
        Assert.AreEqual(ratingResponse.ReleaseId, releaseId);
        Assert.IsNotNull(ratingResponse.Rating);
        Assert.Less(0, ratingResponse.Rating.Count);
        Assert.Less(0, ratingResponse.Rating.Average);
    }

    [Test]
    public void GetReleaseCommunityRating_NotExistingReleaseId()
    {
        var releaseId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetReleaseCommunityRating(releaseId, default));
    }


    [Test]
    public async Task GetReleaseStats_Success()
    {
        var releaseId = 5134861;

        var stats = await ApiClient.GetReleaseStats(releaseId, default);

        Assert.IsNotNull(stats);
    }

    [Test]
    public void GetReleaseStats_NotExistingReleaseId()
    {
        var releaseId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetReleaseStats(releaseId, default));
    }
}
