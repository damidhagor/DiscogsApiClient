namespace DiscogsApiClient.QueryParameters;

/// <summary>
/// Interface which an implementation of query parameters must implement.
/// </summary>
internal interface IQueryParameters
{
    /// <summary>
    /// Creates the Url query representation of the query parameters.
    /// The starting '?' query indicator is not included.
    /// </summary>
    /// <returns>Url query representation of the parameters without '?'.</returns>
    string CreateQueryParameterString();
}
