using System.Net;
using System.Threading;
using System.Threading.RateLimiting;
using DiscogsApiClient.Authentication.PlainOAuth;
using DiscogsApiClient.Authentication.UserToken;
using DiscogsApiClient.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Refit;

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

        var httpClientBuilder = services.AddRefitClient<IDiscogsApiClient>(
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(DiscogsSerializerOptions.Options),
                ExceptionFactory = async (response) =>
                {
                    if (response.IsSuccessStatusCode)
                        return null;

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
