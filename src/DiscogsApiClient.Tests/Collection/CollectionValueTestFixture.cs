namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionValueTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetCollectionValue_Success(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var collectionValue = await ApiClient.GetCollectionValue(username, cancellationToken);

        await Assert.That(collectionValue).IsNotNull();
        await Assert.That(collectionValue.Minimum).IsNotNullOrWhiteSpace();
        await Assert.That(collectionValue.Median).IsNotNullOrWhiteSpace();
        await Assert.That(collectionValue.Maximum).IsNotNullOrWhiteSpace();
    }

    [Test]
    [Arguments(null!, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("   ", typeof(ArgumentException))]
    public async Task GetCollectionValue_Username_Guard(string? username, Type exceptionType, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await ApiClient.GetCollectionValue(username!, cancellationToken))
            .Throws<Exception>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(exceptionType);
    }

    [Test]
    public async Task GetCollectionValue_InvalidUsername(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await ApiClient.GetCollectionValue(username, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>();
    }
}
