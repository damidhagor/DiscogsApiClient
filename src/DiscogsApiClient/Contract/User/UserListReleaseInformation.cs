namespace DiscogsApiClient.Contract.User;

public sealed record UserListReleaseInformation(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("master_id")]
    int MasterId,
    [property:JsonPropertyName("master_url")]
    string MasterUrl,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("title")]
    string Title,
    [property:JsonPropertyName("year")]
    int Year,
    [property:JsonPropertyName("formats")]
    List<ReleaseFormat> Formats,
    [property:JsonPropertyName("labels")]
    List<ReleaseLabel> Labels,
    [property:JsonPropertyName("artists")]
    List<ReleaseArtist> Artists,
    [property:JsonPropertyName("thumb")]
    string ThumbnailUrl,
    [property:JsonPropertyName("cover_image")]
    string CoverImageUrl,
    [property:JsonPropertyName("genres")]
    List<string> Genres,
    [property:JsonPropertyName("styles")]
    List<string> Styles);


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
