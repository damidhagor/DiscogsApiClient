using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class AuthenticationServiceTestFixture : ApiBaseTestFixture
{
    [Test]
    public void PersonalAccessTokenAuthentication_Successful()
    {
        var token = "myusertoken";
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!));

        Assert.IsFalse(authService.IsAuthenticated);

        authService.AuthenticateWithPersonalAccessToken(token);

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual($"Discogs token={token}", authService.CreateAuthenticationHeader());
    }

    [Test]
    public void Unauthenticated_Service_Throws_UnauthorizedException()
    {
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!));

        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());
    }

    [Test]
    public void PersonalAccessToken_Guard_Works()
    {
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!));

        Assert.Throws<ArgumentNullException>(() => authService.AuthenticateWithPersonalAccessToken(null!));

        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());

        Assert.Throws<ArgumentException>(() => authService.AuthenticateWithPersonalAccessToken(""));

        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());

        Assert.Throws<ArgumentException>(() => authService.AuthenticateWithPersonalAccessToken("   "));

        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());
    }

    [Test]
    public void Failed_Only_PersonalAccessToken_Resets_IsAuthenticated()
    {
        var token = "myusertoken";
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!));

        Assert.IsFalse(authService.IsAuthenticated);

        authService.AuthenticateWithPersonalAccessToken(token);

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual($"Discogs token={token}", authService.CreateAuthenticationHeader());

        Assert.Throws<ArgumentException>(() => authService.AuthenticateWithPersonalAccessToken(""));
        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());
    }
}
