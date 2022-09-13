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
    string? Query,
    string? Type,
    string? Title,
    string? ReleaseTitle,
    string? Artist,
    string? Country,
    string? Year,
    string? Format,
    string? Barcode)
{
    public SearchQueryParameters()
        : this(null, null, null, null, null, null, null, null, null)
    { }

    /// <summary>
    /// Creates the Url query representation of the search parameters.
    /// The starting '?' query indicator is not included.
    /// </summary>
    /// <returns>Url query representation of the search parameters without '?'.</returns>
    public string CreateQueryParameterString()
    {
        var parameters = new List<string>();

        if (!String.IsNullOrWhiteSpace(Query))
            parameters.Add($"q={Query}");

        if (!String.IsNullOrWhiteSpace(Type))
            parameters.Add($"type={Type}");

        if (!String.IsNullOrWhiteSpace(Title))
            parameters.Add($"title={Title}");

        if (!String.IsNullOrWhiteSpace(ReleaseTitle))
            parameters.Add($"release_title={ReleaseTitle}");

        if (!String.IsNullOrWhiteSpace(Artist))
            parameters.Add($"artist={Artist}");

        if (!String.IsNullOrWhiteSpace(Country))
            parameters.Add($"country={Country}");

        if (!String.IsNullOrWhiteSpace(Year))
            parameters.Add($"year={Year}");

        if (!String.IsNullOrWhiteSpace(Format))
            parameters.Add($"format={Format}");

        if (!String.IsNullOrWhiteSpace(Barcode))
            parameters.Add($"barcode={Barcode}");

        return String.Join('&', parameters);
    }
}
