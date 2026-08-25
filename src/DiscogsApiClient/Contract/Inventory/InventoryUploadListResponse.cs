namespace DiscogsApiClient.Contract.Inventory;

/// <summary>
/// A paginated list of recent inventory uploads.
/// </summary>
/// <param name="Pagination">Pagination information.</param>
/// <param name="Items">The uploads on this page.</param>
public sealed record InventoryUploadListResponse(
    [property: JsonPropertyName("pagination")]
    Pagination Pagination,
    [property: JsonPropertyName("items")]
    IReadOnlyList<InventoryUpload> Items);
