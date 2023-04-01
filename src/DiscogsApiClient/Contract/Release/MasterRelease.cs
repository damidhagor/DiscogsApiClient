namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// The master version of a release which represents its multiple versions
/// </summary>
/// <param name="Id">Master release id</param>
/// <param name="MainReleaseId">Id of the main release version</param>
/// <param name="MostRecentReleaseId">Od of the most recently released versions</param>
/// <param name="ResourceUrl">The Api url for this master release</param>
/// <param name="Uri">The url to the master release's page on the Discogs website</param>
/// <param name="VersionsUrl">The Api url for the release versions list</param>
/// <param name="MainReleaseUrl">The Api url for the main release version</param>
/// <param name="MostRecentReleaseUrl">The Api url for the most recently released version</param>
/// <param name="NumForSale">How many of this release are for sale</param>
/// <param name="LowestPrice">Lowest price for this release</param>
/// <param name="Images">List of images of this master release</param>
/// <param name="Genres">List of this master release's genres</param>
/// <param name="Styles">List of this master release's styles</param>
/// <param name="Year">Release year</param>
/// <param name="Tracklist">Track list</param>
/// <param name="Artists">Artists of this master release</param>
/// <param name="Title">Release title</param>
/// <param name="DataQuality"></param>
/// <param name="Videos">List of related videos for this master release</param>
public sealed record MasterRelease(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("main_release")]
    int MainReleaseId,
    [property:JsonPropertyName("most_recent_release")]
    int MostRecentReleaseId,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("versions_url")]
    string VersionsUrl,
    [property:JsonPropertyName("main_release_url")]
    string MainReleaseUrl,
    [property:JsonPropertyName("most_recent_release_url")]
    string MostRecentReleaseUrl,
    [property:JsonPropertyName("num_for_sale")]
    int NumForSale,
    [property:JsonPropertyName("lowest_price")]
    float? LowestPrice,
    [property:JsonPropertyName("images")]
    List<Image> Images,
    [property:JsonPropertyName("genres")]
    List<string> Genres,
    [property:JsonPropertyName("styles")]
    List<string> Styles,
    [property:JsonPropertyName("year")]
    int Year,
    [property:JsonPropertyName("tracklist")]
    List<TracklistItem> Tracklist,
    [property:JsonPropertyName("artists")]
    List<ReleaseArtist> Artists,
    [property:JsonPropertyName("title")]
    string Title,
    [property:JsonPropertyName("data_quality")]
    string DataQuality,
    [property:JsonPropertyName("videos")]
    List<Video> Videos);


/**
{
    "id":156551,
    "main_release":5134861,
    "most_recent_release":13391159,
    "resource_url":"https://api.discogs.com/masters/156551",
    "uri":"https://www.discogs.com/master/156551-HammerFall-Glory-To-The-Brave",
    "versions_url":"https://api.discogs.com/masters/156551/versions",
    "main_release_url":"https://api.discogs.com/releases/5134861",
    "most_recent_release_url":"https://api.discogs.com/releases/13391159",
    "num_for_sale":232,
    "lowest_price":2.22,
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
    "genres":[
        "Rock"
    ],
    "styles":[
        "Heavy Metal"
    ],
    "year":1997,
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
    "title":"Glory To The Brave",
    "data_quality":"Correct",
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
    ]
}
*/
