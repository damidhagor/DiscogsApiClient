using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace DiscogsApiClient.Authentication.PlainOAuth;

/// <summary>
/// This <see cref="IAuthenticationProvider"/> implementation authenticates against the Discogs Api
/// using the OAuth 1.0a flow described <a href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">here</a>
/// and should be provided to the <see cref="DiscogsApiClient"/>'s constructor.
/// </summary>
public class PlainOAuthAuthenticationProvider : IAuthenticationProvider
{
    private string _userAgent = "";
    private string _consumerKey = "";
    private string _consumerSecret = "";
    private string _accessToken = "";
    private string _accessTokenSecret = "";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool IsAuthenticated => !String.IsNullOrWhiteSpace(_accessToken) && !String.IsNullOrWhiteSpace(_accessTokenSecret);


    /// <summary>
    /// Authenticates the client by requesting him to log in with his Discogs account.
    /// <para/>
    /// This method returns the obtained access token and secret if successful
    /// which should be persisted and reused by the app.
    /// </summary>
    /// <param name="authenticationRequest">The <see cref="PlainOAuthAuthenticationRequest"/> providing the callback delegate, url and OAuth parameters.</param>
    /// <returns>The <see cref="PlainOAuthAuthenticationResponse"/> indicating if the authentication was successful.</returns>
    /// <exception cref="ArgumentException">Fires this exception if the provided <see cref="IAuthenticationRequest"/> is not a <see cref="PlainOAuthAuthenticationRequest"/>.</exception>
    /// <exception cref="InvalidOperationException">Fires this exception if no consumer key or secret are provided.</exception>
    public async Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
    {
        if (authenticationRequest is not PlainOAuthAuthenticationRequest authAuthenticationRequest)
            throw new ArgumentException($"The provided authentication request must be of type {typeof(PlainOAuthAuthenticationRequest).Name}", nameof(authenticationRequest));

        _userAgent = authAuthenticationRequest.UserAgent;
        _consumerKey = authAuthenticationRequest.ConsumerKey;
        _consumerSecret = authAuthenticationRequest.ConsumerSecret;
        _accessToken = authAuthenticationRequest.AccessToken;
        _accessTokenSecret = authAuthenticationRequest.AccessTokenSecret;

        if (String.IsNullOrWhiteSpace(_accessToken) || String.IsNullOrWhiteSpace(_accessTokenSecret))
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

    /// <summary>
    /// Creates an authenticated <see cref="HttpRequestMessage"/> with an added OAuth authorization header.
    /// </summary>
    /// <param name="httpMethod"><inheritdoc/></param>
    /// <param name="url"><inheritdoc/></param>
    /// <returns><inheritdoc/></returns>
    public HttpRequestMessage CreateAuthenticatedRequest(HttpMethod httpMethod, string url)
    {
        var request = new HttpRequestMessage(httpMethod, url);
        request.Headers.Add("Authorization", CreateAuthenticationHeader());

        return request;
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

    /// <summary>
    /// Gets the verifier token from the app by notifying the app via the provided <see cref="GetVerifierCallback"/>
    /// which login url should be opened and at which redirect url the verifier token is returned.
    /// </summary>
    /// <param name="requestToken">The request token obtained earlier in the flow.</param>
    /// <param name="callback">The callback url at which to return the verifier token.</param>
    /// <param name="getVerifier">The callback for the app to implement the login process.</param>
    /// <returns>The veriefier token.</returns>
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


    /// <summary>
    /// Creates the <see cref="HttpRequestMessage"/> for getting the request token from the Discogs api.
    /// </summary>
    /// <param name="callback">The callback url at which the reqtest token is returned later.</param>
    private HttpRequestMessage CreateRequestTokenRequest(string callback)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, DiscogsApiUrls.OAuthRequestTokenUrl);

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

    /// <summary>
    /// Creates the <see cref="HttpRequestMessage"/> for getting the access token and secret from the Discogs api.
    /// </summary>
    /// <param name="requestToken">The obtained request token.</param>
    /// <param name="requestTokenSecret">The obtained request token secret.</param>
    /// <param name="verifier">The obtained verifier token.</param>
    private HttpRequestMessage CreateAccessTokenRequest(string requestToken, string requestTokenSecret, string verifier)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, DiscogsApiUrls.OAuthAccessTokenUrl);

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


    /// <summary>
    /// Creates the value needed for the authentication header for an authenticated request to the Discogs Api.
    /// </summary>
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

    /// <summary>
    /// Created the timestamp and nonce used by the <see cref="PlainOAuthAuthenticationProvider.CreateAuthenticationHeader"/> method.
    /// </summary>
    private (string timestamp, string nonce) CreateTimestampAndNonce()
    {
        TimeSpan ellapsedTimeSince1970 = DateTime.UtcNow - new DateTime(1970, 1, 1);

        long timestamp = (long)ellapsedTimeSince1970.TotalSeconds;
        long nonce = (long)ellapsedTimeSince1970.TotalMilliseconds;

        return (timestamp.ToString(), nonce.ToString());
    }
}