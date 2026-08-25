namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetInventoryUploadTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task GetInventoryUpload_ShouldThrowArgumentOutOfRangeException_WhenUploadIdIsInvalid(long uploadId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _unauthenticatedApiClient.GetInventoryUpload(uploadId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("uploadId");
    }

    [Test]
    public async Task GetInventoryUpload_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.GetInventoryUpload(1, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    public async Task GetInventoryUpload_ShouldThrowResourceNotFoundException_WhenUploadDoesNotExist(CancellationToken cancellationToken)
    {
        var uploadId = 999999999;

        await Assert.That(async () => await _apiClient.GetInventoryUpload(uploadId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested resource was not found.");
    }

    [Test]
    public async Task GetInventoryUpload_ShouldReturnUpload_WhenUploadSucceededFully(CancellationToken cancellationToken)
    {
        var upload = await _apiClient.GetInventoryUpload(3572301, cancellationToken);

        await Assert.That(upload.Id).IsEqualTo(3572301);
        await Assert.That(upload.Type).IsEqualTo("delete");
        await Assert.That(upload.Filename).IsEqualTo("upload.csv");
        await Assert.That(upload.Status).IsEqualTo("success");
        await Assert.That(upload.Results).IsEqualTo("CSV file contains 1 records.<p>Processed 1 records.");
        await Assert.That(upload.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 40, 56, DateTimeKind.Unspecified));
        await Assert.That(upload.FinishedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 40, 56, DateTimeKind.Unspecified));
    }

    [Test]
    public async Task GetInventoryUpload_ShouldReturnUpload_WhenUploadSucceededPartially(CancellationToken cancellationToken)
    {
        var upload = await _apiClient.GetInventoryUpload(3572304, cancellationToken);

        await Assert.That(upload.Id).IsEqualTo(3572304);
        await Assert.That(upload.Type).IsEqualTo("delete");
        await Assert.That(upload.Filename).IsEqualTo("upload.csv");
        await Assert.That(upload.Status).IsEqualTo("success");
        await Assert.That(upload.Results).IsEqualTo("CSV file contains 2 records.<p>Skipped row 3 because ERROR: -1 is an invalid listing_id<p>Processed 1 records.");
        await Assert.That(upload.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 40, 58, DateTimeKind.Unspecified));
        await Assert.That(upload.FinishedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 40, 58, DateTimeKind.Unspecified));
    }

    [Test]
    public async Task GetInventoryUpload_ShouldReturnUpload_WhenUploadFailed(CancellationToken cancellationToken)
    {
        var upload = await _apiClient.GetInventoryUpload(3572217, cancellationToken);

        await Assert.That(upload.Id).IsEqualTo(3572217);
        await Assert.That(upload.Type).IsEqualTo("delete");
        await Assert.That(upload.Filename).IsEqualTo("upload.csv");
        await Assert.That(upload.Status).IsEqualTo("failed");
        await Assert.That(upload.Results).IsEqualTo("ERROR: Uploaded file no longer exists.");
        await Assert.That(upload.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 18, 47, DateTimeKind.Unspecified));
        await Assert.That(upload.FinishedAt).IsNull();
    }
}
