namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionValueTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetCollectionValue_Success()
    {
        var username = "DamIDhagor";

        var collectionValue = await ApiClient.GetCollectionValue(username);

        Assert.IsNotNull(collectionValue);
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Minimum));
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Median));
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Maximum));
    }

    [Test]
    public void GetCollectionValue_Username_Guard()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.GetCollectionValue(null!), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionValue(""), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionValue("   "), "username");
    }

    [Test]
    public void GetCollectionValue_InvalidUsername()
    {
        var username = "awrbaerhnqw54";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionValue(username));
    }
}
