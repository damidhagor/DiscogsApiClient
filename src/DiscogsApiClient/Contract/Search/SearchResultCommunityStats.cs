﻿namespace DiscogsApiClient.Contract.Search;

/// <summary>
/// Community statistics for a search result
/// </summary>
/// <param name="UsersWantingCount">How many users have this result on ther wantlist</param>
/// <param name="UsersHavingCount">How many users own this search result</param>
public sealed record SearchResultCommunityStats(
    [property:JsonPropertyName("want")]
    int UsersWantingCount,
    [property:JsonPropertyName("have")]
    int UsersHavingCount);


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
