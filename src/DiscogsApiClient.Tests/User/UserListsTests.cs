namespace DiscogsApiClient.Tests.User;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class UserListsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetUserLists_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetUserLists(username!, null, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task GetUserLists_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetUserLists(username, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    public async Task GetUserLists_ShouldReturnEmptyLists_WhenUserHasNoLists(CancellationToken cancellationToken)
    {
        var username = "Murder.Records";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await _apiClient.GetUserLists(username, paginationParams, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(0);
        await Assert.That(response.Lists).IsEmpty();
    }

    [Test]
    public async Task GetUserLists_ShouldReturnLists_WhenUserHasLists(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await _apiClient.GetUserLists(username, paginationParams, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(2);
        await Assert.That(response.Lists).Count().IsEqualTo(2);

        var publicList = response.Lists[0];
        await Assert.That(publicList.Id).IsEqualTo(1710462);
        await Assert.That(publicList.Name).IsEqualTo("API Test Public List");
        await Assert.That(publicList.Description).IsEqualTo("This is a public test list for the api client.");
        await Assert.That(publicList.Public).IsTrue();
        await Assert.That(publicList.DateAdded).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:44:21-07:00"));
        await Assert.That(publicList.DateChanged).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:47:42-07:00"));
        await Assert.That(publicList.Uri).IsEqualTo("https://www.discogs.com/lists/API-Test-Public-List/1710462");
        await Assert.That(publicList.ResourceUrl).IsEqualTo("https://api.discogs.com/lists/1710462");
        await Assert.That(publicList.ImageUrl).IsEqualTo("https://i.discogs.com/m4ct-1DE52L-y4QbkqKUXv9aoNVPTVYy0ZwjXH6qmH8/rs:fit/g:sm/q:40/h:100/w:100/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTQ5NDMy/OC0xNDI0Mzg1Nzgx/LTc1MDAuanBlZw.jpeg");
        await Assert.That(publicList.User.Id).IsEqualTo(12579295);
        await Assert.That(publicList.User.Username).IsEqualTo("DamIDhagor");
        await Assert.That(publicList.User.AvatarUrl).IsEqualTo("https://i.discogs.com/W0OR6Z5OewEwKpar3QyLVCuXbhw2_uNLIwahkmSKqNk/rs:fill/g:sm/q:40/h:500/w:500/czM6Ly9kaXNjb2dz/LXVzZXItYXZhdGFy/cy9VLTEyNTc5Mjk1/LTE2Mzk3ODM2MTIu/anBlZw.jpeg");
        await Assert.That(publicList.User.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");

        var privateList = response.Lists[1];
        await Assert.That(privateList.Id).IsEqualTo(1710463);
        await Assert.That(privateList.Name).IsEqualTo("API Test Private List");
        await Assert.That(privateList.Description).IsEqualTo("This is a private test list for the api client.");
        await Assert.That(privateList.Public).IsFalse();
        await Assert.That(privateList.DateAdded).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:45:36-07:00"));
        await Assert.That(privateList.DateChanged).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:47:00-07:00"));
        await Assert.That(privateList.Uri).IsEqualTo("https://www.discogs.com/lists/API-Test-Private-List/1710463");
        await Assert.That(privateList.ResourceUrl).IsEqualTo("https://api.discogs.com/lists/1710463");
        await Assert.That(privateList.ImageUrl).IsEqualTo("https://i.discogs.com/m4ct-1DE52L-y4QbkqKUXv9aoNVPTVYy0ZwjXH6qmH8/rs:fit/g:sm/q:40/h:100/w:100/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTQ5NDMy/OC0xNDI0Mzg1Nzgx/LTc1MDAuanBlZw.jpeg");
        await Assert.That(privateList.User.Id).IsEqualTo(12579295);
        await Assert.That(privateList.User.Username).IsEqualTo("DamIDhagor");
        await Assert.That(privateList.User.AvatarUrl).IsEqualTo("https://i.discogs.com/W0OR6Z5OewEwKpar3QyLVCuXbhw2_uNLIwahkmSKqNk/rs:fill/g:sm/q:40/h:500/w:500/czM6Ly9kaXNjb2dz/LXVzZXItYXZhdGFy/cy9VLTEyNTc5Mjk1/LTE2Mzk3ODM2MTIu/anBlZw.jpeg");
        await Assert.That(privateList.User.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
    }

    [Test]
    public async Task GetUserLists_ShouldReturnOnlyPublicLists_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var paginationParams = new PaginationQueryParameters { Page = 1, PageSize = 50 };

        var response = await _unauthenticatedApiClient.GetUserLists(username, paginationParams, cancellationToken);

        await Assert.That(response).IsNotNull();
        await Assert.That(response.Pagination.TotalItems).IsEqualTo(1);
        await Assert.That(response.Lists).Count().IsEqualTo(1);

        var publicList = response.Lists[0];
        await Assert.That(publicList.Id).IsEqualTo(1710462);
        await Assert.That(publicList.Name).IsEqualTo("API Test Public List");
        await Assert.That(publicList.Description).IsEqualTo("This is a public test list for the api client.");
        await Assert.That(publicList.Public).IsTrue();
        await Assert.That(publicList.DateAdded).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:44:21-07:00"));
        await Assert.That(publicList.DateChanged).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:47:42-07:00"));
        await Assert.That(publicList.Uri).IsEqualTo("https://www.discogs.com/lists/API-Test-Public-List/1710462");
        await Assert.That(publicList.ResourceUrl).IsEqualTo("https://api.discogs.com/lists/1710462");
        await Assert.That(publicList.ImageUrl).IsEqualTo(string.Empty);
        await Assert.That(publicList.User.Id).IsEqualTo(12579295);
        await Assert.That(publicList.User.Username).IsEqualTo("DamIDhagor");
        await Assert.That(publicList.User.AvatarUrl).IsEqualTo("https://i.discogs.com/W0OR6Z5OewEwKpar3QyLVCuXbhw2_uNLIwahkmSKqNk/rs:fill/g:sm/q:40/h:500/w:500/czM6Ly9kaXNjb2dz/LXVzZXItYXZhdGFy/cy9VLTEyNTc5Mjk1/LTE2Mzk3ODM2MTIu/anBlZw.jpeg");
        await Assert.That(publicList.User.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
    }
}
