namespace DiscogsApiClient.Middleware;

internal sealed class AuthenticationDelegatingHandler(IDiscogsAuthenticationHeaderProvider authenticationHeaderProvider) : DelegatingHandler
{
    private readonly IDiscogsAuthenticationHeaderProvider _authenticationHeaderProvider = authenticationHeaderProvider;


    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (_authenticationHeaderProvider.IsAuthenticated)
        {
            var authHeader = _authenticationHeaderProvider.GetHeader();
            request.Headers.Add("Authorization", authHeader);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
