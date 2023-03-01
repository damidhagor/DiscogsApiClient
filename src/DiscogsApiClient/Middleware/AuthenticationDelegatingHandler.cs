﻿namespace DiscogsApiClient.Middleware;

internal sealed class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IAuthenticationProvider _authenticationProvider;

    public AuthenticationDelegatingHandler(IAuthenticationProvider authenticationProvider)
        => _authenticationProvider = authenticationProvider;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(request.RequestUri);

        if (_authenticationProvider.IsAuthenticated)
        {
            var authHeader = _authenticationProvider.CreateAuthenticationHeader(request.Method, request.RequestUri.ToString());
            request.Headers.Add("Authorization", authHeader);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
