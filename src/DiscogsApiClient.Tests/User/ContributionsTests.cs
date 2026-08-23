namespace DiscogsApiClient.Tests.User;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class ContributionsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetContributions_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetContributions(username!, null, null, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task GetContributions_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetContributions(username, null, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    public async Task GetContributions_ShouldReturnEmptyContributions_WhenUserHasNoContributions(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await _apiClient.GetContributions(username, paginationParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(0);
        await Assert.That(response.Contributions).IsEmpty();
    }

    [Test]
    public async Task GetContributions_ShouldReturnContributions_WhenUserHasContributions(CancellationToken cancellationToken)
    {
        var username = "Murder.Records";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 100 };

        var response = await _apiClient.GetContributions(username, paginationParams, null, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination.TotalItems).IsGreaterThan(0);
        await Assert.That(response.Contributions).IsNotEmpty();

        var contribution = response.Contributions[0];
        await Assert.That(contribution.Id).IsEqualTo(38215206);
        await Assert.That(contribution.Status).IsEqualTo("Accepted");
        await Assert.That(contribution.Title).IsEqualTo("Neseblod Compilation");
        await Assert.That(contribution.Year).IsEqualTo(2026);
        await Assert.That(contribution.Country).IsEqualTo("UK & US");
        await Assert.That(contribution.DataQuality).IsEqualTo("Needs Vote");
        await Assert.That(contribution.FormatCount).IsEqualTo(1);
        await Assert.That(contribution.NumForSale).IsEqualTo(1);
    }
}
