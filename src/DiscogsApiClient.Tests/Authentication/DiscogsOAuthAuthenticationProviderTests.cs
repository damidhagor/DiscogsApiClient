using DiscogsApiClient.Authentication.OAuth;
using Microsoft.Extensions.Options;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class DiscogsOAuthAuthenticationProviderTests
{
    [Test]
    public async Task Authenticate_ShouldAuthenticate_WhenOAuthFlowCompletes(CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsOAuthOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "http://localhost/access_token" };

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(httpClient), Options.Create(options));

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
    public async Task CreateAuthenticationHeader_ShouldThrowUnauthenticatedDiscogsException_WhenNotAuthenticated()
    {
        var httpClient = new HttpClient(new OAuthMockDelegatingHandler());
        var options = new DiscogsOAuthOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(httpClient), Options.Create(options));

        await Assert.That(authProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => authProvider.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("  ")]
    public async Task StartAuthentication_ShouldThrowInvalidOperationException_WhenVerifierCallbackUrlIsInvalid(string? verifierCallbackUrl, CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsOAuthOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = verifierCallbackUrl };

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(httpClient), Options.Create(options));

        await Assert.That(async () => await authProvider.StartAuthentication(cancellationToken))
            .Throws<InvalidOperationException>();
    }

    [Test]
    [Arguments(null, "", "x", typeof(ArgumentNullException), "_oAuthOptions.ConsumerKey")]
    [Arguments("", "", "x", typeof(ArgumentException), "_oAuthOptions.ConsumerKey")]
    [Arguments("  ", "", "x", typeof(ArgumentException), "_oAuthOptions.ConsumerKey")]
    [Arguments("x", null, "x", typeof(ArgumentNullException), "_oAuthOptions.ConsumerSecret")]
    [Arguments("x", "", "x", typeof(ArgumentException), "_oAuthOptions.ConsumerSecret")]
    [Arguments("x", "  ", "x", typeof(ArgumentException), "_oAuthOptions.ConsumerSecret")]
    public async Task StartAuthentication_ShouldThrowException_WhenParametersAreInvalid(
        string? consumerKey,
        string? consumerSecret,
        string? verifierCallbackUrl,
        Type exceptionType,
        string paramName,
        CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsOAuthOptions { ConsumerKey = consumerKey!, ConsumerSecret = consumerSecret!, VerifierCallbackUrl = verifierCallbackUrl };

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(httpClient), Options.Create(options));

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
    public async Task StartAuthentication_ShouldThrowAuthenticationFailedDiscogsException_WhenApiFails(string? requestToken, string? requestTokenSecret, CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { RequestToken = requestToken!, RequestTokenSecret = requestTokenSecret! };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsOAuthOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "callback" };

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(httpClient), Options.Create(options));

        await Assert.That(async () => await authProvider.StartAuthentication(cancellationToken))
            .Throws<AuthenticationFailedDiscogsException>();
    }

    [Test]
    [Arguments("", "", null, "", "", typeof(ArgumentNullException), "session.RequestToken")]
    [Arguments("", "", "", "", "", typeof(ArgumentException), "session.RequestToken")]
    [Arguments("", "", "  ", "", "", typeof(ArgumentException), "session.RequestToken")]
    [Arguments("", "", "x", null, "", typeof(ArgumentNullException), "session.RequestTokenSecret")]
    [Arguments("", "", "x", "", "", typeof(ArgumentException), "session.RequestTokenSecret")]
    [Arguments("", "", "x", "  ", "", typeof(ArgumentException), "session.RequestTokenSecret")]
    [Arguments("", "", "x", "x", null, typeof(ArgumentNullException), "verifierToken")]
    [Arguments("", "", "x", "x", "", typeof(ArgumentException), "verifierToken")]
    [Arguments("", "", "x", "x", "  ", typeof(ArgumentException), "verifierToken")]
    [Arguments(null, "", "x", "x", "x", typeof(ArgumentNullException), "_oAuthOptions.ConsumerKey")]
    [Arguments("", "", "x", "x", "x", typeof(ArgumentException), "_oAuthOptions.ConsumerKey")]
    [Arguments("  ", "", "x", "x", "x", typeof(ArgumentException), "_oAuthOptions.ConsumerKey")]
    [Arguments("x", null, "x", "x", "x", typeof(ArgumentNullException), "_oAuthOptions.ConsumerSecret")]
    [Arguments("x", "", "x", "x", "x", typeof(ArgumentException), "_oAuthOptions.ConsumerSecret")]
    [Arguments("x", "  ", "x", "x", "x", typeof(ArgumentException), "_oAuthOptions.ConsumerSecret")]
    public async Task CompleteAuthentication_ShouldThrowException_WhenParametersAreInvalid(
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
        var options = new DiscogsOAuthOptions { ConsumerKey = consumerKey!, ConsumerSecret = consumerSecret! };
        var session = new OAuthAuthenticationSession("", "", requestToken!, requestTokenSecret!);

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(httpClient), Options.Create(options));

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
    public async Task CompleteAuthentication_ShouldThrowAuthenticationFailedDiscogsException_WhenApiFails(string? accessToken, string? accessTokenSecret, CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { AccessToken = accessToken!, AccessTokenSecret = accessTokenSecret! };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsOAuthOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(httpClient), Options.Create(options));

        await Assert.That(async () => await authProvider.CompleteAuthentication(session, "verifier", cancellationToken))
            .Throws<AuthenticationFailedDiscogsException>();
    }

    [Test]
    public async Task CompleteAuthentication_ShouldNotResetAuthenticationState_WhenReauthenticationFails(CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsOAuthOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(httpClient), Options.Create(options));

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
    public async Task Authenticate_ShouldAuthenticate_WhenUsingShortCircuitOAuth()
    {
        var options = new DiscogsOAuthOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(null!), Options.Create(options));

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
    public async Task Authenticate_ShouldThrowException_WhenShortCircuitParametersAreInvalid(
        string? accessToken,
        string? accessTokenSecret,
        Type exceptionType,
        string paramName)
    {
        var authProvider = new DiscogsOAuthAuthenticationProvider(new FakeHttpClientFactory(null!), Options.Create<DiscogsOAuthOptions>(null!));

        var exception = await Assert.That(() => authProvider.Authenticate(accessToken!, accessTokenSecret!))
            .Throws<ArgumentException>();

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception).IsOfType(exceptionType);
        await Assert.That(exception.ParamName).IsEqualTo(paramName);
    }
}
