namespace DiscogsApiClient.QueryParameters;

public record SearchQueryParameters(
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
