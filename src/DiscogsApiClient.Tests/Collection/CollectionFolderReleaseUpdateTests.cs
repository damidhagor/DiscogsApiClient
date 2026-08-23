namespace DiscogsApiClient.Tests.Collection;

[ClassDataSource<DiscogsApiClientFixture>(Shared = SharedType.PerTestSession)]
public sealed class CollectionFolderReleaseUpdateTests(DiscogsApiClientFixture fixture)
{
    private const string RatingMutationConstraint = "CollectionReleaseRatingMutation";

    private readonly IDiscogsApiClient _apiClient = fixture.GetAuthenticatedClient();
    private readonly IDiscogsApiClient _unauthenticatedApiClient = fixture.GetUnauthenticatedClient();

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateCollectionFolderRelease_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease(username!, 1, 5134861, 1, 3, null, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task UpdateCollectionFolderRelease_ShouldThrowArgumentOutOfRangeException_WhenFolderIdIsInvalid(int folderId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease("DamIDhagor", folderId, 5134861, 1, 3, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("folderId");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task UpdateCollectionFolderRelease_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease("DamIDhagor", 1, releaseId, 1, 3, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    [Arguments(-1L)]
    [Arguments(0L)]
    public async Task UpdateCollectionFolderRelease_ShouldThrowArgumentOutOfRangeException_WhenInstanceIdIsInvalid(long instanceId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease("DamIDhagor", 1, 5134861, instanceId, 3, null, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("instanceId");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task UpdateCollectionFolderRelease_ShouldThrowArgumentOutOfRangeException_WhenTargetFolderIdIsInvalid(int targetFolderId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease("DamIDhagor", 1, 5134861, 1, 3, targetFolderId, cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("targetFolderId.Value");
    }

    [Test]
    public async Task UpdateCollectionFolderRelease_ShouldThrowResourceNotFoundException_WhenInstanceDoesNotExist(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease("DamIDhagor", 1, 5134861, long.MaxValue, 3, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested collection item does not exist in this user's collection.");
    }

    [Test]
    public async Task UpdateCollectionFolderRelease_ShouldThrowResourceNotFoundException_WhenFolderDoesNotExist(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease("DamIDhagor", 999999999, 5134861, 1, 3, null, cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("The requested collection item does not exist in this user's collection.");
    }

    [Test]
    public async Task UpdateCollectionFolderRelease_ShouldThrowDiscogsException_WhenTargetFolderDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "UpdateCollectionFolderRelease_TargetNotFound";
        var releaseId = 5134861;

        var folder = await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken);
        var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, folder.Id, releaseId, cancellationToken);

        try
        {
            await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease(username, folder.Id, releaseId, addedRelease.InstanceId, null, 999999999, cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("Invalid folder_id: folder does not exist.");
        }
        finally
        {
            await _apiClient.DeleteReleaseFromCollectionFolder(username, folder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
            await _apiClient.DeleteCollectionFolder(username, folder.Id, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateCollectionFolderRelease_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.UpdateCollectionFolderRelease("DamIDhagor", 1, 5134861, 1, 3, null, cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    [NotInParallel(RatingMutationConstraint)]
    public async Task UpdateCollectionFolderRelease_ShouldChangeRatingAndMoveFolder_WhenValuesAreValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var sourceFolderName = "UpdateCollectionFolderRelease_Source";
        var targetFolderName = "UpdateCollectionFolderRelease_Target";
        var releaseId = 5134861;

        var sourceFolder = await _apiClient.CreateCollectionFolder(username, sourceFolderName, cancellationToken);
        var targetFolder = await _apiClient.CreateCollectionFolder(username, targetFolderName, cancellationToken);

        try
        {
            var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, sourceFolder.Id, releaseId, cancellationToken);

            await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease(username, sourceFolder.Id, releaseId, addedRelease.InstanceId, 4, targetFolder.Id, cancellationToken))
                .ThrowsNothing();

            var targetFolderReleases = await _apiClient.GetCollectionItemsByFolder(username, targetFolder.Id, null, null, cancellationToken);

            await Assert.That(targetFolderReleases.Releases).IsNotEmpty();
            await Assert.That(targetFolderReleases.Releases[0].Id).IsEqualTo(releaseId);
            await Assert.That(targetFolderReleases.Releases[0].Rating).IsEqualTo(4);

            await _apiClient.DeleteReleaseFromCollectionFolder(username, targetFolder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
        }
        finally
        {
            await _apiClient.DeleteCollectionFolder(username, sourceFolder.Id, cancellationToken);
            await _apiClient.DeleteCollectionFolder(username, targetFolder.Id, cancellationToken);
        }
    }

    [Test]
    [NotInParallel(RatingMutationConstraint)]
    public async Task UpdateCollectionFolderRelease_ShouldChangeRatingOnly_WhenTargetFolderIdIsNull(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "UpdateCollectionFolderRelease_RatingOnly";
        var releaseId = 5134861;

        var folder = await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken);

        try
        {
            var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, folder.Id, releaseId, cancellationToken);

            await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease(username, folder.Id, releaseId, addedRelease.InstanceId, 4, null, cancellationToken))
                .ThrowsNothing();

            var folderReleases = await _apiClient.GetCollectionItemsByFolder(username, folder.Id, null, null, cancellationToken);

            await Assert.That(folderReleases.Releases).IsNotEmpty();
            await Assert.That(folderReleases.Releases[0].Id).IsEqualTo(releaseId);
            await Assert.That(folderReleases.Releases[0].Rating).IsEqualTo(4);

            await _apiClient.DeleteReleaseFromCollectionFolder(username, folder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
        }
        finally
        {
            await _apiClient.DeleteCollectionFolder(username, folder.Id, cancellationToken);
        }
    }

    [Test]
    [NotInParallel(RatingMutationConstraint)]
    public async Task UpdateCollectionFolderRelease_ShouldMoveFolderOnly_WhenRatingIsNull(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var sourceFolderName = "UpdateCollectionFolderRelease_FolderOnly_Source";
        var targetFolderName = "UpdateCollectionFolderRelease_FolderOnly_Target";
        var releaseId = 5134861;

        var sourceFolder = await _apiClient.CreateCollectionFolder(username, sourceFolderName, cancellationToken);
        var targetFolder = await _apiClient.CreateCollectionFolder(username, targetFolderName, cancellationToken);

        try
        {
            var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, sourceFolder.Id, releaseId, cancellationToken);

            // Establish a known baseline rating before the folder-only move, so we can prove it survives untouched.
            await _apiClient.UpdateCollectionFolderRelease(username, sourceFolder.Id, releaseId, addedRelease.InstanceId, 3, null, cancellationToken);

            await Assert.That(async () => await _apiClient.UpdateCollectionFolderRelease(username, sourceFolder.Id, releaseId, addedRelease.InstanceId, null, targetFolder.Id, cancellationToken))
                .ThrowsNothing();

            var targetFolderReleases = await _apiClient.GetCollectionItemsByFolder(username, targetFolder.Id, null, null, cancellationToken);

            await Assert.That(targetFolderReleases.Releases).IsNotEmpty();
            await Assert.That(targetFolderReleases.Releases[0].Id).IsEqualTo(releaseId);
            await Assert.That(targetFolderReleases.Releases[0].Rating).IsEqualTo(3);

            await _apiClient.DeleteReleaseFromCollectionFolder(username, targetFolder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
        }
        finally
        {
            await _apiClient.DeleteCollectionFolder(username, sourceFolder.Id, cancellationToken);
            await _apiClient.DeleteCollectionFolder(username, targetFolder.Id, cancellationToken);
        }
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowException_WhenUsernameIsInvalid(string? username, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField(username!, 1, 5134861, 1, 1, "value", cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("username");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("username");
    }

    [Test]
    [Arguments(null, typeof(ArgumentNullException))]
    [Arguments("", typeof(ArgumentException))]
    [Arguments("  ", typeof(ArgumentException))]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowException_WhenValueIsInvalid(string? value, Type expectedException, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField("DamIDhagor", 1, 5134861, 1, 1, value!, cancellationToken))
            .Throws<ArgumentException>()
            .WithMessageContaining("value");

        await Assert.That(exception).IsOfType(expectedException);
        await Assert.That(exception!.ParamName).IsEqualTo("value");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowArgumentOutOfRangeException_WhenFolderIdIsInvalid(int folderId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField("DamIDhagor", folderId, 5134861, 1, 1, "value", cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("folderId");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowArgumentOutOfRangeException_WhenReleaseIdIsInvalid(int releaseId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField("DamIDhagor", 1, releaseId, 1, 1, "value", cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("releaseId");
    }

    [Test]
    [Arguments(-1L)]
    [Arguments(0L)]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowArgumentOutOfRangeException_WhenInstanceIdIsInvalid(long instanceId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField("DamIDhagor", 1, 5134861, instanceId, 1, "value", cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("instanceId");
    }

    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowArgumentOutOfRangeException_WhenFieldIdIsInvalid(int fieldId, CancellationToken cancellationToken)
    {
        var exception = await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField("DamIDhagor", 1, 5134861, 1, fieldId, "value", cancellationToken))
            .Throws<ArgumentOutOfRangeException>();

        await Assert.That(exception!.ParamName).IsEqualTo("fieldId");
    }

    [Test]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowResourceNotFoundException_WhenInstanceDoesNotExist(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField("DamIDhagor", 1, 5134861, long.MaxValue, 1, "value", cancellationToken))
            .Throws<ResourceNotFoundDiscogsException>()
            .WithMessage("Release not found in user's collection.");
    }

    [Test]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowDiscogsException_WhenFolderDoesNotExist(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField("DamIDhagor", 999999999, 5134861, 1, 1, "value", cancellationToken))
            .Throws<DiscogsException>()
            .WithMessage("See \"detail\" for validation errors.");
    }

    [Test]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowDiscogsException_WhenFieldDoesNotExist(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "UpdateCollectionFolderReleaseField_FieldNotFound";
        var releaseId = 5134861;

        var collectionFolder = await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken);
        var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId, cancellationToken);

        try
        {
            await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, 999999999, "value", cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("See \"detail\" for validation errors.");
        }
        finally
        {
            await _apiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
            await _apiClient.DeleteCollectionFolder(username, collectionFolder.Id, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateCollectionFolderReleaseField_ShouldThrowUnauthenticatedDiscogsException_WhenUnauthenticated(CancellationToken cancellationToken)
    {
        await Assert.That(async () => await _unauthenticatedApiClient.UpdateCollectionFolderReleaseField("DamIDhagor", 1, 5134861, 1, 1, "value", cancellationToken))
            .Throws<UnauthenticatedDiscogsException>();
    }

    [Test]
    public async Task UpdateCollectionFolderReleaseField_ShouldSetValue_WhenTextareaFieldValueIsValid(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "UpdateCollectionFolderReleaseField_Textarea";
        var releaseId = 5134861;
        var testValue = "API_TEST_VALUE";

        var fieldsResponse = await _apiClient.GetCollectionFields(username, cancellationToken);
        var field = fieldsResponse.Fields.First(f => f.Name == "API_TEST_TEXTAREA_FIELD");

        var collectionFolder = await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken);

        try
        {
            var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId, cancellationToken);

            await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, field.Id, testValue, cancellationToken))
                .ThrowsNothing();

            var releasesResponse = await _apiClient.GetCollectionItemsByFolder(username, collectionFolder.Id, null, null, cancellationToken);
            var note = releasesResponse.Releases[0].Notes?.FirstOrDefault(n => n.FieldId == field.Id);

            await Assert.That(note).IsNotNull();
            await Assert.That(note!.Value).IsEqualTo(testValue);

            await _apiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
        }
        finally
        {
            await _apiClient.DeleteCollectionFolder(username, collectionFolder.Id, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateCollectionFolderReleaseField_ShouldSetValue_WhenDropdownFieldValueIsValidOption(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "UpdateCollectionFolderReleaseField_Dropdown";
        var releaseId = 5134861;

        var fieldsResponse = await _apiClient.GetCollectionFields(username, cancellationToken);
        var field = fieldsResponse.Fields.First(f => f.Name == "API_TEST_DROPDOWN_FIELD");
        var testValue = field.Options!.First();

        var collectionFolder = await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken);

        try
        {
            var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId, cancellationToken);

            await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, field.Id, testValue, cancellationToken))
                .ThrowsNothing();

            var releasesResponse = await _apiClient.GetCollectionItemsByFolder(username, collectionFolder.Id, null, null, cancellationToken);
            var note = releasesResponse.Releases[0].Notes?.FirstOrDefault(n => n.FieldId == field.Id);

            await Assert.That(note).IsNotNull();
            await Assert.That(note!.Value).IsEqualTo(testValue);

            await _apiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
        }
        finally
        {
            await _apiClient.DeleteCollectionFolder(username, collectionFolder.Id, cancellationToken);
        }
    }

    [Test]
    public async Task UpdateCollectionFolderReleaseField_ShouldSetValue_WhenDropdownFieldValueIsNotAValidOption(CancellationToken cancellationToken)
    {
        var username = "DamIDhagor";
        var folderName = "UpdateCollectionFolderReleaseField_Dropdown_InvalidOption";
        var releaseId = 5134861;
        var testValue = "API_TEST_NOT_A_REAL_OPTION";

        var fieldsResponse = await _apiClient.GetCollectionFields(username, cancellationToken);
        var field = fieldsResponse.Fields.First(f => f.Name == "API_TEST_DROPDOWN_FIELD");

        var collectionFolder = await _apiClient.CreateCollectionFolder(username, folderName, cancellationToken);
        var addedRelease = await _apiClient.AddReleaseToCollectionFolder(username, collectionFolder.Id, releaseId, cancellationToken);

        try
        {
            await Assert.That(async () => await _apiClient.UpdateCollectionFolderReleaseField(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, field.Id, testValue, cancellationToken))
                .Throws<DiscogsException>()
                .WithMessage("See \"detail\" for validation errors.");
        }
        finally
        {
            await _apiClient.DeleteReleaseFromCollectionFolder(username, collectionFolder.Id, releaseId, addedRelease.InstanceId, cancellationToken);
            await _apiClient.DeleteCollectionFolder(username, collectionFolder.Id, cancellationToken);
        }
    }
}
