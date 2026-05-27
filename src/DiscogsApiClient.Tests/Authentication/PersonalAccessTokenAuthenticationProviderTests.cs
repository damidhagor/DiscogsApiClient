using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class PersonalAccessTokenAuthenticationProviderTests
{
    [Test]
    public async Task Authenticate_ShouldAuthenticate_WhenTokenIsValid()
    {
        var token = "myusertoken";
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        await Assert.That(authProvider.IsAuthenticated).IsFalse();

        authProvider.Authenticate(token);

        await Assert.That(authProvider.IsAuthenticated).IsTrue();
        await Assert.That(authProvider.CreateAuthenticationHeader()).IsEqualTo($"Discogs token={token}");
    }

    [Test]
    public async Task CreateAuthenticationHeader_ShouldThrowUnauthenticatedDiscogsException_WhenNotAuthenticated()
    {
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        await Assert.That(authProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => authProvider.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    [Arguments(null!, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("   ", typeof(ArgumentException))]
    public async Task Authenticate_ShouldThrowException_WhenTokenIsInvalid(string? token, Type expectedException)
    {
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        var exception = await Assert.That(() => authProvider.Authenticate(token!)).Throws<Exception>();

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception).IsOfType(expectedException);

        await Assert.That(authProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => authProvider.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task Authenticate_ShouldResetAuthenticationState_WhenAuthenticationFails()
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
