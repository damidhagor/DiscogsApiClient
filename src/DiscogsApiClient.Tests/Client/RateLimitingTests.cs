using System.Net;
using DiscogsApiClient.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace DiscogsApiClient.Tests.Client;

public sealed class RateLimitingTests
{
    [Test]
    public async Task HandlerPipeline_ShouldUpdateRateLimitState_WhenResponseIsAnError()
    {
        var services = new ServiceCollection();
        services.AddDiscogsApiClient(
            options =>
            {
                options.BaseUrl = "https://api.discogs.com";
                options.UserAgent = "TestUserAgent";
            },
            configureClient: builder => builder.ConfigurePrimaryHttpMessageHandler(
                () => new HttpMessageHandlerFixture(_ =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
                    response.Headers.Add("x-discogs-ratelimit", "60");
                    response.Headers.Add("x-discogs-ratelimit-remaining", "50");
                    response.Headers.Add("x-discogs-ratelimit-used", "10");
                    return response;
                })));

        using var serviceProvider = services.BuildServiceProvider();

        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient(nameof(IDiscogsApiClient));

        await Assert.That(async () => await httpClient.GetAsync(new Uri("/oauth/identity", UriKind.Relative)))
            .Throws<RateLimitExceededDiscogsException>();

        var rateLimitState = serviceProvider.GetRequiredService<IDiscogsRateLimitStateService>().GetCurrentState();
        await Assert.That(rateLimitState).IsNotNull();
        await Assert.That(rateLimitState!.Limit).IsEqualTo(60);
        await Assert.That(rateLimitState.Remaining).IsEqualTo(50);
        await Assert.That(rateLimitState.Used).IsEqualTo(10);
    }
}
