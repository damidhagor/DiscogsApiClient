namespace DiscogsApiClient.Contract;

public sealed record CollectionValue(
    string Minimum,
    string Median,
    string Maximum);

/*
{
   "minimum":"\u20ac120.09",
   "median":"\u20ac178.10",
   "maximum":"\u20ac295.96"
}
 */