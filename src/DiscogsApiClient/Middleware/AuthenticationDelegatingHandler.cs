namespace DiscogsApiClient.Middleware;

internal sealed class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IDiscogsAuthenticationService _authenticationService;

    public AuthenticationDelegatingHandler(IDiscogsAuthenticationService authenticationService)
        => _authenticationService = authenticationService;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(request.RequestUri);

        if (_authenticationService.IsAuthenticated)
        {
            var authHeader = _authenticationService.CreateAuthenticationHeader();
            request.Headers.Add("Authorization", authHeader);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
