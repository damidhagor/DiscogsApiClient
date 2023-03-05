namespace DiscogsApiClient.Contract.User.Collection;

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
