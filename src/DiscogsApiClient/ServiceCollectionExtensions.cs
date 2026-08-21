using System.Text.Json;
using DiscogsApiClient;
using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.Pat;
using DiscogsApiClient.Middleware;
using DiscogsApiClient.RateLimiting;
using DiscogsApiClient.SourceGenerator.JsonSerialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

#pragma warning disable IDE0130 // Namespace does not match folder structure: conventional namespace for IServiceCollection extensions
namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="IDiscogsApiClient"/> to the services collection, unauthenticated by default.
    /// <para/>
    /// Chain <see cref="WithPatAuthentication"/> or <see cref="WithOAuthAuthentication"/> to opt into an
    /// authentication mechanism. Without either, requests are sent without an <c>Authorization</c> header.
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

        var ciWarningTestProbe = 42; // TEMP: intentional CS0219 to validate CI build-warning reporting

        services.AddOptions<DiscogsApiClientOptions>()
            .Configure<IServiceProvider>(static (options, serviceProvider) => serviceProvider.GetService<IConfiguration>()?
                .GetSection(DiscogsApiClientOptions.SectionName)
                .Bind(options))
            .Configure(configureOptions)
            .ValidateOnStart();

        return services.AddDiscogsApiClientCore(configureClient);
    }

    /// <summary>
    /// Adds the <see cref="IDiscogsApiClient"/> to the services collection, unauthenticated by default.
    /// <para/>
    /// Chain <see cref="WithPatAuthentication"/> or <see cref="WithOAuthAuthentication"/> to opt into an
    /// authentication mechanism. Without either, requests are sent without an <c>Authorization</c> header.
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
    /// Adds the <see cref="IDiscogsApiClient"/> to the services collection, unauthenticated by default,
    /// binding the <see cref="DiscogsApiClientOptions"/> from the supplied <paramref name="configuration"/>.
    /// <para/>
    /// Chain <see cref="WithPatAuthentication"/> or <see cref="WithOAuthAuthentication"/> to opt into an
    /// authentication mechanism. Without either, requests are sent without an <c>Authorization</c> header.
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

    /// <summary>
    /// Opts into authenticating the <see cref="IDiscogsApiClient"/> with a
    /// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Personal Access Token</see>,
    /// registering <see cref="IDiscogsPatAuthenticationProvider"/>.
    /// <para/>
    /// The <see cref="DiscogsPatOptions.SectionName"/> (<c>"Discogs:Pat"</c>) configuration section is bound
    /// first (if an <see cref="IConfiguration"/> is registered), then <paramref name="configureOptions"/> is
    /// applied on top so code-based configuration overrides configuration values.
    /// </summary>
    /// <param name="services">The service collection to add the authentication mechanism to.</param>
    /// <param name="configureOptions">Optional delegate configuring the <see cref="DiscogsPatOptions"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="WithOAuthAuthentication(IServiceCollection, Action{DiscogsOAuthOptions})"/> was already called on the same <paramref name="services"/>.</exception>
    public static IServiceCollection WithPatAuthentication(
        this IServiceCollection services,
        Action<DiscogsPatOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.EnsureNoOAuthAuthenticationRegistered();

        var optionsBuilder = services.AddOptions<DiscogsPatOptions>()
            .Configure<IServiceProvider>(static (options, serviceProvider) => serviceProvider.GetService<IConfiguration>()?
                .GetSection(DiscogsPatOptions.SectionName)
                .Bind(options));

        if (configureOptions is not null)
        {
            optionsBuilder.Configure(configureOptions);
        }

        optionsBuilder.ValidateOnStart();

        return services.WithPatAuthenticationCore();
    }

    /// <summary>
    /// Opts into authenticating the <see cref="IDiscogsApiClient"/> with a
    /// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Personal Access Token</see>,
    /// registering <see cref="IDiscogsPatAuthenticationProvider"/>.
    /// <para/>
    /// The <see cref="DiscogsPatOptions.SectionName"/> (<c>"Discogs:Pat"</c>) configuration section is bound
    /// first (if an <see cref="IConfiguration"/> is registered), then <paramref name="configureOptions"/> is
    /// applied on top so code-based configuration overrides configuration values.
    /// </summary>
    /// <param name="services">The service collection to add the authentication mechanism to.</param>
    /// <param name="configureOptions">Delegate configuring the <see cref="DiscogsPatOptions"/> with access to the <see cref="IServiceProvider"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="WithOAuthAuthentication(IServiceCollection, Action{DiscogsOAuthOptions})"/> was already called on the same <paramref name="services"/>.</exception>
    public static IServiceCollection WithPatAuthentication(
        this IServiceCollection services,
        Action<IServiceProvider, DiscogsPatOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.EnsureNoOAuthAuthenticationRegistered();

        services.AddOptions<DiscogsPatOptions>()
            .Configure<IServiceProvider>(static (options, serviceProvider) => serviceProvider.GetService<IConfiguration>()?
                .GetSection(DiscogsPatOptions.SectionName)
                .Bind(options))
            .Configure<IServiceProvider>((options, serviceProvider) => configureOptions(serviceProvider, options))
            .ValidateOnStart();

        return services.WithPatAuthenticationCore();
    }

    /// <summary>
    /// Opts into authenticating the <see cref="IDiscogsApiClient"/> with a
    /// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Personal Access Token</see>,
    /// registering <see cref="IDiscogsPatAuthenticationProvider"/>, binding the <see cref="DiscogsPatOptions"/> from the supplied <paramref name="configuration"/>.
    /// </summary>
    /// <param name="services">The service collection to add the authentication mechanism to.</param>
    /// <param name="configuration">The configuration to bind. Pass the section to bind directly, e.g. <c>configuration.GetSection(DiscogsPatOptions.SectionName)</c>.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="WithOAuthAuthentication(IServiceCollection, Action{DiscogsOAuthOptions})"/> was already called on the same <paramref name="services"/>.</exception>
    public static IServiceCollection WithPatAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.EnsureNoOAuthAuthenticationRegistered();

        services.AddOptions<DiscogsPatOptions>()
            .Bind(configuration)
            .ValidateOnStart();

        return services.WithPatAuthenticationCore();
    }

    /// <summary>
    /// Opts into authenticating the <see cref="IDiscogsApiClient"/> with the
    /// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see> flow,
    /// registering <see cref="IDiscogsOAuthAuthenticationProvider"/>.
    /// <para/>
    /// The <see cref="DiscogsOAuthOptions.SectionName"/> (<c>"Discogs:OAuth"</c>) configuration section is bound
    /// first (if an <see cref="IConfiguration"/> is registered), then <paramref name="configureOptions"/> is
    /// applied on top so code-based configuration overrides configuration values.
    /// </summary>
    /// <param name="services">The service collection to add the authentication mechanism to.</param>
    /// <param name="configureOptions">Optional delegate configuring the <see cref="DiscogsOAuthOptions"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="WithPatAuthentication(IServiceCollection, Action{DiscogsPatOptions})"/> was already called on the same <paramref name="services"/>.</exception>
    public static IServiceCollection WithOAuthAuthentication(
        this IServiceCollection services,
        Action<DiscogsOAuthOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.EnsureNoPatAuthenticationRegistered();

        var optionsBuilder = services.AddOptions<DiscogsOAuthOptions>()
            .Configure<IServiceProvider>(static (options, serviceProvider) => serviceProvider.GetService<IConfiguration>()?
                .GetSection(DiscogsOAuthOptions.SectionName)
                .Bind(options));

        if (configureOptions is not null)
        {
            optionsBuilder.Configure(configureOptions);
        }

        optionsBuilder.ValidateOnStart();

        return services.WithOAuthAuthenticationCore();
    }

    /// <summary>
    /// Opts into authenticating the <see cref="IDiscogsApiClient"/> with the
    /// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see> flow,
    /// registering <see cref="IDiscogsOAuthAuthenticationProvider"/>.
    /// <para/>
    /// The <see cref="DiscogsOAuthOptions.SectionName"/> (<c>"Discogs:OAuth"</c>) configuration section is bound
    /// first (if an <see cref="IConfiguration"/> is registered), then <paramref name="configureOptions"/> is
    /// applied on top so code-based configuration overrides configuration values.
    /// </summary>
    /// <param name="services">The service collection to add the authentication mechanism to.</param>
    /// <param name="configureOptions">Delegate configuring the <see cref="DiscogsOAuthOptions"/> with access to the <see cref="IServiceProvider"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="WithPatAuthentication(IServiceCollection, Action{DiscogsPatOptions})"/> was already called on the same <paramref name="services"/>.</exception>
    public static IServiceCollection WithOAuthAuthentication(
        this IServiceCollection services,
        Action<IServiceProvider, DiscogsOAuthOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.EnsureNoPatAuthenticationRegistered();

        services.AddOptions<DiscogsOAuthOptions>()
            .Configure<IServiceProvider>(static (options, serviceProvider) => serviceProvider.GetService<IConfiguration>()?
                .GetSection(DiscogsOAuthOptions.SectionName)
                .Bind(options))
            .Configure<IServiceProvider>((options, serviceProvider) => configureOptions(serviceProvider, options))
            .ValidateOnStart();

        return services.WithOAuthAuthenticationCore();
    }

    /// <summary>
    /// Opts into authenticating the <see cref="IDiscogsApiClient"/> with the
    /// <see href="https://www.discogs.com/developers#page:authentication,header:authentication-oauth-flow">OAuth 1.0a</see> flow,
    /// registering <see cref="IDiscogsOAuthAuthenticationProvider"/>, binding the <see cref="DiscogsOAuthOptions"/> from the supplied <paramref name="configuration"/>.
    /// </summary>
    /// <param name="services">The service collection to add the authentication mechanism to.</param>
    /// <param name="configuration">The configuration to bind. Pass the section to bind directly, e.g. <c>configuration.GetSection(DiscogsOAuthOptions.SectionName)</c>.</param>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="WithPatAuthentication(IServiceCollection, Action{DiscogsPatOptions})"/> was already called on the same <paramref name="services"/>.</exception>
    public static IServiceCollection WithOAuthAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.EnsureNoPatAuthenticationRegistered();

        services.AddOptions<DiscogsOAuthOptions>()
            .Bind(configuration)
            .ValidateOnStart();

        return services.WithOAuthAuthenticationCore();
    }

    private static void EnsureNoOAuthAuthenticationRegistered(this IServiceCollection services)
    {
        if (services.Any(static descriptor => descriptor.ServiceType == typeof(IDiscogsOAuthAuthenticationProvider)))
        {
            throw new InvalidOperationException($"{nameof(WithOAuthAuthentication)} was already called on this service collection. Only one authentication mechanism can be registered.");
        }
    }

    private static void EnsureNoPatAuthenticationRegistered(this IServiceCollection services)
    {
        if (services.Any(static descriptor => descriptor.ServiceType == typeof(IDiscogsPatAuthenticationProvider)))
        {
            throw new InvalidOperationException($"{nameof(WithPatAuthentication)} was already called on this service collection. Only one authentication mechanism can be registered.");
        }
    }

    private static IServiceCollection WithPatAuthenticationCore(this IServiceCollection services)
    {
        services.AddSingleton<IValidateOptions<DiscogsPatOptions>, DiscogsPatOptionsValidator>();
        services.TryAddSingleton<IDiscogsPatAuthenticationProvider, DiscogsPatAuthenticationProvider>();

        return services;
    }

    private static IServiceCollection WithOAuthAuthenticationCore(this IServiceCollection services)
    {
        services.AddSingleton<IValidateOptions<DiscogsOAuthOptions>, DiscogsOAuthOptionsValidator>();

        services.AddHttpClient(DiscogsOAuthAuthenticationProvider.HttpClientName)
            .ConfigureHttpClient(static (serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value;
                httpClient.BaseAddress = new(options.BaseUrl);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
            })
            .AddHttpMessageHandler<ErrorHandlingDelegatingHandler>();

        services.TryAddSingleton<IDiscogsOAuthAuthenticationProvider, DiscogsOAuthAuthenticationProvider>();

        return services;
    }

    private static IServiceCollection AddDiscogsApiClientCore(this IServiceCollection services, Action<IHttpClientBuilder>? configureClient)
    {
        services.AddSingleton<IValidateOptions<DiscogsApiClientOptions>, DiscogsApiClientOptionsValidator>();

        services.TryAddTransient<AuthenticationDelegatingHandler>();
        services.TryAddTransient<ErrorHandlingDelegatingHandler>();
        services.TryAddTransient<RateLimitStateDelegatingHandler>();
        services.TryAddSingleton<IDiscogsAuthenticationHeaderProvider, DiscogsAuthenticationHeaderProvider>();

        services.TryAddSingleton<DiscogsRateLimitStateService>();
        services.TryAddSingleton<IDiscogsRateLimitStateService>(static sp => sp.GetRequiredService<DiscogsRateLimitStateService>());
        services.TryAddSingleton<IDiscogsRateLimitStateUpdateService>(static sp => sp.GetRequiredService<DiscogsRateLimitStateService>());

        services.TryAddSingleton(static _ => new DiscogsJsonSerializerContext(
            new JsonSerializerOptions().AddGeneratedEnumJsonConverters()));

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
