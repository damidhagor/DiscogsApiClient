using System.Net;
using System.Text.Json;
using System.Threading.RateLimiting;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Authentication.UserToken;
using DiscogsApiClient.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace DiscogsApiClient;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="DiscogsApiClient"/> and a typed <see cref="HttpClient"/> via a <see cref="IHttpClientFactory"/> to the services collection.
    /// <para/>
    /// An implementation of <see cref="IAuthenticationProvider"/> needs to be added to the services collection as well.
    /// E.g. via the <see cref="ServiceCollectionExtensions.AddDiscogsUserTokenAuthentication(IServiceCollection)"/> or <see cref="ServiceCollectionExtensions.AddDiscogsPlainOAuthAuthentication(IServiceCollection)"/> method.
    /// </summary>
    /// <param name="configure">Method with an options object to configure the DiscogsApiClient.</param>
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
                        HttpStatusCode.Unauthorized => new UnauthorizedDiscogsException(message),
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

    public sealed class AddDiscogsApiClientOptions
    {
        public string BaseUrl { get; set; } = "https://api.discogs.com";

        public string UserAgent { get; set; } = "";

        public bool UseRateLimiting { get; set; } = false;

        public TimeSpan RateLimitingWindow { get; set; } = TimeSpan.FromSeconds(60);

        public int RateLimitingWindowSegments { get; set; } = 12;

        public int RateLimitingPermits { get; set; } = 40;

        public int RateLimitingQueueSize { get; set; } = 100;
    }
}
