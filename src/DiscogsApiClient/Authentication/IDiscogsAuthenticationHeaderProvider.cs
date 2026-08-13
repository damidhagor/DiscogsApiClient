namespace DiscogsApiClient.Authentication;

internal interface IDiscogsAuthenticationHeaderProvider
{
    bool IsAuthenticated { get; }

    string GetHeader();
}
