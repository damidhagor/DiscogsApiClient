namespace DiscogsApiClient.Contract.Inventory;

/// <summary>
/// Represents the status of a requested inventory upload (bulk add/change/delete).
/// </summary>
/// <param name="Id">The upload's id.</param>
/// <param name="Type">The kind of mutation this upload performs.</param>
/// <param name="Status">The upload's processing status.</param>
/// <param name="Filename">The filename of the uploaded CSV file.</param>
/// <param name="CreatedAt">When the upload was submitted.</param>
/// <param name="FinishedAt">When the upload finished processing. <see langword="null"/> while still in progress.</param>
/// <param name="Results">A human-readable summary of the processing results. <see langword="null"/> while still in progress.</param>
public sealed record InventoryUpload(
    [property: JsonPropertyName("id")]
    long Id,
    [property: JsonPropertyName("type")]
    string Type,
    [property: JsonPropertyName("status")]
    string Status,
    [property: JsonPropertyName("filename")]
    string Filename,
    [property: JsonPropertyName("created_ts")]
    DateTime CreatedAt,
    [property: JsonPropertyName("finished_ts")]
    [property: JsonConverter(typeof(EmptyStringAsNullDateTimeConverter))]
    DateTime? FinishedAt,
    [property: JsonPropertyName("results")]
    string? Results);
