using System.Net;

namespace DiscogsApiClient.Middleware;

internal sealed class DiscogsResponseDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        await CheckAndHandleHttpErrorCodes(response, cancellationToken);
        return response;
    }

    /// <summary>
    /// Checks a <see cref="HttpResponseMessage"/> for errors.
    /// If the <see cref="HttpResponseMessage.StatusCode"/> doesn't indicate a successful call,
    /// the helper function attempts to extract the error message from the body
    /// and throws a corresponding <see cref="DiscogsException"/> for the <see cref="HttpResponseMessage.StatusCode"/> with the extracted message.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage.StatusCode"/> to check.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Awaitable Task which returns when the <see cref="HttpResponseMessage.StatusCode"/> doesn't contain an error.</returns>
    /// <exception cref="UnauthorizedDiscogsException">The Http request wasn't authenticated.</exception>
    /// <exception cref="ResourceNotFoundDiscogsException">The requested resource was not found.</exception>
    /// <exception cref="DiscogsException">The request returned a general error.</exception>
    public async Task CheckAndHandleHttpErrorCodes(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        string? message = null;
        try
        {
            var errorMessage = await response.Content.DeserializeAsJsonAsync<ErrorMessage>(cancellationToken);
            message = errorMessage.Message;
        }
        catch { }

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new UnauthorizedDiscogsException(message),
            HttpStatusCode.NotFound => new ResourceNotFoundDiscogsException(message),
            _ => new DiscogsException(message),
        };
    }
}
