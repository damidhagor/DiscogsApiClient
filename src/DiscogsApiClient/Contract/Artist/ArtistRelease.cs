﻿namespace DiscogsApiClient.Contract.Artist;

/// <summary>
/// Information about a release of an artist.
/// </summary>
/// <param name="Id">Release id</param>
/// <param name="ResourceUrl">The Api url to this release</param>
/// <param name="ThumbnailUrl">Thumbnail url for the cover image</param>
/// <param name="Type">The type of the release (e.g. master or normal release)</param>
/// <param name="Title">Release title</param>
/// <param name="MainReleaseId">Id of the main release</param>
/// <param name="Artist">Artist name</param>
/// <param name="Role">The contributing role of the artist on this release</param>
/// <param name="Year">Release year</param>
/// <param name="Statistics">Statistics for this release</param>
public sealed record ArtistRelease(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("thumb")]
    string ThumbnailUrl,
    [property:JsonPropertyName("type")]
    string Type,
    [property:JsonPropertyName("title")]
    string Title,
    [property:JsonPropertyName("main_release")]
    int MainReleaseId,
    [property:JsonPropertyName("artist")]
    string Artist,
    [property:JsonPropertyName("role")]
    string Role,
    [property:JsonPropertyName("year")]
    int Year,
    [property:JsonPropertyName("stats")]
    ReleaseStats Statistics);


/*
{
    "pagination":{
        "page":1,
        "pages":8,
        "per_page":50,
        "items":397,
        "urls":{
            "last":"https://api.discogs.com/artists/287459/releases?page=8&per_page=50",
            "next":"https://api.discogs.com/artists/287459/releases?page=2&per_page=50"
        }
    },
    "releases":[
        {
            "id":388731,
            "title":"Glory To The Brave",
            "type":"master",
            "main_release":4970915,
            "artist":"HammerFall",
            "role":"Main",
            "resource_url":"https://api.discogs.com/masters/388731",
            "year":1997,
            "thumb":"https://i.discogs.com/4ki15UCT95uRyIjAzl7QxpMDfv3sZUBriRM4L4-GFv4/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTQ5/NzA5MTUtMTQzMjcz/MTc2MC0zMjc1Lmpw/ZWc.jpeg",
            "stats":{
                "community":{
                    "in_wantlist":26,
                    "in_collection":29
                },
                "user":{
                    "in_wantlist":0,
                    "in_collection":0
                }
            }
        },
        {
            "id":156551,
            "title":"Glory To The Brave",
            "type":"master",
            "main_release":5134861,
            "artist":"HammerFall",
            "role":"Main",
            "resource_url":"https://api.discogs.com/masters/156551",
            "year":1997,
            "thumb":"https://i.discogs.com/Ltere8XlWzpcylS6RtAJmmDZqViWouYoRVz_RSMR1M4/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTUx/MzQ4NjEtMTM4NTQz/NTQ0Ny0yNjU5Lmpw/ZWc.jpeg",
            "stats":{
                "community":{
                    "in_wantlist":28,
                    "in_collection":289
                },
                "user":{
                    "in_wantlist":0,
                    "in_collection":0
                }
            }
        }
    ]
}
 */
