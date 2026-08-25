namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetInventoryExportsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetInventoryExports_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.GetInventoryExports(null, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    public async Task GetInventoryExports_ShouldReturnExports_WhenExportsExist(CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetInventoryExports(null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Items).IsNotEmpty();

        var firstExport = response.Items.SingleOrDefault(x => x.Id == 18866724);
        await Assert.That(firstExport).IsNotNull();
        await Assert.That(firstExport!.Status).IsEqualTo("success");
        await Assert.That(firstExport.Url).IsEqualTo("https://api.discogs.com/inventory/export/18866724");
        await Assert.That(firstExport.Filename).IsEqualTo("DamIDhagor-inventory-20260825-0648.csv");
        await Assert.That(firstExport.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 6, 48, 8, DateTimeKind.Unspecified));
        await Assert.That(firstExport.FinishedAt).IsEqualTo(new DateTime(2026, 8, 25, 6, 48, 8, DateTimeKind.Unspecified));
        await Assert.That(firstExport.DownloadUrl).IsEqualTo("https://api.discogs.com/inventory/export/18866724/download");

        var secondExport = response.Items.SingleOrDefault(x => x.Id == 18866802);
        await Assert.That(secondExport).IsNotNull();
        await Assert.That(secondExport!.Status).IsEqualTo("success");
        await Assert.That(secondExport.Url).IsEqualTo("https://api.discogs.com/inventory/export/18866802");
        await Assert.That(secondExport.Filename).IsEqualTo("DamIDhagor-inventory-20260825-0651.csv");
        await Assert.That(secondExport.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 6, 51, 1, DateTimeKind.Unspecified));
        await Assert.That(secondExport.FinishedAt).IsEqualTo(new DateTime(2026, 8, 25, 6, 51, 1, DateTimeKind.Unspecified));
        await Assert.That(secondExport.DownloadUrl).IsEqualTo("https://api.discogs.com/inventory/export/18866802/download");

        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.TotalPages).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(50);
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(2);
    }
}
