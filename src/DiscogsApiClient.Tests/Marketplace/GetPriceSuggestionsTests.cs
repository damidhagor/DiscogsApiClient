namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetPriceSuggestionsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task GetPriceSuggestions_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetPriceSuggestions(releaseId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    public async Task GetPriceSuggestions_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.GetPriceSuggestions(5134861, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task GetPriceSuggestions_ShouldReturnSuggestionsByCondition_WhenReleaseExists(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var suggestions = await _apiClient.GetPriceSuggestions(releaseId, cancellationToken);

        await Assert.That(suggestions).IsNotNull();
        await Assert.That(suggestions.Count).IsEqualTo(8);

        await Assert.That(suggestions["Mint (M)"].Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(suggestions["Mint (M)"].Value).IsEqualTo(9.405m);

        await Assert.That(suggestions["Near Mint (NM or M-)"].Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(suggestions["Near Mint (NM or M-)"].Value).IsEqualTo(8.415000000000001m);

        await Assert.That(suggestions["Very Good Plus (VG+)"].Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(suggestions["Very Good Plus (VG+)"].Value).IsEqualTo(6.4350000000000005m);

        await Assert.That(suggestions["Very Good (VG)"].Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(suggestions["Very Good (VG)"].Value).IsEqualTo(4.455000000000001m);

        await Assert.That(suggestions["Good Plus (G+)"].Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(suggestions["Good Plus (G+)"].Value).IsEqualTo(2.475m);

        await Assert.That(suggestions["Good (G)"].Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(suggestions["Good (G)"].Value).IsEqualTo(1.485m);

        await Assert.That(suggestions["Fair (F)"].Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(suggestions["Fair (F)"].Value).IsEqualTo(0.99m);

        await Assert.That(suggestions["Poor (P)"].Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(suggestions["Poor (P)"].Value).IsEqualTo(0.495m);
    }

    [Test]
    public async Task GetPriceSuggestions_ShouldReturnEmptyResponse_WhenReleaseHasNoSuggestions(CancellationToken cancellationToken)
    {
        var releaseId = 38022159;

        var suggestions = await _apiClient.GetPriceSuggestions(releaseId, cancellationToken);

        await Assert.That(suggestions).IsNotNull();
        await Assert.That(suggestions.Count).IsEqualTo(0);
    }
}
