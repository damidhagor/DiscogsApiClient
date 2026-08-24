namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetMarketplaceStatsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task GetMarketplaceStats_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _unauthenticatedApiClient.GetMarketplaceStats(releaseId, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    public async Task GetMarketplaceStats_ShouldReturnStats_WhenReleaseHasItemsForSale(CancellationToken cancellationToken)
    {
        var releaseId = 249504;

        var stats = await _unauthenticatedApiClient.GetMarketplaceStats(releaseId, null, cancellationToken);

        await Assert.That(stats).IsNotNull();
        await Assert.That(stats.BlockedFromSale).IsFalse();
        await Assert.That(stats.NumForSale).IsEqualTo(120);

        var lowestPrice = await Assert.That(stats.LowestPrice).IsNotNull();
        await Assert.That(lowestPrice.Currency).IsEqualTo(MarketplaceCurrency.Usd);
        await Assert.That(lowestPrice.Value).IsEqualTo(0.68m);
    }

    [Test]
    public async Task GetMarketplaceStats_ShouldConvertLowestPrice_WhenCurrencyQueryParameterIsProvided(CancellationToken cancellationToken)
    {
        var releaseId = 249504;

        var stats = await _unauthenticatedApiClient.GetMarketplaceStats(
            releaseId,
            new(MarketplaceCurrency.Eur),
            cancellationToken);

        var lowestPrice = await Assert.That(stats.LowestPrice).IsNotNull();
        await Assert.That(lowestPrice.Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(lowestPrice.Value).IsEqualTo(0.59m);
        await Assert.That(stats.NumForSale).IsEqualTo(120);
        await Assert.That(stats.BlockedFromSale).IsFalse();
    }

    [Test]
    public async Task GetMarketplaceStats_ShouldReturnEmptyStats_WhenReleaseHasNoItemsForSale(CancellationToken cancellationToken)
    {
        var releaseId = 38022159;

        var stats = await _unauthenticatedApiClient.GetMarketplaceStats(releaseId, null, cancellationToken);

        await Assert.That(stats).IsNotNull();
        await Assert.That(stats.BlockedFromSale).IsFalse();
        await Assert.That(stats.NumForSale).IsEqualTo(0);
        await Assert.That(stats.LowestPrice).IsNull();
    }
}
