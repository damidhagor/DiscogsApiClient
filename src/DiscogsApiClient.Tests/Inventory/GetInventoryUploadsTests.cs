namespace DiscogsApiClient.Tests.Inventory;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetInventoryUploadsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetInventoryUploads_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.GetInventoryUploads(null, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>()
            .WithMessage("You must authenticate to access this resource.");
    }

    [Test]
    public async Task GetInventoryUploads_ShouldReturnUploads_WhenUploadsExist(CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetInventoryUploads(null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Items).IsNotEmpty();
        await Assert.That(response.Pagination.Page).IsEqualTo(1);
        await Assert.That(response.Pagination.ItemsPerPage).IsEqualTo(50);
        await Assert.That(response.Pagination.TotalItems).IsGreaterThanOrEqualTo(16);

        var fullSuccessUpload = response.Items.FirstOrDefault(u => u.Id == 3572301);
        await Assert.That(fullSuccessUpload).IsNotNull();
        await Assert.That(fullSuccessUpload!.Type).IsEqualTo("delete");
        await Assert.That(fullSuccessUpload.Filename).IsEqualTo("upload.csv");
        await Assert.That(fullSuccessUpload.Status).IsEqualTo("success");
        await Assert.That(fullSuccessUpload.Results).IsEqualTo("CSV file contains 1 records.<p>Processed 1 records.");
        await Assert.That(fullSuccessUpload.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 40, 56, DateTimeKind.Unspecified));
        await Assert.That(fullSuccessUpload.FinishedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 40, 56, DateTimeKind.Unspecified));

        var partialSuccessUpload = response.Items.FirstOrDefault(u => u.Id == 3572304);
        await Assert.That(partialSuccessUpload).IsNotNull();
        await Assert.That(partialSuccessUpload!.Type).IsEqualTo("delete");
        await Assert.That(partialSuccessUpload.Filename).IsEqualTo("upload.csv");
        await Assert.That(partialSuccessUpload.Status).IsEqualTo("success");
        await Assert.That(partialSuccessUpload.Results).IsEqualTo("CSV file contains 2 records.<p>Skipped row 3 because ERROR: -1 is an invalid listing_id<p>Processed 1 records.");
        await Assert.That(partialSuccessUpload.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 40, 58, DateTimeKind.Unspecified));
        await Assert.That(partialSuccessUpload.FinishedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 40, 58, DateTimeKind.Unspecified));

        var failedUpload = response.Items.FirstOrDefault(u => u.Id == 3572217);
        await Assert.That(failedUpload).IsNotNull();
        await Assert.That(failedUpload!.Type).IsEqualTo("delete");
        await Assert.That(failedUpload.Filename).IsEqualTo("upload.csv");
        await Assert.That(failedUpload.Status).IsEqualTo("failed");
        await Assert.That(failedUpload.Results).IsEqualTo("ERROR: Uploaded file no longer exists.");
        await Assert.That(failedUpload.CreatedAt).IsEqualTo(new DateTime(2026, 8, 25, 5, 18, 47, DateTimeKind.Unspecified));
        await Assert.That(failedUpload.FinishedAt).IsNull();
    }
}
