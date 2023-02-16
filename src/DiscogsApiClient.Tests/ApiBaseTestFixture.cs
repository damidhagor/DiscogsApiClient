using System;
using System.Net.Http;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using DiscogsApiClient.Authentication.UserToken;
using DiscogsApiClient.RateLimiting;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DiscogsApiClient.Tests;

[TestFixture]
public abstract class ApiBaseTestFixture
{
    protected DiscogsApiClient ApiClient;
    protected IConfiguration Configuration;

    private static HttpClient _httpClient = new(new RateLimitedDelegatingHandler(new SlidingWindowRateLimiter(
                    new SlidingWindowRateLimiterOptions()
                    {
                        Window = TimeSpan.FromSeconds(60),
                        SegmentsPerWindow = 12,
                        PermitLimit = 40,
                        AutoReplenishment = true,
                        QueueLimit = 1_000,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }))
    { InnerHandler = new HttpClientHandler() });

    public ApiBaseTestFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.Development.json", true)
            .Build();

        var userAgent = Configuration["DiscogsApiOptions:UserAgent"];

        if (_httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

        var authenticationProvider = new UserTokenAuthenticationProvider();
        ApiClient = new DiscogsApiClient(_httpClient, authenticationProvider);
    }


    [OneTimeSetUp]
    public virtual async Task Initialize()
    {
        var userToken = Configuration["DiscogsApiOptions:UserToken"];
        var authenticationRequest = new UserTokenAuthenticationRequest(userToken);
        await ApiClient.AuthenticateAsync(authenticationRequest, default);
    }

    [OneTimeTearDown]
    public virtual void Cleanup()
    {
        //_httpClient.Dispose();
    }
}