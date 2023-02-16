using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DiscogsApiClient.Exceptions;
using NUnit.Framework;

namespace DiscogsApiClient.Tests;

[TestFixture]
public sealed class RateLimitingTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task ClientIsRateLimited_Success()
    {
        int succeeded = 0;
        int failed = 0;

        for (int i = 0; i < 100; i++)
        {
            try
            {
                _ = await ApiClient.GetIdentityAsync(default);
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
