using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace DiscogsApiClient.Authentication.OAuth;

/// <summary>
/// This <see cref="IAuthenticationProvider"/> implementation authenticates against the Discogs Api
/// using the OAuth 1.0a flow described <a href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">here</a>
/// and should be provided to the <see cref="DiscogsApiClient"/>'s constructor.
/// </summary>
public sealed class OAuthAuthenticationProvider : IOAuthAuthenticationProvider
{
    private readonly HttpClient _httpClient;
    private readonly DiscogsApiClientOptions _discogsOptions;
    private string _accessToken = "";
    private string _accessTokenSecret = "";

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_accessToken) && !string.IsNullOrWhiteSpace(_accessTokenSecret);

    public OAuthAuthenticationProvider(HttpClient httpClient, DiscogsApiClientOptions discogsOptions)
    {
        _httpClient = httpClient;
        _discogsOptions = discogsOptions;
    }

    /// <inheritdoc/>
    /// <exception cref="AuthenticationFailedDiscogsException"></exception>
    public async Task<OAuthAuthenticationSession> StartAuthentication(CancellationToken cancellationToken)
    {
        var verifierCallbackUrl = _discogsOptions.VerifierCallbackUrl;
        if (string.IsNullOrWhiteSpace(verifierCallbackUrl))
        {
            throw new ArgumentException($"A valid {nameof(DiscogsApiClientOptions.VerifierCallbackUrl)} must be specified in the {nameof(DiscogsApiClientOptions)}.", nameof(DiscogsApiClientOptions.VerifierCallbackUrl));
        }

        var (requestToken, requestTokenSecret) = await GetRequestToken(_httpClient, verifierCallbackUrl, cancellationToken);
        if (string.IsNullOrWhiteSpace(requestToken) || string.IsNullOrWhiteSpace(requestTokenSecret))
            throw new AuthenticationFailedDiscogsException("Getting request token failed.");

        var authorizeUrl = $"https://discogs.com/oauth/authorize?oauth_token={requestToken}";
        return new(authorizeUrl, verifierCallbackUrl, requestToken, requestTokenSecret);
    }

    /// <inheritdoc/>
    /// <exception cref="AuthenticationFailedDiscogsException"></exception>
    public async Task<(string AccessToken, string AccessTokenSecret)> CompleteAuthentication(
        OAuthAuthenticationSession session,
        string verifierToken,
        CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(session.RequestToken, nameof(OAuthAuthenticationSession.RequestToken));
        Guard.IsNotNullOrWhiteSpace(session.RequestTokenSecret, nameof(OAuthAuthenticationSession.RequestTokenSecret));
        Guard.IsNotNullOrWhiteSpace(verifierToken);

        var (accessToken, accessTokenSecret) = await GetAccessToken(_httpClient, session.RequestToken, session.RequestTokenSecret, verifierToken, cancellationToken);
        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(accessTokenSecret))
            throw new AuthenticationFailedDiscogsException("Failed getting access token.");

        _accessToken = accessToken;
        _accessTokenSecret = accessTokenSecret;

        return (_accessToken, _accessTokenSecret);
    }

    /// <inheritdoc/>
    public void Authenticate(
        string accessToken,
        string accessTokenSecret)
    {
        Guard.IsNotNullOrWhiteSpace(accessToken);
        Guard.IsNotNullOrWhiteSpace(accessTokenSecret);

        _accessToken = accessToken;
        _accessTokenSecret = accessTokenSecret;
    }

    public string CreateAuthenticationHeader()
    {
        Guard.IsNotNullOrWhiteSpace(_discogsOptions.ConsumerKey);
        Guard.IsNotNullOrWhiteSpace(_discogsOptions.ConsumerSecret);

        if (!IsAuthenticated)
            throw new UnauthenticatedDiscogsException($"The {nameof(OAuthAuthenticationProvider)} must be authenticated before creating an authentication header.");

        (var timestamp, var nonce) = CreateTimestampAndNonce();

        var header = "OAuth ";
        header += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_discogsOptions.ConsumerKey)}\",";
        header += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
        header += $"oauth_token=\"{WebUtility.UrlEncode(_accessToken)}\",";
        header += $"oauth_signature=\"{WebUtility.UrlEncode($"{_discogsOptions.ConsumerSecret}&{_accessTokenSecret}")}\",";
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
        Guard.IsNotNullOrWhiteSpace(_discogsOptions.ConsumerKey, nameof(DiscogsApiClientOptions.ConsumerKey));
        Guard.IsNotNullOrWhiteSpace(_discogsOptions.ConsumerSecret, nameof(DiscogsApiClientOptions.ConsumerSecret));

        var requestToken = "";
        var requestTokenSecret = "";

        try
        {
            (var timestamp, var nonce) = CreateTimestampAndNonce();

            var authHeader = "OAuth ";
            authHeader += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_discogsOptions.ConsumerKey)}\",";
            authHeader += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
            authHeader += $"oauth_signature=\"{WebUtility.UrlEncode($"{_discogsOptions.ConsumerSecret}&")}\",";
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
    /// Gets the access token and secret from the Discogs api with the request token nd secret obtained earlier in the flow.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used by the authentication flow.</param>
    /// <param name="requestToken">The request token obtained earlier in the flow.</param>
    /// <param name="requestTokenSecret">The request secret obtained earlier in the flow.</param>
    /// <param name="verifier">The verifier token obtained earlier in the flow.</param>
    /// <returns>The access token and secret which authenticate the logged in user.</returns>
    private async Task<(string accessToken, string accessTokenSecret)> GetAccessToken(HttpClient httpClient, string requestToken, string requestTokenSecret, string verifier, CancellationToken cancellationToken)
    {
        Guard.IsNotNullOrWhiteSpace(_discogsOptions.ConsumerKey, nameof(DiscogsApiClientOptions.ConsumerKey));
        Guard.IsNotNullOrWhiteSpace(_discogsOptions.ConsumerSecret, nameof(DiscogsApiClientOptions.ConsumerSecret));

        var accessToken = "";
        var accessTokenSecret = "";

        try
        {
            (var timestamp, var nonce) = CreateTimestampAndNonce();

            var authHeader = "OAuth ";
            authHeader += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_discogsOptions.ConsumerKey)}\",";
            authHeader += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
            authHeader += $"oauth_token=\"{WebUtility.UrlEncode(requestToken)}\",";
            authHeader += $"oauth_signature=\"{WebUtility.UrlEncode($"{_discogsOptions.ConsumerSecret}&{requestTokenSecret}")}\",";
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
