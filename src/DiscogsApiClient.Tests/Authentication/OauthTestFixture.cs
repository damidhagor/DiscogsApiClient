using System.Net;
using System.Net.Http;
using DiscogsApiClient.Authentication.OAuth;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class OAuthTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task Authentication_Successful()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(new OAuthMockDelegatingHandler()) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authProvider = new OAuthAuthenticationProvider(httpClient);

        Assert.IsFalse(authProvider.IsAuthenticated);

        var (accessToken, accessTokenSecret) = await authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
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
        var authProvider = new OAuthAuthenticationProvider(httpClient);

        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());
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
    public void Authenticate_Guards_Works(
        string consumerKey,
        string consumerSecret,
        string verifierCallbackUrl,
        GetVerifierCallback verifierCallback,
        Type exceptionType,
        string paramName)
    {
        var httpClient = new HttpClient(new OAuthMockDelegatingHandler());
        var authProvider = new OAuthAuthenticationProvider(httpClient);

        var exception = Assert.ThrowsAsync(
            exceptionType,
            () => authProvider.Authenticate(consumerKey, consumerSecret, verifierCallbackUrl, verifierCallback, default!))
            as ArgumentException;
        Assert.AreEqual(paramName, exception!.ParamName);
    }

    [Test]
    public void Get_Request_Tokens_Fails()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authProvider = new OAuthAuthenticationProvider(httpClient);

        oauthMockHandler.RequestTokenStatusCode = HttpStatusCode.InternalServerError;
        oauthMockHandler.RequestToken = "";
        oauthMockHandler.RequestTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Getting request token failed.",
            null);
    }

    [Test]
    public void Get_Access_Tokens_Fails()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authProvider = new OAuthAuthenticationProvider(httpClient);

        oauthMockHandler.AccessTokenStatusCode = HttpStatusCode.InternalServerError;
        oauthMockHandler.AccessToken = "";
        oauthMockHandler.AccessTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Getting access token failed.",
            null);
    }

    [Test]
    public void Get_Verifier_Tokens_Fails()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authProvider = new OAuthAuthenticationProvider(httpClient);

        oauthMockHandler.GetVerifierCallbackMock = (authUrl, callbackUrl, cancellationToken) => throw new Exception("Bad Error.");

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Failed getting verifier token.",
            null);

        oauthMockHandler.GetVerifierCallbackMock = (authUrl, callbackUrl, cancellationToken) => Task.FromResult<string>(null!);

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Failed getting verifier token.",
            null);

        oauthMockHandler.GetVerifierCallbackMock = (authUrl, callbackUrl, cancellationToken) => Task.FromResult("");

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Failed getting verifier token.",
            null);

        oauthMockHandler.GetVerifierCallbackMock = (authUrl, callbackUrl, cancellationToken) => Task.FromResult("  ");

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Failed getting verifier token.",
            null);
    }

    [Test]
    public async Task Failed_Authentication_Resets_Token()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authProvider = new OAuthAuthenticationProvider(httpClient);

        Assert.IsFalse(authProvider.IsAuthenticated);

        var (accessToken, accessTokenSecret) = await authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
        var authHeader = authProvider.CreateAuthenticationHeader();

        Assert.IsTrue(authProvider.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));

        oauthMockHandler.RequestToken = "";
        oauthMockHandler.RequestTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(() => authProvider.Authenticate("key", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default));


        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());
    }

    [Test]
    public void Authenticate_Short_Circuit_Successfully()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler
        {
            RequestTokenStatusCode = HttpStatusCode.InternalServerError,
            RequestToken = "",
            RequestTokenSecret = "",
            AccessTokenStatusCode = HttpStatusCode.InternalServerError,
            AccessToken = "",
            AccessTokenSecret = "",
            GetVerifierCallbackMock = (authUrl, callbackUrl, cancellationToken) =>
            {
                Assert.Fail("Provider should not do the authentication flow.");
                return Task.FromResult("");
            }
        };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authProvider = new OAuthAuthenticationProvider(httpClient);

        Assert.IsFalse(authProvider.IsAuthenticated);

        authProvider.Authenticate("key", "secret", "testtoken", "testsecret");
        var authHeader = authProvider.CreateAuthenticationHeader();

        Assert.IsTrue(authProvider.IsAuthenticated);
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
    public void Authenticate_Short_Circuit_Guards_Works(
        string consumerKey,
        string consumerSecret,
        string accessToken,
        string accessTokenSecret,
        Type exceptionType,
        string paramName)
    {
        var httpClient = new HttpClient(new OAuthMockDelegatingHandler());
        var authProvider = new OAuthAuthenticationProvider(httpClient);

        var exception = Assert.Throws(
            exceptionType,
            () => authProvider.Authenticate(consumerKey, consumerSecret, accessToken, accessTokenSecret))
            as ArgumentException;
        Assert.AreEqual(paramName, exception!.ParamName);
    }
}
