namespace DiscogsApiClient.Tests.User;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class UserTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetUser_ShouldReturnUserWithPrivateFields_WhenAuthenticatedAndRequestingOwnProfile(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var user = await _apiClient.GetUser(username, cancellationToken);

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
    public async Task GetUser_ShouldReturnUserWithoutPrivateFields_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var user = await _unauthenticatedApiClient.GetUser(username, cancellationToken);

        await Assert.That(user).IsNotNull();
        await Assert.That(user.Id).IsEqualTo(12579295);
        await Assert.That(user.Username).IsEqualTo("DamIDhagor");
        await Assert.That(user.Email).IsNull();
        await Assert.That(user.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
        await Assert.That(user.IsActivated).IsTrue();
        await Assert.That(user.AvatarUrl).IsNotNullOrWhiteSpace();
        await Assert.That(user.CollectionFoldersUrl).IsNotNullOrWhiteSpace();
    }

    [Test]
    public async Task GetUser_ShouldThrowArgumentException_WhenUsernameIsEmpty(CancellationToken cancellationToken)
    {
        var username = "";

        await Assert.That(async () => await _apiClient.GetUser(username, cancellationToken)).Throws<ArgumentException>();
    }

    [Test]
    public async Task GetUser_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetUser(username, cancellationToken)).Throws<ResourceNotFoundDiscogsException>();
    }
}
