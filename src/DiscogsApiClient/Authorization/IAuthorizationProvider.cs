namespace DiscogsApiClient.Authorization;

public interface IAuthorizationProvider
{
    bool IsAuthorized { get; }

    Task<IAuthorizationResponse> AuthorizeAsync(IAuthorizationRequest authorizationRequest, CancellationToken cancellationToken);

    HttpRequestMessage CreateAuthorizedRequest(HttpMethod httpMethod, string url);
}
