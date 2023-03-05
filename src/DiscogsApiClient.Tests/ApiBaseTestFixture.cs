using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using DiscogsApiClient.Authentication;
using DiscogsApiClient.Authentication.UserToken;
using DiscogsApiClient.Contract;
using DiscogsApiClient.Exceptions;
using DiscogsApiClient.Middleware;
using DiscogsApiClient.Serialization;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Refit;

namespace DiscogsApiClient.Tests;

[TestFixture]
public abstract class ApiBaseTestFixture
{
    private static readonly IAuthenticationProvider _authenticationProvider;
    private static readonly HttpClient _httpClient;

    protected IDiscogsApiClient ApiClient;
    protected IConfiguration Configuration;

    static ApiBaseTestFixture()
    {
        _authenticationProvider = new UserTokenAuthenticationProvider();
        _httpClient = new(
            new RateLimitedDelegatingHandler(
                new SlidingWindowRateLimiter(
                    new SlidingWindowRateLimiterOptions()
                    {
                        Window = TimeSpan.FromSeconds(60),
                        SegmentsPerWindow = 12,
                        PermitLimit = 40,
                        AutoReplenishment = true,
                        QueueLimit = 1_000,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }))
            {
                InnerHandler = new AuthenticationDelegatingHandler(_authenticationProvider)
                {
                    InnerHandler = new HttpClientHandler()
                }
            })
        {
            BaseAddress = new Uri("https://api.discogs.com")
        };
    }

    public ApiBaseTestFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.Development.json", true)
            .Build();

        var userAgent = Configuration["DiscogsApiOptions:UserAgent"];

        if (_httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

        ApiClient = RestService.For<IDiscogsApiClient>(
            _httpClient,
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(DiscogsSerializerOptions.Options),
                ExceptionFactory = async (response) =>
                {
                    if (response.IsSuccessStatusCode)
                    {
                        if (Debugger.IsAttached)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            ;
                        }
                        return null;
                    }

                    string? message = null;
                    try
                    {
                        var errorMessage = await response.Content.DeserializeAsJsonAsync<ErrorMessage>(default);
                        message = errorMessage.Message;
                    }
                    catch { }

                    return response.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => new UnauthorizedDiscogsException(message),
                        HttpStatusCode.NotFound => new ResourceNotFoundDiscogsException(message),
                        _ => new DiscogsException(message),
                    };
                }
            });
    }


    [OneTimeSetUp]
    public virtual async Task Initialize()
    {
        var userToken = Configuration["DiscogsApiOptions:UserToken"]!;
        var authenticationRequest = new UserTokenAuthenticationRequest(userToken);
        await _authenticationProvider.AuthenticateAsync(authenticationRequest, default);
        await ApiClient.AuthenticateAsync(authenticationRequest, default);
    }
}
