using DiscogsApiClient.SourceGenerator.Shared;
using DiscogsApiClient.SourceGenerator.JsonSerialization;

namespace DiscogsApiClient.Contract;

/// <summary>
/// Represents a Discogs image.
/// </summary>
/// <param name="Type">The image type.</param>
/// <param name="ResourceUrl">The url to the image.</param>
/// <param name="ImageUri">The url to the image.</param>
/// <param name="ImageUri150">The url to a 150x150 scaled version of the image.</param>
/// <param name="Width">The width of the image.</param>
/// <param name="Height">The height of the image.</param>
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

/// <summary>
/// The possible types of <see cref="Image"/>s.
/// </summary>
[GenerateJsonConverter]
public enum ImageType : int
{
    [AliasAs("primary")]
    Primary = 1,
    [AliasAs("secondary")]
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
