using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace DiscogsApiClient.Tests.MockMiddleware;

public sealed class OAuthMockDelegatingHandler : DelegatingHandler
{
    public HttpStatusCode RequestTokenStatusCode { get; set; } = HttpStatusCode.OK;
    public string RequestToken { get; set; } = "requesttoken";
    public string RequestTokenSecret { get; set; } = "requesttokensecret";

    public HttpStatusCode AccessTokenStatusCode { get; set; } = HttpStatusCode.OK;
    public string AccessToken { get; set; } = "accesstoken";
    public string AccessTokenSecret { get; set; } = "accesstokensecret";

    public Func<string, string, CancellationToken, Task<string>> GetVerifierCallbackMock { get; set; } = (authUrl, callbackUrl, cancellationToken) => Task.FromResult("verifiertoken");

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri?.AbsolutePath == "/oauth/request_token")
        {
            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = RequestTokenStatusCode,
                Content = new StringContent($"oauth_token={RequestToken}&oauth_token_secret={RequestTokenSecret}")
            });
        }

        if (request.RequestUri?.AbsolutePath == "/oauth/access_token")
        {
            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = AccessTokenStatusCode,
                Content = new StringContent($"oauth_token={AccessToken}&oauth_token_secret={AccessTokenSecret}")
            });
        }

        Debugger.Break();
        return Task.FromResult(new HttpResponseMessage());
    }

    public Task<string> GetVerifierCallbackMockCaller(string authUrl, string callbackUrl, CancellationToken cancellationToken)
    {
        return GetVerifierCallbackMock(authUrl, callbackUrl, cancellationToken);
    }
}
