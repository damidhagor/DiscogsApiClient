using System.Net;
using System.Text.Json;

namespace DiscogsApiClient.Middleware;

/// <summary>
/// Converts Api errors into <see cref="DiscogsException"/>s.
/// </summary>
public sealed class ErrorHandlingDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
            return response;

        string? message = null;
        try
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            message = JsonSerializer.Deserialize<ErrorMessage>(content)?.Message;
        }
        catch { }

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new UnauthenticatedDiscogsException(message),
            HttpStatusCode.Forbidden => new UnauthenticatedDiscogsException(message),
            HttpStatusCode.NotFound => new ResourceNotFoundDiscogsException(message),
            HttpStatusCode.TooManyRequests => new RateLimitExceededDiscogsException(message),
            _ => new DiscogsException(message),
        };
    }
}
