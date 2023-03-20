namespace DiscogsApiClient.Contract.Label;

/// <summary>
/// Information about a label
/// </summary>
/// <param name="Id">Label id</param>
/// <param name="ResourceUrl">The Api url to the label</param>
/// <param name="Name">Label name</param>
/// <param name="ContactInfo">Label's contact information</param>
/// <param name="Profile">Profile text of the label</param>
/// <param name="Uri">Url to the label's profile on the Discogs website</param>
/// <param name="ReleasesUrl">The Api url to the list of release of the label</param>
/// <param name="Images">List of images of the label</param>
/// <param name="ParentLabel">Parent label of this label</param>
/// <param name="SubLabels">Sub-labels of this label</param>
/// <param name="Urls">Urls of this label</param>
/// <param name="DataQuality"></param>
public sealed record Label(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("contact_info")]
    string ContactInfo,
    [property:JsonPropertyName("profile")]
    string Profile,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("releases_url")]
    string ReleasesUrl,
    [property:JsonPropertyName("images")]
    List<Image> Images,
    [property:JsonPropertyName("parent_label")]
    LabelShortInfo ParentLabel,
    [property:JsonPropertyName("sublabels")]
    List<LabelShortInfo> SubLabels,
    [property:JsonPropertyName("urls")]
    List<string> Urls,
    [property:JsonPropertyName("data_quality")]
    string DataQuality);


/*
{
    "id":11499,
    "name":"Nuclear Blast",
    "resource_url":"https://api.discogs.com/labels/11499",
    "uri":"https://www.discogs.com/label/11499-Nuclear-Blast",
    "releases_url":"https://api.discogs.com/labels/11499/releases",
    "images":[
        {
            "type":"primary",
            "uri":"https://img.discogs.com/0FiFukEFCa9uBzHYhi5dp9l6_SU=/fit-in/535x188/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/L-11499-1301404145.jpeg.jpg",
            "resource_url":"https://img.discogs.com/0FiFukEFCa9uBzHYhi5dp9l6_SU=/fit-in/535x188/filters:strip_icc():format(jpeg):mode_rgb():quality(90)/discogs-images/L-11499-1301404145.jpeg.jpg",
            "uri150":"https://img.discogs.com/ZQxuz20TviDDhT6B-bxLy2y3gOI=/fit-in/150x150/filters:strip_icc():format(jpeg):mode_rgb():quality(40)/discogs-images/L-11499-1301404145.jpeg.jpg",
            "width":535,
            "height":188
        }
    ],
    "contact_info":"[b]Germany[/b]:\r\nNuclear Blast GmbH\r\nOeschstrasse 40 \r\n73072 Donzdorf\r\nGermany\r\n\r\nphone: (+49) 7162 92800\r\ne-mail: info@nuclearblast.de\r\n\r\n[b]United States[/b]:\r\nP.O. Box 43618\r\nPhiladelphia, PA 19106\r\n\r\nphone: (215) 923-0770\r\nfax. (215) 923-9166\r\ne-mail:  mail@nuclearblast-usa.com\r\n\r\n[b]South America[/b]:\r\nLabelmanager: Gerard Werron\r\nRua Paolistania 407\r\nBR 05440-000 Sao Paulo - SP -Metro Vila Magdalena\r\n\r\ne-mail: gerard@nuclearblast.de\r\nphone: +55-11-30978117\r\nfax: +55-11-38161195\r\n",
    "profile":"German independent label, specialised in metal and related music styles; founded in 1987 by [a252876].\r\nLabel Code: LC 7027 / LC 07027\r\nGVL-registered label name is [i]Nuclear Blast Records[/i].\r\nFor all unofficial releases (bootlegs, counterfeits) that pretend to be releases of this label, use [l=Nuclear Blast (2)].\r\n\r\n[b]About NB and their different regional companies:[/b]\r\n\r\n[b]Nuclear Blast Europe[/b] \u2013 Label, Mailorder, Wholesale, Fulfilment Service and Distribution for several labels spread over the whole European continent; located in Donzdorf, Germany. \r\n\r\n[b]Nuclear Blast America[/b] \u2013 US Company with identical business tasks in cooperation with [l=Century Media].\r\n\r\n[b]Nuclear Blast South America[/b] \u2013 In cooperation with [l=Century Media].\r\n\r\nA typical Nuclear Blast catalogue number has the following format: \"NB XXXX-F\".\r\n\r\nNote: Earlier releases might only have 3 digits.\r\n\r\nF determines the format. This is not always consistent, but here is a general idea of what you might find:\r\n[b]-0[/b] limited first edition (Digipak, O-Card, etc.) for both CD and DVD\r\n[b]-1[/b] LP or other vinyl format\r\n[b]-2[/b] jewelcase CD or promo CD, DVD, sometimes also used for the limited first edition\r\n[b]-3[/b] video tape (VHS), tape (MC)\r\n[b]-4[/b] tape (MC)\r\n[b]-5[/b] box sets, limited mailorder editions, bonus CDs and DVDs (in general: limited bonus editions)\r\n[b]-7[/b] some vinyl 7\u201ds carry this as format\r\n[b]-8[/b] shape CDs (not all, however)\r\n[b]-9[/b] picture vinyl (not all, however, esp. when the release was only available as a picture vinyl or comes with a cover, you may find a [b]\u20131[/b] here)",
    "parent_label":{
        "id":222987,
        "name":"Nuclear Blast GmbH",
        "resource_url":"https://api.discogs.com/labels/222987"
    },
    "data_quality":"Needs Vote",
    "urls":[
        "https://www.nuclearblast.de/",
        "https://www.facebook.com/nuclearblastrecords/",
        "https://www.instagram.com/nuclearblastrecords/"
    ],
    "sublabels":[
        {
            "id":281114,
            "name":"Anstalt Records",
            "resource_url":"https://api.discogs.com/labels/281114"
        },
        {
            "id":1498485,
            "name":"Arising Empire Tontr\u00e4ger Produktions- und Vertriebs GmbH",
            "resource_url":"https://api.discogs.com/labels/1498485"
        }
    ]
}
 */
