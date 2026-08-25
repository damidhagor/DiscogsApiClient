namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetInventoryExportTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task GetInventoryExport_ShouldThrowArgumentOutOfRangeException_WhenExportIdIsInvalid(long exportId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _unauthenticatedApiClient.GetInventoryExport(exportId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("exportId");
    }

    [Test]
    public async Task GetInventoryExport_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.GetInventoryExport(1, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    public async Task GetInventoryExport_ShouldThrowResourceNotFoundException_WhenExportDoesNotExist(CancellationToken cancellationToken)
    {
        var exportId = 999999999;

        await Assert.That(async () => await _apiClient.GetInventoryExport(exportId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested resource was not found.");
    }

    [Test]
    public async Task GetInventoryExport_ShouldReturnExport_WhenExportExists(CancellationToken cancellationToken)
    {
        var exportId = 18866724;

        var export = await _apiClient.GetInventoryExport(exportId, cancellationToken);

        await Assert.That(export).IsNotNull();
        await Assert.That(export!.Id).IsEqualTo(exportId);
        await Assert.That(export.Status).IsEqualTo("success");
        await Assert.That(export.Url).IsEqualTo("https://api.discogs.com/inventory/export/18866724");
        await Assert.That(export.Filename).IsEqualTo("DamIDhagor-inventory-20260825-0648.csv");
        await Assert.That(export.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 6, 48, 8, DateTimeKind.Unspecified));
        await Assert.That(export.FinishedAt).IsEqualTo(new DateTime(2026, 8, 25, 6, 48, 8, DateTimeKind.Unspecified));
        await Assert.That(export.DownloadUrl).IsEqualTo("https://api.discogs.com/inventory/export/18866724/download");
    }
}
