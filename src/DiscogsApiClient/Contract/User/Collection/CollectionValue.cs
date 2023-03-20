namespace DiscogsApiClient.Contract.User.Collection;

/// <summary>
/// The estimated value of the user's collection
/// </summary>
/// <param name="Minimum">The minimum value</param>
/// <param name="Median">The average value</param>
/// <param name="Maximum">The maximum value</param>
public sealed record CollectionValue(
    [property:JsonPropertyName("minimum")]
    string Minimum,
    [property:JsonPropertyName("median")]
    string Median,
    [property:JsonPropertyName("maximum")]
    string Maximum);


/*
{
   "minimum":"\u20ac120.09",
   "median":"\u20ac178.10",
   "maximum":"\u20ac295.96"
}
 */
