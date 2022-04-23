namespace DiscogsApiClient.Authentication.PlainOAuth;

public delegate Task<string> GetVerifierCallback(string authorizeUrl, string verifierCallbackUrl, CancellationToken cancellationToken);

public class PlainOAuthAuthenticationRequest : IAuthenticationRequest
{
    public string VerifierCallbackUrl { get; init; }

    public GetVerifierCallback GetVerifierCallback { get; init; }


    public PlainOAuthAuthenticationRequest(string verifierCallbackUrl, GetVerifierCallback getVerifierCallback)
    {
        VerifierCallbackUrl = verifierCallbackUrl;
        GetVerifierCallback = getVerifierCallback;
    }
}
