using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
    public async Task AddDiscogsApiClient_ShouldBeIdempotent_WhenCalledTwice()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        });
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var publicService = serviceProvider.GetService<IDiscogsRateLimitStateService>();
        var updateService = serviceProvider.GetService<IDiscogsRateLimitStateUpdateService>();

        await Assert.That(serviceProvider.GetService<IDiscogsApiClient>()).IsNotNull();
        await Assert.That(publicService).IsSameReferenceAs(updateService);
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldFailValidation_WhenBaseUrlIsEmpty()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "";
            options.UserAgent = "TestUserAgent";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value)
            .Throws<OptionsValidationException>();
        await Assert.That(exception!.Message).IsEqualTo("BaseUrl is required.");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldFailValidation_WhenBaseUrlIsNotAValidUrl()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "not a url";
            options.UserAgent = "TestUserAgent";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value)
            .Throws<OptionsValidationException>();
        await Assert.That(exception!.Message).IsEqualTo("BaseUrl must be a valid URL.");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldFailValidation_WhenUserAgentIsEmpty()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value)
            .Throws<OptionsValidationException>();
        await Assert.That(exception!.Message).IsEqualTo("UserAgent is required.");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldFailValidation_WhenOnlyConsumerKeyIsSet()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
            options.ConsumerKey = "key";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value)
            .Throws<OptionsValidationException>();
        await Assert.That(exception!.Message).IsEqualTo("ConsumerKey, ConsumerSecret and VerifierCallbackUrl must all be provided to use OAuth authentication.");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldFailValidation_WhenOnlyConsumerSecretIsSet()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
            options.ConsumerSecret = "secret";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value)
            .Throws<OptionsValidationException>();
        await Assert.That(exception!.Message).IsEqualTo("ConsumerKey, ConsumerSecret and VerifierCallbackUrl must all be provided to use OAuth authentication.");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldFailValidation_WhenVerifierCallbackUrlIsMissing()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value)
            .Throws<OptionsValidationException>();
        await Assert.That(exception!.Message).IsEqualTo("ConsumerKey, ConsumerSecret and VerifierCallbackUrl must all be provided to use OAuth authentication.");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldPassValidation_WhenAllOAuthSettingsAreSet()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
            options.VerifierCallbackUrl = "https://example.com/callback";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value;
        await Assert.That(options.ConsumerKey).IsEqualTo("key");
        await Assert.That(options.ConsumerSecret).IsEqualTo("secret");
        await Assert.That(options.VerifierCallbackUrl).IsEqualTo("https://example.com/callback");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldFailValidation_WhenVerifierCallbackUrlIsNotAValidUrl()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
            options.VerifierCallbackUrl = "not a url";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value)
            .Throws<OptionsValidationException>();
        await Assert.That(exception!.Message).IsEqualTo("VerifierCallbackUrl must be a valid URL.");
    }

    [Test]
    public async Task AddDiscogsApiClient_ShouldBindFromConfiguration_WhenConfigurationOverloadIsUsed()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["BaseUrl"] = "https://api.discogs.com",
                ["UserAgent"] = "ConfigUserAgent",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddDiscogsApiClient(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptions<DiscogsApiClientOptions>>().Value;
        await Assert.That(options.UserAgent).IsEqualTo("ConfigUserAgent");
    }
}
