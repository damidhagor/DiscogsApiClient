using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace DiscogsApiClient.Tests.Fixtures.Recording;

public sealed class RecordingHttpMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
{
    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            // Run other filters first so our handler is placed at the end of the pipeline (closest to primary handler)
            next(builder);

            var isRecording = Environment.GetEnvironmentVariable("DISCOGS_RECORD");
            DelegatingHandler handler = string.Equals(isRecording, "true", StringComparison.OrdinalIgnoreCase)
                ? builder.Services.GetRequiredService<RecordingDelegatingHandler>()
                : builder.Services.GetRequiredService<PlaybackDelegatingHandler>();

            builder.AdditionalHandlers.Add(handler);
        };
    }
}
