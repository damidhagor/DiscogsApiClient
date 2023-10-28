using System.Net.Http;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Tests.MockMiddleware;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class AuthenticationServiceTestFixture : ApiBaseTestFixture
{
    [Test]
    public void Unauthenticated_Service_Throws_UnauthorizedException()
    {
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, null!));

        Assert.IsFalse(authService.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authService.CreateAuthenticationHeader());
    }


    [Test]
    public void PersonalAccessTokenAuthentication_Successful()
    {
        var token = "myusertoken";
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, null!));

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
            new OAuthAuthenticationProvider(null!, null!));

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
            new OAuthAuthenticationProvider(null!, null!));

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
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "http://localhost/access_token" };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        Assert.IsFalse(authService.IsAuthenticated);

        var session = await authService.StartOAuthAuthentication(default);

        Assert.IsFalse(authService.IsAuthenticated);
        Assert.AreEqual("https://discogs.com/oauth/authorize?oauth_token=requesttoken", session.AuthorizeUrl);
        Assert.AreEqual("http://localhost/access_token", session.VerifierCallbackUrl);
        Assert.AreEqual("requesttoken", session.RequestToken);
        Assert.AreEqual("requesttokensecret", session.RequestTokenSecret);

        var (accessToken, accessTokenSecret) = await authService.CompleteOAuthAuthentication(session, "verifier", default);

        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));
    }

    [Theory]
    [TestCase("", "", null, typeof(ArgumentException), "VerifierCallbackUrl")]
    [TestCase("", "", "", typeof(ArgumentException), "VerifierCallbackUrl")]
    [TestCase("", "", "  ", typeof(ArgumentException), "VerifierCallbackUrl")]
    [TestCase(null, "", "x", typeof(ArgumentNullException), "ConsumerKey")]
    [TestCase("", "", "x", typeof(ArgumentException), "ConsumerKey")]
    [TestCase("  ", "", "x", typeof(ArgumentException), "ConsumerKey")]
    [TestCase("x", null, "x", typeof(ArgumentNullException), "ConsumerSecret")]
    [TestCase("x", "", "x", typeof(ArgumentException), "ConsumerSecret")]
    [TestCase("x", "  ", "x", typeof(ArgumentException), "ConsumerSecret")]
    public void OAuthAuthentication_Start_Guards_Work(
        string consumerKey,
        string consumerSecret,
        string verifierCallbackUrl,
        Type exceptionType,
        string paramName)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, VerifierCallbackUrl = verifierCallbackUrl };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        var exception = Assert.ThrowsAsync(
            exceptionType,
            () => authService.StartOAuthAuthentication(default))
            as ArgumentException;
        Assert.AreEqual(paramName, exception!.ParamName);
    }

    [Theory]
    [TestCase(null, "x")]
    [TestCase("", "x")]
    [TestCase("  ", "x")]
    [TestCase("x", null)]
    [TestCase("x", "")]
    [TestCase("x", "  ")]
    public void OAuthAuthentication_Start_Unauthenticated(string requestToken, string requestTokenSecret)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { RequestToken = requestToken, RequestTokenSecret = requestTokenSecret };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "callback" };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(() => authService.StartOAuthAuthentication(default));
    }

    [Theory]
    [TestCase("", "", null, "", "", typeof(ArgumentNullException), "RequestToken")]
    [TestCase("", "", "", "", "", typeof(ArgumentException), "RequestToken")]
    [TestCase("", "", "  ", "", "", typeof(ArgumentException), "RequestToken")]
    [TestCase("", "", "x", null, "", typeof(ArgumentNullException), "RequestTokenSecret")]
    [TestCase("", "", "x", "", "", typeof(ArgumentException), "RequestTokenSecret")]
    [TestCase("", "", "x", "  ", "", typeof(ArgumentException), "RequestTokenSecret")]
    [TestCase("", "", "x", "x", null, typeof(ArgumentNullException), "verifierToken")]
    [TestCase("", "", "x", "x", "", typeof(ArgumentException), "verifierToken")]
    [TestCase("", "", "x", "x", "  ", typeof(ArgumentException), "verifierToken")]
    [TestCase(null, "", "x", "x", "x", typeof(ArgumentNullException), "ConsumerKey")]
    [TestCase("", "", "x", "x", "x", typeof(ArgumentException), "ConsumerKey")]
    [TestCase("  ", "", "x", "x", "x", typeof(ArgumentException), "ConsumerKey")]
    [TestCase("x", null, "x", "x", "x", typeof(ArgumentNullException), "ConsumerSecret")]
    [TestCase("x", "", "x", "x", "x", typeof(ArgumentException), "ConsumerSecret")]
    [TestCase("x", "  ", "x", "x", "x", typeof(ArgumentException), "ConsumerSecret")]
    public void OAuthAuthentication_Complete_Guards_Work(
        string consumerKey,
        string consumerSecret,
        string requestToken,
        string requestTokenSecret,
        string verifierToken,
        Type exceptionType,
        string paramName)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret };
        var session = new OAuthAuthenticationSession("", "", requestToken, requestTokenSecret);

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        var exception = Assert.ThrowsAsync(
            exceptionType,
            () => authService.CompleteOAuthAuthentication(session, verifierToken, default))
            as ArgumentException;
        Assert.AreEqual(paramName, exception!.ParamName);
    }

    [Theory]
    [TestCase(null, "x")]
    [TestCase("", "x")]
    [TestCase("  ", "x")]
    [TestCase("x", null)]
    [TestCase("x", "")]
    [TestCase("x", "  ")]
    public void OAuthAuthentication_Complete_Unauthenticated(string accessToken, string accessTokenSecret)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { AccessToken = accessToken, AccessTokenSecret = accessTokenSecret };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(() => authService.CompleteOAuthAuthentication(session, "verifier", default));
    }

    [Test]
    public void OAuthAuthentication_Short_Circuit_Successful()
    {
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, options));

        Assert.IsFalse(authService.IsAuthenticated);

        authService.AuthenticateWithOAuth("testtoken", "testsecret");
        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"testtoken\""));
        Assert.IsTrue(authHeader.Contains("oauth_signature=\"secret%26testsecret\""));
    }

    [Theory]
    [TestCase(null, "", typeof(ArgumentNullException), "accessToken")]
    [TestCase("", "", typeof(ArgumentException), "accessToken")]
    [TestCase("   ", "", typeof(ArgumentException), "accessToken")]
    [TestCase("x", null, typeof(ArgumentNullException), "accessTokenSecret")]
    [TestCase("x", "", typeof(ArgumentException), "accessTokenSecret")]
    [TestCase("x", "   ", typeof(ArgumentException), "accessTokenSecret")]
    public void OAuthAuthentication_Short_Circuit_Guards_Work(
        string accessToken,
        string accessTokenSecret,
        Type exceptionType,
        string paramName)
    {
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, null!));

        var exception = Assert.Throws(
            exceptionType,
            () => authService.AuthenticateWithOAuth(accessToken, accessTokenSecret))
            as ArgumentException;
        Assert.AreEqual(paramName, exception!.ParamName);
    }

    [Test]
    public async Task Failed_OAuth_NotResets_IsAuthenticated()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        Assert.IsFalse(authService.IsAuthenticated);

        var (accessToken, accessTokenSecret) = await authService.CompleteOAuthAuthentication(session, "verifierToken", default);
        var authHeader = authService.CreateAuthenticationHeader();

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));

        oauthMockHandler.AccessToken = "";
        oauthMockHandler.AccessTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authService.CompleteOAuthAuthentication(session, "verifierToken", default));
        Assert.IsTrue(authService.IsAuthenticated);
        Assert.DoesNotThrow(() => authService.CreateAuthenticationHeader());
    }

    [Test]
    public async Task Reauthentication_With_Different_Method_Switches_Method()
    {
        var userToken = "userToken";
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");
        var verifierToken = "verifier";

        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        Assert.IsFalse(authService.IsAuthenticated);

        // Only personal access token
        authService.AuthenticateWithPersonalAccessToken(userToken);

        Assert.IsTrue(authService.IsAuthenticated);
        Assert.AreEqual($"Discogs token={userToken}", authService.CreateAuthenticationHeader());

        // OAuth replaces personal access token
        var (accessToken, accessTokenSecret) = await authService.CompleteOAuthAuthentication(session, verifierToken, default);
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
    }
}
