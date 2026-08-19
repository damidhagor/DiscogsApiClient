using DiscogsApiClient.Authentication.OAuth;
using DiscogsApiClient.Authentication.Pat;
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

    [Test]
    public async Task AddDiscogsApiClient_ShouldBeUnauthenticated_WhenNeitherAuthenticationMechanismIsRegistered()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var headerProvider = serviceProvider.GetRequiredService<IDiscogsAuthenticationHeaderProvider>();

        await Assert.That(serviceProvider.GetService<IDiscogsPatAuthenticationProvider>()).IsNull();
        await Assert.That(serviceProvider.GetService<IDiscogsOAuthAuthenticationProvider>()).IsNull();
        await Assert.That(headerProvider.IsAuthenticated).IsFalse();
        await Assert.That(() => headerProvider.GetHeader()).Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task WithPatAuthentication_ShouldRegisterPatProvider()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithPatAuthentication(options => options.Token = "mytoken");

        using var serviceProvider = services.BuildServiceProvider();

        var patProvider = serviceProvider.GetService<IDiscogsPatAuthenticationProvider>();
        var headerProvider = serviceProvider.GetRequiredService<IDiscogsAuthenticationHeaderProvider>();

        await Assert.That(patProvider).IsNotNull();
        await Assert.That(serviceProvider.GetService<IDiscogsOAuthAuthenticationProvider>()).IsNull();
        await Assert.That(patProvider!.IsAuthenticated).IsTrue();
        await Assert.That(headerProvider.IsAuthenticated).IsTrue();
        await Assert.That(headerProvider.GetHeader()).IsEqualTo("Discogs token=mytoken");
    }

    [Test]
    public async Task WithPatAuthentication_ShouldBindFromConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Discogs:Pat:Token"] = "configtoken",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithPatAuthentication();

        using var serviceProvider = services.BuildServiceProvider();

        var patProvider = serviceProvider.GetRequiredService<IDiscogsPatAuthenticationProvider>();

        await Assert.That(patProvider.IsAuthenticated).IsTrue();
        await Assert.That(patProvider.CreateAuthenticationHeader()).IsEqualTo("Discogs token=configtoken");
    }

    [Test]
    public async Task WithPatAuthentication_ShouldBindFromConfigurationOverload()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Token"] = "sectiontoken",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithPatAuthentication(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        var patProvider = serviceProvider.GetRequiredService<IDiscogsPatAuthenticationProvider>();

        await Assert.That(patProvider.IsAuthenticated).IsTrue();
        await Assert.That(patProvider.CreateAuthenticationHeader()).IsEqualTo("Discogs token=sectiontoken");
    }

    [Test]
    public async Task WithPatAuthentication_ShouldConfigureWithServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton("providedtoken");
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithPatAuthentication((serviceProvider, options) => options.Token = serviceProvider.GetRequiredService<string>());

        using var serviceProvider = services.BuildServiceProvider();

        var patProvider = serviceProvider.GetRequiredService<IDiscogsPatAuthenticationProvider>();

        await Assert.That(patProvider.IsAuthenticated).IsTrue();
        await Assert.That(patProvider.CreateAuthenticationHeader()).IsEqualTo("Discogs token=providedtoken");
    }

    [Test]
    public async Task WithOAuthAuthentication_ShouldBindFromConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Discogs:OAuth:ConsumerKey"] = "configkey",
                ["Discogs:OAuth:ConsumerSecret"] = "configsecret",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithOAuthAuthentication();

        using var serviceProvider = services.BuildServiceProvider();

        var oAuthOptions = serviceProvider.GetRequiredService<IOptions<DiscogsOAuthOptions>>().Value;

        await Assert.That(oAuthOptions.ConsumerKey).IsEqualTo("configkey");
        await Assert.That(oAuthOptions.ConsumerSecret).IsEqualTo("configsecret");
    }

    [Test]
    public async Task WithOAuthAuthentication_ShouldBindFromConfigurationOverload()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConsumerKey"] = "sectionkey",
                ["ConsumerSecret"] = "sectionsecret",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithOAuthAuthentication(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        var oAuthOptions = serviceProvider.GetRequiredService<IOptions<DiscogsOAuthOptions>>().Value;

        await Assert.That(oAuthOptions.ConsumerKey).IsEqualTo("sectionkey");
        await Assert.That(oAuthOptions.ConsumerSecret).IsEqualTo("sectionsecret");
    }

    [Test]
    public async Task WithOAuthAuthentication_ShouldConfigureWithServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton("providedkey");
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithOAuthAuthentication((serviceProvider, options) =>
        {
            options.ConsumerKey = serviceProvider.GetRequiredService<string>();
            options.ConsumerSecret = "secret";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var oAuthOptions = serviceProvider.GetRequiredService<IOptions<DiscogsOAuthOptions>>().Value;

        await Assert.That(oAuthOptions.ConsumerKey).IsEqualTo("providedkey");
        await Assert.That(oAuthOptions.ConsumerSecret).IsEqualTo("secret");
    }

    [Test]
    public async Task WithOAuthAuthentication_ShouldRegisterOAuthProvider()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithOAuthAuthentication(options =>
        {
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
        });

        using var serviceProvider = services.BuildServiceProvider();

        var oAuthProvider = serviceProvider.GetService<IDiscogsOAuthAuthenticationProvider>();
        var headerProvider = serviceProvider.GetRequiredService<IDiscogsAuthenticationHeaderProvider>();

        await Assert.That(oAuthProvider).IsNotNull();
        await Assert.That(serviceProvider.GetService<IDiscogsPatAuthenticationProvider>()).IsNull();
        await Assert.That(headerProvider.IsAuthenticated).IsFalse();

        oAuthProvider!.Authenticate("accesstoken", "accesstokensecret");

        await Assert.That(headerProvider.IsAuthenticated).IsTrue();
    }

    [Test]
    public async Task WithOAuthAuthentication_ShouldFailValidation_WhenConsumerKeyIsMissing()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithOAuthAuthentication(options => options.ConsumerSecret = "secret");

        using var serviceProvider = services.BuildServiceProvider();

        var exception = await Assert.That(() => serviceProvider.GetRequiredService<IOptions<DiscogsOAuthOptions>>().Value)
            .Throws<OptionsValidationException>();
        await Assert.That(exception!.Message).IsEqualTo("ConsumerKey is required.");
    }

    [Test]
    public async Task WithPatAuthentication_ShouldThrow_WhenOAuthAuthenticationAlreadyRegistered()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithOAuthAuthentication(options =>
        {
            options.ConsumerKey = "key";
            options.ConsumerSecret = "secret";
        });

        await Assert.That(() => services.WithPatAuthentication())
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task WithOAuthAuthentication_ShouldThrow_WhenPatAuthenticationAlreadyRegistered()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(options =>
        {
            options.BaseUrl = "https://api.discogs.com";
            options.UserAgent = "TestUserAgent";
        })
        .WithPatAuthentication();

        await Assert.That(() => services.WithOAuthAuthentication(options =>
            {
                options.ConsumerKey = "key";
                options.ConsumerSecret = "secret";
            }))
            .Throws<InvalidOperationException>();
    }
}
