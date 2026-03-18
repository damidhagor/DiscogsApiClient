using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Tests.MockMiddleware;

namespace DiscogsApiClient.Tests.Authentication;

public sealed class DiscogsAuthenticationServiceTestFixture
{
    [Test]
    public async Task Unauthenticated_Service_Throws_UnauthorizedException()
    {
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, null!));

        await Assert.That(authService.IsAuthenticated).IsFalse();
        await Assert.That(() => authService.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task PersonalAccessTokenAuthentication_Successful()
    {
        var token = "myusertoken";
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, null!));

        await Assert.That(authService.IsAuthenticated).IsFalse();

        authService.AuthenticateWithPersonalAccessToken(token);

        await Assert.That(authService.IsAuthenticated).IsTrue();
        await Assert.That(authService.CreateAuthenticationHeader()).IsEqualTo($"Discogs token={token}");
    }

    [Test]
    [Arguments(null!)]
    [Arguments("")]
    [Arguments("   ")]
    public async Task PersonalAccessToken_Guard_Works(string? tokene)
    {
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, null!));

        await Assert.That(() => authService.AuthenticateWithPersonalAccessToken(""))
            .Throws<ArgumentException>();

        await Assert.That(authService.IsAuthenticated).IsFalse();

        await Assert.That(() => authService.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task Failed_Only_PersonalAccessToken_Resets_IsAuthenticated()
    {
        var token = "myusertoken";
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, null!));

        await Assert.That(authService.IsAuthenticated).IsFalse();

        authService.AuthenticateWithPersonalAccessToken(token);

        await Assert.That(authService.IsAuthenticated).IsTrue();
        await Assert.That(authService.CreateAuthenticationHeader()).IsEqualTo($"Discogs token={token}");

        await Assert.That(() => authService.AuthenticateWithPersonalAccessToken("")).Throws<ArgumentException>();
        await Assert.That(authService.IsAuthenticated).IsFalse();
        await Assert.That(() => authService.CreateAuthenticationHeader()).Throws<UnauthenticatedDiscogsException>();
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

        await Assert.That(authService.IsAuthenticated).IsFalse();

        var session = await authService.StartOAuthAuthentication(default);

        await Assert.That(authService.IsAuthenticated).IsFalse();
        await Assert.That(session.AuthorizeUrl).IsEqualTo("https://discogs.com/oauth/authorize?oauth_token=requesttoken");
        await Assert.That(session.VerifierCallbackUrl).IsEqualTo("http://localhost/access_token");
        await Assert.That(session.RequestToken).IsEqualTo("requesttoken");
        await Assert.That(session.RequestTokenSecret).IsEqualTo("requesttokensecret");

        var (accessToken, accessTokenSecret) = await authService.CompleteOAuthAuthentication(session, "verifier", default);

        var authHeader = authService.CreateAuthenticationHeader();

        await Assert.That(authService.IsAuthenticated).IsTrue();
        await Assert.That(accessToken).IsEqualTo("accesstoken");
        await Assert.That(accessTokenSecret).IsEqualTo("accesstokensecret");
        await Assert.That(authHeader).Contains("oauth_consumer_key=\"key\"");
        await Assert.That(authHeader).Contains("oauth_token=\"accesstoken\"");
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
    public async Task OAuthAuthentication_Start_Guards_Work(
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

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        var exception = await Assert.That(async () => await authService.StartOAuthAuthentication(cancellationToken))
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
    public async Task OAuthAuthentication_Start_Unauthenticated(string? requestToken, string? requestTokenSecret, CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { RequestToken = requestToken!, RequestTokenSecret = requestTokenSecret! };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret", VerifierCallbackUrl = "callback" };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        await Assert.That(async () => await authService.StartOAuthAuthentication(cancellationToken))
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
    public async Task OAuthAuthentication_Complete_Guards_Work(
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

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        var exception = await Assert.That(async () => await authService.CompleteOAuthAuthentication(session, verifierToken!, cancellationToken))
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
    public async Task OAuthAuthentication_Complete_Unauthenticated(string? accessToken, string? accessTokenSecret, CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler { AccessToken = accessToken!, AccessTokenSecret = accessTokenSecret! };
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        await Assert.That(async () => await authService.CompleteOAuthAuthentication(session, "verifier", cancellationToken))
            .Throws<AuthenticationFailedDiscogsException>();
    }

    [Test]
    public async Task OAuthAuthentication_Short_Circuit_Successful()
    {
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, options));

        await Assert.That(authService.IsAuthenticated).IsFalse();

        authService.AuthenticateWithOAuth("testtoken", "testsecret");
        var authHeader = authService.CreateAuthenticationHeader();

        await Assert.That(authService.IsAuthenticated).IsTrue();
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
    public async Task OAuthAuthentication_Short_Circuit_Guards_Work(
        string? accessToken,
        string? accessTokenSecret,
        Type exceptionType,
        string paramName)
    {
        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(null!, null!));

        var exception = await Assert.That(() => authService.AuthenticateWithOAuth(accessToken!, accessTokenSecret!))
            .Throws<ArgumentException>();

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception).IsOfType(exceptionType);
        await Assert.That(exception.ParamName).IsEqualTo(paramName);
    }

    [Test]
    public async Task Failed_OAuth_NotResets_IsAuthenticated(CancellationToken cancellationToken)
    {
        var oauthMockHandler = new OAuthMockDelegatingHandler();
        var httpClient = new HttpClient(oauthMockHandler) { BaseAddress = new Uri("http://mock.discogs.com") };
        var options = new DiscogsApiClientOptions { ConsumerKey = "key", ConsumerSecret = "secret" };
        var session = new OAuthAuthenticationSession("", "", "requesttoken", "requesttokensecret");

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(httpClient, options));

        await Assert.That(authService.IsAuthenticated).IsFalse();

        var (accessToken, accessTokenSecret) = await authService.CompleteOAuthAuthentication(session, "verifierToken", cancellationToken);
        var authHeader = authService.CreateAuthenticationHeader();

        await Assert.That(authService.IsAuthenticated).IsTrue();
        await Assert.That(accessToken).IsEqualTo("accesstoken");
        await Assert.That(accessTokenSecret).IsEqualTo("accesstokensecret");
        await Assert.That(authHeader).Contains("oauth_consumer_key=\"key\"");
        await Assert.That(authHeader).Contains("oauth_token=\"accesstoken\"");

        oauthMockHandler.AccessToken = "";
        oauthMockHandler.AccessTokenSecret = "";

        await Assert.That(async () => await authService.CompleteOAuthAuthentication(session, "verifierToken", cancellationToken))
            .Throws<AuthenticationFailedDiscogsException>();
        await Assert.That(authService.IsAuthenticated).IsTrue();
        await Assert.That(() => authService.CreateAuthenticationHeader()).ThrowsNothing();
    }

    [Test]
    public async Task Reauthentication_With_Different_Method_Switches_Method(CancellationToken cancellationToken)
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

        await Assert.That(authService.IsAuthenticated).IsFalse();

        // Only personal access token
        authService.AuthenticateWithPersonalAccessToken(userToken);

        await Assert.That(authService.IsAuthenticated).IsTrue();
        await Assert.That(authService.CreateAuthenticationHeader()).IsEqualTo($"Discogs token={userToken}");

        // OAuth replaces personal access token
        var (accessToken, accessTokenSecret) = await authService.CompleteOAuthAuthentication(session, verifierToken, cancellationToken);
        var authHeader = authService.CreateAuthenticationHeader();

        await Assert.That(authService.IsAuthenticated).IsTrue();
        await Assert.That(accessToken).IsEqualTo("accesstoken");
        await Assert.That(accessTokenSecret).IsEqualTo("accesstokensecret");
        await Assert.That(authHeader).Contains("oauth_consumer_key=\"key\"");
        await Assert.That(authHeader).Contains("oauth_token=\"accesstoken\"");

        // Personal access token replaces OAuth
        authService.AuthenticateWithPersonalAccessToken(userToken);

        await Assert.That(authService.IsAuthenticated).IsTrue();
        await Assert.That(authService.CreateAuthenticationHeader()).IsEqualTo($"Discogs token={userToken}");
    }
}
