namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class CreateInventoryExportTests(DiscogsApiClientFixture fixture)
{
    private const string InventoryUploadMutationConstraint = "InventoryUploadMutation";

    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task CreateInventoryExport_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.CreateInventoryExport(cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    [NotInParallel(InventoryUploadMutationConstraint)]
    public async Task CreateInventoryExport_ShouldCreateExport_WhenRequested(CancellationToken cancellationToken)
    {
        var createResponse = await _apiClient.CreateMarketplaceListing(
            new MarketplaceListingCreateRequest(
                ReleaseId: 5134861,
                Condition: MarketplaceListingCondition.NearMint,
                Price: 100m,
                Status: MarketplaceListingStatus.Draft,
                SleeveCondition: MarketplaceListingSleeveCondition.NearMint,
                Comments: "API_TEST_CREATE_INVENTORY_EXPORT",
                AllowOffers: false,
                ExternalId: "API_TEST_EXTERNAL_ID",
                Location: "API_TEST_LOCATION",
                Weight: 100,
                FormatQuantity: 1),
            cancellationToken);

        try
        {
            var exportId = await _apiClient.CreateInventoryExport(cancellationToken);
            await Assert.That(exportId).IsGreaterThan(0);

            InventoryExport? export = null;

            for (var attempt = 0; attempt < 20 && export?.FinishedAt is null; attempt++)
            {
                await Task.Delay(500, cancellationToken);
                export = await _apiClient.GetInventoryExport(exportId, cancellationToken);
            }

            await Assert.That(export).IsNotNull();
            await Assert.That(export!.Id).IsEqualTo(exportId);
            await Assert.That(export.Status).IsEqualTo("success");
            await Assert.That(export.Url).IsEqualTo($"https://api.discogs.com/inventory/export/{exportId}");
            await Assert.That(export.Filename).IsEqualTo("DamIDhagor-inventory-20260825-0704.csv");
            await Assert.That(export.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 7, 4, 23, DateTimeKind.Unspecified));
            await Assert.That(export.FinishedAt).IsEqualTo(new DateTime(2026, 8, 25, 7, 4, 23, DateTimeKind.Unspecified));
            await Assert.That(export.DownloadUrl).IsEqualTo($"https://api.discogs.com/inventory/export/{exportId}/download");
        }
        finally
        {
            await _apiClient.DeleteMarketplaceListing(createResponse.ListingId, cancellationToken);
        }
    }
}
