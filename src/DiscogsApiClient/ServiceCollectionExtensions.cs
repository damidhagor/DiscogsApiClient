using System.Threading.RateLimiting;
using DiscogsApiClient.Authentication.PlainOAuth;
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
            throw new InvalidOperationException(ExceptionMessages.GetBaseUrlMissingMessage());
        }

        if (string.IsNullOrWhiteSpace(options.UserAgent))
        {
            throw new InvalidOperationException(ExceptionMessages.GetUserAgentMissingMessage());
        }

        services.AddTransient<AuthenticationDelegatingHandler>();
        services.AddTransient<DiscogsResponseDelegatingHandler>();

        var httpBuilder = services.AddHttpClient<IDiscogsApiClient, DiscogsApiClient>(httpClient
            =>
        {
            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
        })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>()
            .AddHttpMessageHandler<DiscogsResponseDelegatingHandler>();

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

            httpBuilder.AddHttpMessageHandler<RateLimitedDelegatingHandler>();
        }

        return services;
    }

    /// <summary>
    /// Adds the <see cref="UserTokenAuthenticationProvider"/> to the services collection.
    /// </summary>
    public static IServiceCollection AddDiscogsUserTokenAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IAuthenticationProvider, UserTokenAuthenticationProvider>();
        return services;
    }

    /// <summary>
    /// Adds the <see cref="PlainOAuthAuthenticationProvider"/> to the services collection.
    /// </summary>
    public static IServiceCollection AddDiscogsPlainOAuthAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IAuthenticationProvider, PlainOAuthAuthenticationProvider>();
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
