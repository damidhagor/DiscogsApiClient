﻿namespace DiscogsApiClient.QueryParameters;

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
    [AliasAs("q")]
    string? Query = default,
    [AliasAs("type")]
    string? Type = default,
    [AliasAs("title")]
    string? Title = default,
    [AliasAs("release_title")]
    string? ReleaseTitle = default,
    [AliasAs("artist")]
    string? Artist = default,
    [AliasAs("country")]
    string? Country = default,
    [AliasAs("year")]
    string? Year = default,
    [AliasAs("format")]
    string? Format = default,
    [AliasAs("barcode")]
    string? Barcode = default);
