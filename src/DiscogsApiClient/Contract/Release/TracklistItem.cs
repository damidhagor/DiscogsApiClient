namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// Represents an item in a track list
/// </summary>
/// <param name="Position">Track's position in the list (includes sides)</param>
/// <param name="Type">Track type (e.g. track or video)</param>
/// <param name="Title">Track title</param>
/// <param name="Duration">Track duration (e.g. 03:15)</param>
public sealed record TracklistItem(
    [property:JsonPropertyName("position")]
    string Position,
    [property:JsonPropertyName("type_")]
    string Type,
    [property:JsonPropertyName("title")]
    string Title,
    [property:JsonPropertyName("duration")]
    string Duration);


/*
"tracklist":[
    {
        "position":"1",
        "type_":"track",
        "title":"The Dragon Lies Bleeding",
        "duration":"4:23"
    },
    {
        "position":"2",
        "type_":"track",
        "title":"The Metal Age",
        "duration":"4:29"
    }
],
*/
