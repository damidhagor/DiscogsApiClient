using System.Text;

namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class ChangeInventoryListingsAsStreamTests(DiscogsApiClientFixture fixture)
{
    private const string InventoryUploadMutationConstraint = "InventoryUploadMutation";

    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task ChangeInventoryListings_ShouldThrowArgumentNullException_WhenCsvContentIsNull(CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _unauthenticatedApiClient.ChangeInventoryListings((Stream)null!, cancellationToken))
            .Throws<ArgumentNullException>();

        await Assert.That(exception!.ParamName).IsEqualTo("csvContent");
    }

    [Test]
    public async Task ChangeInventoryListings_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        using var csvContent = new MemoryStream("listing_id,price\n1,10.00"u8.ToArray());

        await Assert.That(async () => await _unauthenticatedApiClient.ChangeInventoryListings(csvContent, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    [NotInParallel(InventoryUploadMutationConstraint)]
    public async Task ChangeInventoryListings_ShouldSucceed_WhenCsvContentIsEmpty(CancellationToken cancellationToken)
    {
        using var csvContent = new MemoryStream();

        var uploadId = await _apiClient.ChangeInventoryListings(csvContent, cancellationToken);
        await Assert.That(uploadId).IsGreaterThan(0);

        await Task.Delay(500, cancellationToken);
        var upload = await _apiClient.GetInventoryUpload(uploadId, cancellationToken);

        await Assert.That(upload.Id).IsEqualTo(uploadId);
        await Assert.That(upload.Type).IsEqualTo("change");
        await Assert.That(upload.Filename).IsEqualTo("upload.csv");
        await Assert.That(upload.Status).IsEqualTo("failed");
        await Assert.That(upload.Results).IsEqualTo("ERROR: Uploaded file no longer exists.");
    }

    [Test]
    [NotInParallel(InventoryUploadMutationConstraint)]
    public async Task ChangeInventoryListings_ShouldReportFailure_WhenListingIdDoesNotExist(CancellationToken cancellationToken)
    {
        using var csvContent = new MemoryStream(Encoding.UTF8.GetBytes("listing_id,price\n999999999,10.00"));

        var uploadId = await _apiClient.ChangeInventoryListings(csvContent, cancellationToken);
        await Assert.That(uploadId).IsGreaterThan(0);

        await Task.Delay(500, cancellationToken);
        var upload = await _apiClient.GetInventoryUpload(uploadId, cancellationToken);

        await Assert.That(upload.Id).IsEqualTo(uploadId);
        await Assert.That(upload.Type).IsEqualTo("change");
        await Assert.That(upload.Filename).IsEqualTo("upload.csv");
        await Assert.That(upload.CreatedAt).IsGreaterThan(DateTime.MinValue);
        await Assert.That(upload.FinishedAt).IsNotNull();
        await Assert.That(upload.FinishedAt!.Value).IsGreaterThanOrEqualTo(upload.CreatedAt);
        await Assert.That(upload.Status).IsEqualTo("success");
        await Assert.That(upload.Results).IsEqualTo("CSV file contains 1 records.<p>Skipped row 2 because ERROR: 999999999 is an invalid listing_id<p>Processed 0 records.");
    }
}
