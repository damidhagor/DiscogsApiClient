namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class CreateMarketplaceListingTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task CreateMarketplaceListing_ShouldThrowException_WhenRequestIsNull(CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.CreateMarketplaceListing(null!, cancellationToken))
            .Throws<ArgumentNullException>();

        await Assert.That(exception!.ParamName).IsEqualTo("request");
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldThrowUnauthenticatedException_WhenCallerIsNotAuthenticated(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        await Assert.That(async () => await _unauthenticatedApiClient.CreateMarketplaceListing(
                new MarketplaceListingCreateRequest(
                    ReleaseId: releaseId,
                    Condition: MarketplaceListingCondition.NearMint,
                    Price: 100m,
                    Status: MarketplaceListingStatus.Draft,
                    SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                    Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_UNAUTHENTICATED",
                    AllowOffers: false,
                    ExternalId: "API_TEST_EXTERNAL_ID",
                    Location: "API_TEST_LOCATION",
                    Weight: 100,
                    FormatQuantity: 1),
                cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldThrowException_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        var releaseId = 999999999;

        // Discogs returns a generic 422 error (not a 404) for a non-existent release_id.
        await Assert.That(async () => await _apiClient.CreateMarketplaceListing(
                new MarketplaceListingCreateRequest(
                    ReleaseId: releaseId,
                    Condition: MarketplaceListingCondition.NearMint,
                    Price: 100m,
                    Status: MarketplaceListingStatus.Draft,
                    SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                    Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_INVALID_RELEASE",
                    AllowOffers: false,
                    ExternalId: "API_TEST_EXTERNAL_ID",
                    Location: "API_TEST_LOCATION",
                    Weight: 100,
                    FormatQuantity: 1),
                cancellationToken))
            .Throws<DiscogsException>()
            .WithMessage("Invalid release_id: release not found.");
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldCreateDraftListing_WhenRequestIsValid(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_DRAFT",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            await Assert.That(createResponse).IsNotNull();
            await Assert.That(createResponse.ListingId).IsGreaterThan(0);
            await Assert.That(createResponse.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");

            var createdListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(createdListing.Id).IsEqualTo(createResponse.ListingId);
            await Assert.That(createdListing.Status).IsEqualTo("Draft");
            await Assert.That(createdListing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
            await Assert.That(createdListing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.NearMint);
            await Assert.That(createdListing.Comments).IsEqualTo("API_TEST_CREATE_MARKETPLACE_LISTING_DRAFT");
            await Assert.That(createdListing.AllowOffers).IsFalse();
            await Assert.That(createdListing.ExternalId).IsEqualTo("API_TEST_EXTERNAL_ID");
            await Assert.That(createdListing.Location).IsEqualTo("API_TEST_LOCATION");
            await Assert.That(createdListing.Weight).IsEqualTo(100);
            await Assert.That(createdListing.FormatQuantity).IsEqualTo(1);
            await Assert.That(createdListing.Quantity).IsEqualTo(1);
            await Assert.That(createdListing.Price.Value).IsEqualTo(100m);
            await Assert.That(createdListing.Release.Id).IsEqualTo(releaseId);
            await Assert.That(createdListing.Seller.Username).IsEqualTo("DamIDhagor");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldCreateDraftListing_WhenOptionalFieldsAreNull(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
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
                FormatQuantity: null),
            cancellationToken);

        try
        {
            await Assert.That(createResponse).IsNotNull();
            await Assert.That(createResponse.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");

            var createdListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(createdListing.Id).IsEqualTo(createResponse.ListingId);
            await Assert.That(createdListing.Status).IsEqualTo("Draft");
            await Assert.That(createdListing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
            await Assert.That(createdListing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.NotGraded);
            await Assert.That(createdListing.Comments).IsEqualTo(string.Empty);
            await Assert.That(createdListing.AllowOffers).IsFalse();
            await Assert.That(createdListing.ExternalId).IsEqualTo(string.Empty);
            await Assert.That(createdListing.Location).IsEqualTo(string.Empty);
            await Assert.That(createdListing.Weight).IsEqualTo(85);
            await Assert.That(createdListing.EstimatedWeight).IsEqualTo(85);
            await Assert.That(createdListing.FormatQuantity).IsEqualTo(1);
            await Assert.That(createdListing.Quantity).IsEqualTo(1);
            await Assert.That(createdListing.Price.Value).IsEqualTo(100m);
            await Assert.That(createdListing.Release.Id).IsEqualTo(releaseId);
            await Assert.That(createdListing.Seller.Username).IsEqualTo("DamIDhagor");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldCreateDraftListing_WhenOptionalStringFieldsAreEmpty(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
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
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            await Assert.That(createResponse).IsNotNull();
            await Assert.That(createResponse.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");

            var createdListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(createdListing.Id).IsEqualTo(createResponse.ListingId);
            await Assert.That(createdListing.Status).IsEqualTo("Draft");
            await Assert.That(createdListing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
            await Assert.That(createdListing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.NearMint);
            await Assert.That(createdListing.Comments).IsEqualTo(string.Empty);
            await Assert.That(createdListing.AllowOffers).IsFalse();
            await Assert.That(createdListing.ExternalId).IsEqualTo(string.Empty);
            await Assert.That(createdListing.Location).IsEqualTo(string.Empty);
            await Assert.That(createdListing.Weight).IsEqualTo(100);
            await Assert.That(createdListing.FormatQuantity).IsEqualTo(1);
            await Assert.That(createdListing.Quantity).IsEqualTo(1);
            await Assert.That(createdListing.Price.Value).IsEqualTo(100m);
            await Assert.That(createdListing.Release.Id).IsEqualTo(releaseId);
            await Assert.That(createdListing.Seller.Username).IsEqualTo("DamIDhagor");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldThrowException_WhenReleaseIdIsNegative(CancellationToken cancellationToken)
    {
        MarketplaceListingCreateResponse? createResponse = null;

        try
        {
            await Assert.That(async () => createResponse = await _apiClient.CreateMarketplaceListing(
                    new MarketplaceListingCreateRequest(
                        ReleaseId: -1,
                        Condition: MarketplaceListingCondition.NearMint,
                        Price: 100m,
                        Status: MarketplaceListingStatus.Draft,
                        SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                        Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_NEGATIVE_RELEASE_ID",
                        AllowOffers: false,
                        ExternalId: "API_TEST_EXTERNAL_ID",
                        Location: "API_TEST_LOCATION",
                        Weight: 100,
                        FormatQuantity: 1),
                    cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("Invalid release_id: release not found.");
        }
        finally
        {
            if (createResponse is not null)
            {
                await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
            }
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldThrowException_WhenReleaseIdIsZero(CancellationToken cancellationToken)
    {
        MarketplaceListingCreateResponse? createResponse = null;

        try
        {
            await Assert.That(async () => createResponse = await _apiClient.CreateMarketplaceListing(
                    new MarketplaceListingCreateRequest(
                        ReleaseId: 0,
                        Condition: MarketplaceListingCondition.NearMint,
                        Price: 100m,
                        Status: MarketplaceListingStatus.Draft,
                        SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                        Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_ZERO_RELEASE_ID",
                        AllowOffers: false,
                        ExternalId: "API_TEST_EXTERNAL_ID",
                        Location: "API_TEST_LOCATION",
                        Weight: 100,
                        FormatQuantity: 1),
                    cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("Invalid release_id: release not found.");
        }
        finally
        {
            if (createResponse is not null)
            {
                await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
            }
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldCreateDraftListing_WhenPriceIsNegative(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: -1m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_NEGATIVE_PRICE",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            await Assert.That(createResponse).IsNotNull();
            await Assert.That(createResponse.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");

            var createdListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(createdListing.Price.Value).IsEqualTo(1m);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldThrowException_WhenPriceIsZero(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;
        MarketplaceListingCreateResponse? createResponse = null;

        try
        {
            await Assert.That(async () => createResponse = await _apiClient.CreateMarketplaceListing(
                    new MarketplaceListingCreateRequest(
                        ReleaseId: releaseId,
                        Condition: MarketplaceListingCondition.NearMint,
                        Price: 0.00m,
                        Status: MarketplaceListingStatus.Draft,
                        SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                        Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_ZERO_PRICE",
                        AllowOffers: false,
                        ExternalId: "API_TEST_EXTERNAL_ID",
                        Location: "API_TEST_LOCATION",
                        Weight: 100,
                        FormatQuantity: 1),
                    cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("Invalid price: expected float.");
        }
        finally
        {
            if (createResponse is not null)
            {
                await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
            }
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldCreateDraftListing_WhenWeightIsNegative(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_NEGATIVE_WEIGHT",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: -1,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            await Assert.That(createResponse).IsNotNull();
            await Assert.That(createResponse.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");

            var createdListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(createdListing.Weight).IsEqualTo(85);
            await Assert.That(createdListing.EstimatedWeight).IsEqualTo(85);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldCreateDraftListing_WhenWeightIsZero(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_ZERO_WEIGHT",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 0,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            await Assert.That(createResponse).IsNotNull();
            await Assert.That(createResponse.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");

            var createdListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(createdListing.Weight).IsEqualTo(85);
            await Assert.That(createdListing.EstimatedWeight).IsEqualTo(85);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldThrowException_WhenFormatQuantityIsNegative(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;
        MarketplaceListingCreateResponse? createResponse = null;

        try
        {
            await Assert.That(async () => createResponse = await _apiClient.CreateMarketplaceListing(
                    new MarketplaceListingCreateRequest(
                        ReleaseId: releaseId,
                        Condition: MarketplaceListingCondition.NearMint,
                        Price: 100m,
                        Status: MarketplaceListingStatus.Draft,
                        SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                        Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_NEGATIVE_FORMAT_QUANTITY",
                        AllowOffers: false,
                        ExternalId: "API_TEST_EXTERNAL_ID",
                        Location: "API_TEST_LOCATION",
                        Weight: 100,
                        FormatQuantity: -1),
                    cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("Format quantity cannot be negative");
        }
        finally
        {
            if (createResponse is not null)
            {
                await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
            }
        }
    }

    [Test]
    public async Task CreateMarketplaceListing_ShouldCreateDraftListing_WhenFormatQuantityIsZero(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_CREATE_MARKETPLACE_LISTING_ZERO_FORMAT_QUANTITY",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 0),
            cancellationToken);

        try
        {
            await Assert.That(createResponse).IsNotNull();
            await Assert.That(createResponse.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");

            var createdListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(createdListing.FormatQuantity).IsEqualTo(1);
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }
}
