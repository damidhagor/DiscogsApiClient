﻿namespace DiscogsApiClient.Tests.User;

public sealed class UserTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetUser_Success()
    {
        var username = "DamIDhagor";

        var user = await ApiClient.GetUser(username);

        Assert.IsNotNull(user);
        Assert.AreEqual(12579295, user.Id);
        Assert.AreEqual("DamIDhagor", user.Username);
        Assert.AreEqual("alexander.jurk@outlook.com", user.Email);
        Assert.AreEqual("https://api.discogs.com/users/DamIDhagor", user.ResourceUrl);
        Assert.IsTrue(user.IsActivated);
        Assert.IsFalse(string.IsNullOrWhiteSpace(user.AvatarUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(user.CollectionFoldersUrl));
    }

    [Test]
    public async Task GetUser_Unauthenticated()
    {
        var username = "DamIDhagor";
        var unauthenticatedClients = CreateUnauthenticatedDiscogsApiClient();

        var user = await unauthenticatedClients.discogsApiClient.GetUser(username);

        Assert.IsNotNull(user);
        Assert.AreEqual(12579295, user.Id);
        Assert.AreEqual("DamIDhagor", user.Username);
        Assert.AreEqual(null, user.Email);
        Assert.AreEqual("https://api.discogs.com/users/DamIDhagor", user.ResourceUrl);
        Assert.IsTrue(user.IsActivated);
        Assert.IsFalse(string.IsNullOrWhiteSpace(user.AvatarUrl));
        Assert.IsFalse(string.IsNullOrWhiteSpace(user.CollectionFoldersUrl));

        unauthenticatedClients.authHttpClient.Dispose();
        unauthenticatedClients.clientHttpClient.Dispose();
    }

    [Test]
    public void GetUser_EmptyUsername()
    {
        var username = "";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetUser(username), "Parameter \"username\" (string) must not be null or whitespace, was whitespace. (Parameter 'username')");
    }

    [Test]
    public void GetUser_InvalidUsername()
    {
        var username = "awrbaerhnqw54";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetUser(username));
    }
}
