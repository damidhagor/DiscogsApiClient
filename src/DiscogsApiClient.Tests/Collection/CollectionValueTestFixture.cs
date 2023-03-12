namespace DiscogsApiClient.Tests.Collection;

public sealed class CollectionValueTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetCollectionValue_Success()
    {
        var username = "damidhagor";

        var collectionValue = await ApiClient.GetCollectionValue(username, default);

        Assert.IsNotNull(collectionValue);
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Minimum));
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Median));
        Assert.IsFalse(string.IsNullOrWhiteSpace(collectionValue.Maximum));
    }

    [Test]
    public void GetCollectionValue_Username_Guard()
    {
        Assert.ThrowsAsync<ArgumentNullException>(() => ApiClient.GetCollectionValue(null!, default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionValue("", default), "username");
        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionValue("   ", default), "username");
    }

    [Test]
    public void GetCollectionValue_InvalidUsername()
    {
        var username = "awrbaerhnqw54";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionValue(username, default));
    }
}
