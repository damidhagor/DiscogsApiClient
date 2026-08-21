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
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var message = await GetErrorMessage(response, cancellationToken).ConfigureAwait(false);

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new UnauthenticatedDiscogsException(message),
            HttpStatusCode.Forbidden => new UnauthenticatedDiscogsException(message),
            HttpStatusCode.NotFound => new ResourceNotFoundDiscogsException(message),
            HttpStatusCode.TooManyRequests => new RateLimitExceededDiscogsException(message),
            _ => new DiscogsException(message),
        };
    }

    private static async Task<string?> GetErrorMessage(HttpResponseMessage response, CancellationToken cancellationToken)
    {
#pragma warning disable CA1031
        try
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<ErrorMessage>(content, DiscogsJsonSerializerContext.Default.ErrorMessage)?.Message;
        }
        catch
        {
            return null;
        }
#pragma warning restore CA1031
    }
}
