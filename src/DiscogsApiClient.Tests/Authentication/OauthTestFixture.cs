using System.Net;
using System.Net.Http;
using DiscogsApiClient.Authentication.OAuth;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class OauthTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task Authentication_Successful()
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(new OAuthMockDelegatingHandler()) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authProvider = new OAuthAuthenticationProvider(httpClient);

        Assert.IsFalse(authProvider.IsAuthenticated);

        var (accessToken, accessTokenSecret) = await authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
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

    [Test]
    public void Authenticate_Guards_Works()
    {
        var httpClient = new HttpClient(new OAuthMockDelegatingHandler());
        var authProvider = new OAuthAuthenticationProvider(httpClient);

        ArgumentException? exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => authProvider.Authenticate(null!, "", "", "", "", null!, default!));
        Assert.AreEqual("consumerKey", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authProvider.Authenticate("", "", "", "", "", null!, default!));
        Assert.AreEqual("consumerKey", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authProvider.Authenticate("   ", "", "", "", "", null!, default!));
        Assert.AreEqual("consumerKey", exception!.ParamName);


        exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => authProvider.Authenticate("x", null!, "", "", "", null!, default!));
        Assert.AreEqual("consumerSecret", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authProvider.Authenticate("x", "", "", "", "", null!, default!));
        Assert.AreEqual("consumerSecret", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authProvider.Authenticate("x", "   ", "", "", "", null!, default!));
        Assert.AreEqual("consumerSecret", exception!.ParamName);


        exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => authProvider.Authenticate("x", "x", "", "", null!, null!, default!));
        Assert.AreEqual("verifierCallbackUrl", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authProvider.Authenticate("x", "x", "", "", "", null!, default!));
        Assert.AreEqual("verifierCallbackUrl", exception!.ParamName);

        exception = Assert.ThrowsAsync<ArgumentException>(
            () => authProvider.Authenticate("x", "x", "", "", "  ", null!, default!));
        Assert.AreEqual("verifierCallbackUrl", exception!.ParamName);


        exception = Assert.ThrowsAsync<ArgumentNullException>(
            () => authProvider.Authenticate("x", "x", "", "", "x", null!, default!));
        Assert.AreEqual("getVerifierCallback", exception!.ParamName);
    }

    [Test]
    public async Task Existing_Access_Tokens_Short_Circuit_Successfully()
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
                Assert.Fail("Provider should not du the authentication flow.");
                return Task.FromResult("");
            }
        };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };

        var authProvider = new OAuthAuthenticationProvider(httpClient);

        Assert.IsFalse(authProvider.IsAuthenticated);

        var (accessToken, accessTokenSecret) = await authProvider.Authenticate("key", "secret", "token", "secret", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
        var authHeader = authProvider.CreateAuthenticationHeader();

        Assert.IsTrue(authProvider.IsAuthenticated);
        Assert.AreEqual("token", accessToken);
        Assert.AreEqual("secret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"token\""));
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
            () => authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
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
            () => authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
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
            () => authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Failed getting verifier token.",
            null);

        oauthMockHandler.GetVerifierCallbackMock = (authUrl, callbackUrl, cancellationToken) => Task.FromResult<string>(null!);

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Failed getting verifier token.",
            null);

        oauthMockHandler.GetVerifierCallbackMock = (authUrl, callbackUrl, cancellationToken) => Task.FromResult("");

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
            "Failed getting verifier token.",
            null);

        oauthMockHandler.GetVerifierCallbackMock = (authUrl, callbackUrl, cancellationToken) => Task.FromResult("  ");

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(
            () => authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default),
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

        var (accessToken, accessTokenSecret) = await authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default);
        var authHeader = authProvider.CreateAuthenticationHeader();

        Assert.IsTrue(authProvider.IsAuthenticated);
        Assert.AreEqual("accesstoken", accessToken);
        Assert.AreEqual("accesstokensecret", accessTokenSecret);
        Assert.IsTrue(authHeader.Contains("oauth_consumer_key=\"key\""));
        Assert.IsTrue(authHeader.Contains("oauth_token=\"accesstoken\""));

        oauthMockHandler.RequestToken = "";
        oauthMockHandler.RequestTokenSecret = "";

        Assert.ThrowsAsync<AuthenticationFailedDiscogsException>(() => authProvider.Authenticate("key", "secret", "", "", "http://localhost/access_token", oauthMockHandler.GetVerifierCallbackMockCaller, default));


        Assert.IsFalse(authProvider.IsAuthenticated);
        Assert.Throws<UnauthenticatedDiscogsException>(() => authProvider.CreateAuthenticationHeader());
    }
}
