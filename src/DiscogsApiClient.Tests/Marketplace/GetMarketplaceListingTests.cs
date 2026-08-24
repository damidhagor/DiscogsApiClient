namespace DiscogsApiClient.Tests.Marketplace;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetMarketplaceListingTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task GetMarketplaceListing_ShouldThrowException_WhenListingIdIsInvalid(long listingId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetMarketplaceListing(listingId, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("listingId");
    }

    [Test]
    public async Task GetMarketplaceListing_ShouldThrowResourceNotFoundException_WhenListingDoesNotExist(CancellationToken cancellationToken)
    {
        var listingId = 999999999;

        await Assert.That(async () => await _apiClient.GetMarketplaceListing(listingId, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Item not found. It may have been deleted.");
    }

    [Test]
    public async Task GetMarketplaceListing_ShouldReturnListing_WhenListingExistsAndUnauthenticated(CancellationToken cancellationToken)
    {
        var listingId = 4075375078;

        var listing = await _unauthenticatedApiClient.GetMarketplaceListing(listingId, null, cancellationToken);

        await Assert.That(listing).IsNotNull();
        await Assert.That(listing.Id).IsEqualTo(listingId);
        await Assert.That(listing.Status).IsEqualTo("For Sale");
        await Assert.That(listing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
        await Assert.That(listing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.VeryGood);
        await Assert.That(listing.Comments).IsEqualTo("centre pushed out");
        await Assert.That(listing.ShipsFrom).IsEqualTo("United Kingdom");
        await Assert.That(listing.AllowOffers).IsFalse();
        await Assert.That(listing.OfferSubmitted).IsFalse();
        await Assert.That(listing.Audio).IsFalse();
        await Assert.That(listing.ShippingIsBlocked).IsTrue();
        await Assert.That(listing.Posted).IsEqualTo(DateTimeOffset.Parse("2026-03-17T07:41:55-07:00"));
        await Assert.That(listing.Uri).IsEqualTo("https://www.discogs.com/sell/item/4075375078");
        await Assert.That(listing.ResourceUrl).IsEqualTo("https://api.discogs.com/marketplace/listings/4075375078");

        // Owner-only fields must never be populated for a listing the caller does not own.
        await Assert.That(listing.InCart).IsNull();
        await Assert.That(listing.Weight).IsNull();
        await Assert.That(listing.FormatQuantity).IsNull();
        await Assert.That(listing.ExternalId).IsNull();
        await Assert.That(listing.Location).IsNull();
        await Assert.That(listing.Quantity).IsNull();

        await Assert.That(listing.Price.Currency).IsEqualTo(MarketplaceCurrency.Usd);
        await Assert.That(listing.Price.Value).IsEqualTo(2.5616438356164384m);

        var originalPrice = await Assert.That(listing.OriginalPrice).IsNotNull();
        await Assert.That(originalPrice.CurrencyAbbreviation).IsEqualTo(MarketplaceCurrency.Gbp);
        await Assert.That(originalPrice.CurrencyId).IsEqualTo(2);
        await Assert.That(originalPrice.Value).IsEqualTo(1.87m);
        await Assert.That(originalPrice.Formatted).IsEqualTo("£1.87");

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
        await Assert.That(seller.Username).IsEqualTo("STIFFBEGGER");
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
        await Assert.That(release.Id).IsEqualTo(249504);
        await Assert.That(release.Description).IsEqualTo("Rick Astley - Never Gonna Give You Up (7\", Single)");
        await Assert.That(release.CatalogNumber).IsEqualTo("PB 41447");
        await Assert.That(release.Year).IsEqualTo(1987);
        await Assert.That(release.Thumbnail).IsEqualTo("https://i.discogs.com/HG2xChKN-rIHHSfgL53W9z2vJWeFknfevpOMwSHtIaM/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTI0OTUw/NC0xMzM0NTkyMjEy/LmpwZWc.jpeg");
        await Assert.That(release.ResourceUrl).IsEqualTo("https://api.discogs.com/releases/249504");
        await Assert.That(release.Title).IsEqualTo("Never Gonna Give You Up");
        await Assert.That(release.Artist).IsEqualTo("Rick Astley");
        await Assert.That(release.Format).IsEqualTo("7\", Single");
        await Assert.That(release.Label).IsEqualTo("RCA");

        var images = await Assert.That(release.Images).IsNotNull();
        await Assert.That(images!.Count).IsEqualTo(4);

        var image = images[0];
        await Assert.That(image.Type).IsEqualTo(ImageType.Primary);
        await Assert.That(image.ResourceUrl).IsEqualTo("https://i.discogs.com/-DPFA5hKT8i91jnjn4rLB1zSiuUBFTrGWspu1TpLV30/rs:fit/g:sm/q:90/h:600/w:600/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTI0OTUw/NC0xMzM0NTkyMjEy/LmpwZWc.jpeg");
        await Assert.That(image.ImageUri).IsEqualTo("https://i.discogs.com/-DPFA5hKT8i91jnjn4rLB1zSiuUBFTrGWspu1TpLV30/rs:fit/g:sm/q:90/h:600/w:600/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTI0OTUw/NC0xMzM0NTkyMjEy/LmpwZWc.jpeg");
        await Assert.That(image.ImageUri150).IsEqualTo("https://i.discogs.com/HG2xChKN-rIHHSfgL53W9z2vJWeFknfevpOMwSHtIaM/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTI0OTUw/NC0xMzM0NTkyMjEy/LmpwZWc.jpeg");
        await Assert.That(image.Width).IsEqualTo(600);
        await Assert.That(image.Height).IsEqualTo(600);

        var releaseStats = await Assert.That(release.Stats).IsNotNull();
        await Assert.That(releaseStats.Community.ReleasesInWantlistCount).IsEqualTo(584);
        await Assert.That(releaseStats.Community.ReleasesInCollectionCount).IsEqualTo(4102);
    }

    [Test]
    public async Task GetMarketplaceListing_ShouldConvertPrice_WhenCurrencyQueryParameterIsProvided(CancellationToken cancellationToken)
    {
        var listingId = 4075375078;

        var listing = await _unauthenticatedApiClient.GetMarketplaceListing(
            listingId,
            new(MarketplaceCurrency.Eur),
            cancellationToken);

        await Assert.That(listing.Price.Currency).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(listing.Price.Value).IsEqualTo(2.2030136986301367m);

        var originalPrice = await Assert.That(listing.OriginalPrice).IsNotNull();
        await Assert.That(originalPrice.CurrencyAbbreviation).IsEqualTo(MarketplaceCurrency.Gbp);
        await Assert.That(originalPrice.Value).IsEqualTo(1.87m);
        await Assert.That(originalPrice.Formatted).IsEqualTo("£1.87");
    }

    [Test]
    public async Task GetMarketplaceListing_ShouldReturnDraftToOwnerButRequireAuthenticationForUnauthenticated_WhenListingIsDraft(CancellationToken cancellationToken)
    {
        var releaseId = 5134861;

        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: releaseId,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_GET_MARKETPLACE_LISTING_DRAFT",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            var ownedListing = await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken);

            await Assert.That(ownedListing.Id).IsEqualTo(createResponse.ListingId);
            await Assert.That(ownedListing.ResourceUrl).IsEqualTo($"https://api.discogs.com/marketplace/listings/{createResponse.ListingId}");
            await Assert.That(ownedListing.Uri).IsEqualTo($"https://www.discogs.com/sell/item/{createResponse.ListingId}");
            await Assert.That(ownedListing.Status).IsEqualTo("Draft");
            await Assert.That(ownedListing.Condition).IsEqualTo(MarketplaceListingCondition.NearMint);
            await Assert.That(ownedListing.SleeveCondition).IsEqualTo(MarketplaceListingSleeveCondition.NearMint);
            await Assert.That(ownedListing.Comments).IsEqualTo("API_TEST_GET_MARKETPLACE_LISTING_DRAFT");
            await Assert.That(ownedListing.ShipsFrom).IsEqualTo("Germany");
            await Assert.That(ownedListing.Posted).IsEqualTo(DateTimeOffset.Parse("2026-08-24T02:28:38-07:00"));
            await Assert.That(ownedListing.AllowOffers).IsFalse();
            await Assert.That(ownedListing.OfferSubmitted).IsFalse();
            await Assert.That(ownedListing.Audio).IsFalse();
            await Assert.That(ownedListing.ShippingIsBlocked).IsTrue();
            await Assert.That(ownedListing.InCart).IsFalse();
            await Assert.That(ownedListing.ExternalId).IsEqualTo("API_TEST_EXTERNAL_ID");
            await Assert.That(ownedListing.Location).IsEqualTo("API_TEST_LOCATION");
            await Assert.That(ownedListing.Weight).IsEqualTo(100);
            await Assert.That(ownedListing.EstimatedWeight).IsEqualTo(85);
            await Assert.That(ownedListing.FormatQuantity).IsEqualTo(1);
            await Assert.That(ownedListing.Quantity).IsEqualTo(1);

            await Assert.That(ownedListing.Price.Currency).IsEqualTo(MarketplaceCurrency.Eur);
            await Assert.That(ownedListing.Price.Value).IsEqualTo(100m);

            var originalPrice = await Assert.That(ownedListing.OriginalPrice).IsNotNull();
            await Assert.That(originalPrice.CurrencyAbbreviation).IsEqualTo(MarketplaceCurrency.Eur);
            await Assert.That(originalPrice.CurrencyId).IsEqualTo(3);
            await Assert.That(originalPrice.Value).IsEqualTo(100m);
            await Assert.That(originalPrice.Formatted).IsEqualTo("€100.00");

            // Discogs sends an empty JSON object (rather than omitting the key or sending null) when no shipping price is set.
            var shippingPrice = await Assert.That(ownedListing.ShippingPrice).IsNotNull();
            await Assert.That(shippingPrice.Currency).IsNull();
            await Assert.That(shippingPrice.Value).IsNull();

            var originalShippingPrice = await Assert.That(ownedListing.OriginalShippingPrice).IsNotNull();
            await Assert.That(originalShippingPrice.CurrencyAbbreviation).IsNull();
            await Assert.That(originalShippingPrice.CurrencyId).IsNull();
            await Assert.That(originalShippingPrice.Formatted).IsNull();
            await Assert.That(originalShippingPrice.Value).IsNull();

            var seller = ownedListing.Seller;
            await Assert.That(seller.Id).IsEqualTo(12579295);
            await Assert.That(seller.Username).IsEqualTo("DamIDhagor");
            await Assert.That(seller.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
            await Assert.That(seller.Url).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
            await Assert.That(seller.HtmlUrl).IsEqualTo("https://www.discogs.com/user/DamIDhagor");
            await Assert.That(seller.UserId).IsEqualTo(12579295);
            await Assert.That(seller.AvatarUrl).IsEqualTo("https://i.discogs.com/W0OR6Z5OewEwKpar3QyLVCuXbhw2_uNLIwahkmSKqNk/rs:fill/g:sm/q:40/h:500/w:500/czM6Ly9kaXNjb2dz/LXVzZXItYXZhdGFy/cy9VLTEyNTc5Mjk1/LTE2Mzk3ODM2MTIu/anBlZw.jpeg");
            await Assert.That(seller.MinOrderTotal).IsEqualTo(0m);
            await Assert.That(seller.Payment).IsEqualTo("PayPal Commerce");
            await Assert.That(seller.Shipping).IsEqualTo("Shipping\r\nOrders are shipped within 4 business days of receiving cleared payment. All items are securely packed to protect against damage in transit.\r\n\r\nReturns & Refunds\r\nI accept returns within 7 days of receipt if the item is not as described, damaged, or defective. Please contact me before returning any item. If the return is due to my error (wrong item, inaccurate grading, or damage in transit), I will cover the return shipping cost. Refunds are issued once the returned item is received in its original condition.\r\n\r\nSeller Status\r\nI am a private/occasional seller selling items from my own collection, not operating as a business. As such, no VAT is charged on my sales.\r\n\r\nGeneral\r\nFeel free to contact me with any questions before ordering.");

            var sellerStats = await Assert.That(seller.Stats).IsNotNull();
            await Assert.That(sellerStats.Rating).IsEqualTo("0.0");
            await Assert.That(sellerStats.Stars).IsEqualTo(0.0);
            await Assert.That(sellerStats.Total).IsEqualTo(0);

            var release = ownedListing.Release;
            await Assert.That(release.Id).IsEqualTo(releaseId);
            await Assert.That(release.Description).IsEqualTo("HammerFall - Glory To The Brave (CD, Album, Promo)");
            await Assert.That(release.CatalogNumber).IsEqualTo("NB 265-2");
            await Assert.That(release.Year).IsEqualTo(1997);
            await Assert.That(release.Thumbnail).IsEqualTo("https://i.discogs.com/z-WOVc6-4Jj7uQfLfZlcOLKmTR6Pdal_cBRJm5WoBKY/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTUxMzQ4/NjEtMTcyMzk4OTE5/Ny04MTkyLmpwZWc.jpeg");
            await Assert.That(release.ResourceUrl).IsEqualTo("https://api.discogs.com/releases/5134861");
            await Assert.That(release.Title).IsEqualTo("Glory To The Brave");
            await Assert.That(release.Artist).IsEqualTo("HammerFall");
            await Assert.That(release.Format).IsEqualTo("CD, Album, Promo");
            await Assert.That(release.Label).IsEqualTo("Nuclear Blast");

            var images = await Assert.That(release.Images).IsNotNull();
            await Assert.That(images!.Count).IsEqualTo(6);

            var releaseStats = await Assert.That(release.Stats).IsNotNull();
            var communityStats = await Assert.That(releaseStats.Community).IsNotNull();
            await Assert.That(communityStats.ReleasesInWantlistCount).IsEqualTo(41);
            await Assert.That(communityStats.ReleasesInCollectionCount).IsEqualTo(403);

            var userStats = await Assert.That(releaseStats.User).IsNotNull();
            await Assert.That(userStats.ReleasesInWantlistCount).IsEqualTo(0);
            await Assert.That(userStats.ReleasesInCollectionCount).IsEqualTo(0);

            await Assert.That(async () => await _unauthenticatedApiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken))
                .Throws<UnauthenticatedDiscogsException>()
                .WithMessage("You must authenticate to access this resource.");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }
}
