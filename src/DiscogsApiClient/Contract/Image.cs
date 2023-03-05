using System.Runtime.Serialization;

namespace DiscogsApiClient.Contract;

public sealed record Image(
    [property:JsonPropertyName("type")]
    ImageType Type,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("uri")]
    string ImageUri,
    [property:JsonPropertyName("uri150")]
    string ImageUri150,
    [property:JsonPropertyName("width")]
    int Width,
    [property:JsonPropertyName("height")]
    int Height);

public enum ImageType : int
{
    [EnumMember(Value = "primary")]
    Primary = 1,
    [EnumMember(Value = "secondary")]
    Secondary = 2
}

/**
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
]
*/
