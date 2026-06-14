using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace DiscogsApiClient.Tests.Client;

public sealed class ServiceCollectionTests
{
    [Test]
    public async Task AddDiscogsApiClient_ShouldRegisterRequiredServices_WhenConfigurationIsValid()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        });

        using var serviceProvider = services.BuildServiceProvider();

        await Assert.That(serviceProvider.GetService<IDiscogsApiClient>()).IsNotNull();
        await Assert.That(serviceProvider.GetService<IDiscogsAuthenticationService>()).IsNotNull();
        await Assert.That(serviceProvider.GetService<IOAuthAuthenticationProvider>()).IsNotNull();
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldRegisterRateLimitStateService()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var publicService = serviceProvider.GetService<IDiscogsRateLimitStateService>();
        var updateService = serviceProvider.GetService<IDiscogsRateLimitStateUpdateService>();

        await Assert.That(publicService).IsNotNull();
        await Assert.That(updateService).IsNotNull();
        await Assert.That(publicService).IsSameReferenceAs(updateService);
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldThrowInvalidOperationException_WhenBaseUrlIsEmpty()
    {
        var services = new ServiceCollection();

        await Assert.That(() => services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "";
            options.UserAgent = "TestUserAgent";
        })).Throws<InvalidOperationException>().WithMessageContaining("base url");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldThrowInvalidOperationException_WhenUserAgentIsEmpty()
    {
        var services = new ServiceCollection();

        await Assert.That(() => services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "";
        })).Throws<InvalidOperationException>().WithMessageContaining("user agent");
    }
}

