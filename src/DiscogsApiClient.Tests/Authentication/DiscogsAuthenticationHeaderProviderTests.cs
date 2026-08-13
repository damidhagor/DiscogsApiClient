using DiscogsApiClient.Authentication;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.Pat;
using Microsoft.Extensions.Options;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class DiscogsAuthenticationHeaderProviderTests
{
    [Test]
    public async Task IsAuthenticated_ShouldBeFalse_WhenNeitherProviderIsRegistered()
    {
        var headerProvider = new DiscogsAuthenticationHeaderProvider();

        await Assert.That(headerProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => headerProvider.GetHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task IsAuthenticated_ShouldReflectPatProvider_WhenOnlyPatIsRegistered()
    {
        var patProvider = new DiscogsPatAuthenticationProvider(Options.Create(new DiscogsPatOptions()));
        var headerProvider = new DiscogsAuthenticationHeaderProvider(patProvider: patProvider);

        await Assert.That(headerProvider.IsAuthenticated).IsFalse();

        patProvider.Authenticate("token");

        await Assert.That(headerProvider.IsAuthenticated).IsTrue();
        await Assert.That(headerProvider.GetHeader()).IsEqualTo("Discogs token=token");
    }

    [Test]
    public async Task IsAuthenticated_ShouldReflectOAuthProvider_WhenOnlyOAuthIsRegistered()
    {
        var oAuthProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(null!), Options.Create(new DiscogsOAuthOptions { ConsumerKey = "key", ConsumerSecret = "secret" }));
        var headerProvider = new DiscogsAuthenticationHeaderProvider(oAuthProvider: oAuthProvider);

        await Assert.That(headerProvider.IsAuthenticated).IsFalse();

        oAuthProvider.Authenticate("accesstoken", "accesstokensecret");

        await Assert.That(headerProvider.IsAuthenticated).IsTrue();
        await Assert.That(headerProvider.GetHeader()).Contains("oauth_token=\"accesstoken\"");
    }
}
