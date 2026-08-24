namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetMarketplaceFeeTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task GetMarketplaceFee_ShouldThrowArgumentOutOfRangeException_WhenPriceIsInvalid(decimal price, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetMarketplaceFee(price, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("price");
    }

    [Test]
    public async Task GetMarketplaceFee_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.GetMarketplaceFee(10m, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task GetMarketplaceFee_ShouldReturnFee_WhenPriceIsValid(CancellationToken cancellationToken)
    {
        var fee = await _apiClient.GetMarketplaceFee(10m, cancellationToken);

        await Assert.That(fee).IsNotNull();
        await Assert.That(fee.Currency).IsEqualTo(MarketplaceCurrency.Usd);
        await Assert.That(fee.Value).IsEqualTo(0.9m);
    }
}
