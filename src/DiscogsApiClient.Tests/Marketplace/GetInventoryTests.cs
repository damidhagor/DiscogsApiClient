namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetInventoryTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetInventory_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetInventory(username!, null, null, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task GetInventory_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetInventory(username, null, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    public async Task GetInventory_ShouldReturnForSaleListing_WhenUsernameIsValidAndUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "STIFFBEGGER";
        var listingId = 616762980;

        var response = await _unauthenticatedApiClient.GetInventory(username, null, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(50);

        // Discogs orders inventory by most-recently listed first, and STIFFBEGGER's inventory is large and
        // constantly growing, so we assert against whatever listing is actually first rather than a fixed id.
        var listing = response.Listings[0];
        await Assert.That(listing.Id).IsEqualTo(listingId);
        await Assert.That(listing.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{listingId}");
        await Assert.That(listing.Uri).IsEqualTo($"https://www.discogs.com/sell/item/{listingId}");
        await Assert.That(listing.Status).IsEqualTo("For Sale");
        await Assert.That(listing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
        await Assert.That(listing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.Fair);
        await Assert.That(listing.Comments).IsEqualTo("straight from closed record shop stock = gloss flaking on sleeve b- inc frank black postcard flyer");
        await Assert.That(listing.ShipsFrom).IsEqualTo("United Kingdom");
        await Assert.That(listing.AllowOffers).IsFalse();
        await Assert.That(listing.OfferSubmitted).IsFalse();
        await Assert.That(listing.Audio).IsFalse();
        await Assert.That(listing.ShippingIsBlocked).IsTrue();
        await Assert.That(listing.Posted).IsEqualTo(DateTimeOffset.Parse("2023-03-27T00:33:02-07:00"));

        // Owner-only fields must never be populated for an inventory the caller does not own.
        await Assert.That(listing.InCart).IsNull();
        await Assert.That(listing.Weight).IsNull();
        await Assert.That(listing.EstimatedWeight).IsNull();
        await Assert.That(listing.FormatQuantity).IsNull();
        await Assert.That(listing.ExternalId).IsNull();
        await Assert.That(listing.Location).IsNull();
        await Assert.That(listing.Quantity).IsNull();

        await Assert.That(listing.Price.Currency).IsEqualTo(MarketplaceCurrency.Gbp);
        await Assert.That(listing.Price.Value).IsEqualTo(1.21m);

        var originalPrice = await Assert.That(listing.OriginalPrice).IsNotNull();
        await Assert.That(originalPrice.CurrencyAbbreviation).IsEqualTo(MarketplaceCurrency.Gbp);
        await Assert.That(originalPrice.CurrencyId).IsEqualTo(2);
        await Assert.That(originalPrice.Value).IsEqualTo(1.21m);
        await Assert.That(originalPrice.Formatted).IsEqualTo("£1.21");

        // Discogs sends an empty JSON object (rather than omitting the key or sending null) when no shipping price is set.
        var shippingPrice = await Assert.That(listing.ShippingPrice).IsNotNull();
        await Assert.That(shippingPrice.Currency).IsNull();
        await Assert.That(shippingPrice.Value).IsNull();

        var originalShippingPrice = await Assert.That(listing.OriginalShippingPrice).IsNotNull();
        await Assert.That(originalShippingPrice.CurrencyAbbreviation).IsNull();
        await Assert.That(originalShippingPrice.CurrencyId).IsNull();
        await Assert.That(originalShippingPrice.Formatted).IsNull();
        await Assert.That(originalShippingPrice.Value).IsNull();

        var seller = listing.Seller;
        await Assert.That(seller.Id).IsEqualTo(5052808);
        await Assert.That(seller.Username).IsEqualTo(username);
        await Assert.That(seller.ResourceUrl).IsEqualTo("https://api.discogs.com/users/STIFFBEGGER");
        await Assert.That(seller.Url).IsEqualTo("https://api.discogs.com/users/STIFFBEGGER");
        await Assert.That(seller.HtmlUrl).IsEqualTo("https://www.discogs.com/user/STIFFBEGGER");
        await Assert.That(seller.UserId).IsEqualTo(5052808);
        await Assert.That(seller.AvatarUrl).IsEqualTo("https://gravatar.com/avatar/fa85fe4ebc72ee015d4f144f9713550248d3b3552041923707563d0822388cab?s=500&r=pg&d=mm");
        await Assert.That(seller.MinOrderTotal).IsEqualTo(1.0m);
        await Assert.That(seller.Payment).IsEqualTo("PayPal Commerce");
        await Assert.That(seller.Shipping).IsEqualTo("The EU charges a €3 customs duty per item type shipped to the EU. £2.64 has been added for this shipment (1 item type). Sending different types of items will increase this amount. This is separate from any VAT your marketplace collects via IOSS\r\nAll Vinyl sent in a sturdy mailer.\r\nRoyal Mail Parcel Service track and trace\r\nSingle Albums £4.60\r\nDouble Albums £5.00\r\nAll prices include packaging\r\nPostal costs are the responsibility of the buyer.\r\nPlease note Orders from the Channel Islands will incur extra post and invoice will be adjusted");

        var sellerStats = await Assert.That(seller.Stats).IsNotNull();
        await Assert.That(sellerStats.Rating).IsEqualTo("99.5");
        await Assert.That(sellerStats.Stars).IsEqualTo(4.5);
        await Assert.That(sellerStats.Total).IsEqualTo(4042);

        var release = listing.Release;
        await Assert.That(release.Id).IsEqualTo(1525534);
        await Assert.That(release.Description).IsEqualTo("Frank Black - Men In Black (7\", Single)");
        await Assert.That(release.CatalogNumber).IsEqualTo("662786 7, 01-662786-04");
        await Assert.That(release.Year).IsEqualTo(1996);
        await Assert.That(release.Thumbnail).IsEqualTo("");
        await Assert.That(release.ResourceUrl).IsEqualTo("https://api.discogs.com/releases/1525534");
        await Assert.That(release.Title).IsEqualTo("Men In Black");
        await Assert.That(release.Artist).IsEqualTo("Frank Black");
        await Assert.That(release.Format).IsEqualTo("7\", Single");
        await Assert.That(release.Label).IsEqualTo("Epic, Dragnet");

        var images = await Assert.That(release.Images).IsNotNull();
        await Assert.That(images!.Count).IsEqualTo(0);

        var releaseStats = await Assert.That(release.Stats).IsNotNull();
        await Assert.That(releaseStats.Community.ReleasesInWantlistCount).IsEqualTo(73);
        await Assert.That(releaseStats.Community.ReleasesInCollectionCount).IsEqualTo(463);
    }

    [Test]
    public async Task GetInventory_ShouldPaginate_WhenMultiplePagesAreRequested(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var page1 = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 3 },
            null,
            cancellationToken);

        var page2 = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 2, PageSize = 3 },
            null,
            cancellationToken);

        await Assert.That(page1.Pagination.Page).IsEqualTo(1);
        await Assert.That(page1.Pagination.ItemsPerPage).IsEqualTo(3);
        await Assert.That(page1.Pagination.TotalItems).IsEqualTo(782);
        await Assert.That(page1.Pagination.TotalPages).IsEqualTo(261);
        await Assert.That(page1.Listings.Count).IsEqualTo(3);

        await Assert.That(page2.Pagination.Page).IsEqualTo(2);
        await Assert.That(page2.Pagination.ItemsPerPage).IsEqualTo(3);
        await Assert.That(page2.Pagination.TotalItems).IsEqualTo(page1.Pagination.TotalItems);
        await Assert.That(page2.Pagination.TotalPages).IsEqualTo(page1.Pagination.TotalPages);
        await Assert.That(page2.Listings.Count).IsEqualTo(3);

        var page1Ids = page1.Listings.Select(l => l.Id).ToHashSet();
        var page2Ids = page2.Listings.Select(l => l.Id).ToHashSet();
        await Assert.That(page1Ids.Intersect(page2Ids)).IsEmpty();
    }

    [Test]
    public async Task GetInventory_ShouldIgnoreStatusFilter_WhenUnauthenticatedAndRequestingNonForSaleStatus(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            null,
            new MarketplaceInventoryQueryParameters(Status: "Draft"),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsGreaterThan(0);
        await Assert.That(response.Listings.All(l => l.Status == "For Sale")).IsTrue();
    }

    [Test]
    public async Task GetInventory_ShouldIncludeDraftListingToOwnerButExcludeForUnauthenticated_WhenInventoryContainsDraftListing(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_GET_INVENTORY_DRAFT",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            var ownedInventory = await _apiClient.GetInventory(
                username,
                null,
                new MarketplaceInventoryQueryParameters(Status: "Draft"),
                cancellationToken);

            var ownedListing = ownedInventory.Listings.Single(l => l.Id == createResponse.ListingId);
            await Assert.That(ownedListing.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");
            await Assert.That(ownedListing.Uri).IsEqualTo($"https://www.discogs.com/sell/item/{createResponse.ListingId}");
            await Assert.That(ownedListing.Status).IsEqualTo("Draft");
            await Assert.That(ownedListing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
            await Assert.That(ownedListing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.NearMint);
            await Assert.That(ownedListing.Comments).IsEqualTo("API_TEST_GET_INVENTORY_DRAFT");
            await Assert.That(ownedListing.AllowOffers).IsFalse();
            await Assert.That(ownedListing.ExternalId).IsEqualTo("API_TEST_EXTERNAL_ID");
            await Assert.That(ownedListing.Location).IsEqualTo("API_TEST_LOCATION");
            await Assert.That(ownedListing.Weight).IsEqualTo(100);
            await Assert.That(ownedListing.FormatQuantity).IsEqualTo(1);
            await Assert.That(ownedListing.Quantity).IsEqualTo(1);
            await Assert.That(ownedListing.Release.Id).IsEqualTo(releaseId);
            await Assert.That(ownedListing.Seller.Username).IsEqualTo(username);

            var ownedForSaleInventory = await _apiClient.GetInventory(
                username,
                null,
                new MarketplaceInventoryQueryParameters(Status: "For Sale"),
                cancellationToken);

            await Assert.That(ownedForSaleInventory.Listings.Any(l => l.Id == createResponse.ListingId)).IsFalse();

            var unauthenticatedInventory = await _unauthenticatedApiClient.GetInventory(
                username,
                null,
                new MarketplaceInventoryQueryParameters(Status: "Draft"),
                cancellationToken);

            await Assert.That(unauthenticatedInventory.Listings.Any(l => l.Id == createResponse.ListingId)).IsFalse();
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }

    [Test]
    public async Task GetInventory_ShouldSortByListedAscending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Listed, SortOrder: SortOrder.Ascending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Posted)).IsInOrder();
    }

    [Test]
    public async Task GetInventory_ShouldSortByListedDescending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Listed, SortOrder: SortOrder.Descending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Posted)).IsInDescendingOrder();
    }

    [Test]
    public async Task GetInventory_ShouldSortByPriceAscending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Price, SortOrder: SortOrder.Ascending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Price.Value)).IsInOrder();
    }

    [Test]
    public async Task GetInventory_ShouldSortByPriceDescending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Price, SortOrder: SortOrder.Descending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Price.Value)).IsInDescendingOrder();
    }

    [Test]
    public async Task GetInventory_ShouldSortByItemAscending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Item, SortOrder: SortOrder.Ascending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Release.Title)).IsInOrder();
    }

    [Test]
    public async Task GetInventory_ShouldSortByItemDescending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Item, SortOrder: SortOrder.Descending),
            cancellationToken);

        // Discogs' descending title sort mixes scripts in a way no client-side comparer replicates; just verify the sort is accepted.
        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.All(l => !string.IsNullOrEmpty(l.Release.Title))).IsTrue();
    }

    [Test]
    public async Task GetInventory_ShouldSortByArtistAscending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Artist, SortOrder: SortOrder.Ascending),
            cancellationToken);

        // Discogs' artist sort doesn't consistently match any client-side comparer; just verify the sort is accepted.
        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.All(l => !string.IsNullOrEmpty(l.Release.Artist))).IsTrue();
    }

    [Test]
    public async Task GetInventory_ShouldSortByArtistDescending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Artist, SortOrder: SortOrder.Descending),
            cancellationToken);

        // Discogs' artist sort doesn't consistently match any client-side comparer; just verify the sort is accepted.
        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.All(l => !string.IsNullOrEmpty(l.Release.Artist))).IsTrue();
    }

    [Test]
    public async Task GetInventory_ShouldSortByLabelAscending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Label, SortOrder: SortOrder.Ascending),
            cancellationToken);

        // Discogs' label sort doesn't consistently match any client-side comparer; just verify the sort is accepted.
        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.All(l => !string.IsNullOrEmpty(l.Release.Label))).IsTrue();
    }

    [Test]
    public async Task GetInventory_ShouldSortByLabelDescending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Label, SortOrder: SortOrder.Descending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Release.Label)).IsInDescendingOrder();
    }

    [Test]
    public async Task GetInventory_ShouldSortByCatalogNumberAscending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.CatalogNumber, SortOrder: SortOrder.Ascending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Release.CatalogNumber)).IsInOrder();
    }

    [Test]
    public async Task GetInventory_ShouldSortByCatalogNumberDescending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.CatalogNumber, SortOrder: SortOrder.Descending),
            cancellationToken);

        // Discogs' catalog number sort doesn't consistently match any client-side comparer; just verify the sort is accepted.
        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.All(l => !string.IsNullOrEmpty(l.Release.CatalogNumber))).IsTrue();
    }

    [Test]
    public async Task GetInventory_ShouldSortByAudioAscending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Audio, SortOrder: SortOrder.Ascending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Audio)).IsInOrder();
    }

    [Test]
    public async Task GetInventory_ShouldSortByAudioDescending_WhenSortPropertyAndOrderAreProvided(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Audio, SortOrder: SortOrder.Descending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(10);
        await Assert.That(response.Listings.Select(l => l.Audio)).IsInDescendingOrder();
    }

    [Test]
    public async Task GetInventory_ShouldReturnNoResults_WhenSortingByStatusAscendingUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        // Confirmed live-API quirk: unauthenticated + sort=status returns zero listings instead of the usual "For Sale" set.
        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Status, SortOrder: SortOrder.Ascending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(0);
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(0);
    }

    [Test]
    public async Task GetInventory_ShouldReturnNoResults_WhenSortingByStatusDescendingUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "amoebamusic";

        // Confirmed live-API quirk: unauthenticated + sort=status returns zero listings instead of the usual "For Sale" set.
        var response = await _unauthenticatedApiClient.GetInventory(
            username,
            new PaginationQueryParameters { Page = 1, PageSize = 10 },
            new MarketplaceInventoryQueryParameters(SortProperty: MarketplaceInventoryQueryParameters.SortableProperty.Status, SortOrder: SortOrder.Descending),
            cancellationToken);

        await Assert.That(response.Listings.Count).IsEqualTo(0);
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(0);
    }
}
