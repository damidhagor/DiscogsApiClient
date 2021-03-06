using System;
using System.Threading.Tasks;
using DiscogsApiClient.Exceptions;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.User;

public class UserTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetUser_Success()
    {
        var username = "damidhagor";

        var user = await ApiClient.GetUserAsync(username, default);

        Assert.IsNotNull(user);
        Assert.AreEqual(12579295, user.Id);
        Assert.AreEqual("DamIDhagor", user.Username);
        Assert.AreEqual("alexander.jurk@outlook.com", user.Email);
        Assert.AreEqual("https://api.discogs.com/users/DamIDhagor", user.ResourceUrl);
        Assert.IsTrue(user.Activated);
        Assert.IsFalse(String.IsNullOrWhiteSpace(user.AvatarUrl));
        Assert.IsFalse(String.IsNullOrWhiteSpace(user.CollectionFoldersUrl));
    }

    [Test]
    public void GetUser_EmptyUsername()
    {
        var username = "";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetUserAsync(username, default), "username");
    }

    [Test]
    public void GetUser_InvalidUsername()
    {
        var username = "awrbaerhnqw54";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetUserAsync(username, default));
    }
}