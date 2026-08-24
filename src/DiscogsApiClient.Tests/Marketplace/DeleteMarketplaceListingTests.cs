namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class DeleteMarketplaceListingTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task DeleteMarketplaceListing_ShouldThrowException_WhenListingIdIsInvalid(long listingId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.DeleteMarketplaceListing(listingId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("listingId");
    }

    [Test]
    public async Task DeleteMarketplaceListing_ShouldThrowResourceNotFoundException_WhenListingDoesNotExist(CancellationToken cancellationToken)
    {
        var listingId = 999999999;

        await Assert.That(async () => await _apiClient.DeleteMarketplaceListing(listingId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Item not found. It may have been deleted.");
    }

    [Test]
    public async Task DeleteMarketplaceListing_ShouldThrowUnauthenticatedException_WhenCallerIsNotAuthenticated(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_DELETE_MARKETPLACE_LISTING_UNAUTHENTICATED",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            await Assert.That(async () => await _unauthenticatedApiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken))
                .Throws<UnauthenticatedDiscogsException>()
                .WithMessage("You must authenticate to access this resource.");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task DeleteMarketplaceListing_ShouldDeleteListing_WhenListingExists(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_DELETE_MARKETPLACE_LISTING",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1),
            cancellationToken);

        await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);

        await Assert.That(async () => await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Item not found. It may have been deleted.");
    }
}
