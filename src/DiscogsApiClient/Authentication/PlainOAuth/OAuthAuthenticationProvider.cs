using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace DiscogsApiClient.Authentication.OAuth;

/// <summary>
/// This <see cref="IAuthenticationProvider"/> implementation authenticates against the Discogs Api
/// using the OAuth 1.0a flow described <a href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">here</a>
/// and should be provided to the <see cref="DiscogsApiClient"/>'s constructor.
/// </summary>
internal sealed class OAuthAuthenticationProvider : IOAuthAuthenticationProvider
{
    private readonly HttpClient _httpClient;
    private string _consumerKey = "";
    private string _consumerSecret = "";
    private string _accessToken = "";
    private string _accessTokenSecret = "";

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_accessToken) && !string.IsNullOrWhiteSpace(_accessTokenSecret);

    public OAuthAuthenticationProvider(HttpClient httpClient) => _httpClient = httpClient;

    /// <inheritdoc/>
    /// <exception cref="AuthenticationFailedDiscogsException"></exception>
    public async Task<(string accessToken, string accessTokenSecret)> Authenticate(
        string consumerKey,
        string consumerSecret,
        string? accessToken,
        string? accessTokenSecret,
        string verifierCallbackUrl,
        GetVerifierCallback getVerifierCallback,
        CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(consumerKey);
        Guard.IsNotNullOrWhiteSpace(consumerSecret);
        Guard.IsNotNullOrWhiteSpace(verifierCallbackUrl);
        Guard.IsNotNull(getVerifierCallback);

        _consumerKey = consumerKey;
        _consumerSecret = consumerSecret;
        _accessToken = accessToken ?? "";
        _accessTokenSecret = accessTokenSecret ?? "";

        if (string.IsNullOrWhiteSpace(_accessToken) || string.IsNullOrWhiteSpace(_accessTokenSecret))
        {
            _accessToken = "";
            _accessTokenSecret = "";

            var (requestToken, requestTokenSecret) = await GetRequestToken(_httpClient, verifierCallbackUrl, cancellationToken);
            if (string.IsNullOrWhiteSpace(requestToken) || string.IsNullOrWhiteSpace(requestTokenSecret))
                throw new AuthenticationFailedDiscogsException("Getting request token failed.");

            var verifier = await GetVerifier(requestToken, verifierCallbackUrl, getVerifierCallback, cancellationToken);
            if (string.IsNullOrWhiteSpace(verifier))
                throw new AuthenticationFailedDiscogsException("Failed getting verifier token.");

            (accessToken, accessTokenSecret) = await GetAccessToken(_httpClient, requestToken, requestTokenSecret, verifier, cancellationToken);
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(accessTokenSecret))
                throw new AuthenticationFailedDiscogsException("Failed getting access token.");

            _accessToken = accessToken;
            _accessTokenSecret = accessTokenSecret;
        }

        return (_accessToken, _accessTokenSecret);
    }

    public string CreateAuthenticationHeader()
    {
        if (!IsAuthenticated)
            throw new UnauthenticatedDiscogsException($"The {nameof(OAuthAuthenticationProvider)} must be authenticated before creating an authentication header.");

        (var timestamp, var nonce) = CreateTimestampAndNonce();

        var header = "OAuth ";
        header += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_consumerKey)}\",";
        header += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
        header += $"oauth_token=\"{WebUtility.UrlEncode(_accessToken)}\",";
        header += $"oauth_signature=\"{WebUtility.UrlEncode($"{_consumerSecret}&{_accessTokenSecret}")}\",";
        header += $"oauth_signature_method=\"PLAINTEXT\",";
        header += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\"";

        return header;
    }


    /// <summary>
    /// Gets the request token from the Discogs api.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used by the authentication flow.</param>
    /// <param name="callback">The callback url the Discogs login page redirects the browser to to return the request token to the app.</param>
    /// <returns>Returns the obtained request token and secret.</returns>
    private async Task<(string requestToken, string requestTokenSecret)> GetRequestToken(HttpClient httpClient, string callback, CancellationToken cancellationToken)
    {
        var requestToken = "";
        var requestTokenSecret = "";

        try
        {
            (var timestamp, var nonce) = CreateTimestampAndNonce();

            var authHeader = "OAuth ";
            authHeader += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_consumerKey)}\",";
            authHeader += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
            authHeader += $"oauth_signature=\"{WebUtility.UrlEncode($"{_consumerSecret}&")}\",";
            authHeader += $"oauth_signature_method=\"PLAINTEXT\",";
            authHeader += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\",";
            authHeader += $"oauth_callback=\"{WebUtility.UrlEncode(callback)}\"";

            using var request = new HttpRequestMessage(HttpMethod.Get, "/oauth/request_token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            request.Headers.Add("Authorization", authHeader);

            using var response = await httpClient.SendAsync(request, cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var parameters = HttpUtility.ParseQueryString(content);

            requestToken = parameters.Get("oauth_token") ?? "";
            requestTokenSecret = parameters.Get("oauth_token_secret") ?? "";
        }
        catch (Exception e)
        {
            throw new AuthenticationFailedDiscogsException("OAuth authentication failed while getting a request token. See the inner exception for details.", e);
        }

        return (requestToken, requestTokenSecret);
    }

    /// <summary>
    /// Gets the verifier token from the app by notifying the app via the provided <see cref="GetVerifierCallback"/>
    /// which login url should be opened and at which redirect url the verifier token is returned.
    /// </summary>
    /// <param name="requestToken">The request token obtained earlier in the flow.</param>
    /// <param name="callback">The callback url at which to return the verifier token.</param>
    /// <param name="getVerifier">The callback for the app to implement the login process.</param>
    /// <returns>The verifier token.</returns>
    private async Task<string?> GetVerifier(string requestToken, string callback, GetVerifierCallback getVerifier, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"https://discogs.com/oauth/authorize?oauth_token={requestToken}";
            return await getVerifier(url, callback, cancellationToken);
        }
        catch (Exception e)
        {
            throw new AuthenticationFailedDiscogsException("OAuth authentication failed while getting the verifier token. See the inner exception for details.", e);
        }
    }

    /// <summary>
    /// Gets the access token and secret from the Discogs api with the request token nd secret obtained earlier in the flow.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used by the authentication flow.</param>
    /// <param name="requestToken">The request token obtained earlier in the flow.</param>
    /// <param name="requestTokenSecret">The request secret obtained earlier in the flow.</param>
    /// <param name="verifier">The verifier token obtained earlier in the flow.</param>
    /// <returns>The access token and secret which authenticate the logged in user.</returns>
    private async Task<(string accessToken, string accessTokenSecret)> GetAccessToken(HttpClient httpClient, string requestToken, string requestTokenSecret, string verifier, CancellationToken cancellationToken)
    {
        var accessToken = "";
        var accessTokenSecret = "";

        try
        {
            (var timestamp, var nonce) = CreateTimestampAndNonce();

            var authHeader = "OAuth ";
            authHeader += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_consumerKey)}\",";
            authHeader += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
            authHeader += $"oauth_token=\"{WebUtility.UrlEncode(requestToken)}\",";
            authHeader += $"oauth_signature=\"{WebUtility.UrlEncode($"{_consumerSecret}&{requestTokenSecret}")}\",";
            authHeader += $"oauth_signature_method=\"PLAINTEXT\",";
            authHeader += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\",";
            authHeader += $"oauth_verifier=\"{WebUtility.UrlEncode(verifier)}\"";

            using var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/access_token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            request.Headers.Add("Authorization", authHeader);

            using var response = await httpClient.SendAsync(request, cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var parameters = HttpUtility.ParseQueryString(content);

            accessToken = parameters.Get("oauth_token") ?? "";
            accessTokenSecret = parameters.Get("oauth_token_secret") ?? "";
        }
        catch (Exception e)
        {
            throw new AuthenticationFailedDiscogsException("OAuth authentication failed while getting the access tokens. See the inner exception for details.", e);
        }

        return (accessToken, accessTokenSecret);
    }

    /// <summary>
    /// Created the timestamp and nonce used by the <see cref="PlainOAuthAuthenticationProvider.CreateAuthenticationHeader"/> method.
    /// </summary>
    private (string timestamp, string nonce) CreateTimestampAndNonce()
    {
        var elapsedTimeSince1970 = DateTime.UtcNow - new DateTime(1970, 1, 1);

        var timestamp = (long)elapsedTimeSince1970.TotalSeconds;
        var nonce = (long)elapsedTimeSince1970.TotalMilliseconds;

        return (timestamp.ToString(), nonce.ToString());
    }
}
