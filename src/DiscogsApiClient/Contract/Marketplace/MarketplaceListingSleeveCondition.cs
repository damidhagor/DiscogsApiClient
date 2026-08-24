using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.Contract.Marketplace;

/// <summary>
/// The condition of the sleeve/cover in a Marketplace listing.
/// </summary>
[GenerateJsonConverter]
public enum MarketplaceListingSleeveCondition
{
    [AliasAs("Mint (M)")]
    Mint,
    [AliasAs("Near Mint (NM or M-)")]
    NearMint,
    [AliasAs("Very Good Plus (VG+)")]
    VeryGoodPlus,
    [AliasAs("Very Good (VG)")]
    VeryGood,
    [AliasAs("Good Plus (G+)")]
    GoodPlus,
    [AliasAs("Good (G)")]
    Good,
    [AliasAs("Fair (F)")]
    Fair,
    [AliasAs("Poor (P)")]
    Poor,
    [AliasAs("Generic")]
    Generic,
    [AliasAs("Not Graded")]
    NotGraded,
    [AliasAs("No Cover")]
    NoCover
}
