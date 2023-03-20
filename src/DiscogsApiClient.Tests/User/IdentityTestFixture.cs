namespace DiscogsApiClient.Tests.User;

[TestFixture]
public sealed class IdentityTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetIdentity_Success()
    {
        var identity = await ApiClient.GetIdentity(default);

        Assert.IsNotNull(identity);
        Assert.AreEqual("DamIDhagor", identity.Username);
        Assert.AreEqual("DamIDhagor", identity.ConsumerName);
        Assert.AreEqual(12579295, identity.Id);
        Assert.AreEqual("https://api.discogs.com/users/DamIDhagor", identity.ResourceUrl);
    }

    [Test]
    public void GetIdentity_Unauthenticated()
    {
        var unauthenticatedClients = CreateUnauthenticatedDiscogsApiClient();

        Assert.ThrowsAsync<UnauthenticatedDiscogsException>(() => unauthenticatedClients.discogsApiClient.GetIdentity(default));

        unauthenticatedClients.authHttpClient.Dispose();
        unauthenticatedClients.clientHttpClient.Dispose();
    }
}
