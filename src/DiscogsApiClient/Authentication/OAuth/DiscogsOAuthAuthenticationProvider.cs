using System.Globalization;
using System.Net;
using System.Web;
using Microsoft.Extensions.Options;

namespace DiscogsApiClient.Authentication.OAuth;

internal sealed class DiscogsOAuthAuthenticationProvider(IHttpClientFactory httpClientFactory, IOptions<DiscogsOAuthOptions> options)
    : IDiscogsOAuthAuthenticationProvider,
      IDiscogsAuthenticationProvider
{
    public const string HttpClientName = nameof(DiscogsOAuthAuthenticationProvider);

    private sealed record TokenState(string AccessToken, string AccessTokenSecret);

    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(HttpClientName);
    private readonly DiscogsOAuthOptions _oAuthOptions = options.Value;
    private volatile TokenState? _tokenState;

    public bool IsAuthenticated => _tokenState is not null;

    public async Task<OAuthAuthenticationSession> StartAuthentication(CancellationToken cancellationToken)
    {
        var verifierCallbackUrl = _oAuthOptions.VerifierCallbackUrl;
        if (string.IsNullOrWhiteSpace(verifierCallbackUrl))
        {
            throw new InvalidOperationException($"A valid {nameof(DiscogsOAuthOptions.VerifierCallbackUrl)} must be specified in the {nameof(DiscogsOAuthOptions)}.");
        }

        var (requestToken, requestTokenSecret) = await GetRequestToken(_httpClient, verifierCallbackUrl, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(requestToken) || string.IsNullOrWhiteSpace(requestTokenSecret))
        {
            throw new AuthenticationFailedDiscogsException("Getting request token failed.");
        }

        var authorizeUrl = $"https://discogs.com/oauth/authorize?oauth_token={requestToken}";
        return new(authorizeUrl, verifierCallbackUrl, requestToken, requestTokenSecret);
    }

    public async Task<(string AccessToken, string AccessTokenSecret)> CompleteAuthentication(
        OAuthAuthenticationSession session,
        string verifierToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentException.ThrowIfNullOrWhiteSpace(session.RequestToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(session.RequestTokenSecret);
        ArgumentException.ThrowIfNullOrWhiteSpace(verifierToken);

        var (accessToken, accessTokenSecret) = await GetAccessToken(_httpClient, session.RequestToken, session.RequestTokenSecret, verifierToken, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(accessTokenSecret))
        {
            throw new AuthenticationFailedDiscogsException("Failed getting access token.");
        }

        _tokenState = new(accessToken, accessTokenSecret);

        return (accessToken, accessTokenSecret);
    }

    public void Authenticate(string accessToken, string accessTokenSecret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(accessTokenSecret);

        _tokenState = new(accessToken, accessTokenSecret);
    }

    public string CreateAuthenticationHeader()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_oAuthOptions.ConsumerKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(_oAuthOptions.ConsumerSecret);

        var tokenState = _tokenState
            ?? throw new UnauthenticatedDiscogsException($"The {nameof(DiscogsOAuthAuthenticationProvider)} must be authenticated before creating an authentication header.");

        (var timestamp, var nonce) = CreateTimestampAndNonce();

        var header = "OAuth ";
        header += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_oAuthOptions.ConsumerKey)}\",";
        header += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
        header += $"oauth_token=\"{WebUtility.UrlEncode(tokenState.AccessToken)}\",";
        header += $"oauth_signature=\"{WebUtility.UrlEncode($"{_oAuthOptions.ConsumerSecret}&{tokenState.AccessTokenSecret}")}\",";
        header += $"oauth_signature_method=\"PLAINTEXT\",";
        header += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\"";

        return header;
    }

    private async Task<(string requestToken, string requestTokenSecret)> GetRequestToken(HttpClient httpClient, string callback, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_oAuthOptions.ConsumerKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(_oAuthOptions.ConsumerSecret);

        var requestToken = "";
        var requestTokenSecret = "";

        try
        {
            (var timestamp, var nonce) = CreateTimestampAndNonce();

            var authHeader = "OAuth ";
            authHeader += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_oAuthOptions.ConsumerKey)}\",";
            authHeader += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
            authHeader += $"oauth_signature=\"{WebUtility.UrlEncode($"{_oAuthOptions.ConsumerSecret}&")}\",";
            authHeader += $"oauth_signature_method=\"PLAINTEXT\",";
            authHeader += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\",";
            authHeader += $"oauth_callback=\"{WebUtility.UrlEncode(callback)}\"";

            using var request = new HttpRequestMessage(HttpMethod.Get, "/oauth/request_token");
            request.Headers.Accept.Add(new("application/x-www-form-urlencoded"));
            request.Headers.Add("Authorization", authHeader);

            using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

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

    private async Task<(string accessToken, string accessTokenSecret)> GetAccessToken(HttpClient httpClient, string requestToken, string requestTokenSecret, string verifier, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_oAuthOptions.ConsumerKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(_oAuthOptions.ConsumerSecret);

        var accessToken = "";
        var accessTokenSecret = "";

        try
        {
            (var timestamp, var nonce) = CreateTimestampAndNonce();

            var authHeader = "OAuth ";
            authHeader += $"oauth_consumer_key=\"{WebUtility.UrlEncode(_oAuthOptions.ConsumerKey)}\",";
            authHeader += $"oauth_nonce=\"{WebUtility.UrlEncode(nonce)}\",";
            authHeader += $"oauth_token=\"{WebUtility.UrlEncode(requestToken)}\",";
            authHeader += $"oauth_signature=\"{WebUtility.UrlEncode($"{_oAuthOptions.ConsumerSecret}&{requestTokenSecret}")}\",";
            authHeader += $"oauth_signature_method=\"PLAINTEXT\",";
            authHeader += $"oauth_timestamp=\"{WebUtility.UrlEncode(timestamp)}\",";
            authHeader += $"oauth_verifier=\"{WebUtility.UrlEncode(verifier)}\"";

            using var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/access_token");
            request.Headers.Accept.Add(new("application/x-www-form-urlencoded"));
            request.Headers.Add("Authorization", authHeader);

            using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

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

    private static (string timestamp, string nonce) CreateTimestampAndNonce()
    {
        var elapsedTimeSince1970 = DateTime.UtcNow - DateTime.UnixEpoch;

        var timestamp = (long)elapsedTimeSince1970.TotalSeconds;
        var nonce = (long)elapsedTimeSince1970.TotalMilliseconds;

        return (timestamp.ToString(CultureInfo.InvariantCulture), nonce.ToString(CultureInfo.InvariantCulture));
    }
}
