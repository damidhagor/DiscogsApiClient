namespace DiscogsApiClient.Contract.Inventory;

/// <summary>
/// A paginated list of recent inventory exports.
/// </summary>
/// <param name="Pagination">Pagination information.</param>
/// <param name="Items">The exports on this page.</param>
public sealed record InventoryExportListResponse(
    [property: JsonPropertyName("pagination")]
    Pagination Pagination,
    [property: JsonPropertyName("items")]
    IReadOnlyList<InventoryExport> Items);
