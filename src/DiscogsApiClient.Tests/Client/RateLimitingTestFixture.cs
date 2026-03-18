namespace DiscogsApiClient.Tests.Client;

public sealed class RateLimitingTestFixture : ApiBaseTestFixture
{
    [Test]
    [Explicit]
    public async Task ClientIsRateLimited_Success(CancellationToken cancellationToken)
    {
        var succeeded = 0;
        var failed = 0;

        for (var i = 0; i < 100; i++)
        {
            try
            {
                _ = await ApiClient.GetIdentity(cancellationToken);
                succeeded++;
                TestContext.Current!.Output.WriteLine($"[{DateTime.Now:HH:mm:ss:fff}] {i:D2} SUCCESS");
            }
            catch (Exception)
            {
                failed++;
                TestContext.Current!.Output.WriteLine($"[{DateTime.Now:HH:mm:ss:fff}] {i:D2} FAIL");
            }
        }

        await Assert.That(succeeded).IsEqualTo(100);
        await Assert.That(failed).IsEqualTo(0);
    }
}
