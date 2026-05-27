namespace DiscogsApiClient.Tests.Collection;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class CollectionValueTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();

    [Test]
    public async Task GetCollectionValue_ShouldReturnCollectionValue_WhenUsernameIsValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var collectionValue = await _apiClient.GetCollectionValue(username, cancellationToken);

        await Assert.That(collectionValue).IsNotNull();
        await Assert.That(collectionValue.Minimum).IsNotNullOrWhiteSpace();
        await Assert.That(collectionValue.Median).IsNotNullOrWhiteSpace();
        await Assert.That(collectionValue.Maximum).IsNotNullOrWhiteSpace();
    }

    [Test]
    [Arguments(null!, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("   ", typeof(ArgumentException))]
    public async Task GetCollectionValue_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type exceptionType, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetCollectionValue(username!, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(exceptionType);
    }

    [Test]
    public async Task GetCollectionValue_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetCollectionValue(username, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }
}
