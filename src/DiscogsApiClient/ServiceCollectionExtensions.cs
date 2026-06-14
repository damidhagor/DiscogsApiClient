using System.Text.Json;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Middleware;
using DiscogsApiClient.RateLimiting;
using DiscogsApiClient.SourceGenerator.ApiClient;
using DiscogsApiClient.SourceGenerator.JsonSerialization;
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
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

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
        services.AddTransient<ErrorHandlingDelegatingHandler>();
        services.AddSingleton<IDiscogsAuthenticationService, DiscogsAuthenticationService>();
        services.AddSingleton<IPersonalAccessTokenAuthenticationProvider, PersonalAccessTokenAuthenticationProvider>();

        var rateLimitStateService = new DiscogsRateLimitStateService();
        services.AddSingleton<IDiscogsRateLimitStateService>(rateLimitStateService);
        services.AddSingleton<IDiscogsRateLimitStateUpdateService>(rateLimitStateService);
        services.AddTransient<RateLimitStateDelegatingHandler>();

        services.AddHttpClient<IOAuthAuthenticationProvider, OAuthAuthenticationProvider>()
            .ConfigureHttpClient((serviceProvider, httpClient) =>
             {
                 var options = serviceProvider.GetRequiredService<DiscogsApiClientOptions>();
                 httpClient.BaseAddress = new(options.BaseUrl);
                 httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
             })
            .AddHttpMessageHandler<ErrorHandlingDelegatingHandler>();


        var apiClientSettings = new ApiClientSettings<IDiscogsApiClient, DiscogsJsonSerializerContext>(
            new(new JsonSerializerOptions().AddGeneratedEnumJsonConverters()));
        services.AddSingleton(apiClientSettings);

        services.AddHttpClient<IDiscogsApiClient, Generated.DiscogsApiClient>()
            .ConfigureHttpClient((serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<DiscogsApiClientOptions>();
                httpClient.BaseAddress = new(options.BaseUrl);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
            })
            .AddHttpMessageHandler<RateLimitStateDelegatingHandler>()
            .AddHttpMessageHandler<ErrorHandlingDelegatingHandler>()
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        return services;
    }
}
