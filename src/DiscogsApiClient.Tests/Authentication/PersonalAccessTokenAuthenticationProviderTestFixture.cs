using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class PersonalAccessTokenAuthenticationProviderTestFixture
{
    [Test]
    public async Task Authentication_Successful()
    {
        var token = "myusertoken";
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        await Assert.That(authProvider.IsAuthenticated).IsFalse();

        authProvider.Authenticate(token);

        await Assert.That(authProvider.IsAuthenticated).IsTrue();
        await Assert.That(authProvider.CreateAuthenticationHeader()).IsEqualTo($"Discogs token={token}");
    }

    [Test]
    public async Task Unauthenticated_Provider_Throws_UnauthorizedException()
    {
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        await Assert.That(authProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => authProvider.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    [Arguments(null!, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("   ", typeof(ArgumentException))]
    public async Task Token_Guard_Works(string? token, Type expectedException)
    {
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        var exception = await Assert.That(() => authProvider.Authenticate(token!)).Throws<Exception>();

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception).IsOfType(expectedException);

        await Assert.That(authProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => authProvider.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task Failed_Authentication_Resets_UserToken()
    {
        var token = "myusertoken";
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        await Assert.That(authProvider.IsAuthenticated).IsFalse();

        authProvider.Authenticate(token);

        await Assert.That(authProvider.IsAuthenticated).IsTrue();
        await Assert.That(authProvider.CreateAuthenticationHeader()).IsEqualTo($"Discogs token={token}");

        await Assert.That(() => authProvider.Authenticate("")).Throws<ArgumentException>();
        await Assert.That(authProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => authProvider.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }
}
