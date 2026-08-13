namespace DiscogsApiClient.Tests.Client;

[TestFixture]
public sealed class RateLimitingTestFixture : ApiBaseTestFixture
{
    [Test]
    [Explicit]
    public async Task ClientIsRateLimited_Success()
    {
        var succeeded = 0;
        var failed = 0;

        for (var i = 0; i < 100; i++)
        {
            try
            {
                _ = await ApiClient.GetIdentity(default);
                succeeded++;
                TestContext.WriteLine($"[{DateTime.Now:HH:mm:ss:fff}] {i:D2} SUCCESS");
            }
            catch (Exception)
            {
                failed++;
                TestContext.WriteLine($"[{DateTime.Now:HH:mm:ss:fff}] {i:D2} FAIL");
            }
        }

        Assert.AreEqual(100, succeeded);
        Assert.AreEqual(0, failed);
    }
}
