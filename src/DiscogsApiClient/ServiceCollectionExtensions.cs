using System.Text.Json;
using DiscogsApiClient;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.PersonalAccessToken;
using DiscogsApiClient.Middleware;
using DiscogsApiClient.RateLimiting;
using DiscogsApiClient.SourceGenerator.JsonSerialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="IDiscogsApiClient"/> and an <see cref="IDiscogsAuthenticationService"/> to the services collection.
    /// <para/>
    /// NOTE: You need first to resolve the <see cref="IDiscogsAuthenticationService"/> and authenticate with the Discogs Api before using the <see cref="IDiscogsApiClient"/>.
    /// </summary>
    /// <param name="services">The service collection to add the client to.</param>
    /// <param name="configureOptions">Delegate configuring the <see cref="DiscogsApiClientOptions"/>.</param>
    /// <param name="configureClient">Optional hook to customize the underlying <see cref="IHttpClientBuilder"/>. Handlers added here sit outermost in the pipeline (they wrap the client's error, authentication and rate-limit handlers).</param>
    /// <remarks>
    /// The <see cref="DiscogsApiClientOptions.SectionName"/> (<c>"Discogs"</c>) configuration section is bound
    /// first (if an <see cref="IConfiguration"/> is registered), then <paramref name="configureOptions"/> is applied
    /// on top so code-based configuration overrides configuration values.
    /// </remarks>
    public static IServiceCollection AddDiscogsApiClient(
        this IServiceCollection services,
        Action<DiscogsApiClientOptions> configureOptions,
        Action<IHttpClientBuilder>? configureClient = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.AddOptions<DiscogsApiClientOptions>()
            .Configure<IServiceProvider>(static (options, serviceProvider) => serviceProvider.GetService<IConfiguration>()?
                .GetSection(DiscogsApiClientOptions.SectionName)
                .Bind(options))
            .Configure(configureOptions)
            .ValidateOnStart();

        return services.AddDiscogsApiClientCore(configureClient);
    }

    /// <summary>
    /// Adds the <see cref="IDiscogsApiClient"/> and an <see cref="IDiscogsAuthenticationService"/> to the services collection.
    /// <para/>
    /// NOTE: You need first to resolve the <see cref="IDiscogsAuthenticationService"/> and authenticate with the Discogs Api before using the <see cref="IDiscogsApiClient"/>.
    /// </summary>
    /// <param name="services">The service collection to add the client to.</param>
    /// <param name="configureOptions">Delegate configuring the <see cref="DiscogsApiClientOptions"/> with access to the <see cref="IServiceProvider"/>.</param>
    /// <param name="configureClient">Optional hook to customize the underlying <see cref="IHttpClientBuilder"/>. Handlers added here sit outermost in the pipeline (they wrap the client's error, authentication and rate-limit handlers).</param>
    /// <remarks>
    /// The <see cref="DiscogsApiClientOptions.SectionName"/> (<c>"Discogs"</c>) configuration section is bound
    /// first (if an <see cref="IConfiguration"/> is registered), then <paramref name="configureOptions"/> is applied
    /// on top so code-based configuration overrides configuration values.
    /// </remarks>
    public static IServiceCollection AddDiscogsApiClient(
        this IServiceCollection services,
        Action<IServiceProvider, DiscogsApiClientOptions> configureOptions,
        Action<IHttpClientBuilder>? configureClient = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.AddOptions<DiscogsApiClientOptions>()
            .Configure<IServiceProvider>(static (options, serviceProvider) => serviceProvider.GetService<IConfiguration>()?
                .GetSection(DiscogsApiClientOptions.SectionName)
                .Bind(options))
            .Configure<IServiceProvider>((options, serviceProvider) => configureOptions(serviceProvider, options))
            .ValidateOnStart();

        return services.AddDiscogsApiClientCore(configureClient);
    }

    /// <summary>
    /// Adds the <see cref="IDiscogsApiClient"/> and an <see cref="IDiscogsAuthenticationService"/> to the services collection,
    /// binding the <see cref="DiscogsApiClientOptions"/> from the supplied <paramref name="configuration"/>.
    /// <para/>
    /// NOTE: You need first to resolve the <see cref="IDiscogsAuthenticationService"/> and authenticate with the Discogs Api before using the <see cref="IDiscogsApiClient"/>.
    /// </summary>
    /// <param name="services">The service collection to add the client to.</param>
    /// <param name="configuration">The configuration to bind. Pass the section to bind directly, e.g. <c>configuration.GetSection(DiscogsApiClientOptions.SectionName)</c>.</param>
    /// <param name="configureClient">Optional hook to customize the underlying <see cref="IHttpClientBuilder"/>. Handlers added here sit outermost in the pipeline (they wrap the client's error, authentication and rate-limit handlers).</param>
    public static IServiceCollection AddDiscogsApiClient(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IHttpClientBuilder>? configureClient = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddOptions<DiscogsApiClientOptions>()
            .Bind(configuration)
            .ValidateOnStart();

        return services.AddDiscogsApiClientCore(configureClient);
    }

    private static IServiceCollection AddDiscogsApiClientCore(this IServiceCollection services, Action<IHttpClientBuilder>? configureClient)
    {
        services.AddSingleton<IValidateOptions<DiscogsApiClientOptions>, DiscogsApiClientOptionsValidator>();

        services.TryAddTransient<AuthenticationDelegatingHandler>();
        services.TryAddTransient<ErrorHandlingDelegatingHandler>();
        services.TryAddTransient<RateLimitStateDelegatingHandler>();
        services.TryAddSingleton<IDiscogsAuthenticationService, DiscogsAuthenticationService>();
        services.TryAddSingleton<IPersonalAccessTokenAuthenticationProvider, PersonalAccessTokenAuthenticationProvider>();

        services.TryAddSingleton<DiscogsRateLimitStateService>();
        services.TryAddSingleton<IDiscogsRateLimitStateService>(static sp => sp.GetRequiredService<DiscogsRateLimitStateService>());
        services.TryAddSingleton<IDiscogsRateLimitStateUpdateService>(static sp => sp.GetRequiredService<DiscogsRateLimitStateService>());

        services.TryAddSingleton(static _ => new DiscogsJsonSerializerContext(
            new JsonSerializerOptions().AddGeneratedEnumJsonConverters()));

        services.AddHttpClient<IOAuthAuthenticationProvider, OAuthAuthenticationProvider>()
            .ConfigureHttpClient(static (serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value;
                httpClient.BaseAddress = new(options.BaseUrl);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
            })
            .AddHttpMessageHandler<ErrorHandlingDelegatingHandler>();

        var clientBuilder = services.AddHttpClient<IDiscogsApiClient, global::DiscogsApiClient.DiscogsApiClient>()
            .ConfigureHttpClient(static (serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value;
                httpClient.BaseAddress = new(options.BaseUrl);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
            });

        configureClient?.Invoke(clientBuilder);

        clientBuilder
            .AddHttpMessageHandler<ErrorHandlingDelegatingHandler>()
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>()
            .AddHttpMessageHandler<RateLimitStateDelegatingHandler>();

        return services;
    }
}
