namespace DiscogsApiClient.Authorization.PlainOAuth;

public delegate Task<string> GetVerifierCallback(string authorizeUrl, string verifierCallbackUrl, CancellationToken cancellationToken);

public class PlainOAuthAuthorizationRequest : IAuthorizationRequest
{
    public string VerifierCallbackUrl { get; init; }

    public GetVerifierCallback GetVerifierCallback { get; init; }


    public PlainOAuthAuthorizationRequest(string verifierCallbackUrl, GetVerifierCallback getVerifierCallback)
    {
        VerifierCallbackUrl = verifierCallbackUrl;
        GetVerifierCallback = getVerifierCallback;
    }
}
