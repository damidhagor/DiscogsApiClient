using System.Net.Http;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class AuthenticationServiceTestFixture : ApiBaseTestFixture
{
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


    [Test]
    public async Task OAuthAuthentication_Successful()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient));

        Assert.IsFalse(authService.IsAuthenticated);

        var (accessToken, accessTokenSecret) = await authService.AuthenticateWithOAuth("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));
    }

    [Test]
    public void OAuth_Guards_Works()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient));

        ArgumentException? exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => authService.AuthenticateWithOAuth(null!, "", "", "", "", null!, default!));
        Assert.AreEqual("consumerKey", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authService.AuthenticateWithOAuth("", "", "", "", "", null!, default!));
        Assert.AreEqual("consumerKey", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authService.AuthenticateWithOAuth("   ", "", "", "", "", null!, default!));
        Assert.AreEqual("consumerKey", exception!.ParamName);


        exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => authService.AuthenticateWithOAuth("x", null!, "", "", "", null!, default!));
        Assert.AreEqual("consumerSecret", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authService.AuthenticateWithOAuth("x", "", "", "", "", null!, default!));
        Assert.AreEqual("consumerSecret", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authService.AuthenticateWithOAuth("x", "   ", "", "", "", null!, default!));
        Assert.AreEqual("consumerSecret", exception!.ParamName);


        exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => authService.AuthenticateWithOAuth("x", "x", "", "", null!, null!, default!));
        Assert.AreEqual("verifierCallbackUrl", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authService.AuthenticateWithOAuth("x", "x", "", "", "", null!, default!));
        Assert.AreEqual("verifierCallbackUrl", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authService.AuthenticateWithOAuth("x", "x", "", "", "  ", null!, default!));
        Assert.AreEqual("verifierCallbackUrl", exception!.ParamName);


        exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => authService.AuthenticateWithOAuth("x", "x", "", "", "x", null!, default!));
        Assert.AreEqual("getVerifierCallback", exception!.ParamName);
    }

    [Test]
    public async Task Failed_Only_OAuth_Resets_IsAuthenticated()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient));

        Assert.IsFalse(authService.IsAuthenticated);

        var (accessToken, accessTokenSecret) = await authService.AuthenticateWithOAuth("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));

        oauthMockHandler.RequestToken = "";
        oauthMockHandler.RequestTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authService.AuthenticateWithOAuth("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default));
        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());
    }

    [Test]
    public async Task Reauthentication_With_Different_Method_Switches_Method()
    {
        var userToken = "userToken";

        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient));

        Assert.IsFalse(authService.IsAuthenticated);

        // Only personal access token
        authService.AuthenticateWithPersonalAccessToken(userToken);

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual($"Discogs token={userToken}", authService.CreateAuthenticationHeader());

        // OAuth replaces personal access token
        var (accessToken, accessTokenSecret) = await authService.AuthenticateWithOAuth("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));

        // Personal access token replaces OAuth
        authService.AuthenticateWithPersonalAccessToken(userToken);

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual($"Discogs token={userToken}", authService.CreateAuthenticationHeader());

        // Unauthenticated personal access token leaves OAuth
        Assert.Throws<ArgumentException>(() => authService.AuthenticateWithPersonalAccessToken(""));

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));

        // Unauthenticated OAuth leaves nothing
        oauthMockHandler.RequestToken = "";
        oauthMockHandler.RequestTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authService.AuthenticateWithOAuth("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default));
        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());
    }
}
