using System.Text;

namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class DownloadInventoryExportAsBytesTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task DownloadInventoryExportAsBytes_ShouldThrowArgumentOutOfRangeException_WhenExportIdIsInvalid(long exportId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _unauthenticatedApiClient.DownloadInventoryExportAsBytes(exportId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("exportId");
    }

    [Test]
    public async Task DownloadInventoryExportAsBytes_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.DownloadInventoryExportAsBytes(1, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    public async Task DownloadInventoryExportAsBytes_ShouldThrowResourceNotFoundException_WhenExportDoesNotExist(CancellationToken cancellationToken)
    {
        var exportId = 999999999;

        await Assert.That(async () => await _apiClient.DownloadInventoryExportAsBytes(exportId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested resource was not found.");
    }

    [Test]
    public async Task DownloadInventoryExportAsBytes_ShouldDownloadExport_WhenExportHasNoListings(CancellationToken cancellationToken)
    {
        var exportId = 18866802;

        var bytes = await _apiClient.DownloadInventoryExportAsBytes(exportId, cancellationToken);
        var content = Encoding.UTF8.GetString(bytes);

        await Assert.That(content).IsEqualTo(
            "listing_id,artist,title,label,catno,format,release_id,status,price,listed,comments,media_condition,sleeve_condition,accept_offer,external_id,weight,format_quantity,location,quantity\r\n");
    }

    [Test]
    public async Task DownloadInventoryExportAsBytes_ShouldDownloadExport_WhenExportHasListings(CancellationToken cancellationToken)
    {
        var exportId = 18867036;

        var bytes = await _apiClient.DownloadInventoryExportAsBytes(exportId, cancellationToken);
        var content = Encoding.UTF8.GetString(bytes);

        await Assert.That(content).IsEqualTo(
            "listing_id,artist,title,label,catno,format,release_id,status,price,listed,comments,media_condition,sleeve_condition,accept_offer,external_id,weight,format_quantity,location,quantity\r\n"
            + "4332778344,HammerFall,Glory To The Brave,Nuclear Blast,NB 265-2,\"CD, Album, Promo\",5134861,Draft,100.0,2026-08-25 07:04:22,API_TEST_CREATE_INVENTORY_EXPORT,Near Mint (NM or M-),Near Mint (NM or M-),N,API_TEST_EXTERNAL_ID,100,1,API_TEST_LOCATION,1\r\n");
    }
}
