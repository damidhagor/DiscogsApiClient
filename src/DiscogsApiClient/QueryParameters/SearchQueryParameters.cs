using DiscogsApiClient.ApiClientGenerator;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Search parameters used by search requests to the Discogs Api.
/// All search parameters are optional and can be combined.
/// </summary>
/// <param name="Query">A generic search string. (E.g. like using the search box on the website)</param>
/// <param name="Type">Which type of search results to include. ("release", "master", "artist", "label")</param>
/// <param name="Title">Search by combined “Artist Name - Release Title” title field.</param>
/// <param name="ReleaseTitle">Search by the title of a release.</param>
/// <param name="Artist">Search by artist name.</param>
/// <param name="Country">Search by release country.</param>
/// <param name="Year">Search by release year.</param>
/// <param name="Format">Search by release format.</param>
/// <param name="Barcode">Search by barcode.</param>
public sealed record SearchQueryParameters(
    [property: AliasAs("q")]
    string? Query = default,
    [property: AliasAs("type")]
    string? Type = default,
    [property: AliasAs("title")]
    string? Title = default,
    [property: AliasAs("release_title")]
    string? ReleaseTitle = default,
    [property: AliasAs("artist")]
    string? Artist = default,
    [property: AliasAs("country")]
    string? Country = default,
    [property: AliasAs("year")]
    string? Year = default,
    [property: AliasAs("format")]
    string? Format = default,
    [property: AliasAs("barcode")]
    string? Barcode = default);
