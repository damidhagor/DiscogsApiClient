using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace DiscogsApiClient;

/// <summary>
/// The Discogs api client for making requests to the Discogs api.
/// <para/>
/// It needs an <see cref="IAuthenticationProvider"/> and an initial call to <see cref="DiscogsApiClient.AuthenticateAsync"/>
/// with the corresponding <see cref="IAuthenticationRequest"/> to make the authenticated requests.
/// </summary>
public sealed partial class DiscogsApiClient : IDiscogsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationProvider _authenticationProvider;

    /// <inheritdoc/>
    public bool IsAuthenticated => _authenticationProvider.IsAuthenticated;


    /// <summary>
    /// Creates a new <see cref="DiscogsApiClient"/> with injectable options.
    /// </summary>
    /// <param name="httpClient">The HttpClient to be used for the requests.</param>
    /// <param name="authenticationProvider">An implementation of the <see cref="IAuthenticationProvider"/> for the authentication method to be used.</param>
    public DiscogsApiClient(HttpClient httpClient, IAuthenticationProvider authenticationProvider)
    {
        _httpClient = httpClient;
        _authenticationProvider = authenticationProvider;
    }

    /// <inheritdoc/>
    public async Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
    {
        return await _authenticationProvider.AuthenticateAsync(authenticationRequest, cancellationToken);
    }

    private async Task<T> GetAsync<T>(
#if NET7_0
        [StringSyntax(StringSyntaxAttribute.Uri)] string url,
#else
        string url,
#endif
        CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var result = await response.Content.DeserializeAsJsonAsync<T>(cancellationToken);

        return result;
    }

    private async Task<T> PostAsync<T>(
#if NET7_0
        [StringSyntax(StringSyntaxAttribute.Uri)] string url,
#else
        string url,
#endif
        object? content,
        CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Post, url);

        if (content is not null)
        {
            request.Content = CreateJsonContent(content);
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var result = await response.Content.DeserializeAsJsonAsync<T>(cancellationToken);

        return result;
    }

    private async Task<T> PutAsync<T>(
#if NET7_0
        [StringSyntax(StringSyntaxAttribute.Uri)] string url,
#else
        string url,
#endif
        object? content,
        CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Put, url);

        if (content is not null)
        {
            request.Content = CreateJsonContent(content);
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var result = await response.Content.DeserializeAsJsonAsync<T>(cancellationToken);

        return result;
    }

    private async Task<bool> DeleteAsync(
#if NET7_0
        [StringSyntax(StringSyntaxAttribute.Uri)] string url,
#else
        string url,
#endif
        CancellationToken cancellationToken)
    {
        using var request = _authenticationProvider.CreateAuthenticatedRequest(HttpMethod.Delete, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);
        await response.CheckAndHandleHttpErrorCodes(cancellationToken);
        return response.StatusCode == HttpStatusCode.NoContent;
    }

    /// <summary>
    /// Helper method to create a Json payload for a request.
    /// </summary>
    /// <typeparam name="T">The type of the serialized payload.</typeparam>
    /// <param name="payload">The payload to serialize.</param>
    /// <returns>the serialized payload in a <see cref="StringContent"/>.</returns>
    private StringContent CreateJsonContent<T>(T payload)
    {
        string json = payload.SerializeAsJson<T>();

        var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        return stringContent;
    }
}
