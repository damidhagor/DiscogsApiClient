using System.Text.Json;
using DiscogsApiClient.Contract.Search;
using DiscogsApiClient.Contract.User.Collection;
using DiscogsApiClient.SourceGenerator.JsonSerialization;

namespace DiscogsApiClient.Tests.Serialization;

public sealed class EnumConverterTests
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions().AddGeneratedEnumJsonConverters();

    [Test]
    public async Task ImageType_ShouldSerializeAndDeserializeCorrectly()
    {
        var primaryJson = JsonSerializer.Serialize(ImageType.Primary, _options);
        var secondaryJson = JsonSerializer.Serialize(ImageType.Secondary, _options);

        await Assert.That(primaryJson).IsEqualTo("\"primary\"");
        await Assert.That(secondaryJson).IsEqualTo("\"secondary\"");

        var primaryValue = JsonSerializer.Deserialize<ImageType>("\"primary\"", _options);
        var secondaryValue = JsonSerializer.Deserialize<ImageType>("\"secondary\"", _options);

        await Assert.That(primaryValue).IsEqualTo(ImageType.Primary);
        await Assert.That(secondaryValue).IsEqualTo(ImageType.Secondary);
    }

    [Test]
    public async Task SearchResultType_ShouldSerializeAndDeserializeCorrectly()
    {
        var artistJson = JsonSerializer.Serialize(SearchResultType.Artist, _options);
        var masterJson = JsonSerializer.Serialize(SearchResultType.Master, _options);
        var releaseJson = JsonSerializer.Serialize(SearchResultType.Release, _options);
        var labelJson = JsonSerializer.Serialize(SearchResultType.Label, _options);

        await Assert.That(artistJson).IsEqualTo("\"artist\"");
        await Assert.That(masterJson).IsEqualTo("\"master\"");
        await Assert.That(releaseJson).IsEqualTo("\"release\"");
        await Assert.That(labelJson).IsEqualTo("\"label\"");

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
    public async Task SortOrder_ShouldSerializeAndDeserializeCorrectly()
    {
        var ascJson = JsonSerializer.Serialize(SortOrder.Ascending, _options);
        var descJson = JsonSerializer.Serialize(SortOrder.Descending, _options);

        await Assert.That(ascJson).IsEqualTo("\"asc\"");
        await Assert.That(descJson).IsEqualTo("\"desc\"");

        var ascValue = JsonSerializer.Deserialize<SortOrder>("\"asc\"", _options);
        var descValue = JsonSerializer.Deserialize<SortOrder>("\"desc\"", _options);

        await Assert.That(ascValue).IsEqualTo(SortOrder.Ascending);
        await Assert.That(descValue).IsEqualTo(SortOrder.Descending);
    }

    [Test]
    public async Task ArtistReleaseSortableProperty_ShouldSerializeAndDeserializeCorrectly()
    {
        var yearJson = JsonSerializer.Serialize(ArtistReleaseSortQueryParameters.SortableProperty.Year, _options);

        await Assert.That(yearJson).IsEqualTo("\"year\"");

        var yearValue = JsonSerializer.Deserialize<ArtistReleaseSortQueryParameters.SortableProperty>("\"year\"", _options);
        var titleValue = JsonSerializer.Deserialize<ArtistReleaseSortQueryParameters.SortableProperty>("\"title\"", _options);
        var formatValue = JsonSerializer.Deserialize<ArtistReleaseSortQueryParameters.SortableProperty>("\"format\"", _options);

        await Assert.That(yearValue).IsEqualTo(ArtistReleaseSortQueryParameters.SortableProperty.Year);
        await Assert.That(titleValue).IsEqualTo(ArtistReleaseSortQueryParameters.SortableProperty.Title);
        await Assert.That(formatValue).IsEqualTo(ArtistReleaseSortQueryParameters.SortableProperty.Format);
    }

    [Test]
    public async Task CollectionFolderReleaseSortableProperty_ShouldSerializeAndDeserializeCorrectly()
    {
        var artistJson = JsonSerializer.Serialize(CollectionFolderReleaseSortQueryParameters.SortableProperty.Artist, _options);

        await Assert.That(artistJson).IsEqualTo("\"artist\"");

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
    public async Task MasterReleaseVersionSortableProperty_ShouldSerializeAndDeserializeCorrectly()
    {
        var catalogJson = JsonSerializer.Serialize(MasterReleaseVersionFilterQueryParameters.SortableProperty.CatalogNumber, _options);

        await Assert.That(catalogJson).IsEqualTo("\"catno\"");

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
    public async Task ContributionSortableProperty_ShouldSerializeAndDeserializeCorrectly()
    {
        var labelJson = JsonSerializer.Serialize(ContributionSortQueryParameters.SortableProperty.Label, _options);

        await Assert.That(labelJson).IsEqualTo("\"label\"");

        var labelValue = JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"label\"", _options);
        var artistValue = JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"artist\"", _options);
        var titleValue = JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"title\"", _options);
        var catalogValue = JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"catno\"", _options);
        var formatValue = JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"format\"", _options);
        var ratingValue = JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"rating\"", _options);
        var yearValue = JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"year\"", _options);
        var addedValue = JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"added\"", _options);

        await Assert.That(labelValue).IsEqualTo(ContributionSortQueryParameters.SortableProperty.Label);
        await Assert.That(artistValue).IsEqualTo(ContributionSortQueryParameters.SortableProperty.Artist);
        await Assert.That(titleValue).IsEqualTo(ContributionSortQueryParameters.SortableProperty.Title);
        await Assert.That(catalogValue).IsEqualTo(ContributionSortQueryParameters.SortableProperty.CatalogNumber);
        await Assert.That(formatValue).IsEqualTo(ContributionSortQueryParameters.SortableProperty.Format);
        await Assert.That(ratingValue).IsEqualTo(ContributionSortQueryParameters.SortableProperty.Rating);
        await Assert.That(yearValue).IsEqualTo(ContributionSortQueryParameters.SortableProperty.Year);
        await Assert.That(addedValue).IsEqualTo(ContributionSortQueryParameters.SortableProperty.AddedAt);
    }

    [Test]
    public async Task MarketplaceInventorySortableProperty_ShouldSerializeAndDeserializeCorrectly()
    {
        var listedJson = JsonSerializer.Serialize(MarketplaceInventoryQueryParameters.SortableProperty.Listed, _options);

        await Assert.That(listedJson).IsEqualTo("\"listed\"");

        var listedValue = JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"listed\"", _options);
        var priceValue = JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"price\"", _options);
        var itemValue = JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"item\"", _options);
        var artistValue = JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"artist\"", _options);
        var labelValue = JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"label\"", _options);
        var catalogValue = JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"catno\"", _options);
        var audioValue = JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"audio\"", _options);
        var statusValue = JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"status\"", _options);

        await Assert.That(listedValue).IsEqualTo(MarketplaceInventoryQueryParameters.SortableProperty.Listed);
        await Assert.That(priceValue).IsEqualTo(MarketplaceInventoryQueryParameters.SortableProperty.Price);
        await Assert.That(itemValue).IsEqualTo(MarketplaceInventoryQueryParameters.SortableProperty.Item);
        await Assert.That(artistValue).IsEqualTo(MarketplaceInventoryQueryParameters.SortableProperty.Artist);
        await Assert.That(labelValue).IsEqualTo(MarketplaceInventoryQueryParameters.SortableProperty.Label);
        await Assert.That(catalogValue).IsEqualTo(MarketplaceInventoryQueryParameters.SortableProperty.CatalogNumber);
        await Assert.That(audioValue).IsEqualTo(MarketplaceInventoryQueryParameters.SortableProperty.Audio);
        await Assert.That(statusValue).IsEqualTo(MarketplaceInventoryQueryParameters.SortableProperty.Status);
    }

    [Test]
    public async Task MarketplaceCurrency_ShouldSerializeAndDeserializeCorrectly()
    {
        var usdJson = JsonSerializer.Serialize(MarketplaceCurrency.Usd, _options);

        await Assert.That(usdJson).IsEqualTo("\"USD\"");

        var usdValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"USD\"", _options);
        var gbpValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"GBP\"", _options);
        var eurValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"EUR\"", _options);
        var cadValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"CAD\"", _options);
        var audValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"AUD\"", _options);
        var jpyValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"JPY\"", _options);
        var chfValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"CHF\"", _options);
        var mxnValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"MXN\"", _options);
        var brlValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"BRL\"", _options);
        var nzdValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"NZD\"", _options);
        var sekValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"SEK\"", _options);
        var zarValue = JsonSerializer.Deserialize<MarketplaceCurrency>("\"ZAR\"", _options);

        await Assert.That(usdValue).IsEqualTo(MarketplaceCurrency.Usd);
        await Assert.That(gbpValue).IsEqualTo(MarketplaceCurrency.Gbp);
        await Assert.That(eurValue).IsEqualTo(MarketplaceCurrency.Eur);
        await Assert.That(cadValue).IsEqualTo(MarketplaceCurrency.Cad);
        await Assert.That(audValue).IsEqualTo(MarketplaceCurrency.Aud);
        await Assert.That(jpyValue).IsEqualTo(MarketplaceCurrency.Jpy);
        await Assert.That(chfValue).IsEqualTo(MarketplaceCurrency.Chf);
        await Assert.That(mxnValue).IsEqualTo(MarketplaceCurrency.Mxn);
        await Assert.That(brlValue).IsEqualTo(MarketplaceCurrency.Brl);
        await Assert.That(nzdValue).IsEqualTo(MarketplaceCurrency.Nzd);
        await Assert.That(sekValue).IsEqualTo(MarketplaceCurrency.Sek);
        await Assert.That(zarValue).IsEqualTo(MarketplaceCurrency.Zar);
    }

    [Test]
    public async Task MarketplaceListingCondition_ShouldSerializeAndDeserializeCorrectly()
    {
        var mintJson = JsonSerializer.Serialize(MarketplaceListingCondition.Mint, _options);

        await Assert.That(mintJson).IsEqualTo("\"Mint (M)\"");

        var mintValue = JsonSerializer.Deserialize<MarketplaceListingCondition>("\"Mint (M)\"", _options);
        var nearMintValue = JsonSerializer.Deserialize<MarketplaceListingCondition>("\"Near Mint (NM or M-)\"", _options);
        var veryGoodPlusValue = JsonSerializer.Deserialize<MarketplaceListingCondition>("\"Very Good Plus (VG+)\"", _options);
        var veryGoodValue = JsonSerializer.Deserialize<MarketplaceListingCondition>("\"Very Good (VG)\"", _options);
        var goodPlusValue = JsonSerializer.Deserialize<MarketplaceListingCondition>("\"Good Plus (G+)\"", _options);
        var goodValue = JsonSerializer.Deserialize<MarketplaceListingCondition>("\"Good (G)\"", _options);
        var fairValue = JsonSerializer.Deserialize<MarketplaceListingCondition>("\"Fair (F)\"", _options);
        var poorValue = JsonSerializer.Deserialize<MarketplaceListingCondition>("\"Poor (P)\"", _options);

        await Assert.That(mintValue).IsEqualTo(MarketplaceListingCondition.Mint);
        await Assert.That(nearMintValue).IsEqualTo(MarketplaceListingCondition.NearMint);
        await Assert.That(veryGoodPlusValue).IsEqualTo(MarketplaceListingCondition.VeryGoodPlus);
        await Assert.That(veryGoodValue).IsEqualTo(MarketplaceListingCondition.VeryGood);
        await Assert.That(goodPlusValue).IsEqualTo(MarketplaceListingCondition.GoodPlus);
        await Assert.That(goodValue).IsEqualTo(MarketplaceListingCondition.Good);
        await Assert.That(fairValue).IsEqualTo(MarketplaceListingCondition.Fair);
        await Assert.That(poorValue).IsEqualTo(MarketplaceListingCondition.Poor);
    }

    [Test]
    public async Task MarketplaceListingSleeveCondition_ShouldSerializeAndDeserializeCorrectly()
    {
        var mintJson = JsonSerializer.Serialize(MarketplaceListingSleeveCondition.Mint, _options);

        await Assert.That(mintJson).IsEqualTo("\"Mint (M)\"");

        var mintValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Mint (M)\"", _options);
        var nearMintValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Near Mint (NM or M-)\"", _options);
        var veryGoodPlusValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Very Good Plus (VG+)\"", _options);
        var veryGoodValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Very Good (VG)\"", _options);
        var goodPlusValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Good Plus (G+)\"", _options);
        var goodValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Good (G)\"", _options);
        var fairValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Fair (F)\"", _options);
        var poorValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Poor (P)\"", _options);
        var genericValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Generic\"", _options);
        var notGradedValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"Not Graded\"", _options);
        var noCoverValue = JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"No Cover\"", _options);

        await Assert.That(mintValue).IsEqualTo(MarketplaceListingSleeveCondition.Mint);
        await Assert.That(nearMintValue).IsEqualTo(MarketplaceListingSleeveCondition.NearMint);
        await Assert.That(veryGoodPlusValue).IsEqualTo(MarketplaceListingSleeveCondition.VeryGoodPlus);
        await Assert.That(veryGoodValue).IsEqualTo(MarketplaceListingSleeveCondition.VeryGood);
        await Assert.That(goodPlusValue).IsEqualTo(MarketplaceListingSleeveCondition.GoodPlus);
        await Assert.That(goodValue).IsEqualTo(MarketplaceListingSleeveCondition.Good);
        await Assert.That(fairValue).IsEqualTo(MarketplaceListingSleeveCondition.Fair);
        await Assert.That(poorValue).IsEqualTo(MarketplaceListingSleeveCondition.Poor);
        await Assert.That(genericValue).IsEqualTo(MarketplaceListingSleeveCondition.Generic);
        await Assert.That(notGradedValue).IsEqualTo(MarketplaceListingSleeveCondition.NotGraded);
        await Assert.That(noCoverValue).IsEqualTo(MarketplaceListingSleeveCondition.NoCover);
    }

    [Test]
    public async Task MarketplaceListingStatus_ShouldSerializeAndDeserializeCorrectly()
    {
        var forSaleJson = JsonSerializer.Serialize(MarketplaceListingStatus.ForSale, _options);
        var draftJson = JsonSerializer.Serialize(MarketplaceListingStatus.Draft, _options);

        await Assert.That(forSaleJson).IsEqualTo("\"For Sale\"");
        await Assert.That(draftJson).IsEqualTo("\"Draft\"");

        var forSaleValue = JsonSerializer.Deserialize<MarketplaceListingStatus>("\"For Sale\"", _options);
        var draftValue = JsonSerializer.Deserialize<MarketplaceListingStatus>("\"Draft\"", _options);

        await Assert.That(forSaleValue).IsEqualTo(MarketplaceListingStatus.ForSale);
        await Assert.That(draftValue).IsEqualTo(MarketplaceListingStatus.Draft);
    }

    [Test]
    public async Task CollectionFieldType_ShouldSerializeAndDeserializeCorrectly()
    {
        var dropdownJson = JsonSerializer.Serialize(CollectionFieldType.Dropdown, _options);
        var textareaJson = JsonSerializer.Serialize(CollectionFieldType.Textarea, _options);

        await Assert.That(dropdownJson).IsEqualTo("\"dropdown\"");
        await Assert.That(textareaJson).IsEqualTo("\"textarea\"");

        var dropdownValue = JsonSerializer.Deserialize<CollectionFieldType>("\"dropdown\"", _options);
        var textareaValue = JsonSerializer.Deserialize<CollectionFieldType>("\"textarea\"", _options);

        await Assert.That(dropdownValue).IsEqualTo(CollectionFieldType.Dropdown);
        await Assert.That(textareaValue).IsEqualTo(CollectionFieldType.Textarea);
    }

    [Test]
    public async Task ListItemType_ShouldSerializeAndDeserializeCorrectly()
    {
        var artistJson = JsonSerializer.Serialize(ListItemType.Artist, _options);
        var masterJson = JsonSerializer.Serialize(ListItemType.Master, _options);
        var releaseJson = JsonSerializer.Serialize(ListItemType.Release, _options);
        var labelJson = JsonSerializer.Serialize(ListItemType.Label, _options);

        await Assert.That(artistJson).IsEqualTo("\"artist\"");
        await Assert.That(masterJson).IsEqualTo("\"master\"");
        await Assert.That(releaseJson).IsEqualTo("\"release\"");
        await Assert.That(labelJson).IsEqualTo("\"label\"");

        var artistValue = JsonSerializer.Deserialize<ListItemType>("\"artist\"", _options);
        var masterValue = JsonSerializer.Deserialize<ListItemType>("\"master\"", _options);
        var releaseValue = JsonSerializer.Deserialize<ListItemType>("\"release\"", _options);
        var labelValue = JsonSerializer.Deserialize<ListItemType>("\"label\"", _options);

        await Assert.That(artistValue).IsEqualTo(ListItemType.Artist);
        await Assert.That(masterValue).IsEqualTo(ListItemType.Master);
        await Assert.That(releaseValue).IsEqualTo(ListItemType.Release);
        await Assert.That(labelValue).IsEqualTo(ListItemType.Label);
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

        // ContributionSortQueryParameters.SortableProperty
        await Assert.That(() => JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<ContributionSortQueryParameters.SortableProperty>("\"unknown\"", _options)).Throws<JsonException>();

        // MarketplaceInventoryQueryParameters.SortableProperty
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceInventoryQueryParameters.SortableProperty>("\"unknown\"", _options)).Throws<JsonException>();

        // MarketplaceCurrency
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceCurrency>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceCurrency>("\"unknown\"", _options)).Throws<JsonException>();

        // MarketplaceListingCondition
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceListingCondition>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceListingCondition>("\"unknown\"", _options)).Throws<JsonException>();

        // MarketplaceListingSleeveCondition
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceListingSleeveCondition>("\"unknown\"", _options)).Throws<JsonException>();

        // MarketplaceListingStatus
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceListingStatus>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<MarketplaceListingStatus>("\"unknown\"", _options)).Throws<JsonException>();

        // CollectionFieldType
        await Assert.That(() => JsonSerializer.Deserialize<CollectionFieldType>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<CollectionFieldType>("\"unknown\"", _options)).Throws<JsonException>();

        // ListItemType
        await Assert.That(() => JsonSerializer.Deserialize<ListItemType>("123", _options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Deserialize<ListItemType>("\"unknown\"", _options)).Throws<JsonException>();
    }

    [Test]
    public async Task Converters_ShouldThrowArgumentOutOfRangeException_WhenSerializingUnmappedEnumValue()
    {
        await Assert.That(() => JsonSerializer.Serialize((ImageType)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((SearchResultType)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((SortOrder)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((ArtistReleaseSortQueryParameters.SortableProperty)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((CollectionFolderReleaseSortQueryParameters.SortableProperty)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((MasterReleaseVersionFilterQueryParameters.SortableProperty)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((ContributionSortQueryParameters.SortableProperty)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((MarketplaceInventoryQueryParameters.SortableProperty)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((MarketplaceCurrency)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((MarketplaceListingCondition)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((MarketplaceListingSleeveCondition)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((MarketplaceListingStatus)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((CollectionFieldType)999, _options)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => JsonSerializer.Serialize((ListItemType)999, _options)).Throws<ArgumentOutOfRangeException>();
    }
}
