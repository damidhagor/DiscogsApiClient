using System.Net;
using System.Text.Json;
using System.Threading.RateLimiting;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Middleware;

namespace DiscogsApiClient;

public static class DiscogsApiClient
{
    public static (IDiscogsApiClient discogsApiClient, IDiscogsAuthenticationService authenticationService) Create(DiscogsApiClientOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            throw new InvalidOperationException("The base url string must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.UserAgent))
        {
            throw new InvalidOperationException("The user agent string must not be empty.");
        }

        var oauthHttpClient = new HttpClient();
        oauthHttpClient.BaseAddress = new Uri(options.BaseUrl);
        oauthHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);

        var authenticationService = new DiscogsAuthenticationService(
            new PersonalAccessTokenAuthenticationProvider(),
            new OAuthAuthenticationProvider(oauthHttpClient));

        DelegatingHandler delegatingHandler = new AuthenticationDelegatingHandler(authenticationService)
        {
            InnerHandler = new HttpClientHandler()
        };

        if (options.UseRateLimiting)
        {
            var rateLimitingOptions = new SlidingWindowRateLimiterOptions()
            {
                Window = options.RateLimitingWindow,
                SegmentsPerWindow = options.RateLimitingWindowSegments,
                PermitLimit = options.RateLimitingPermits,
                QueueLimit = options.RateLimitingQueueSize,
                AutoReplenishment = true,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            };

            var rateLimiter = new SlidingWindowRateLimiter(rateLimitingOptions);
            var rateLimitedDelegatingHandler = new RateLimitedDelegatingHandler(rateLimiter);
            rateLimitedDelegatingHandler.InnerHandler = delegatingHandler;
            delegatingHandler = rateLimitedDelegatingHandler;
        }

        var discogsHttpClient = new HttpClient(delegatingHandler);
        discogsHttpClient.BaseAddress = new Uri(options.BaseUrl);
        discogsHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);

        var discogsApiClient = RestService.For<IDiscogsApiClient>(
            discogsHttpClient,
            new RefitSettings
            {
                ExceptionFactory = async (response) =>
                {
                    if (response.IsSuccessStatusCode)
                        return null;

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
                        HttpStatusCode.Forbidden => new UnauthenticatedDiscogsException(message),
                        HttpStatusCode.NotFound => new ResourceNotFoundDiscogsException(message),
                        HttpStatusCode.TooManyRequests => new RateLimitExceededDiscogsException(message),
                        _ => new DiscogsException(message),
                    };
                }
            });

        return (discogsApiClient, authenticationService);
    }
}
