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

        var (accessToken, accessTokenSecret) = await authService.AuthenticateWithOAuth("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));
    }

    [Theory]
    [TestCase(null, "", "", null, typeof(ArgumentNullException), "consumerKey")]
    [TestCase("", "", "", null, typeof(ArgumentException), "consumerKey")]
    [TestCase("   ", "", "", null, typeof(ArgumentException), "consumerKey")]
    [TestCase("x", null, "", null, typeof(ArgumentNullException), "consumerSecret")]
    [TestCase("x", "", "", null, typeof(ArgumentException), "consumerSecret")]
    [TestCase("x", "   ", "", null, typeof(ArgumentException), "consumerSecret")]
    [TestCase("x", "x", null, null, typeof(ArgumentNullException), "verifierCallbackUrl")]
    [TestCase("x", "x", "", null, typeof(ArgumentException), "verifierCallbackUrl")]
    [TestCase("x", "x", "  ", null, typeof(ArgumentException), "verifierCallbackUrl")]
    [TestCase("x", "x", "x", null, typeof(ArgumentNullException), "getVerifierCallback")]
    public void OAuthAuthentication_Guards_Works(
        string consumerKey,
        string consumerSecret,
        string verifierCallbackUrl,
        GetVerifierCallback verifierCallback,
        Type exceptionType,
        string paramName)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient));

        var exception = Assert.ThrowsAsync(
            exceptionType,
            () => authService.AuthenticateWithOAuth(consumerKey, consumerSecret, verifierCallbackUrl, verifierCallback, default!))
            as ArgumentException;
        Assert.AreEqual(paramName, exception!.ParamName);
    }

    [Test]
    public void OAuthAuthentication_Short_Circuit_Successful()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient));

        Assert.IsFalse(authService.IsAuthenticated);

        authService.AuthenticateWithOAuth("key", "secret", "testtoken", "testsecret");
        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"testtoken\""));
        Assert.IsTrue(authHeader.Contains("oauth_signature=\"secret%26testsecret\""));
    }

    [Theory]
    [TestCase(null, "", "", "", typeof(ArgumentNullException), "consumerKey")]
    [TestCase("", "", "", "", typeof(ArgumentException), "consumerKey")]
    [TestCase("   ", "", "", "", typeof(ArgumentException), "consumerKey")]
    [TestCase("x", null, "", "", typeof(ArgumentNullException), "consumerSecret")]
    [TestCase("x", "", "", "", typeof(ArgumentException), "consumerSecret")]
    [TestCase("x", "   ", "", "", typeof(ArgumentException), "consumerSecret")]
    [TestCase("x", "x", null, "", typeof(ArgumentNullException), "accessToken")]
    [TestCase("x", "x", "", "", typeof(ArgumentException), "accessToken")]
    [TestCase("x", "x", "   ", "", typeof(ArgumentException), "accessToken")]
    [TestCase("x", "x", "x", null, typeof(ArgumentNullException), "accessTokenSecret")]
    [TestCase("x", "x", "x", "", typeof(ArgumentException), "accessTokenSecret")]
    [TestCase("x", "x", "x", "   ", typeof(ArgumentException), "accessTokenSecret")]
    public void OAuthAuthentication_Short_Circuit_Guards_Works(
        string consumerKey,
        string consumerSecret,
        string accessToken,
        string accessTokenSecret,
        Type exceptionType,
        string paramName)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient));

        var exception = Assert.Throws(
            exceptionType,
            () => authService.AuthenticateWithOAuth(consumerKey, consumerSecret, accessToken, accessTokenSecret))
            as ArgumentException;
        Assert.AreEqual(paramName, exception!.ParamName);
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

        var (accessToken, accessTokenSecret) = await authService.AuthenticateWithOAuth("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));

        oauthMockHandler.RequestToken = "";
        oauthMockHandler.RequestTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authService.AuthenticateWithOAuth("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default));
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
        var (accessToken, accessTokenSecret) = await authService.AuthenticateWithOAuth("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
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
            () => authService.AuthenticateWithOAuth("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default));
        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());
    }
}
