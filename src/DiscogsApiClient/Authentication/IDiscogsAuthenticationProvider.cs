namespace DiscogsApiClient.Authentication;

internal interface IDiscogsAuthenticationProvider
{
    bool IsAuthenticated { get; }

    string CreateAuthenticationHeader();
}
