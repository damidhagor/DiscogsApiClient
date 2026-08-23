using DiscogsApiClient.Contract.User.Collection;

namespace DiscogsApiClient.Tests.Collection;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class CollectionFieldsTests(DiscogsApiClientFixture fixture)
{
    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    public async Task GetCollectionFields_ShouldReturnFields_WhenUsernameIsValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        var fieldsResponse = await _apiClient.GetCollectionFields(username, cancellationToken);

        await Assert.That(fieldsResponse).IsNotNull();
        await Assert.That(fieldsResponse.Fields).IsNotNull();
        await Assert.That(fieldsResponse.Fields.Count).IsEqualTo(2);

        var textareaField = fieldsResponse.Fields.Single(f => f.Type == CollectionFieldType.Textarea);
        await Assert.That(textareaField.Id).IsEqualTo(2);
        await Assert.That(textareaField.Name).IsEqualTo("API_TEST_TEXTAREA_FIELD");
        await Assert.That(textareaField.Position).IsEqualTo(1);
        await Assert.That(textareaField.Public).IsFalse();
        await Assert.That(textareaField.Lines).IsEqualTo(1);
        await Assert.That(textareaField.Options).IsNull();

        var dropdownField = fieldsResponse.Fields.Single(f => f.Type == CollectionFieldType.Dropdown);
        await Assert.That(dropdownField.Id).IsEqualTo(1);
        await Assert.That(dropdownField.Name).IsEqualTo("API_TEST_DROPDOWN_FIELD");
        await Assert.That(dropdownField.Position).IsEqualTo(2);
        await Assert.That(dropdownField.Public).IsFalse();
        await Assert.That(dropdownField.Lines).IsNull();
        await Assert.That(dropdownField.Options).IsNotNull();
        await Assert.That(dropdownField.Options!.Count).IsEqualTo(2);
        await Assert.That(dropdownField.Options).Contains("API_TEST_OPTION_A");
        await Assert.That(dropdownField.Options).Contains("API_TEST_OPTION_B");
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task GetCollectionFields_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.GetCollectionFields(username!, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    public async Task GetCollectionFields_ShouldThrowResourceNotFoundException_WhenUsernameDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "awrbaerhnqw54";

        await Assert.That(async () => await _apiClient.GetCollectionFields(username, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("User does not exist or may have been deleted.");
    }

    [Test]
    public async Task GetCollectionFields_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";

        await Assert.That(async () => await _unauthenticatedApiClient.GetCollectionFields(username, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }
}
