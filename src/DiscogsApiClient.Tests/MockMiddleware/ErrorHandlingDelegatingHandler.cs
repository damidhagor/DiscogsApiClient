using System.Diagnostics;

namespace DiscogsApiClient.Tests.MockMiddleware;

public sealed class DebugMessageContentDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (Debugger.IsAttached && request.Content is not null)
        {
            var content = await request.Content.ReadAsStringAsync(cancellationToken);
            Debugger.Break();
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (Debugger.IsAttached && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            Debugger.Break();
        }

        return response;
    }
}
