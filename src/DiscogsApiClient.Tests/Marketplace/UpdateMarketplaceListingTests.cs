namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class UpdateMarketplaceListingTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    private static MarketplaceListingCreateRequest CreateBaselineRequest(string comments)
        => new(
            ReleaseId: 5134861,
            Condition: MarketplaceListingCondition.NearMint,
            Price: 100m,
            Status: MarketplaceListingStatus.Draft,
            SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
            Comments: comments,
            AllowOffers: false,
            ExternalId: "API_TEST_EXTERNAL_ID",
            Location: "API_TEST_LOCATION",
            Weight: 100,
            FormatQuantity: 1);

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task UpdateMarketplaceListing_ShouldThrowException_WhenListingIdIsInvalid(long listingId, CancellationToken cancellationToken)
    {
        var request = new MarketplaceListingUpdateRequest(
            ReleaseId: 5134861,
            Condition: MarketplaceListingCondition.NearMint,
            Price: 100m,
            Status: MarketplaceListingStatus.Draft,
            SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
            Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_INVALID_LISTING_ID",
            AllowOffers: false,
            ExternalId: "API_TEST_EXTERNAL_ID",
            Location: "API_TEST_LOCATION",
            Weight: 100,
            FormatQuantity: 1);

        var exception = await Assert.That(async () => await _apiClient.UpdateMarketplaceListing(listingId, request, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("listingId");
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldThrowException_WhenRequestIsNull(CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateMarketplaceListing(1, null!, cancellationToken))
            .Throws<ArgumentNullException>();

        await Assert.That(exception!.ParamName).IsEqualTo("request");
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldThrowResourceNotFoundException_WhenListingDoesNotExist(CancellationToken cancellationToken)
    {
        var listingId = 999999999;

        var request = new MarketplaceListingUpdateRequest(
            ReleaseId: 5134861,
            Condition: MarketplaceListingCondition.NearMint,
            Price: 100m,
            Status: MarketplaceListingStatus.Draft,
            SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
            Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_NOT_FOUND",
            AllowOffers: false,
            ExternalId: "API_TEST_EXTERNAL_ID",
            Location: "API_TEST_LOCATION",
            Weight: 100,
            FormatQuantity: 1);

        await Assert.That(async () => await _apiClient.UpdateMarketplaceListing(listingId, request, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Item not found. It may have been deleted.");
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldThrowUnauthenticatedException_WhenCallerIsNotAuthenticated(CancellationToken cancellationToken)
    {
        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_UNAUTHENTICATED"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: 5134861,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_UNAUTHENTICATED",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1);

            await Assert.That(async () => await _unauthenticatedApiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken))
                .Throws<UnauthenticatedDiscogsException>()
                .WithMessage("You must authenticate to access this resource.");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldUpdateListing_WhenRequestIsValid(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_BEFORE"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.VeryGoodPlus,
                Price: 200m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.VeryGoodPlus,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_AFTER",
                AllowOffers: true,
                ExternalId: "API_TEST_EXTERNAL_ID_UPDATED",
                Location: "API_TEST_LOCATION_UPDATED",
                Weight: 200,
                FormatQuantity: 2);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.Id).IsEqualTo(createResponse.ListingId);
            await Assert.That(updatedListing.Status).IsEqualTo("Draft");
            await Assert.That(updatedListing.Condition).IsEqualTo(MarketplaceListingCondition.VeryGoodPlus);
            await Assert.That(updatedListing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.VeryGoodPlus);
            await Assert.That(updatedListing.Comments).IsEqualTo("API_TEST_UPDATE_MARKETPLACE_LISTING_AFTER");
            await Assert.That(updatedListing.AllowOffers).IsTrue();
            await Assert.That(updatedListing.ExternalId).IsEqualTo("API_TEST_EXTERNAL_ID_UPDATED");
            await Assert.That(updatedListing.Location).IsEqualTo("API_TEST_LOCATION_UPDATED");
            await Assert.That(updatedListing.Weight).IsEqualTo(200);
            await Assert.That(updatedListing.FormatQuantity).IsEqualTo(2);
            // Quantity is read-only for this seller's account tier and always reports 1, regardless of FormatQuantity.
            await Assert.That(updatedListing.Quantity).IsEqualTo(1);
            await Assert.That(updatedListing.Price.Value).IsEqualTo(200m);
            await Assert.That(updatedListing.Release.Id).IsEqualTo(releaseId);
            await Assert.That(updatedListing.Seller.Username).IsEqualTo("DamIDhagor");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldPreserveExistingValues_WhenOptionalFieldsAreNull(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_NULL_FIELDS_BEFORE"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: null,
                Comments: null,
                AllowOffers: null,
                ExternalId: null,
                Location: null,
                Weight: null,
                FormatQuantity: null);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.Id).IsEqualTo(createResponse.ListingId);
            await Assert.That(updatedListing.Status).IsEqualTo("Draft");
            await Assert.That(updatedListing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
            await Assert.That(updatedListing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.NearMint);
            await Assert.That(updatedListing.Comments).IsEqualTo("API_TEST_UPDATE_MARKETPLACE_LISTING_NULL_FIELDS_BEFORE");
            await Assert.That(updatedListing.AllowOffers).IsFalse();
            await Assert.That(updatedListing.ExternalId).IsEqualTo("API_TEST_EXTERNAL_ID");
            await Assert.That(updatedListing.Location).IsEqualTo("API_TEST_LOCATION");
            await Assert.That(updatedListing.Weight).IsEqualTo(100);
            await Assert.That(updatedListing.EstimatedWeight).IsEqualTo(85);
            await Assert.That(updatedListing.FormatQuantity).IsEqualTo(1);
            await Assert.That(updatedListing.Quantity).IsEqualTo(1);
            await Assert.That(updatedListing.Price.Value).IsEqualTo(100m);
            await Assert.That(updatedListing.Release.Id).IsEqualTo(releaseId);
            await Assert.That(updatedListing.Seller.Username).IsEqualTo("DamIDhagor");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldUpdateListing_WhenOptionalStringFieldsAreEmpty(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_EMPTY_FIELDS_BEFORE"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: string.Empty,
                AllowOffers: false,
                ExternalId: string.Empty,
                Location: string.Empty,
                Weight: 100,
                FormatQuantity: 1);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.Id).IsEqualTo(createResponse.ListingId);
            await Assert.That(updatedListing.Status).IsEqualTo("Draft");
            await Assert.That(updatedListing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
            await Assert.That(updatedListing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.NearMint);
            await Assert.That(updatedListing.Comments).IsEqualTo(string.Empty);
            await Assert.That(updatedListing.AllowOffers).IsFalse();
            await Assert.That(updatedListing.ExternalId).IsEqualTo(string.Empty);
            await Assert.That(updatedListing.Location).IsEqualTo(string.Empty);
            await Assert.That(updatedListing.Weight).IsEqualTo(100);
            await Assert.That(updatedListing.FormatQuantity).IsEqualTo(1);
            await Assert.That(updatedListing.Quantity).IsEqualTo(1);
            await Assert.That(updatedListing.Price.Value).IsEqualTo(100m);
            await Assert.That(updatedListing.Release.Id).IsEqualTo(releaseId);
            await Assert.That(updatedListing.Seller.Username).IsEqualTo("DamIDhagor");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldIgnoreReleaseId_WhenReleaseIdIsNegative(CancellationToken cancellationToken)
    {
        var originalReleaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_NEGATIVE_RELEASE_ID"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: -1,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_NEGATIVE_RELEASE_ID",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.Release.Id).IsEqualTo(originalReleaseId);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldIgnoreReleaseId_WhenReleaseIdIsZero(CancellationToken cancellationToken)
    {
        var originalReleaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_ZERO_RELEASE_ID"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: 0,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_ZERO_RELEASE_ID",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.Release.Id).IsEqualTo(originalReleaseId);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldUpdateListing_WhenPriceIsNegative(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_NEGATIVE_PRICE"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: -1m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_NEGATIVE_PRICE",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.Price.Value).IsEqualTo(1m);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldThrowException_WhenPriceIsZero(CancellationToken cancellationToken)
    {
        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_ZERO_PRICE"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: 5134861,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 0.00m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_ZERO_PRICE",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1);

            await Assert.That(async () => await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("Invalid price: expected float.");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldUpdateListing_WhenWeightIsNegative(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_NEGATIVE_WEIGHT"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_NEGATIVE_WEIGHT",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: -1,
                FormatQuantity: 1);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.Weight).IsEqualTo(85);
            await Assert.That(updatedListing.EstimatedWeight).IsEqualTo(85);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldPreserveExistingWeight_WhenWeightIsZero(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_ZERO_WEIGHT"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_ZERO_WEIGHT",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 0,
                FormatQuantity: 1);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.Weight).IsEqualTo(100);
            await Assert.That(updatedListing.EstimatedWeight).IsEqualTo(85);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldThrowException_WhenFormatQuantityIsNegative(CancellationToken cancellationToken)
    {
        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_NEGATIVE_FORMAT_QUANTITY"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: 5134861,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_NEGATIVE_FORMAT_QUANTITY",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: -1);

            await Assert.That(async () => await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("Format quantity cannot be negative");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateMarketplaceListing_ShouldUpdateListing_WhenFormatQuantityIsZero(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            CreateBaselineRequest("API_TEST_UPDATE_MARKETPLACE_LISTING_ZERO_FORMAT_QUANTITY"),
            cancellationToken);

        try
        {
            var request = new MarketplaceListingUpdateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_UPDATE_MARKETPLACE_LISTING_ZERO_FORMAT_QUANTITY",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 0);

            await _apiClient.UpdateMarketplaceListing(createResponse.ListingId, request, cancellationToken);

            var updatedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(updatedListing.FormatQuantity).IsEqualTo(1);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }
}
