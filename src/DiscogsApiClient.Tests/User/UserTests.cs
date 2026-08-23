namespace DiscogsApiClient.Tests.User;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class UserTests(DiscogsApiClientFixture fixture)
{
    private const string ProfileMutationConstraint = "UserProfileMutation";

    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetUser_ShouldReturnUserWithPrivateFields_WhenAuthenticatedAndRequestingOwnProfile(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var user = await _apiClient.GetUser(username, cancellationToken);

        await Assert.That(user).IsNotNull();
        await Assert.That(user.Id).IsEqualTo(12579295);
        await Assert.That(user.Username).IsEqualTo("DamIDhagor");
        await Assert.That(user.Email).IsEqualTo("alexander.jurk@outlook.com");
        await Assert.That(user.NumCollection).IsNotNull();
        await Assert.That(user.NumWantlist).IsNotNull();
        await Assert.That(user.NumUnread).IsNotNull();
        await Assert.That(user.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
        await Assert.That(user.IsActivated).IsTrue();
        await Assert.That(user.AvatarUrl).IsNotNullOrWhiteSpace();
        await Assert.That(user.CollectionFoldersUrl).IsNotNullOrWhiteSpace();
    }

    [Test]
    public async Task GetUser_ShouldReturnUserWithoutPrivateFields_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var user = await _unauthenticatedApiClient.GetUser(username, cancellationToken);

        await Assert.That(user).IsNotNull();
        await Assert.That(user.Id).IsEqualTo(12579295);
        await Assert.That(user.Username).IsEqualTo("DamIDhagor");
        await Assert.That(user.Email).IsNull();
        await Assert.That(user.NumCollection).IsNull();
        await Assert.That(user.NumWantlist).IsNull();
        await Assert.That(user.NumUnread).IsNull();
        await Assert.That(user.ResourceUrl).IsEqualTo("https://api.discogs.com/users/DamIDhagor");
        await Assert.That(user.IsActivated).IsTrue();
        await Assert.That(user.AvatarUrl).IsNotNullOrWhiteSpace();
        await Assert.That(user.CollectionFoldersUrl).IsNotNullOrWhiteSpace();
    }

    [Test]
    public async Task GetUser_ShouldThrowArgumentException_WhenUsernameIsEmpty(CancellationToken cancellationToken)
    {
        var username = "";

        var exception = await Assert.That(async () => await _apiClient.GetUser(username, cancellationToken))
            .Throws<ArgumentException>();

        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task GetUser_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetUser(username, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateUser_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var request = new UserProfileUpdateRequest();

        var exception = await Assert.That(async () => await _apiClient.UpdateUser(username!, request, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task UpdateUser_ShouldThrowArgumentNullException_WhenRequestIsNull(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var exception = await Assert.That(async () => await _apiClient.UpdateUser(username, null!, cancellationToken))
            .Throws<ArgumentNullException>();

        await Assert.That(exception!.ParamName).IsEqualTo("request");
    }

    [Test]
    public async Task UpdateUser_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";
        var request = new UserProfileUpdateRequest();

        await Assert.That(async () => await _apiClient.UpdateUser(username, request, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    public async Task UpdateUser_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var request = new UserProfileUpdateRequest();

        await Assert.That(async () => await _unauthenticatedApiClient.UpdateUser(username, request, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    [NotInParallel(ProfileMutationConstraint)]
    public async Task UpdateUser_ShouldUpdateAllProperties_WhenValuesAreValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var originalUser = await _apiClient.GetUser(username, cancellationToken);

        try
        {
            var testRequest = new UserProfileUpdateRequest(
                Name: "API_TEST_NAME",
                HomePage: "https://example.com/api-test",
                Location: "API_TEST_LOCATION",
                Profile: "API_TEST_PROFILE",
                CurrencyAbbreviation: "USD");

            var updatedUser = await _apiClient.UpdateUser(username, testRequest, cancellationToken);

            await Assert.That(updatedUser).IsNotNull();
            await Assert.That(updatedUser.Name).IsEqualTo(testRequest.Name);
            await Assert.That(updatedUser.HomePage).IsEqualTo(testRequest.HomePage);
            await Assert.That(updatedUser.Location).IsEqualTo(testRequest.Location);
            await Assert.That(updatedUser.Profile).IsEqualTo(testRequest.Profile);
            await Assert.That(updatedUser.CurrencyAbbreviation).IsEqualTo(testRequest.CurrencyAbbreviation);
        }
        finally
        {
            var restoreRequest = new UserProfileUpdateRequest(
                Name: originalUser.Name,
                HomePage: originalUser.HomePage,
                Location: originalUser.Location,
                Profile: originalUser.Profile,
                CurrencyAbbreviation: originalUser.CurrencyAbbreviation);

            await _apiClient.UpdateUser(username, restoreRequest, cancellationToken);
        }
    }

    [Test]
    [NotInParallel(ProfileMutationConstraint)]
    public async Task UpdateUser_ShouldIgnoreAllProperties_WhenValuesAreNull(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var originalUser = await _apiClient.GetUser(username, cancellationToken);

        try
        {
            var testRequest = new UserProfileUpdateRequest(
                Name: null,
                HomePage: null,
                Location: null,
                Profile: null,
                CurrencyAbbreviation: null);

            var updatedUser = await _apiClient.UpdateUser(username, testRequest, cancellationToken);

            await Assert.That(updatedUser).IsNotNull();
            await Assert.That(updatedUser.Name).IsEqualTo(originalUser.Name);
            await Assert.That(updatedUser.HomePage).IsEqualTo(originalUser.HomePage);
            await Assert.That(updatedUser.Location).IsEqualTo(originalUser.Location);
            await Assert.That(updatedUser.Profile).IsEqualTo(originalUser.Profile);
            await Assert.That(updatedUser.CurrencyAbbreviation).IsEqualTo(originalUser.CurrencyAbbreviation);
        }
        finally
        {
            var restoreRequest = new UserProfileUpdateRequest(
                Name: originalUser.Name,
                HomePage: originalUser.HomePage,
                Location: originalUser.Location,
                Profile: originalUser.Profile,
                CurrencyAbbreviation: originalUser.CurrencyAbbreviation);

            await _apiClient.UpdateUser(username, restoreRequest, cancellationToken);
        }
    }

    [Test]
    [NotInParallel(ProfileMutationConstraint)]
    public async Task UpdateUser_ShouldClearPropertiesExceptCurrencyAbbreviation_WhenValuesAreEmpty(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var originalUser = await _apiClient.GetUser(username, cancellationToken);

        try
        {
            var testRequest = new UserProfileUpdateRequest(
                Name: "",
                HomePage: "",
                Location: "",
                Profile: "",
                CurrencyAbbreviation: "");

            var updatedUser = await _apiClient.UpdateUser(username, testRequest, cancellationToken);

            await Assert.That(updatedUser).IsNotNull();
            await Assert.That(updatedUser.Name).IsEqualTo("");
            await Assert.That(updatedUser.HomePage).IsEqualTo("");
            await Assert.That(updatedUser.Location).IsEqualTo("");
            await Assert.That(updatedUser.Profile).IsEqualTo("");

            // Unlike the other profile fields, an empty CurrencyAbbreviation does not clear the value - Discogs leaves it unchanged.
            await Assert.That(updatedUser.CurrencyAbbreviation).IsEqualTo(originalUser.CurrencyAbbreviation);
        }
        finally
        {
            var restoreRequest = new UserProfileUpdateRequest(
                Name: originalUser.Name,
                HomePage: originalUser.HomePage,
                Location: originalUser.Location,
                Profile: originalUser.Profile,
                CurrencyAbbreviation: originalUser.CurrencyAbbreviation);

            await _apiClient.UpdateUser(username, restoreRequest, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateUser_ShouldThrowResourceNotFoundException_WhenCurrencyAbbreviationIsInvalid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var request = new UserProfileUpdateRequest(CurrencyAbbreviation: "XXX");

        await Assert.That(async () => await _apiClient.UpdateUser(username, request, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Invalid currency abbreviation.");
    }
}
