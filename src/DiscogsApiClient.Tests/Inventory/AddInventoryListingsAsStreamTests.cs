using System.Text;

namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class AddInventoryListingsAsStreamTests(DiscogsApiClientFixture fixture)
{
    private const string InventoryUploadMutationConstraint = "InventoryUploadMutation";

    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task AddInventoryListings_ShouldThrowArgumentNullException_WhenCsvContentIsNull(CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _unauthenticatedApiClient.AddInventoryListings((Stream)null!, cancellationToken))
            .Throws<ArgumentNullException>();

        await Assert.That(exception!.ParamName).IsEqualTo("csvContent");
    }

    [Test]
    public async Task AddInventoryListings_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        using var csvContent = new MemoryStream("release_id,price,media_condition\n1,10.00,Near Mint (NM or M-)"u8.ToArray());

        await Assert.That(async () => await _unauthenticatedApiClient.AddInventoryListings(csvContent, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    [NotInParallel(InventoryUploadMutationConstraint)]
    public async Task AddInventoryListings_ShouldSucceed_WhenCsvContentIsEmpty(CancellationToken cancellationToken)
    {
        using var csvContent = new MemoryStream();

        var uploadId = await _apiClient.AddInventoryListings(csvContent, cancellationToken);
        await Assert.That(uploadId).IsGreaterThan(0);

        await Task.Delay(500, cancellationToken);
        var upload = await _apiClient.GetInventoryUpload(uploadId, cancellationToken);

        await Assert.That(upload.Id).IsEqualTo(uploadId);
        await Assert.That(upload.Type).IsEqualTo("add");
        await Assert.That(upload.Filename).IsEqualTo("upload.csv");
        await Assert.That(upload.Status).IsEqualTo("failed");
        await Assert.That(upload.Results).IsEqualTo("ERROR: Uploaded file no longer exists.");
    }

    [Test]
    [NotInParallel(InventoryUploadMutationConstraint)]
    public async Task AddInventoryListings_ShouldReportFailure_WhenReleaseIdDoesNotExist(CancellationToken cancellationToken)
    {
        using var csvContent = new MemoryStream(Encoding.UTF8.GetBytes("release_id,price,media_condition\n999999999,10.00,Near Mint (NM or M-)"));

        var uploadId = await _apiClient.AddInventoryListings(csvContent, cancellationToken);
        await Assert.That(uploadId).IsGreaterThan(0);

        await Task.Delay(500, cancellationToken);
        var upload = await _apiClient.GetInventoryUpload(uploadId, cancellationToken);

        await Assert.That(upload.Id).IsEqualTo(uploadId);
        await Assert.That(upload.Type).IsEqualTo("add");
        await Assert.That(upload.Filename).IsEqualTo("upload.csv");
        await Assert.That(upload.CreatedAt).IsGreaterThan(DateTime.MinValue);
        await Assert.That(upload.FinishedAt).IsNotNull();
        await Assert.That(upload.FinishedAt!.Value).IsGreaterThanOrEqualTo(upload.CreatedAt);
        await Assert.That(upload.Status).IsEqualTo("success");
        await Assert.That(upload.Results).IsEqualTo("CSV file contains 1 records.<p>Skipped row 2 because ERROR: release_id 999999999 does not exist<p>Processed 0 records.");
    }
}
