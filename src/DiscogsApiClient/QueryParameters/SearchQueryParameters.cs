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
    string? Query = default,
    string? Type = default,
    string? Title = default,
    string? ReleaseTitle = default,
    string? Artist = default,
    string? Country = default,
    string? Year = default,
    string? Format = default,
    string? Barcode = default)
    : IQueryParameters
{
    /// <inheritdoc/>
    public string CreateQueryParameterString()
    {
        var parameters = new List<string>();

        if (!string.IsNullOrWhiteSpace(Query))
            parameters.Add($"q={Query}");

        if (!string.IsNullOrWhiteSpace(Type))
            parameters.Add($"type={Type}");

        if (!string.IsNullOrWhiteSpace(Title))
            parameters.Add($"title={Title}");

        if (!string.IsNullOrWhiteSpace(ReleaseTitle))
            parameters.Add($"release_title={ReleaseTitle}");

        if (!string.IsNullOrWhiteSpace(Artist))
            parameters.Add($"artist={Artist}");

        if (!string.IsNullOrWhiteSpace(Country))
            parameters.Add($"country={Country}");

        if (!string.IsNullOrWhiteSpace(Year))
            parameters.Add($"year={Year}");

        if (!string.IsNullOrWhiteSpace(Format))
            parameters.Add($"format={Format}");

        if (!string.IsNullOrWhiteSpace(Barcode))
            parameters.Add($"barcode={Barcode}");

        return string.Join('&', parameters);
    }
}
