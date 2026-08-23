namespace DiscogsApiClient.Tests.User;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class ListDetailsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    public async Task GetList_ShouldThrowException_WhenListIdIsInvalid(int listId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetList(listId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("listId");
    }

    [Test]
    public async Task GetList_ShouldThrowResourceNotFoundException_WhenListDoesNotExist(CancellationToken cancellationToken)
    {
        var listId = 999999999;

        await Assert.That(async () => await _apiClient.GetList(listId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("List not found");
    }

    [Test]
    public async Task GetList_ShouldThrowResourceNotFoundException_WhenListIsPrivateAndUnauthenticated(CancellationToken cancellationToken)
    {
        var listId = 1710463;

        await Assert.That(async () => await _unauthenticatedApiClient.GetList(listId, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("List not found");
    }

    [Test]
    public async Task GetList_ShouldReturnListWithItems_WhenListIsPublicAndUnauthenticated(CancellationToken cancellationToken)
    {
        var listId = 1710462;

        var list = await _unauthenticatedApiClient.GetList(listId, cancellationToken);

        await Assert.That(list).IsNotNull();
        await Assert.That(list.Id).IsEqualTo(1710462);
        await Assert.That(list.Name).IsEqualTo("API Test Public List");
        await Assert.That(list.Description).IsEqualTo("This is a public test list for the api client.");
        await Assert.That(list.Public).IsTrue();
        await Assert.That(list.DateAdded).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:44:21-07:00"));
        await Assert.That(list.DateChanged).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:47:42-07:00"));
        await Assert.That(list.Uri).IsEqualTo("https://www.discogs.com/lists/API-Test-Public-List/1710462");
        await Assert.That(list.ResourceUrl).IsEqualTo("https://api.discogs.com/lists/1710462");
        await Assert.That(list.ImageUrl).IsEqualTo(string.Empty);
        await Assert.That(list.User.Id).IsEqualTo(12579295);
        await Assert.That(list.User.Username).IsEqualTo("DamIDhagor");
        await Assert.That(list.User.AvatarUrl).IsEqualTo("https://i.discogs.com/W0OR6Z5OewEwKpar3QyLVCuXbhw2_uNLIwahkmSKqNk/rs:fill/g:sm/q:40/h:500/w:500/czM6Ly9kaXNjb2dz/LXVzZXItYXZhdGFy/cy9VLTEyNTc5Mjk1/LTE2Mzk3ODM2MTIu/anBlZw.jpeg");
        await Assert.That(list.User.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
        await Assert.That(list.Items).Count().IsEqualTo(3);

        var release = list.Items[0];
        await Assert.That(release.Id).IsEqualTo(494328);
        await Assert.That(release.Type).IsEqualTo(ListItemType.Release);
        await Assert.That(release.DisplayTitle).IsEqualTo("HammerFall - Glory To The Brave");
        await Assert.That(release.Comment).IsEqualTo("This is a public test item for the api client.");
        await Assert.That(release.Uri).IsEqualTo("https://www.discogs.com/release/494328-HammerFall-Glory-To-The-Brave");
        await Assert.That(release.ResourceUrl).IsEqualTo("https://api.discogs.com/releases/494328");
        await Assert.That(release.ImageUrl).IsEqualTo(string.Empty);
        var releaseStats = await Assert.That(release.Stats).IsNotNull();
        await Assert.That(releaseStats.Community.InWantlist).IsEqualTo(99);
        await Assert.That(releaseStats.Community.InCollection).IsEqualTo(1538);
        await Assert.That(releaseStats.User).IsNull();

        var master = list.Items[1];
        await Assert.That(master.Id).IsEqualTo(156551);
        await Assert.That(master.Type).IsEqualTo(ListItemType.Master);
        await Assert.That(master.DisplayTitle).IsEqualTo("HammerFall - Glory To The Brave");
        await Assert.That(master.Comment).IsEqualTo("This is a public test item for the api client.");
        await Assert.That(master.Uri).IsEqualTo("https://www.discogs.com/master/156551-HammerFall-Glory-To-The-Brave");
        await Assert.That(master.ResourceUrl).IsEqualTo("https://api.discogs.com/masters/156551");
        await Assert.That(master.ImageUrl).IsEqualTo(string.Empty);
        var masterStats = await Assert.That(master.Stats).IsNotNull();
        await Assert.That(masterStats.Community.InWantlist).IsEqualTo(99);
        await Assert.That(masterStats.Community.InCollection).IsEqualTo(1538);
        await Assert.That(masterStats.User).IsNull();

        var artist = list.Items[2];
        await Assert.That(artist.Id).IsEqualTo(287459);
        await Assert.That(artist.Type).IsEqualTo(ListItemType.Artist);
        await Assert.That(artist.DisplayTitle).IsEqualTo("HammerFall");
        await Assert.That(artist.Comment).IsEqualTo("This is a public test item for the api client.");
        await Assert.That(artist.Uri).IsEqualTo("https://www.discogs.com/artist/287459-HammerFall");
        await Assert.That(artist.ResourceUrl).IsEqualTo("https://api.discogs.com/artists/287459");
        await Assert.That(artist.ImageUrl).IsEqualTo(string.Empty);
        await Assert.That(artist.Stats).IsNull();
    }

    [Test]
    public async Task GetList_ShouldReturnListWithItems_WhenListIsPrivateAndAuthenticated(CancellationToken cancellationToken)
    {
        var listId = 1710463;

        var list = await _apiClient.GetList(listId, cancellationToken);

        await Assert.That(list).IsNotNull();
        await Assert.That(list.Id).IsEqualTo(1710463);
        await Assert.That(list.Name).IsEqualTo("API Test Private List");
        await Assert.That(list.Description).IsEqualTo("This is a private test list for the api client.");
        await Assert.That(list.Public).IsFalse();
        await Assert.That(list.DateAdded).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:45:36-07:00"));
        await Assert.That(list.DateChanged).IsEqualTo(DateTimeOffset.Parse("2026-08-23T13:47:00-07:00"));
        await Assert.That(list.Uri).IsEqualTo("https://www.discogs.com/lists/API-Test-Private-List/1710463");
        await Assert.That(list.ResourceUrl).IsEqualTo("https://api.discogs.com/lists/1710463");
        await Assert.That(list.ImageUrl).IsEqualTo("https://i.discogs.com/m4ct-1DE52L-y4QbkqKUXv9aoNVPTVYy0ZwjXH6qmH8/rs:fit/g:sm/q:40/h:100/w:100/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTQ5NDMy/OC0xNDI0Mzg1Nzgx/LTc1MDAuanBlZw.jpeg");
        await Assert.That(list.User.Id).IsEqualTo(12579295);
        await Assert.That(list.User.Username).IsEqualTo("DamIDhagor");
        await Assert.That(list.User.AvatarUrl).IsEqualTo("https://i.discogs.com/W0OR6Z5OewEwKpar3QyLVCuXbhw2_uNLIwahkmSKqNk/rs:fill/g:sm/q:40/h:500/w:500/czM6Ly9kaXNjb2dz/LXVzZXItYXZhdGFy/cy9VLTEyNTc5Mjk1/LTE2Mzk3ODM2MTIu/anBlZw.jpeg");
        await Assert.That(list.User.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
        await Assert.That(list.Items).Count().IsEqualTo(3);

        var release = list.Items[0];
        await Assert.That(release.Id).IsEqualTo(494328);
        await Assert.That(release.Type).IsEqualTo(ListItemType.Release);
        await Assert.That(release.DisplayTitle).IsEqualTo("HammerFall - Glory To The Brave");
        await Assert.That(release.Comment).IsEqualTo("This is a public test item for the api client.");
        await Assert.That(release.Uri).IsEqualTo("https://www.discogs.com/release/494328-HammerFall-Glory-To-The-Brave");
        await Assert.That(release.ResourceUrl).IsEqualTo("https://api.discogs.com/releases/494328");
        await Assert.That(release.ImageUrl).IsEqualTo("https://i.discogs.com/PB6r5JRHbzF_-RE5sH4XerIj5NRA8T0lCLqCNQnaOkQ/rs:fit/g:sm/q:40/h:300/w:300/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTQ5NDMy/OC0xNDI0Mzg1Nzgx/LTc1MDAuanBlZw.jpeg");
        var releaseStats = await Assert.That(release.Stats).IsNotNull();
        await Assert.That(releaseStats.Community.InWantlist).IsEqualTo(99);
        await Assert.That(releaseStats.Community.InCollection).IsEqualTo(1538);
        var releaseUserStats = await Assert.That(releaseStats.User).IsNotNull();
        await Assert.That(releaseUserStats.InWantlist).IsEqualTo(0);
        await Assert.That(releaseUserStats.InCollection).IsEqualTo(0);

        var master = list.Items[1];
        await Assert.That(master.Id).IsEqualTo(156551);
        await Assert.That(master.Type).IsEqualTo(ListItemType.Master);
        await Assert.That(master.DisplayTitle).IsEqualTo("HammerFall - Glory To The Brave");
        await Assert.That(master.Comment).IsEqualTo("This is a private test item for the api client.");
        await Assert.That(master.Uri).IsEqualTo("https://www.discogs.com/master/156551-HammerFall-Glory-To-The-Brave");
        await Assert.That(master.ResourceUrl).IsEqualTo("https://api.discogs.com/masters/156551");
        await Assert.That(master.ImageUrl).IsEqualTo("https://i.discogs.com/PB6r5JRHbzF_-RE5sH4XerIj5NRA8T0lCLqCNQnaOkQ/rs:fit/g:sm/q:40/h:300/w:300/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTQ5NDMy/OC0xNDI0Mzg1Nzgx/LTc1MDAuanBlZw.jpeg");
        var masterStats = await Assert.That(master.Stats).IsNotNull();
        await Assert.That(masterStats.Community.InWantlist).IsEqualTo(99);
        await Assert.That(masterStats.Community.InCollection).IsEqualTo(1538);
        var masterUserStats = await Assert.That(masterStats.User).IsNotNull();
        await Assert.That(masterUserStats.InWantlist).IsEqualTo(0);
        await Assert.That(masterUserStats.InCollection).IsEqualTo(0);

        var artist = list.Items[2];
        await Assert.That(artist.Id).IsEqualTo(287459);
        await Assert.That(artist.Type).IsEqualTo(ListItemType.Artist);
        await Assert.That(artist.DisplayTitle).IsEqualTo("HammerFall");
        await Assert.That(artist.Comment).IsEqualTo("This is a private test item for the api client.");
        await Assert.That(artist.Uri).IsEqualTo("https://www.discogs.com/artist/287459-HammerFall");
        await Assert.That(artist.ResourceUrl).IsEqualTo("https://api.discogs.com/artists/287459");
        await Assert.That(artist.ImageUrl).IsEqualTo("https://i.discogs.com/lhfg-uN3Jizy_8uPAj1cKLySxJJrmspWqZPm5hpAgVE/rs:fit/g:sm/q:40/h:300/w:300/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9BLTI4NzQ1/OS0xNzI0NDI5MjMy/LTU5ODEuanBlZw.jpeg");
        await Assert.That(artist.Stats).IsNull();
    }
}
