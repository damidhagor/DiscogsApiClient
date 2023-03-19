using System;
using System.Net;
using System.Text.Json;
using System.Threading.RateLimiting;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace DiscogsApiClient;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="IDiscogsApiClient"/> and an <see cref="IDiscogsAuthenticationService"/> to the services collection.
    /// <para/>
    /// NOTE: You need first to resolve the <see cref="IDiscogsAuthenticationService"/> and authenticate with the Discogs Api before using the <see cref="IDiscogsApiClient"/>.
    /// </summary>
    /// <param name="configure">Method with an options object to configure the Discogs Api client.</param>
    public static IServiceCollection AddDiscogsApiClient(this IServiceCollection services, Action<DiscogsApiClientOptions> configure)
    {
        var discogsOptions = new DiscogsApiClientOptions();

        configure(discogsOptions);

        if (string.IsNullOrWhiteSpace(discogsOptions.BaseUrl))
        {
            throw new InvalidOperationException("The base url string must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(discogsOptions.UserAgent))
        {
            throw new InvalidOperationException("The user agent string must not be empty.");
        }

        services.AddSingleton(discogsOptions);
        services.AddTransient<AuthenticationDelegatingHandler>();
        services.AddSingleton<IDiscogsAuthenticationService, DiscogsAuthenticationService>();
        services.AddSingleton<IPersonalAccessTokenAuthenticationProvider, PersonalAccessTokenAuthenticationProvider>();

        services.AddHttpClient<IOAuthAuthenticationProvider, OAuthAuthenticationProvider>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<DiscogsApiClientOptions>();
            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
        });

        var httpClientBuilder = services.AddRefitClient<IDiscogsApiClient>(
            new RefitSettings
            {
                ExceptionFactory = HandleDiscogsHttpResponseMessage
            })
            .ConfigureHttpClient((serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<DiscogsApiClientOptions>();
                httpClient.BaseAddress = new Uri(options.BaseUrl);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        if (discogsOptions.UseRateLimiting)
        {
            var rateLimitingOptions = new SlidingWindowRateLimiterOptions()
            {
                Window = discogsOptions.RateLimitingWindow,
                SegmentsPerWindow = discogsOptions.RateLimitingWindowSegments,
                PermitLimit = discogsOptions.RateLimitingPermits,
                QueueLimit = discogsOptions.RateLimitingQueueSize,
                AutoReplenishment = true,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            };

            services.AddSingleton(rateLimitingOptions);
            services.AddSingleton<RateLimiter, SlidingWindowRateLimiter>();
            services.AddTransient<RateLimitedDelegatingHandler>();

            httpClientBuilder.AddHttpMessageHandler<RateLimitedDelegatingHandler>();
        }

        return services;
    }

    internal static async Task<Exception?> HandleDiscogsHttpResponseMessage(HttpResponseMessage response)
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
}
