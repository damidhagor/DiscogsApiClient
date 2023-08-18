using System.Net.Http;
using System.Threading.RateLimiting;
using DiscogsApiClient.ApiClientGenerator;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Middleware;
using Microsoft.Extensions.Configuration;

namespace DiscogsApiClient.Tests;

[TestFixture]
public abstract class ApiBaseTestFixture
{
    private static readonly RateLimiter _rateLimiter;

    private HttpClient _authHttpClient = null!;
    private HttpClient _clientHttpClient = null!;
    protected IConfiguration Configuration = null!;

    protected IDiscogsClient ApiClient = null!;

    static ApiBaseTestFixture()
    {
        _rateLimiter = new SlidingWindowRateLimiter(
                new SlidingWindowRateLimiterOptions()
                {
                    Window = TimeSpan.FromSeconds(60),
                    SegmentsPerWindow = 12,
                    PermitLimit = 40,
                    AutoReplenishment = true,
                    QueueLimit = 1_000,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
    }

    [OneTimeSetUp]
    public void Setup()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.Development.json", true)
            .Build();

        (ApiClient, _authHttpClient, _clientHttpClient) = CreateDiscogsApiClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _authHttpClient?.Dispose();
        _clientHttpClient?.Dispose();
    }

    protected (IDiscogsClient discogsApiClient, HttpClient authHttpClient, HttpClient clientHttpClient) CreateUnauthenticatedDiscogsApiClient()
        => CreateDiscogsApiClient(userToken: "");

    protected (IDiscogsClient discogsApiClient, HttpClient authHttpClient, HttpClient clientHttpClient) CreateDiscogsApiClient(
        string? authUserAgent = null,
        string? authBaseUrl = null,
        string? clientUserAgent = null,
        string? clientBaseUrl = null,
        string? userToken = null)
    {
        authUserAgent ??= Configuration["DiscogsApiOptions:UserAgent"];
        authBaseUrl ??= Configuration["DiscogsApiOptions:BaseUrl"];
        clientUserAgent ??= Configuration["DiscogsApiOptions:UserAgent"];
        clientBaseUrl ??= Configuration["DiscogsApiOptions:BaseUrl"];
        userToken ??= Configuration["DiscogsApiOptions:UserToken"]!;

        var authHttpClient = new HttpClient() { BaseAddress = new Uri(authBaseUrl!) };
        authHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(authUserAgent);

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(authHttpClient));
        if (!string.IsNullOrWhiteSpace(userToken))
            authService.AuthenticateWithPersonalAccessToken(userToken);

        var clientHttpClient = new HttpClient(
            new RateLimitedDelegatingHandler(_rateLimiter, false)
            {
                InnerHandler = new ErrorHandlingDelegatingHandler()
                {
                    InnerHandler = new DebugMessageContentDelegatingHandler()
                    {
                        InnerHandler = new AuthenticationDelegatingHandler(authService)
                        {
                            InnerHandler = new HttpClientHandler()
                        }
                    }
                }
            })
        {
            BaseAddress = new Uri(clientBaseUrl!)
        };
        clientHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(clientUserAgent);

        var apiClientSettings = new ApiClientSettings<IDiscogsClient>()
            .AddGeneratedJsonConverters();

        var discogsApiClient = new DiscogsClient(clientHttpClient, apiClientSettings);

        return (discogsApiClient, authHttpClient, clientHttpClient);
    }
}

