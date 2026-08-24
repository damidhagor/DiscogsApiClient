namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetMarketplaceFeeWithCurrencyTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task GetMarketplaceFee_ShouldThrowArgumentOutOfRangeException_WhenPriceIsInvalid(decimal price, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetMarketplaceFee(price, MarketplaceCurrency.Eur, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("price");
    }

    [Test]
    public async Task GetMarketplaceFee_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.GetMarketplaceFee(10m, MarketplaceCurrency.Eur, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task GetMarketplaceFee_ShouldReturnFeeInRequestedCurrency_WhenCurrencyIsProvided(CancellationToken cancellationToken)
    {
        var fee = await _apiClient.GetMarketplaceFee(10m, MarketplaceCurrency.Eur, cancellationToken);

        await Assert.That(fee).IsNotNull();
        await Assert.That(fee.Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(fee.Value).IsEqualTo(0.9m);
    }
}
