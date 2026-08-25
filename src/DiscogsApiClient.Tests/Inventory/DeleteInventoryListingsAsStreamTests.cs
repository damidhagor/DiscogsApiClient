using System.Text;

namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class DeleteInventoryListingsAsStreamTests(DiscogsApiClientFixture fixture)
{
    private const string InventoryUploadMutationConstraint = "InventoryUploadMutation";

    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task DeleteInventoryListings_ShouldThrowArgumentNullException_WhenCsvContentIsNull(CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _unauthenticatedApiClient.DeleteInventoryListings((Stream)null!, cancellationToken))
            .Throws<ArgumentNullException>();

        await Assert.That(exception!.ParamName).IsEqualTo("csvContent");
    }

    [Test]
    public async Task DeleteInventoryListings_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        using var csvContent = new MemoryStream("listing_id\n1"u8.ToArray());

        await Assert.That(async () => await _unauthenticatedApiClient.DeleteInventoryListings(csvContent, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    [NotInParallel(InventoryUploadMutationConstraint)]
    public async Task DeleteInventoryListings_ShouldSucceed_WhenCsvContentIsEmpty(CancellationToken cancellationToken)
    {
        using var csvContent = new MemoryStream();

        var uploadId = await _apiClient.DeleteInventoryListings(csvContent, cancellationToken);
        await Assert.That(uploadId).IsGreaterThan(0);

        await Task.Delay(500, cancellationToken);
        var upload = await _apiClient.GetInventoryUpload(uploadId, cancellationToken);

        await Assert.That(upload.Status).IsEqualTo("failed");
        await Assert.That(upload.Results).IsEqualTo("ERROR: Uploaded file no longer exists.");
    }

    [Test]
    [NotInParallel(InventoryUploadMutationConstraint)]
    public async Task DeleteInventoryListings_ShouldDeleteListing_WhenListingIdIsValid(CancellationToken cancellationToken)
    {
        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: 5134861,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_DELETE_INVENTORY_LISTING",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            using var csvContent = new MemoryStream(Encoding.UTF8.GetBytes($"listing_id\n{createResponse.ListingId}"));

            var uploadId = await _apiClient.DeleteInventoryListings(csvContent, cancellationToken);
            await Assert.That(uploadId).IsGreaterThan(0);

            await Task.Delay(500, cancellationToken);
            var upload = await _apiClient.GetInventoryUpload(uploadId, cancellationToken);

            await Assert.That(upload.Status).IsEqualTo("success");
            await Assert.That(upload.Results).IsEqualTo("CSV file contains 1 records.<p>Processed 1 records.");
            await Assert.That(upload.Id).IsEqualTo(uploadId);
            await Assert.That(upload.Type).IsEqualTo("delete");
            await Assert.That(upload.Filename).IsEqualTo("upload.csv");
            await Assert.That(upload.CreatedAt).IsGreaterThan(DateTime.MinValue);
            await Assert.That(upload.FinishedAt).IsNotNull();
            await Assert.That(upload.FinishedAt!.Value).IsGreaterThanOrEqualTo(upload.CreatedAt);

            await Assert.That(async () => await _apiClient.GetMarketplaceListing(createResponse.ListingId, null, cancellationToken))
                .Throws<ResourceNotFoundDiscogsException>();
        }
        finally
        {
            try
            {
                await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
            }
            catch (ResourceNotFoundDiscogsException)
            {
                // Already deleted by the bulk delete under test; nothing left to clean up.
            }
        }
    }
}
