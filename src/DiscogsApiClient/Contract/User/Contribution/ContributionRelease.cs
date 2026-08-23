namespace DiscogsApiClient.Contract.User.Contribution;

/// <summary>
/// A release contributed (added or edited) by a user, as returned in a user's contributions.
/// </summary>
/// <param name="Id">Release id</param>
/// <param name="Status">Release status, e.g. "Accepted"</param>
/// <param name="ResourceUrl">The Api url for this release</param>
/// <param name="Uri">The url to the release's page on Discogs</param>
/// <param name="Artists">The artists of this release</param>
/// <param name="ArtistsSort"></param>
/// <param name="Labels">The labels of this release</param>
/// <param name="Series">The series this release is part of, if any</param>
/// <param name="Companies">The companies involved in this release, if any</param>
/// <param name="Formats">The formats this release was released in</param>
/// <param name="DataQuality"></param>
/// <param name="CommunityStatistics">Community statistics for this release</param>
/// <param name="FormatCount">The amount of formats this release was released in</param>
/// <param name="AddedAt">When this release was added to Discogs</param>
/// <param name="ChangedAt">When this release was last changed</param>
/// <param name="NumForSale">How many copies of this release are for sale in the marketplace</param>
/// <param name="LowestPrice">The lowest marketplace price for this release</param>
/// <param name="MasterId">The id of the master release this release belongs to, if any</param>
/// <param name="MasterUrl">The Api url of the master release this release belongs to, if any</param>
/// <param name="Title">Release title</param>
/// <param name="Year">Release year</param>
/// <param name="Country">Country this release was released in, if known</param>
/// <param name="Released">The release date, if known</param>
/// <param name="Notes">Additional notes about the release, if any</param>
/// <param name="ReleasedFormatted">The formatted release date, if known</param>
/// <param name="Identifiers">Identifiers of this release, e.g. barcodes</param>
/// <param name="Genres">Genres of this release</param>
/// <param name="Styles">Styles of this release</param>
/// <param name="Images">Images of this release</param>
/// <param name="ThumbnailUrl">Thumbnail image url of this release</param>
/// <param name="EstimatedWeight">The estimated weight of this release</param>
public sealed record ContributionRelease(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("status")]
    string Status,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("artists")]
    IReadOnlyList<ContributionReleaseArtist> Artists,
    [property:JsonPropertyName("artists_sort")]
    string ArtistsSort,
    [property:JsonPropertyName("labels")]
    IReadOnlyList<ContributionReleaseLabel> Labels,
    [property:JsonPropertyName("series")]
    IReadOnlyList<ContributionReleaseLabel>? Series,
    [property:JsonPropertyName("companies")]
    IReadOnlyList<ContributionReleaseLabel>? Companies,
    [property:JsonPropertyName("formats")]
    IReadOnlyList<ReleaseFormat> Formats,
    [property:JsonPropertyName("data_quality")]
    string DataQuality,
    [property:JsonPropertyName("community")]
    ReleaseCommunity CommunityStatistics,
    [property:JsonPropertyName("format_quantity")]
    int FormatCount,
    [property:JsonPropertyName("date_added")]
    DateTime AddedAt,
    [property:JsonPropertyName("date_changed")]
    DateTime ChangedAt,
    [property:JsonPropertyName("num_for_sale")]
    int NumForSale,
    [property:JsonPropertyName("lowest_price")]
    float? LowestPrice,
    [property:JsonPropertyName("master_id")]
    int? MasterId,
    [property:JsonPropertyName("master_url")]
    string? MasterUrl,
    [property:JsonPropertyName("title")]
    string Title,
    [property:JsonPropertyName("year")]
    int Year,
    [property:JsonPropertyName("country")]
    string? Country,
    [property:JsonPropertyName("released")]
    string? Released,
    [property:JsonPropertyName("notes")]
    string? Notes,
    [property:JsonPropertyName("released_formatted")]
    string? ReleasedFormatted,
    [property:JsonPropertyName("identifiers")]
    IReadOnlyList<ReleaseIdentifier> Identifiers,
    [property:JsonPropertyName("genres")]
    IReadOnlyList<string> Genres,
    [property:JsonPropertyName("styles")]
    IReadOnlyList<string> Styles,
    [property:JsonPropertyName("images")]
    IReadOnlyList<Image> Images,
    [property:JsonPropertyName("thumb")]
    string ThumbnailUrl,
    [property:JsonPropertyName("estimated_weight")]
    float EstimatedWeight);
