using System.Text.Json;
using DiscogsApiClient.Contract.Search;
using DiscogsApiClient.SourceGenerator.JsonSerialization;

namespace DiscogsApiClient.Tests.Serialization;

public sealed class EnumConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions().AddGeneratedEnumJsonConverters();


    [Test]
    public async Task ImageType_ShouldThrowNotImplementedExceptionOnSerialization_AndDeserializeCorrectly()
    {
        await Assert.That(() => JsonSerializer.Serialize(ImageType.Primary, _options))
            .Throws<NotImplementedException>();
        await Assert.That(() => JsonSerializer.Serialize(ImageType.Secondary, _options))
            .Throws<NotImplementedException>();

        var primaryValue = JsonSerializer.Deserialize<ImageType>("\"primary\"", _options);
        var secondaryValue = JsonSerializer.Deserialize<ImageType>("\"secondary\"", _options);

        await Assert.That(primaryValue).IsEqualTo(ImageType.Primary);
        await Assert.That(secondaryValue).IsEqualTo(ImageType.Secondary);
    }

    [Test]
    public async Task SearchResultType_ShouldThrowNotImplementedExceptionOnSerialization_AndDeserializeCorrectly()
    {
        await Assert.That(() => JsonSerializer.Serialize(SearchResultType.Artist, _options))
            .Throws<NotImplementedException>();
        await Assert.That(() => JsonSerializer.Serialize(SearchResultType.Master, _options))
            .Throws<NotImplementedException>();
        await Assert.That(() => JsonSerializer.Serialize(SearchResultType.Release, _options))
            .Throws<NotImplementedException>();
        await Assert.That(() => JsonSerializer.Serialize(SearchResultType.Label, _options))
            .Throws<NotImplementedException>();

        var artistValue = JsonSerializer.Deserialize<SearchResultType>("\"artist\"", _options);
        var masterValue = JsonSerializer.Deserialize<SearchResultType>("\"master\"", _options);
        var releaseValue = JsonSerializer.Deserialize<SearchResultType>("\"release\"", _options);
        var labelValue = JsonSerializer.Deserialize<SearchResultType>("\"label\"", _options);

        await Assert.That(artistValue).IsEqualTo(SearchResultType.Artist);
        await Assert.That(masterValue).IsEqualTo(SearchResultType.Master);
        await Assert.That(releaseValue).IsEqualTo(SearchResultType.Release);
        await Assert.That(labelValue).IsEqualTo(SearchResultType.Label);
    }

    [Test]
    public async Task SortOrder_ShouldThrowNotImplementedExceptionOnSerialization_AndDeserializeCorrectly()
    {
        await Assert.That(() => JsonSerializer.Serialize(SortOrder.Ascending, _options))
            .Throws<NotImplementedException>();

        var ascValue = JsonSerializer.Deserialize<SortOrder>("\"asc\"", _options);
        var descValue = JsonSerializer.Deserialize<SortOrder>("\"desc\"", _options);

        await Assert.That(ascValue).IsEqualTo(SortOrder.Ascending);
        await Assert.That(descValue).IsEqualTo(SortOrder.Descending);
    }

    [Test]
    public async Task ArtistReleaseSortableProperty_ShouldThrowNotImplementedExceptionOnSerialization_AndDeserializeCorrectly()
    {
        await Assert.That(() => JsonSerializer.Serialize(ArtistReleaseSortQueryParameters.SortableProperty.Year, _options))
            .Throws<NotImplementedException>();

        var yearValue = JsonSerializer.Deserialize<ArtistReleaseSortQueryParameters.SortableProperty>("\"year\"", _options);
        var titleValue = JsonSerializer.Deserialize<ArtistReleaseSortQueryParameters.SortableProperty>("\"title\"", _options);
        var formatValue = JsonSerializer.Deserialize<ArtistReleaseSortQueryParameters.SortableProperty>("\"format\"", _options);

        await Assert.That(yearValue).IsEqualTo(ArtistReleaseSortQueryParameters.SortableProperty.Year);
        await Assert.That(titleValue).IsEqualTo(ArtistReleaseSortQueryParameters.SortableProperty.Title);
        await Assert.That(formatValue).IsEqualTo(ArtistReleaseSortQueryParameters.SortableProperty.Format);
    }

    [Test]
    public async Task CollectionFolderReleaseSortableProperty_ShouldThrowNotImplementedExceptionOnSerialization_AndDeserializeCorrectly()
    {
        await Assert.That(() => JsonSerializer.Serialize(CollectionFolderReleaseSortQueryParameters.SortableProperty.Artist, _options))
            .Throws<NotImplementedException>();

        var artistValue = JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"artist\"", _options);
        var titleValue = JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"title\"", _options);
        var formatValue = JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"format\"", _options);
        var labelValue = JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"label\"", _options);
        var yearValue = JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"year\"", _options);
        var ratingValue = JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"rating\"", _options);
        var catalogValue = JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"catno\"", _options);
        var addedValue = JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"added\"", _options);

        await Assert.That(artistValue).IsEqualTo(CollectionFolderReleaseSortQueryParameters.SortableProperty.Artist);
        await Assert.That(titleValue).IsEqualTo(CollectionFolderReleaseSortQueryParameters.SortableProperty.Title);
        await Assert.That(formatValue).IsEqualTo(CollectionFolderReleaseSortQueryParameters.SortableProperty.Format);
        await Assert.That(labelValue).IsEqualTo(CollectionFolderReleaseSortQueryParameters.SortableProperty.Label);
        await Assert.That(yearValue).IsEqualTo(CollectionFolderReleaseSortQueryParameters.SortableProperty.Year);
        await Assert.That(ratingValue).IsEqualTo(CollectionFolderReleaseSortQueryParameters.SortableProperty.Rating);
        await Assert.That(catalogValue).IsEqualTo(CollectionFolderReleaseSortQueryParameters.SortableProperty.CatalogNumber);
        await Assert.That(addedValue).IsEqualTo(CollectionFolderReleaseSortQueryParameters.SortableProperty.AddedAt);
    }

    [Test]
    public async Task MasterReleaseVersionSortableProperty_ShouldThrowNotImplementedExceptionOnSerialization_AndDeserializeCorrectly()
    {
        await Assert.That(() => JsonSerializer.Serialize(MasterReleaseVersionFilterQueryParameters.SortableProperty.CatalogNumber, _options))
            .Throws<NotImplementedException>();

        var catalogValue = JsonSerializer.Deserialize<MasterReleaseVersionFilterQueryParameters.SortableProperty>("\"catno\"", _options);
        var countryValue = JsonSerializer.Deserialize<MasterReleaseVersionFilterQueryParameters.SortableProperty>("\"country\"", _options);
        var releasedValue = JsonSerializer.Deserialize<MasterReleaseVersionFilterQueryParameters.SortableProperty>("\"released\"", _options);
        var titleValue = JsonSerializer.Deserialize<MasterReleaseVersionFilterQueryParameters.SortableProperty>("\"title\"", _options);
        var formatValue = JsonSerializer.Deserialize<MasterReleaseVersionFilterQueryParameters.SortableProperty>("\"format\"", _options);
        var labelValue = JsonSerializer.Deserialize<MasterReleaseVersionFilterQueryParameters.SortableProperty>("\"label\"", _options);

        await Assert.That(catalogValue).IsEqualTo(MasterReleaseVersionFilterQueryParameters.SortableProperty.CatalogNumber);
        await Assert.That(countryValue).IsEqualTo(MasterReleaseVersionFilterQueryParameters.SortableProperty.Country);
        await Assert.That(releasedValue).IsEqualTo(MasterReleaseVersionFilterQueryParameters.SortableProperty.Year);
        await Assert.That(titleValue).IsEqualTo(MasterReleaseVersionFilterQueryParameters.SortableProperty.Title);
        await Assert.That(formatValue).IsEqualTo(MasterReleaseVersionFilterQueryParameters.SortableProperty.Format);
        await Assert.That(labelValue).IsEqualTo(MasterReleaseVersionFilterQueryParameters.SortableProperty.Label);
    }

    [Test]
    public async Task Converters_ShouldThrowJsonException_WhenTokenIsInvalidOrUnknown()
    {
        // ImageType
        await Assert.That(() => JsonSerializer.Deserialize<ImageType>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<ImageType>("\"unknown\"", _options)).Throws<JsonException>();

        // SearchResultType
        await Assert.That(() => JsonSerializer.Deserialize<SearchResultType>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<SearchResultType>("\"unknown\"", _options)).Throws<JsonException>();

        // SortOrder
        await Assert.That(() => JsonSerializer.Deserialize<SortOrder>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<SortOrder>("\"unknown\"", _options)).Throws<JsonException>();

        // ArtistReleaseSortQueryParameters.SortableProperty
        await Assert.That(() => JsonSerializer.Deserialize<ArtistReleaseSortQueryParameters.SortableProperty>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<ArtistReleaseSortQueryParameters.SortableProperty>("\"unknown\"", _options)).Throws<JsonException>();

        // CollectionFolderReleaseSortQueryParameters.SortableProperty
        await Assert.That(() => JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<CollectionFolderReleaseSortQueryParameters.SortableProperty>("\"unknown\"", _options)).Throws<JsonException>();

        // MasterReleaseVersionFilterQueryParameters.SortableProperty
        await Assert.That(() => JsonSerializer.Deserialize<MasterReleaseVersionFilterQueryParameters.SortableProperty>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<MasterReleaseVersionFilterQueryParameters.SortableProperty>("\"unknown\"", _options)).Throws<JsonException>();
    }
}


