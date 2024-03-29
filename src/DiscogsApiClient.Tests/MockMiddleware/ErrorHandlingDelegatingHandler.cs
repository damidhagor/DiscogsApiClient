﻿using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace DiscogsApiClient.Middleware;

public sealed class DebugMessageContentDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (Debugger.IsAttached && request.Content is not null)
        {
            var content = await request.Content.ReadAsStringAsync();
            Debugger.Break();
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (Debugger.IsAttached && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            Debugger.Break();
        }

        return response;
    }
}
