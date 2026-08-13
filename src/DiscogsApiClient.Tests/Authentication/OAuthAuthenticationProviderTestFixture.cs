using System.Net.Http;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Tests.MockMiddleware;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class OAuthAuthenticationProviderTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task Authentication_Successful()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "http://localhost/access_token" };

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        Assert.IsFalse(authProvider.IsAuthenticated);

        var session = await authProvider.StartAuthentication(default);

        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.AreEqual("https://discogs.com/oauth/authorize?oauth_token=requesttoken", session.AuthorizeUrl);
        Assert.AreEqual("http://localhost/access_token", session.VerifierCallbackUrl);
        Assert.AreEqual("requesttoken", session.RequestToken);
        Assert.AreEqual("requesttokensecret", session.RequestTokenSecret);

        var (accessToken, accessTokenSecret) = await authProvider.CompleteAuthentication(session, "verifier", default);

        var authHeader = authProvider.CreateAuthenticationHeader();

        Assert.IsTrue(authProvider.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));
    }

    [Test]
    public void Unauthenticated_Provider_Throws_UnauthorizedException()
    {
        var httpClient = new HttpClient(new OAuthMockDelegatingHandler());
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());
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
    public void Start_Guards_Work(
        string consumerKey,
        string consumerSecret,
        string verifierCallbackUrl,
        Type exceptionType,
        string paramName)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = consumerKey, ConsumerSecret = consumerSecret, VerifierCallbackUrl = verifierCallbackUrl };

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        var exception = Assert.ThrowsAsync(
            exceptionType,
            () => authProvider.StartAuthentication(default))
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
    public void Start_Unauthenticated(string requestToken, string requestTokenSecret)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { RequestToken = requestToken, RequestTokenSecret = requestTokenSecret };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "callback" };

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(() => authProvider.StartAuthentication(default));
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
    public void Complete_Guards_Work(
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

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        var exception = Assert.ThrowsAsync(
            exceptionType,
            () => authProvider.CompleteAuthentication(session, verifierToken, default))
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
    public void Complete_Unauthenticated(string accessToken, string accessTokenSecret)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { AccessToken = accessToken, AccessTokenSecret = accessTokenSecret };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(() => authProvider.CompleteAuthentication(session, "verifier", default));
    }

    [Test]
    public async Task Failed_Authentication_NotResets_IsAuthenticated()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        Assert.IsFalse(authProvider.IsAuthenticated);

        var (accessToken, accessTokenSecret) = await authProvider.CompleteAuthentication(session, "verifierToken", default);
        var authHeader = authProvider.CreateAuthenticationHeader();

        Assert.IsTrue(authProvider.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));

        oauthMockHandler.AccessToken = "";
        oauthMockHandler.AccessTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.CompleteAuthentication(session, "verifierToken", default));
        Assert.IsTrue(authProvider.IsAuthenticated);
        Assert.DoesNotThrow(() => authProvider.CreateAuthenticationHeader());
    }

    [Test]
    public void Authenticate_Short_Circuit_Successfully()
    {
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authProvider = new OAuthAuthenticationProvider(null!, options);

        Assert.IsFalse(authProvider.IsAuthenticated);

        authProvider.Authenticate("testtoken", "testsecret");
        var authHeader = authProvider.CreateAuthenticationHeader();

        Assert.IsTrue(authProvider.IsAuthenticated);
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
    public void Short_Circuit_Guards_Work(
        string accessToken,
        string accessTokenSecret,
        Type exceptionType,
        string paramName)
    {
        var authProvider = new OAuthAuthenticationProvider(null!, null!);

        var exception = Assert.Throws(
            exceptionType,
            () => authProvider.Authenticate(accessToken, accessTokenSecret))
            as ArgumentException;
        Assert.AreEqual(paramName, exception!.ParamName);
    }
}
