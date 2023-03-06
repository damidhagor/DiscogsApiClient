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

        var userAgent = _configuration["DiscogsApiOptions:UserAgent"];
        var baseUrl = _configuration["DiscogsApiOptions:BaseUrl"];
        var userToken = _configuration["DiscogsApiOptions:UserToken"]!;

        _authHttpClient = new HttpClient() { BaseAddress = new Uri(baseUrl!) };
        _authHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

        var authService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(_authHttpClient));
        authService.AuthenticateWithPersonalAccessToken(userToken);

        _clientHttpClient = new HttpClient(
            new RateLimitedDelegatingHandler(_rateLimiter, false)
            {
                InnerHandler = new AuthenticationDelegatingHandler(authService)
                {
                    InnerHandler = new HttpClientHandler()
                }
            })
        {
            BaseAddress = new Uri(baseUrl!)
        };
        _clientHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

        ApiClient = RestService.For<IDiscogsApiClient>(
            _clientHttpClient,
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
                        HttpStatusCode.Unauthorized => new UnauthorizedDiscogsException(message),
                        HttpStatusCode.NotFound => new ResourceNotFoundDiscogsException(message),
                        HttpStatusCode.TooManyRequests => new RateLimitExceededDiscogsException(message),
                        _ => new DiscogsException(message),
                    };
                }
            });
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _authHttpClient?.Dispose();
        _clientHttpClient?.Dispose();
    }
}
