﻿namespace DiscogsApiClient.Middleware;

/// <summary>
/// Inserts an authentication header for the Discogs Api into the HttpResponseMessage.
/// </summary>
public sealed class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IDiscogsAuthenticationService _authenticationService;

    public AuthenticationDelegatingHandler(IDiscogsAuthenticationService authenticationService)
        => _authenticationService = authenticationService;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_authenticationService.IsAuthenticated)
        {
            var authHeader = _authenticationService.CreateAuthenticationHeader();
            request.Headers.Add("Authorization", authHeader);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
