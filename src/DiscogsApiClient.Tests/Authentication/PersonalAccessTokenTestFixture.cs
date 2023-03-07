using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class PersonalAccessTokenTestFixture : ApiBaseTestFixture
{
    [Test]
    public void Authentication_Successful()
    {
        var token = "myusertoken";
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        Assert.IsFalse(authProvider.IsAuthenticated);

        authProvider.Authenticate(token);

        Assert.IsTrue(authProvider.IsAuthenticated);
        Assert.AreEqual($"Discogs token={token}", authProvider.CreateAuthenticationHeader());
    }

    [Test]
    public void Unauthenticated_Provider_Throws_UnauthorizedException()
    {
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());
    }

    [Test]
    public void Token_Guard_Works()
    {
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        Assert.Throws<ArgumentNullException>(() => authProvider.Authenticate(null!));

        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());

        Assert.Throws<ArgumentException>(() => authProvider.Authenticate(""));

        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());

        Assert.Throws<ArgumentException>(() => authProvider.Authenticate("   "));

        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());
    }

    [Test]
    public void Failed_Authentication_Resets_UserToken()
    {
        var token = "myusertoken";
        var authProvider = new PersonalAccessTokenAuthenticationProvider();

        Assert.IsFalse(authProvider.IsAuthenticated);

        authProvider.Authenticate(token);

        Assert.IsTrue(authProvider.IsAuthenticated);
        Assert.AreEqual($"Discogs token={token}", authProvider.CreateAuthenticationHeader());

        Assert.Throws<ArgumentException>(() => authProvider.Authenticate(""));
        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());
    }
}
