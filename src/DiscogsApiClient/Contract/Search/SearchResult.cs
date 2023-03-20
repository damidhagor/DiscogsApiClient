namespace DiscogsApiClient.Contract.Search;

/// <summary>
/// Represents a search result of a database search.
/// <para/>
/// NOTE: Which properties are populated is dependent of the result type which is indicated by the <see cref="ResultType"/> property.
/// <para/>
/// E.g. the <see cref="Barcodes"/> properties will only be present for results of type <see cref="SearchResultType.Release"/>.
/// </summary>
/// <param name="Id">Id of the result</param>
/// <param name="Title">Title or name of the result</param>
/// <param name="ResourceUrl">The Api url to the result</param>
/// <param name="ResultType">The type of the result</param>
/// <param name="Uri">Url to the Discogs page of the result</param>
/// <param name="ThumbnailUrl">Thumbnail image url</param>
/// <param name="CatalogNumber">Catalog number (if applicable)</param>
/// <param name="Year">Release year (if applicable)</param>
/// <param name="Country">Release country (if applicable)</param>
/// <param name="CoverImageUrl">Image url of the result</param>
/// <param name="MasterReleaseId">Id of the master release (if applicable)</param>
/// <param name="MasterReleaseUrl">The Api url of the master release (if applicable)</param>
/// <param name="Format">Release format (if applicable)</param>
/// <param name="Genres">Release Genres (if applicable)</param>
/// <param name="Styles">release Styles (if applicable)</param>
/// <param name="Labels">Release labels (if applicable)</param>
/// <param name="Barcodes">Release identifying barcodes (if applicable)</param>
/// <param name="FormatCount">Number of release formats (if applicable)</param>
/// <param name="Formats">All release formats (if applicable)</param>
/// <param name="CommunityStatistics">Release community statistics (if applicable)</param>
/// <param name="UserData"></param>
public sealed record SearchResult(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("title")]
    string Title,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("type")]
    SearchResultType ResultType,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("thumb")]
    string ThumbnailUrl,
    [property:JsonPropertyName("catno")]
    string CatalogNumber,
    [property:JsonPropertyName("year")]
    string Year,
    [property:JsonPropertyName("country")]
    string Country,
    [property:JsonPropertyName("cover_image")]
    string CoverImageUrl,
    [property:JsonPropertyName("master_id")]
    int? MasterReleaseId,
    [property:JsonPropertyName("master_url")]
    string MasterReleaseUrl,
    [property:JsonPropertyName("format")]
    List<string> Format,
    [property:JsonPropertyName("genre")]
    List<string> Genres,
    [property:JsonPropertyName("style")]
    List<string> Styles,
    [property:JsonPropertyName("label")]
    List<string> Labels,
    [property:JsonPropertyName("barcode")]
    List<string> Barcodes,
    [property:JsonPropertyName("format_quantity")]
    int FormatCount,
    [property:JsonPropertyName("formats")]
    List<ReleaseFormat> Formats,
    [property:JsonPropertyName("community")]
    SearchResultCommunityStats CommunityStatistics,
    [property:JsonPropertyName("user_data")]
    SearchResultUserData UserData);


/**
RELEASE

{
    "pagination":{
        "page":1,
        "pages":20,
        "per_page":50,
        "items":985,
        "urls":{
            "last":"https://api.discogs.com/database/search?q=hammerfall&type=release&page=20&per_page=50",
            "next":"https://api.discogs.com/database/search?q=hammerfall&type=release&page=2&per_page=50"
        }
    },
    "results":[
        {
            "country":"US",
            "year":"2005",
            "format":[
                "Vinyl",
                "12\""
            ],
            "label":[
                "Bastard Loud Records"
            ],
            "type":"release",
            "genre":[
                "Electronic"
            ],
            "style":[
                "Hardcore",
                "Industrial"
            ],
            "id":577756,
            "barcode":[
                
            ],
            "user_data":{
                "in_wantlist":false,
                "in_collection":false
            },
            "master_id":208731,
            "master_url":"https://api.discogs.com/masters/208731",
            "uri":"/Jensen-Hammerfall/release/577756",
            "catno":"BL 023",
            "title":"Jensen (2) - Hammerfall",
            "thumb":"https://i.discogs.com/iqE790HD7liQlKMgZbVndpSDDUbPkts0oMmmNhQfUNI/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTU3/Nzc1Ni0xMTg0ODU3/MTc4LmpwZWc.jpeg",
            "cover_image":"https://i.discogs.com/WD9w2PWkaKQCkESnfqJEYWrz0wYYBblG_YgSuVowvNg/rs:fit/g:sm/q:90/h:600/w:600/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTU3/Nzc1Ni0xMTg0ODU3/MTc4LmpwZWc.jpeg",
            "resource_url":"https://api.discogs.com/releases/577756",
            "community":{
                "want":62,
                "have":216
            },
            "format_quantity":1,
            "formats":[
                {
                    "name":"Vinyl",
                    "qty":"1",
                    "descriptions":[
                        "12\""
                    ]
                }
            ]
        }
    ]
}



MASTER

{
    "pagination":{
        "page":1,
        "pages":2,
        "per_page":50,
        "items":90,
        "urls":{
            "last":"https://api.discogs.com/database/search?q=hammerfall&type=master&page=2&per_page=50",
            "next":"https://api.discogs.com/database/search?q=hammerfall&type=master&page=2&per_page=50"
        }
    },
    "results":[
        {
            "country":"US",
            "year":"2005",
            "format":[
                "Vinyl",
                "12\""
            ],
            "label":[
                "Bastard Loud Records"
            ],
            "type":"master",
            "genre":[
                "Electronic"
            ],
            "style":[
                "Hardcore",
                "Industrial"
            ],
            "id":208731,
            "barcode":[
                
            ],
            "user_data":{
                "in_wantlist":false,
                "in_collection":false
            },
            "master_id":208731,
            "master_url":"https://api.discogs.com/masters/208731",
            "uri":"/Jensen-Hammerfall/master/208731",
            "catno":"BL 023",
            "title":"Jensen (2) - Hammerfall",
            "thumb":"https://i.discogs.com/iqE790HD7liQlKMgZbVndpSDDUbPkts0oMmmNhQfUNI/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTU3/Nzc1Ni0xMTg0ODU3/MTc4LmpwZWc.jpeg",
            "cover_image":"https://i.discogs.com/WD9w2PWkaKQCkESnfqJEYWrz0wYYBblG_YgSuVowvNg/rs:fit/g:sm/q:90/h:600/w:600/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTU3/Nzc1Ni0xMTg0ODU3/MTc4LmpwZWc.jpeg",
            "resource_url":"https://api.discogs.com/masters/208731",
            "community":{
                "want":80,
                "have":219
            }
        }
    ]
}


ARTIST

{
    "pagination":{
        "page":1,
        "pages":1,
        "per_page":50,
        "items":29,
        "urls":{
            
        }
    },
    "results":[
        {
            "id":287459,
            "type":"artist",
            "user_data":{
                "in_wantlist":false,
                "in_collection":false
            },
            "master_id":null,
            "master_url":null,
            "uri":"/artist/287459-HammerFall",
            "title":"HammerFall",
            "thumb":"https://i.discogs.com/n-sKbja7Syzx8CZ6tjMGf8kUnCqE3aTH_NP0fvrlddY/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9BLTI4/NzQ1OS0xNjIyNDE0/MjUzLTQyNDkuanBl/Zw.jpeg",
            "cover_image":"https://i.discogs.com/j1swjm1vsdr6EO6fHXUvhn1UZd1fZ7Vq0WkwicJ18z0/rs:fit/g:sm/q:90/h:399/w:600/czM6Ly9kaXNjb2dz/LWltYWdlcy9BLTI4/NzQ1OS0xNjIyNDE0/MjUzLTQyNDkuanBl/Zw.jpeg",
            "resource_url":"https://api.discogs.com/artists/287459"
        }
    ]
}



LABEL

{
    "pagination":{
        "page":1,
        "pages":1,
        "per_page":50,
        "items":4,
        "urls":{
            
        }
    },
    "results":[
        {
            "id":960681,
            "type":"label",
            "user_data":{
                "in_wantlist":false,
                "in_collection":false
            },
            "master_id":null,
            "master_url":null,
            "uri":"/label/960681-Not-On-Label-Hammerfall",
            "title":"Not On Label (Hammerfall)",
            "thumb":"",
            "cover_image":"https://s.discogs.com/4fec5d48e5d7ca4d4007282bc56ce4c0a0b949ad/images/spacer.gif",
            "resource_url":"https://api.discogs.com/labels/960681"
        }
    ]
}
*/
