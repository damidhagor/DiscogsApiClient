using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace DiscogsApiClient.Tests.Fixtures.Recording;

public sealed class PlaybackHttpMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
{
    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            next(builder);
            var handler = builder.Services.GetRequiredService<PlaybackDelegatingHandler>();
            builder.AdditionalHandlers.Add(handler);
        };
    }
}
