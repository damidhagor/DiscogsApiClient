namespace DiscogsApiClient.Contract;

public record Artist(
    int Id,
    string Name,
    string Profile,
    string ResourceUrl,
    string Uri,
    string ReleasesUrl,
    List<Image> Images,
    List<string> Urls,
    List<string> Namevariations,
    List<ArtistMember> Members,
    string DataQuality);

public record ArtistMember(
    int Id,
    string Name,
    bool Active,
    string ResourceUrl,
    string ThumbnailUrl);

public enum ArtistImageType : int
{
    None = 0,
    Primary = 1,
    Secondary = 2
}


/**
{
    "name":"HammerFall",
    "id":287459,
    "resource_url":"https://api.discogs.com/artists/287459",
    "uri":"https://www.discogs.com/artist/287459-HammerFall",
    "releases_url":"https://api.discogs.com/artists/287459/releases",
    "images":[
        {
            "type":"primary",
            "uri":"https://img.discogs.com/nXjccemopM6jMNTWkFLMLed5gzI=/600x399/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/A-287459-1622414253-4249.jpeg.jpg",
            "resource_url":"https://img.discogs.com/nXjccemopM6jMNTWkFLMLed5gzI=/600x399/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/A-287459-1622414253-4249.jpeg.jpg",
            "uri150":"https://img.discogs.com/Gq1JCFK_iheo-wDN6TCS1dG_fys=/150x150/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/A-287459-1622414253-4249.jpeg.jpg",
            "width":600,
            "height":399
        },
        {
            "type":"secondary",
            "uri":"https://img.discogs.com/AhIzRhQg6u9TfOBRe4N1nLfzjmU=/600x541/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/A-287459-1482006672-9012.jpeg.jpg",
            "resource_url":"https://img.discogs.com/AhIzRhQg6u9TfOBRe4N1nLfzjmU=/600x541/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/A-287459-1482006672-9012.jpeg.jpg",
            "uri150":"https://img.discogs.com/uMpe1Jw-HUmkEopS_84U7ENgnYw=/150x150/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/A-287459-1482006672-9012.jpeg.jpg",
            "width":600,
            "height":541
        }
    ],
    "profile":"Power Metal (Heavy Metal) band from Gothenburg (Sweden). \r\n\r\nThe band was founded in April 1993 by Oscar Dronjak.\r\n\r\nCurrent lineup:\r\nOscar Dronjak - Guitar (1993- )\r\nFredrik Larsson - Bass (1994-1997, 2007- )\r\nJoacim Cans - Vocals (1996- )\r\nPontus Norgren - Guitar (2008- )\r\nDavid Wallin - Drums (2014-2016, 2017- )\r\n\r\nFormer members:\r\nJohan Larsson - Bass (1993-1994)\r\nNiklas Sundin - Guitar (1993-1995)\r\nMikael Stanne - Vocals (1993-1996)\r\nJesper Str\u00f6mblad - Drums (1993-1997)\r\nGlenn Ljungstr\u00f6m - Guitar (1995-1997)\r\nPatrik R\u00e4fling - Drums (1997-1999)\r\nMagnus Ros\u00e9n - Bass (1997-2007)\r\nStefan Elmgren - Guitar (1997-2008)\r\nAnders Johansson - Drums (1999-2014)\r\nJohan Kullberg - Drums (2016-2017)",
    "urls":[
        "http://www.hammerfall.net/",
        "https://www.facebook.com/hammerfall",
        "https://myspace.com/hammerfall",
        "https://twitter.com/HammerFall",
        "https://www.youtube.com/channel/UC3Yk2TmVkqi_Kmgct5yZaHA",
        "https://en.wikipedia.org/wiki/HammerFall"
    ],
    "namevariations":[
        "Hammerfull",
        "Hammerfall",
        "\u30cf\u30f3\u30de\u30fc\u30d5\u30a9\u30fc\u30eb"
    ],
    "members":[
        {
            "id":262015,
            "name":"Oscar Dronjak",
            "resource_url":"https://api.discogs.com/artists/262015",
            "active":true,
            "thumbnail_url":"https://img.discogs.com/vQ6xZBCeNeUPNbbLcxkHF0GhmJ4=/224x298/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/A-262015-1294861283.jpeg.jpg"
        },
        {
            "id":262022,
            "name":"Jesper Str\u00f6mblad",
            "resource_url":"https://api.discogs.com/artists/262022",
            "active":false,
            "thumbnail_url":"https://img.discogs.com/mo-s8C_AA4Mdskj1FMcshL9yMgg=/600x400/smart/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/A-262022-1472483054-3722.jpeg.jpg"
        }
    ],
    "data_quality":"Needs Vote"
}
*/