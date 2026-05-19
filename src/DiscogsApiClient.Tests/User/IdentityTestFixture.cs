namespace DiscogsApiClient.Tests.User;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class IdentityTestFixture(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetIdentity_Success(CancellationToken cancellationToken)
    {
        var identity = await _apiClient.GetIdentity(cancellationToken);

        await Assert.That(identity).IsNotNull();
        await Assert.That(identity.Username).IsEqualTo("DamIDhagor");
        await Assert.That(identity.ConsumerName).IsEqualTo("DamIDhagor");
        await Assert.That(identity.Id).IsEqualTo(12579295);
        await Assert.That(identity.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
    }

    [Test]
    public async Task GetIdentity_Unauthenticated(CancellationToken cancellationToken)
    {
        
        await Assert.That(async () => await _unauthenticatedApiClient.GetIdentity(cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }
}
