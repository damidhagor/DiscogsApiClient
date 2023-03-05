//using System.Net;
//using System.Net.Http.Json;
//using System.Text.Json;

//namespace DiscogsApiClient;

///// <summary>
///// The Discogs api client for making requests to the Discogs api.
///// <para/>
///// It needs an <see cref="IAuthenticationProvider"/> and an initial call to <see cref="DiscogsApiClient.AuthenticateAsync"/>
///// with the corresponding <see cref="IAuthenticationRequest"/> to make the authenticated requests.
///// </summary>
//public sealed partial class DiscogsApiClient : IDiscogsApiClient
//{
//    private readonly HttpClient _httpClient;
//    private readonly JsonSerializerOptions _jsonSerializerOptions;
//    private readonly IAuthenticationProvider _authenticationProvider;

//    public bool IsAuthenticated => _authenticationProvider.IsAuthenticated;


//    /// <summary>
//    /// Creates a new <see cref="DiscogsApiClient"/> with injectable options.
//    /// </summary>
//    /// <param name="httpClient">The HttpClient to be used for the requests.</param>
//    /// <param name="authenticationProvider">An implementation of the <see cref="IAuthenticationProvider"/> for the authentication method to be used.</param>
//    public DiscogsApiClient(HttpClient httpClient, IAuthenticationProvider authenticationProvider)
//    {
//        _httpClient = httpClient;
//        _authenticationProvider = authenticationProvider;
//        _jsonSerializerOptions = DiscogsSerializerOptions.Options;
//    }

//    public async Task<IAuthenticationResponse> AuthenticateAsync(IAuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
//        => await _authenticationProvider.AuthenticateAsync(authenticationRequest, cancellationToken);
//}
