namespace DiscogsApiClient.Contract.Inventory;

/// <summary>
/// Represents the status of a requested inventory export.
/// </summary>
/// <param name="Id">The export's id.</param>
/// <param name="Status">The export's processing status.</param>
/// <param name="Url">The url to get this export's status.</param>
/// <param name="Filename">The filename of the generated CSV file.</param>
/// <param name="CreatedAt">When the export was requested.</param>
/// <param name="FinishedAt">When the export finished processing. <see langword="null"/> while still in progress.</param>
/// <param name="DownloadUrl">The url to download the generated CSV file. <see langword="null"/> while still in progress.</param>
public sealed record InventoryExport(
    [property: JsonPropertyName("id")]
    long Id,
    [property: JsonPropertyName("status")]
    string Status,
    [property: JsonPropertyName("url")]
    string Url,
    [property: JsonPropertyName("filename")]
    string Filename,
    [property: JsonPropertyName("created_ts")]
    DateTime CreatedAt,
    [property: JsonPropertyName("finished_ts")]
    [property: JsonConverter(typeof(EmptyStringAsNullDateTimeConverter))]
    DateTime? FinishedAt,
    [property: JsonPropertyName("download_url")]
    string? DownloadUrl);
