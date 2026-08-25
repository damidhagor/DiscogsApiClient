namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class DownloadInventoryExportAsStreamTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task DownloadInventoryExportAsStream_ShouldThrowArgumentOutOfRangeException_WhenExportIdIsInvalid(long exportId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _unauthenticatedApiClient.DownloadInventoryExportAsStream(exportId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("exportId");
    }

    [Test]
    public async Task DownloadInventoryExportAsStream_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.DownloadInventoryExportAsStream(1, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    public async Task DownloadInventoryExportAsStream_ShouldThrowResourceNotFoundException_WhenExportDoesNotExist(CancellationToken cancellationToken)
    {
        var exportId = 999999999;

        await Assert.That(async () => await _apiClient.DownloadInventoryExportAsStream(exportId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested resource was not found.");
    }

    [Test]
    public async Task DownloadInventoryExportAsStream_ShouldDownloadExport_WhenExportHasNoListings(CancellationToken cancellationToken)
    {
        var exportId = 18866724;

        using var stream = await _apiClient.DownloadInventoryExportAsStream(exportId, cancellationToken);
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync(cancellationToken);

        await Assert.That(content).IsEqualTo(
            "listing_id,artist,title,label,catno,format,release_id,status,price,listed,comments,media_condition,sleeve_condition,accept_offer,external_id,weight,format_quantity,location,quantity\r\n");
    }

    [Test]
    public async Task DownloadInventoryExportAsStream_ShouldDownloadExport_WhenExportHasListings(CancellationToken cancellationToken)
    {
        var exportId = 18867036;

        using var stream = await _apiClient.DownloadInventoryExportAsStream(exportId, cancellationToken);
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync(cancellationToken);

        await Assert.That(content).IsEqualTo(
            "listing_id,artist,title,label,catno,format,release_id,status,price,listed,comments,media_condition,sleeve_condition,accept_offer,external_id,weight,format_quantity,location,quantity\r\n"
            + "4332778344,HammerFall,Glory To The Brave,Nuclear Blast,NB 265-2,\"CD, Album, Promo\",5134861,Draft,100.0,2026-08-25 07:04:22,API_TEST_CREATE_INVENTORY_EXPORT,Near Mint (NM or M-),Near Mint (NM or M-),N,API_TEST_EXTERNAL_ID,100,1,API_TEST_LOCATION,1\r\n");
    }
}
