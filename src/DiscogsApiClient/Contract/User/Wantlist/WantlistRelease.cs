namespace DiscogsApiClient.Contract.User.Wantlist;

/// <summary>
/// Represents a release from the user's wantlist.
/// </summary>
/// <param name="Id">The release's id.</param>
/// <param name="ResourceUrl">The Api url to the release.</param>
/// <param name="AddedAt">When the release was added to the wantlist.</param>
/// <param name="Rating">The rating of the release.</param>
/// <param name="Release">Information about the release.</param>
/// <param name="Notes">User's notes to this release on the wantlist.</param>
public sealed record WantlistRelease(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("date_added")]
    DateTime AddedAt,
    [property:JsonPropertyName("rating")]
    int Rating,
    [property:JsonPropertyName("basic_information")]
    UserListReleaseInformation Release,
    [property:JsonPropertyName("notes")]
    string Notes);


/**
{
    "id":5134861,
    "resource_url":"https://api.discogs.com/users/DamIDhagor/wants/5134861",
    "rating":0,
    "date_added":"2022-01-22T04:32:49-08:00",
    "basic_information":{
        "id":5134861,
        "master_id":156551,
        "master_url":"https://api.discogs.com/masters/156551",
        "resource_url":"https://api.discogs.com/releases/5134861",
        "title":"Glory To The Brave",
        "year":1997,
        "formats":[
            {
                "name":"CD",
                "qty":"1",
                "descriptions":[
                    "Album",
                    "Promo"
                ]
            }
        ],
        "labels":[
            {
                "name":"Nuclear Blast",
                "catno":"NB 265-2",
                "entity_type":"1",
                "entity_type_name":"Label",
                "id":11499,
                "resource_url":"https://api.discogs.com/labels/11499"
            }
        ],
        "artists":[
            {
                "name":"HammerFall",
                "anv":"",
                "join":"",
                "role":"",
                "tracks":"",
                "id":287459,
                "resource_url":"https://api.discogs.com/artists/287459"
            }
        ],
        "thumb":"https://i.discogs.com/Ltere8XlWzpcylS6RtAJmmDZqViWouYoRVz_RSMR1M4/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTUx/MzQ4NjEtMTM4NTQz/NTQ0Ny0yNjU5Lmpw/ZWc.jpeg",
        "cover_image":"https://i.discogs.com/ZyAJluOoh3pUvc-_P10IZvYXExI14f3LBsqfq1kE1gM/rs:fit/g:sm/q:90/h:500/w:500/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTUx/MzQ4NjEtMTM4NTQz/NTQ0Ny0yNjU5Lmpw/ZWc.jpeg",
        "genres":[
            "Rock"
        ],
        "styles":[
            "Heavy Metal"
        ]
    },
    "notes":""
}
*/
