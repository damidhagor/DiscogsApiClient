using static DiscogsApiClient.QueryParameters.MasterReleaseVersionFilterQueryParameters;

namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Filter and sorting query parameters used for retrieving master release versions.
/// </summary>
/// <param name="Format">The format to filter the results by.</param>
/// <param name="Label">The label to filter the results by.</param>
/// <param name="Released">The release year to filter the results by.</param>
/// <param name="Country">The country to filter the results by.</param>
/// <param name="SortProperty">The release property to sort the results with.</param>
/// <param name="SortOrder">The sorting order.</param>
public sealed record MasterReleaseVersionFilterQueryParameters(
    string? Format = default,
    string? Label = default,
    string? Released = default,
    string? Country = default,
    SortableProperty? SortProperty = default,
    SortOrder? SortOrder = default)
    : IQueryParameters
{
    /// <summary>
    /// Release properties which can be used to sort the results with.
    /// </summary>
    public enum SortableProperty
    {
        Released,
        Title,
        Format,
        Label,
        Catno,
        Country
    }

    /// <inheritdoc/>
    public string CreateQueryParameterString()
    {
        var parameters = new List<string>();

        if (Format is not null)
            parameters.Add($"format={Format}");

        if (Label is not null)
            parameters.Add($"label={Label}");

        if (Released is not null)
            parameters.Add($"released={Released}");

        if (Country is not null)
            parameters.Add($"country={Country}");

        if (SortProperty is not null)
            parameters.Add($"sort={SortProperty switch
            {
                SortableProperty.Released => "released",
                SortableProperty.Title => "title",
                SortableProperty.Format => "format",
                SortableProperty.Label => "label",
                SortableProperty.Catno => "catno",
                SortableProperty.Country => "country",
                _ => throw new NotImplementedException()
            }}");

        if (SortOrder is not null)
            parameters.Add($"sort_order={SortOrder switch
            {
                QueryParameters.SortOrder.Ascending => "asc",
                QueryParameters.SortOrder.Descending => "desc",
                _ => throw new InvalidOperationException()
            }}");

        return string.Join("&", parameters);
    }
}
