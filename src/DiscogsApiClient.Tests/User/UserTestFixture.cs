namespace DiscogsApiClient.Tests.User;

public sealed class UserTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetUser_Success(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var user = await ApiClient.GetUser(username, cancellationToken);

        await Assert.That(user).IsNotNull();
        await Assert.That(user.Id).IsEqualTo(12579295);
        await Assert.That(user.Username).IsEqualTo("DamIDhagor");
        await Assert.That(user.Email).IsEqualTo("alexander.jurk@outlook.com");
        await Assert.That(user.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
        await Assert.That(user.IsActivated).IsTrue();
        await Assert.That(user.AvatarUrl).IsNotNullOrWhiteSpace();
        await Assert.That(user.CollectionFoldersUrl).IsNotNullOrWhiteSpace();
    }

    [Test]
    public async Task GetUser_Unauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var unauthenticatedClients = CreateUnauthenticatedDiscogsApiClient();

        var user = await unauthenticatedClients.discogsApiClient.GetUser(username, cancellationToken);

        await Assert.That(user).IsNotNull();
        await Assert.That(user.Id).IsEqualTo(12579295);
        await Assert.That(user.Username).IsEqualTo("DamIDhagor");
        await Assert.That(user.Email).IsNull();
        await Assert.That(user.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
        await Assert.That(user.IsActivated).IsTrue();
        await Assert.That(user.AvatarUrl).IsNotNullOrWhiteSpace();
        await Assert.That(user.CollectionFoldersUrl).IsNotNullOrWhiteSpace();

        unauthenticatedClients.authHttpClient.Dispose();
        unauthenticatedClients.clientHttpClient.Dispose();
    }

    [Test]
    public async Task GetUser_EmptyUsername(CancellationToken cancellationToken)
    {
        var username = "";

        await Assert.That(async () => await ApiClient.GetUser(username, cancellationToken)).Throws<ArgumentException>();
    }

    [Test]
    public async Task GetUser_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await ApiClient.GetUser(username, cancellationToken)).Throws<ResourceNotFoundDiscogsException>();
    }
}
