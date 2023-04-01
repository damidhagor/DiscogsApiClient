namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// Represents a release in the Discogs database
/// </summary>
/// <param name="Id">Release id</param>
/// <param name="ResourceUrl">The Api url for this release</param>
/// <param name="Uri">The url to the release page on the Discogs website</param>
/// <param name="Artists">List of artists</param>
/// <param name="ArtistsSort"></param>
/// <param name="Labels">List of publishing labels</param>
/// <param name="Formats">List of available physical formats"</param>
/// <param name="CommunityStatistics">Community statistics for this release</param>
/// <param name="FormatCount">Number of available formats</param>
/// <param name="AddedAt">When this release was added to the database</param>
/// <param name="ChangedAt">When this release's database entry was last changed</param>
/// <param name="NumForSale">How many of this release are for sale</param>
/// <param name="LowestPrice">The lowest available price</param>
/// <param name="MasterId">The id of the master release</param>
/// <param name="MasterUrl">The Api url to the master release</param>
/// <param name="Title">Release title</param>
/// <param name="Country">Release country</param>
/// <param name="Year">Release year</param>
/// <param name="Notes">Notes for this release</param>
/// <param name="YearFormatted">Formatted release year</param>
/// <param name="Identifiers">List of identifiers</param>
/// <param name="Videos">List of related videos</param>
/// <param name="Genres">List of genres</param>
/// <param name="Styles">List of styles</param>
/// <param name="Tracklist">track list</param>
/// <param name="ExtraArtists"></param>
/// <param name="Images">List of images for this release</param>
/// <param name="ThumbnailUrl">Thumbnail image url</param>
/// <param name="EstimatedWeight">The estimated weight</param>
/// <param name="IsBlockedFromSale">If this release is blocked for being sold</param>
/// <param name="DataQuality"></param>
public sealed record Release(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("artists")]
    List<ReleaseArtist> Artists,
    [property:JsonPropertyName("artists_sort")]
    string ArtistsSort,
    [property:JsonPropertyName("labels")]
    List<ReleaseLabel> Labels,
    [property:JsonPropertyName("formats")]
    List<ReleaseFormat> Formats,
    [property:JsonPropertyName("community")]
    ReleaseCommunity CommunityStatistics,
    [property:JsonPropertyName("format_quantity")]
    int FormatCount,
    [property:JsonPropertyName("date_added")]
    DateTime AddedAt,
    [property:JsonPropertyName("date_changed")]
    DateTime ChangedAt,
    [property:JsonPropertyName("num_for_sale")]
    int NumForSale,
    [property:JsonPropertyName("lowest_price")]
    float? LowestPrice,
    [property:JsonPropertyName("master_id")]
    int MasterId,
    [property:JsonPropertyName("master_url")]
    string MasterUrl,
    [property:JsonPropertyName("title")]
    string Title,
    [property:JsonPropertyName("country")]
    string Country,
    [property:JsonPropertyName("released")]
    string Year,
    [property:JsonPropertyName("notes")]
    string Notes,
    [property:JsonPropertyName("released_formatted")]
    string YearFormatted,
    [property:JsonPropertyName("identifiers")]
    List<ReleaseIdentifiers> Identifiers,
    [property:JsonPropertyName("videos")]
    List<Video> Videos,
    [property:JsonPropertyName("genres")]
    List<string> Genres,
    [property:JsonPropertyName("styles")]
    List<string> Styles,
    [property:JsonPropertyName("tracklist")]
    List<TracklistItem> Tracklist,
    [property:JsonPropertyName("extraartists")]
    List<ReleaseArtist> ExtraArtists,
    [property:JsonPropertyName("images")]
    List<Image> Images,
    [property:JsonPropertyName("thumb")]
    string ThumbnailUrl,
    [property:JsonPropertyName("estimated_weight")]
    float EstimatedWeight,
    [property:JsonPropertyName("blocked_from_sale")]
    bool IsBlockedFromSale,
    [property:JsonPropertyName("data_quality")]
    string DataQuality);


/**
{
    "id":5134861,
    "status":"Accepted",
    "year":1997,
    "resource_url":"https://api.discogs.com/releases/5134861",
    "uri":"https://www.discogs.com/release/5134861-HammerFall-Glory-To-The-Brave",
    "artists":[
        {
            "name":"HammerFall",
            "anv":"",
            "join":"",
            "role":"",
            "tracks":"",
            "id":287459,
            "resource_url":"https://api.discogs.com/artists/287459",
            "thumbnail_url":"https://img.discogs.com/skjsztMZregZjJabZGtQE79D0HU=/600x399/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/A-287459-1622414253-4249.jpeg.jpg"
        }
    ],
    "artists_sort":"HammerFall",
    "labels":[
        {
            "name":"Nuclear Blast",
            "catno":"NB 265-2",
            "entity_type":"1",
            "entity_type_name":"Label",
            "id":11499,
            "resource_url":"https://api.discogs.com/labels/11499",
            "thumbnail_url":"https://img.discogs.com/IoOtLAyAfRNqKqlTEsuksev0Htg=/fit-in/535x188/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/L-11499-1301404145.jpeg.jpg"
        }
    ],
    "series":[
        
    ],
    "companies":[
        
    ],
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
    "data_quality":"Correct",
    "community":{
        "have":285,
        "want":28,
        "rating":{
            "count":22,
            "average":4.05
        },
        "submitter":{
            "username":"zolike",
            "resource_url":"https://api.discogs.com/users/zolike"
        },
        "contributors":[
            {
                "username":"zolike",
                "resource_url":"https://api.discogs.com/users/zolike"
            },
            {
                "username":"ReuterBJ",
                "resource_url":"https://api.discogs.com/users/ReuterBJ"
            }
        ],
        "data_quality":"Correct",
        "status":"Accepted"
    },
    "format_quantity":1,
    "date_added":"2013-11-25T19:13:03-08:00",
    "date_changed":"2013-12-09T16:44:30-08:00",
    "num_for_sale":7,
    "lowest_price":7.4,
    "master_id":156551,
    "master_url":"https://api.discogs.com/masters/156551",
    "title":"Glory To The Brave",
    "country":"Germany",
    "released":"1997",
    "notes":"Promotional version in a cardboard sleeve.\r\n",
    "released_formatted":"1997",
    "identifiers":[
        {
            "type":"Barcode",
            "value":"7 27361 62652 5"
        }
    ],
    "videos":[
        {
            "uri":"https://www.youtube.com/watch?v=QD1j8c3GBAo",
            "title":"HAMMERFALL - GLORY TO THE BRAVE: 20TH ANNIVERSARY LTD. BOXSET EDITION unboxing",
            "description":"All rights reserved!\n\u24c5 + \u24b8 2017\nN u c l e a r    B l a s t   G m b H\n\nB U Y     T H E    M U S I C!\nR E S P E C T    T H E   A R T I S T S! \n\nBUY THE BOXSET HERE: http://www.nuclearblast.de/en/products/tontraeger/cd/2cd-digi-dvd/hammerfall-glory-to-th",
            "duration":121,
            "embed":true
        },
        {
            "uri":"https://www.youtube.com/watch?v=YeySMqUqeWw",
            "title":"HAMMERFALL - Glory to the Brave [Full Album 1997] + B\u00f6nus tracks",
            "description":"0:00:00 - 01.The Dragon Lies Bleeding\n0:04:22 - 02.The Metal Age\n0:08:51 - 03.HammeFall\n0:13:38 - 04.I Believe\n0:18:32 - 05.Child Of The Damned (Warlord cover)\n0:22:15 - 06.Steel Meets Steel\n0:26:17 - 07.Stone Cold\n*\n0:32:00 - 09.Glory To The Brave\n0:39:2",
            "duration":3273,
            "embed":true
        }
    ],
    "genres":[
        "Rock"
    ],
    "styles":[
        "Heavy Metal"
    ],
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
    "extraartists":[
        
    ],
    "images":[
        {
            "type":"primary",
            "uri":"https://img.discogs.com/xcF-IN7CpqTRugTqJ-HTaAMb2rQ=/fit-in/500x500/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/R-5134861-1385435447-2659.jpeg.jpg",
            "resource_url":"https://img.discogs.com/xcF-IN7CpqTRugTqJ-HTaAMb2rQ=/fit-in/500x500/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/R-5134861-1385435447-2659.jpeg.jpg",
            "uri150":"https://img.discogs.com/7MS62Ak72b7utY-6hrUc3_Cva2I=/fit-in/150x150/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/R-5134861-1385435447-2659.jpeg.jpg",
            "width":500,
            "height":500
        },
        {
            "type":"secondary",
            "uri":"https://img.discogs.com/DuP1jwDs-6HYi_FmtA8Xw4aSyVY=/fit-in/595x600/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/R-5134861-1385435436-7875.jpeg.jpg",
            "resource_url":"https://img.discogs.com/DuP1jwDs-6HYi_FmtA8Xw4aSyVY=/fit-in/595x600/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/R-5134861-1385435436-7875.jpeg.jpg",
            "uri150":"https://img.discogs.com/QisyZS-qh4tsXSKaAhQCw-pTY_c=/fit-in/150x150/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/R-5134861-1385435436-7875.jpeg.jpg",
            "width":595,
            "height":600
        }
    ],
    "thumb":"https://img.discogs.com/7MS62Ak72b7utY-6hrUc3_Cva2I=/fit-in/150x150/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/R-5134861-1385435447-2659.jpeg.jpg",
    "estimated_weight":85,
    "blocked_from_sale":false
}
*/
