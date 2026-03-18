using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Tests.MockMiddleware;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class OAuthAuthenticationProviderTestFixture
{
    [Test]
    public async Task Authentication_Successful(CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "http://localhost/access_token" };

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        await Assert.That(authProvider.IsAuthenticated).IsFalse();

        var session = await authProvider.StartAuthentication(cancellationToken);

        await Assert.That(authProvider.IsAuthenticated).IsFalse();
        await Assert.That(session.AuthorizeUrl).IsEqualTo("https://discogs.com/oauth/authorize?oauth_token=requesttoken");
        await Assert.That(session.VerifierCallbackUrl).IsEqualTo("http://localhost/access_token");
        await Assert.That(session.RequestToken).IsEqualTo("requesttoken");
        await Assert.That(session.RequestTokenSecret).IsEqualTo("requesttokensecret");

        var (accessToken, accessTokenSecret) = await authProvider.CompleteAuthentication(session, "verifier", cancellationToken);

        var authHeader = authProvider.CreateAuthenticationHeader();

        await Assert.That(authProvider.IsAuthenticated).IsTrue();
        await Assert.That(accessToken).IsEqualTo("accesstoken");
        await Assert.That(accessTokenSecret).IsEqualTo("accesstokensecret");
        await Assert.That(authHeader).Contains("oauth_consumer_key=\"key\"");
        await Assert.That(authHeader).Contains("oauth_token=\"accesstoken\"");
    }

    [Test]
    public async Task Unauthenticated_Provider_Throws_UnauthorizedException()
    {
        var httpClient = new HttpClient(new OAuthMockDelegatingHandler());
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        await Assert.That(authProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => authProvider.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    [Arguments("", "", null, typeof(ArgumentException), "VerifierCallbackUrl")]
    [Arguments("", "", "", typeof(ArgumentException), "VerifierCallbackUrl")]
    [Arguments("", "", "  ", typeof(ArgumentException), "VerifierCallbackUrl")]
    [Arguments(null, "", "x", typeof(ArgumentNullException), "ConsumerKey")]
    [Arguments("", "", "x", typeof(ArgumentException), "ConsumerKey")]
    [Arguments("  ", "", "x", typeof(ArgumentException), "ConsumerKey")]
    [Arguments("x", null, "x", typeof(ArgumentNullException), "ConsumerSecret")]
    [Arguments("x", "", "x", typeof(ArgumentException), "ConsumerSecret")]
    [Arguments("x", "  ", "x", typeof(ArgumentException), "ConsumerSecret")]
    public async Task Start_Guards_Work(
        string? consumerKey,
        string? consumerSecret,
        string? verifierCallbackUrl,
        Type exceptionType,
        string paramName,
        CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = consumerKey!, ConsumerSecret = consumerSecret!, VerifierCallbackUrl = verifierCallbackUrl };

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        var exception = await Assert.That(async () => await authProvider.StartAuthentication(cancellationToken))
            .Throws<ArgumentException>();

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception).IsOfType(exceptionType);
        await Assert.That(exception.ParamName).IsEqualTo(paramName);
    }

    [Test]
    [Arguments(null, "x")]
    [Arguments("", "x")]
    [Arguments("  ", "x")]
    [Arguments("x", null)]
    [Arguments("x", "")]
    [Arguments("x", "  ")]
    public async Task Start_Unauthenticated(string? requestToken, string? requestTokenSecret, CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { RequestToken = requestToken!, RequestTokenSecret = requestTokenSecret! };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "callback" };

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        await Assert.That(async () => await authProvider.StartAuthentication(cancellationToken))
            .Throws<AuthenticationFailedDiscogsException>();
    }

    [Test]
    [Arguments("", "", null, "", "", typeof(ArgumentNullException), "RequestToken")]
    [Arguments("", "", "", "", "", typeof(ArgumentException), "RequestToken")]
    [Arguments("", "", "  ", "", "", typeof(ArgumentException), "RequestToken")]
    [Arguments("", "", "x", null, "", typeof(ArgumentNullException), "RequestTokenSecret")]
    [Arguments("", "", "x", "", "", typeof(ArgumentException), "RequestTokenSecret")]
    [Arguments("", "", "x", "  ", "", typeof(ArgumentException), "RequestTokenSecret")]
    [Arguments("", "", "x", "x", null, typeof(ArgumentNullException), "verifierToken")]
    [Arguments("", "", "x", "x", "", typeof(ArgumentException), "verifierToken")]
    [Arguments("", "", "x", "x", "  ", typeof(ArgumentException), "verifierToken")]
    [Arguments(null, "", "x", "x", "x", typeof(ArgumentNullException), "ConsumerKey")]
    [Arguments("", "", "x", "x", "x", typeof(ArgumentException), "ConsumerKey")]
    [Arguments("  ", "", "x", "x", "x", typeof(ArgumentException), "ConsumerKey")]
    [Arguments("x", null, "x", "x", "x", typeof(ArgumentNullException), "ConsumerSecret")]
    [Arguments("x", "", "x", "x", "x", typeof(ArgumentException), "ConsumerSecret")]
    [Arguments("x", "  ", "x", "x", "x", typeof(ArgumentException), "ConsumerSecret")]
    public async Task Complete_Guards_Work(
        string? consumerKey,
        string? consumerSecret,
        string? requestToken,
        string? requestTokenSecret,
        string? verifierToken,
        Type exceptionType,
        string paramName,
        CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = consumerKey!, ConsumerSecret = consumerSecret! };
        var session = new OAuthAuthenticationSession("", "", requestToken!, requestTokenSecret!);

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        var exception = await Assert.That(async () => await authProvider.CompleteAuthentication(session, verifierToken!, cancellationToken))
            .Throws<ArgumentException>();

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception).IsOfType(exceptionType);
        await Assert.That(exception.ParamName).IsEqualTo(paramName);
    }

    [Test]
    [Arguments(null, "x")]
    [Arguments("", "x")]
    [Arguments("  ", "x")]
    [Arguments("x", null)]
    [Arguments("x", "")]
    [Arguments("x", "  ")]
    public async Task Complete_Unauthenticated(string? accessToken, string? accessTokenSecret, CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { AccessToken = accessToken!, AccessTokenSecret = accessTokenSecret! };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        await Assert.That(async () => await authProvider.CompleteAuthentication(session, "verifier", cancellationToken))
            .Throws<AuthenticationFailedDiscogsException>();
    }

    [Test]
    public async Task Failed_Authentication_NotResets_IsAuthenticated(CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authProvider = new OAuthAuthenticationProvider(httpClient, options);

        await Assert.That(authProvider.IsAuthenticated).IsFalse();

        var (accessToken, accessTokenSecret) = await authProvider.CompleteAuthentication(session, "verifierToken", cancellationToken);
        var authHeader = authProvider.CreateAuthenticationHeader();

        await Assert.That(authProvider.IsAuthenticated).IsTrue();
        await Assert.That(accessToken).IsEqualTo("accesstoken");
        await Assert.That(accessTokenSecret).IsEqualTo("accesstokensecret");
        await Assert.That(authHeader).Contains("oauth_consumer_key=\"key\"");
        await Assert.That(authHeader).Contains("oauth_token=\"accesstoken\"");

        oauthMockHandler.AccessToken = "";
        oauthMockHandler.AccessTokenSecret = "";

        await Assert.That(async () => await authProvider.CompleteAuthentication(session, "verifierToken", cancellationToken))
            .Throws<AuthenticationFailedDiscogsException>();
        await Assert.That(authProvider.IsAuthenticated).IsTrue();
        await Assert.That(() => authProvider.CreateAuthenticationHeader()).ThrowsNothing();
    }

    [Test]
    public async Task Authenticate_Short_Circuit_Successfully()
    {
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authProvider = new OAuthAuthenticationProvider(null!, options);

        await Assert.That(authProvider.IsAuthenticated).IsFalse();

        authProvider.Authenticate("testtoken", "testsecret");
        var authHeader = authProvider.CreateAuthenticationHeader();

        await Assert.That(authProvider.IsAuthenticated).IsTrue();
        await Assert.That(authHeader).Contains("oauth_consumer_key=\"key\"");
        await Assert.That(authHeader).Contains("oauth_token=\"testtoken\"");
        await Assert.That(authHeader).Contains("oauth_signature=\"secret%26testsecret\"");
    }

    [Test]
    [Arguments(null, "", typeof(ArgumentNullException), "accessToken")]
    [Arguments("", "", typeof(ArgumentException), "accessToken")]
    [Arguments("   ", "", typeof(ArgumentException), "accessToken")]
    [Arguments("x", null, typeof(ArgumentNullException), "accessTokenSecret")]
    [Arguments("x", "", typeof(ArgumentException), "accessTokenSecret")]
    [Arguments("x", "   ", typeof(ArgumentException), "accessTokenSecret")]
    public async Task Short_Circuit_Guards_Work(
        string? accessToken,
        string? accessTokenSecret,
        Type exceptionType,
        string paramName)
    {
        var authProvider = new OAuthAuthenticationProvider(null!, null!);

        var exception = await Assert.That(() => authProvider.Authenticate(accessToken!, accessTokenSecret!))
            .Throws<ArgumentException>();

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception).IsOfType(exceptionType);
        await Assert.That(exception.ParamName).IsEqualTo(paramName);
    }
}
