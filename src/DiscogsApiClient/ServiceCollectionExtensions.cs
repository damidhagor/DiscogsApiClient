using DiscogsApiClient.Authentication.PlainOAuth;
using DiscogsApiClient.Authentication.UserToken;
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
    /// <param name="userAgent">The suer agent to be used by the <see cref="DiscogsApiClient"/></param>
    public static IServiceCollection AddDiscogsApiClient(this IServiceCollection services, string userAgent)
    {
        services.AddHttpClient<IDiscogsApiClient, DiscogsApiClient>(httpClient
            => httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent));

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
}
