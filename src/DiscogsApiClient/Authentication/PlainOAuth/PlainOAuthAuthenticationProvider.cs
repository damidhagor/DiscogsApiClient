using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace DiscogsApiClient.Authentication.PlainOAuth;

public class PlainOAuthAuthenticationProvider : IAuthenticationProvider
{
    private readonly string _userAgent;
    private readonly string _consumerKey;
    private readonly string _consumerSecret;
    private string _accessToken;
    private string _accessTokenSecret;

    public bool IsAuthenticated => !String.IsNullOrWhiteSpace(_accessToken) && !String.IsNullOrWhiteSpace(_accessTokenSecret);


    public PlainOAuthAuthenticationProvider(string userAgent, string consumerKey, string consumerSecret, string accessToken = "", string accessTokenSecret = "")
    {
        _userAgent = userAgent;
        _consumerKey = consumerKey;
        _consumerSecret = consumerSecret;
        _accessToken = accessToken;
        _accessTokenSecret = accessTokenSecret;
    }

    public async Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
    {
        if (authenticationRequest is PlainOAuthAuthenticationRequest authAuthenticationRequest
            && (String.IsNullOrWhiteSpace(_accessToken) || String.IsNullOrWhiteSpace(_accessTokenSecret)))
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);

            _accessToken = "";
            _accessTokenSecret = "";

            if (String.IsNullOrWhiteSpace(_consumerKey) && String.IsNullOrWhiteSpace(_consumerSecret))
                throw new InvalidOperationException("No consumer token or secret provided.");

            var (requestToken, requestTokenSecret) = await GetRequestToken(httpClient, authAuthenticationRequest.VerifierCallbackUrl, cancellationToken);
            if (String.IsNullOrWhiteSpace(requestToken) || String.IsNullOrWhiteSpace(requestTokenSecret))
                return new PlainOAuthAuthenticationResponse("Getting request token failed.");


            var verifier = await GetVerifier(requestToken, authAuthenticationRequest.VerifierCallbackUrl, authAuthenticationRequest.GetVerifierCallback, cancellationToken);
            if (String.IsNullOrWhiteSpace(verifier))
                return new PlainOAuthAuthenticationResponse("Failed getting verifier token.");


            var (accessToken, accessTokenSecret) = await GetAccessToken(httpClient, requestToken, requestTokenSecret, verifier, cancellationToken);
            if (String.IsNullOrWhiteSpace(accessToken) || String.IsNullOrWhiteSpace(accessTokenSecret))
                return new PlainOAuthAuthenticationResponse("Failed getting access token.");

            _accessToken = accessToken;
            _accessTokenSecret = accessTokenSecret;
        }

        return new PlainOAuthAuthenticationResponse(_accessToken, _accessTokenSecret);
    }

    public HttpRequestMessage CreateAuthenticatedRequest(HttpMethod httpMethod, string url)
    {
        var request = new HttpRequestMessage(httpMethod, url);
        request.Headers.Add("Authorization", CreateAuthenticationHeader());

        return request;
    }


    private async Task<(string requestToken, string requestTokenSecret)> GetRequestToken(HttpClient httpClient, string callback, CancellationToken cancellationToken)
    {
        var requestToken = "";
        var requestTokenSecret = "";

        try
        {
            using (var request = CreateRequestTokenRequest(callback))
            using (var response = await httpClient.SendAsync(request, cancellationToken))
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                var parameters = HttpUtility.ParseQueryString(content);

                requestToken = parameters.Get("oauth_token") ?? "";
                requestTokenSecret = parameters.Get("oauth_token_secret") ?? "";
            }
        }
        catch (Exception) { }

        return (requestToken, requestTokenSecret);
    }

    private async Task<string> GetVerifier(string requestToken, string callback, GetVerifierCallback getVerifier, CancellationToken cancellationToken)
    {
        var verifier = "";

        try
        {
            var url = String.Format(DiscogsApiUrls.VerifierTokenUrl, requestToken);
            var verifierResult = await getVerifier(url, callback, cancellationToken);

            if (verifierResult != null)
            {
                var parameters = HttpUtility.ParseQueryString(verifierResult);

                verifier = parameters.Get("oauth_verifier") ?? "";
            }
        }
        catch (Exception) { }

        return verifier;
    }

    private async Task<(string accessToken, string accessTokenSecret)> GetAccessToken(HttpClient httpClient, string requestToken, string requestTokenSecret, string verifier, CancellationToken cancellationToken)
    {
        var accessToken = "";
        var accessTokenSecret = "";

        try
        {
            using (var request = CreateAccessTokenRequest(requestToken, requestTokenSecret, verifier))
            using (var response = await httpClient.SendAsync(request, cancellationToken))
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                var parameters = HttpUtility.ParseQueryString(content);

                accessToken = parameters.Get("oauth_token") ?? "";
                accessTokenSecret = parameters.Get("oauth_token_secret") ?? "";
            }
        }
        catch (Exception) { }

        return (accessToken, accessTokenSecret);
    }


    private HttpRequestMessage CreateRequestTokenRequest(string callback)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, DiscogsApiUrls.OAuthRequestTokenUrl);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        (string timestamp, string nonce) = CreateTimestampAndNonce();

        string authHeader = "OAuth ";
        authHeader += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_consumerKey)}\",";
        authHeader += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
        authHeader += $"oauth_signature=\"{WebUtility.UrlEncode($"{_consumerSecret}&")}\",";
        authHeader += $"oauth_signature_method=\"PLAINTEXT\",";
        authHeader += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\",";
        authHeader += $"oauth_callback=\"{WebUtility.UrlEncode(callback)}\"";

        request.Headers.Add("Authorization", authHeader);

        return request;
    }

    private HttpRequestMessage CreateAccessTokenRequest(string requestToken, string requestTokenSecret, string verifier)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, DiscogsApiUrls.OAuthAccessTokenUrl);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        (string timestamp, string nonce) = CreateTimestampAndNonce();

        string authHeader = "OAuth ";
        authHeader += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_consumerKey)}\",";
        authHeader += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
        authHeader += $"oauth_token=\"{WebUtility.UrlEncode(requestToken)}\",";
        authHeader += $"oauth_signature=\"{WebUtility.UrlEncode($"{_consumerSecret}&{requestTokenSecret}")}\",";
        authHeader += $"oauth_signature_method=\"PLAINTEXT\",";
        authHeader += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\",";
        authHeader += $"oauth_verifier=\"{WebUtility.UrlEncode(verifier)}\"";

        request.Headers.Add("Authorization", authHeader);

        return request;
    }


    private string CreateAuthenticationHeader()
    {
        (string timestamp, string nonce) = CreateTimestampAndNonce();

        string header = "OAuth ";
        header += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_consumerKey)}\",";
        header += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
        header += $"oauth_token=\"{WebUtility.UrlEncode(_accessToken)}\",";
        header += $"oauth_signature=\"{WebUtility.UrlEncode($"{_consumerSecret}&{_accessTokenSecret}")}\",";
        header += $"oauth_signature_method=\"PLAINTEXT\",";
        header += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\"";

        return header;
    }

    private (string timestamp, string nonce) CreateTimestampAndNonce()
    {
        TimeSpan ellapsedTimeSince1970 = DateTime.UtcNow - new DateTime(1970, 1, 1);

        long timestamp = (long)ellapsedTimeSince1970.TotalSeconds;
        long nonce = (long)ellapsedTimeSince1970.TotalMilliseconds;

        return (timestamp.ToString(), nonce.ToString());
    }
}