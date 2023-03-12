using System.Net;
using System.Text.Json;
using System.Threading.RateLimiting;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace DiscogsApiClient;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="IDiscogsApiClient"/> and an <see cref="IDiscogsAuthenticationService"/> to the services collection.
    /// <para/>
    /// NOTE: You need first to resolve the <see cref="IDiscogsAuthenticationService"/> and authenticate with the Discogs Api before using the <see cref="IDiscogsApiClient"/>.
    /// </summary>
    /// <param name="configure">Method with an options object to configure the Discogs Api client.</param>
    public static IServiceCollection AddDiscogsApiClient(this IServiceCollection services, Action<AddDiscogsApiClientOptions> configure)
    {
        var options = new AddDiscogsApiClientOptions();

        configure(options);

        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            throw new InvalidOperationException("The base url string must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.UserAgent))
        {
            throw new InvalidOperationException("The user agent string must not be empty.");
        }

        services.AddTransient<AuthenticationDelegatingHandler>();
        services.AddSingleton<IDiscogsAuthenticationService, DiscogsAuthenticationService>();
        services.AddSingleton<IPersonalAccessTokenAuthenticationProvider, PersonalAccessTokenAuthenticationProvider>();
        services.AddHttpClient<IOAuthAuthenticationProvider, OAuthAuthenticationProvider>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
        });

        var httpClientBuilder = services.AddRefitClient<IDiscogsApiClient>(
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
            })
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(options.BaseUrl);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

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

            services.AddSingleton(rateLimitingOptions);
            services.AddTransient<RateLimiter, SlidingWindowRateLimiter>();
            services.AddTransient<RateLimitedDelegatingHandler>();

            httpClientBuilder.AddHttpMessageHandler<RateLimitedDelegatingHandler>();
        }

        return services;
    }

    /// <summary>
    /// Options for initializing the library with Dependency Injection.
    /// </summary>
    public sealed class AddDiscogsApiClientOptions
    {
        /// <summary>
        /// Base url of the Discogs Api.
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.discogs.com";

        /// <summary>
        /// User-Agent header value to identify your app.
        /// </summary>
        public string UserAgent { get; set; } = "";

        /// <summary>
        /// If the <see cref="IDiscogsApiClient"/> should be rate limited.
        /// <para/>
        /// The rate limiter uses the <see href="https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-8.0#slide"> sliding window algorithm</see>.
        /// </summary>
        public bool UseRateLimiting { get; set; } = false;

        /// <summary>
        /// The length of the sliding window.
        /// </summary>
        public TimeSpan RateLimitingWindow { get; set; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// In how many segments the window is split up.
        /// </summary>
        public int RateLimitingWindowSegments { get; set; } = 12;

        /// <summary>
        /// How many permits the window allows in total.
        /// </summary>
        public int RateLimitingPermits { get; set; } = 40;

        /// <summary>
        /// How many requests will be allowed to be waiting for leases.
        /// </summary>
        public int RateLimitingQueueSize { get; set; } = 100;
    }
}
