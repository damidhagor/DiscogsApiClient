using System.Threading.Tasks;
using NUnit.Framework;

namespace DiscogsApiClient.Tests.User;

[TestFixture]
public class IdentityTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetIdentity_Success()
    {
        var identity = await ApiClient.GetIdentityAsync(default);

        Assert.IsNotNull(identity);
        Assert.AreEqual("DamIDhagor", identity.Username);
        Assert.AreEqual("DamIDhagor", identity.ConsumerName);
        Assert.AreEqual(12579295, identity.Id);
        Assert.AreEqual("https://api.discogs.com/users/DamIDhagor", identity.ResourceUrl);
    }
}
