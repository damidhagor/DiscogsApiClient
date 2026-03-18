namespace DiscogsApiClient.Tests.User;

public sealed class IdentityTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetIdentity_Success(CancellationToken cancellationToken)
    {
        var identity = await ApiClient.GetIdentity(cancellationToken);

        await Assert.That(identity).IsNotNull();
        await Assert.That(identity.Username).IsEqualTo("DamIDhagor");
        await Assert.That(identity.ConsumerName).IsEqualTo("DamIDhagor");
        await Assert.That(identity.Id).IsEqualTo(12579295);
        await Assert.That(identity.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
    }

    [Test]
    public async Task GetIdentity_Unauthenticated(CancellationToken cancellationToken)
    {
        var unauthenticatedClients = CreateUnauthenticatedDiscogsApiClient();

        await Assert.That(async () => await unauthenticatedClients.discogsApiClient.GetIdentity(cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();

        unauthenticatedClients.authHttpClient.Dispose();
        unauthenticatedClients.clientHttpClient.Dispose();
    }
}
