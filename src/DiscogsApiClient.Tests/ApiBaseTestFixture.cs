using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.RateLimiting;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.UserToken;
using DiscogsApiClient.Middleware;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Refit;

namespace DiscogsApiClient.Tests;

[TestFixture]
public abstract class ApiBaseTestFixture
{
    private static readonly RateLimiter _rateLimiter;

    private HttpClient _authHttpClient = null!;
    private HttpClient _clientHttpClient = null!;
    private IConfiguration _configuration = null!;

    protected IDiscogsApiClient ApiClient = null!;

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
        _configuration = new ConfigurationBuilder()
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

    protected (IDiscogsApiClient discogsApiClient, HttpClient authHttpClient, HttpClient clientHttpClient) CreateUnauthenticatedDiscogsApiClient()
        => CreateDiscogsApiClient(userToken: "");

    protected (IDiscogsApiClient discogsApiClient, HttpClient authHttpClient, HttpClient clientHttpClient) CreateDiscogsApiClient(
        string? authUserAgent = null,
        string? authBaseUrl = null,
        string? clientUserAgent = null,
        string? clientBaseUrl = null,
        string? userToken = null)
    {
        authUserAgent ??= _configuration["DiscogsApiOptions:UserAgent"];
        authBaseUrl ??= _configuration["DiscogsApiOptions:BaseUrl"];
        clientUserAgent ??= _configuration["DiscogsApiOptions:UserAgent"];
        clientBaseUrl ??= _configuration["DiscogsApiOptions:BaseUrl"];
        userToken ??= _configuration["DiscogsApiOptions:UserToken"]!;

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
                InnerHandler = new AuthenticationDelegatingHandler(authService)
                {
                    InnerHandler = new HttpClientHandler()
                }
            })
        {
            BaseAddress = new Uri(clientBaseUrl!)
        };
        clientHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(clientUserAgent);

        var discogsApiClient = RestService.For<IDiscogsApiClient>(
            clientHttpClient,
            new RefitSettings
            {
                ExceptionFactory = async (response) =>
                {
                    if (response.IsSuccessStatusCode)
                    {
                        if (Debugger.IsAttached)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            ;
                        }

                        return null;
                    }

                    string? message = null;
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        message = JsonSerializer.Deserialize<ErrorMessage>(content)?.Message;
                    }
                    catch { }

                    return response.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => new UnauthenticatedDiscogsException(message),
                        HttpStatusCode.NotFound => new ResourceNotFoundDiscogsException(message),
                        HttpStatusCode.TooManyRequests => new RateLimitExceededDiscogsException(message),
                        _ => new DiscogsException(message),
                    };
                }
            });

        return (discogsApiClient, authHttpClient, clientHttpClient);
    }
}
