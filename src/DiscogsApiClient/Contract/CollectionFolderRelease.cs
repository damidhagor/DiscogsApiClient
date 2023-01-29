namespace DiscogsApiClient.Contract;

public sealed record CollectionFolderRelease(
    int Id,
    int InstanceId,
    DateTime DateAdded,
    int Rating,
    string ResourceUrl,
    UserListReleaseInformation BasicInformation,
    int FolderId);

public sealed record CollectionFolderReleasesResponse(
    Pagination Pagination,
    List<CollectionFolderRelease> Releases);


/**
{
    "pagination":{
        "page":1,
        "pages":1,
        "per_page":50,
        "items":12,
        "urls":{
            
        }
    },
    "releases":[
        {
            "id":14373012,
            "instance_id":888013468,
            "date_added":"2021-12-21T15:43:46-08:00",
            "rating":0,
            "basic_information":{
                "id":14373012,
                "master_id":1632532,
                "master_url":"https://api.discogs.com/masters/1632532",
                "resource_url":"https://api.discogs.com/releases/14373012",
                "thumb":"https://i.discogs.com/Uoi0Mbv01nHWR6qHBDxljnmFp2_rHcbFLMwo27L3K9o/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTE0/MzczMDEyLTE1NzMz/MjQyNTgtNjgzOS5q/cGVn.jpeg",
                "cover_image":"https://i.discogs.com/uA08DZ5wyjTIVXhayATtV3F7Es0iM9BXVdU6JK6nvbI/rs:fit/g:sm/q:90/h:597/w:600/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTE0/MzczMDEyLTE1NzMz/MjQyNTgtNjgzOS5q/cGVn.jpeg",
                "title":"Legacy Of The Dark Lands",
                "year":2019,
                "formats":[
                    {
                        "name":"CD",
                        "qty":"1",
                        "descriptions":[
                            "Album"
                        ]
                    },
                    {
                        "name":"CD",
                        "qty":"1"
                    },
                    {
                        "name":"CD",
                        "qty":"1"
                    },
                    {
                        "name":"All Media",
                        "qty":"1",
                        "text":"Earbook",
                        "descriptions":[
                            "Limited Edition"
                        ]
                    }
                ],
                "labels":[
                    {
                        "name":"Nuclear Blast",
                        "catno":"27361 51630",
                        "entity_type":"1",
                        "entity_type_name":"Label",
                        "id":11499,
                        "resource_url":"https://api.discogs.com/labels/11499"
                    }
                ],
                "artists":[
                    {
                        "name":"Blind Guardian Twilight Orchestra",
                        "anv":"",
                        "join":"",
                        "role":"",
                        "tracks":"",
                        "id":7384801,
                        "resource_url":"https://api.discogs.com/artists/7384801"
                    }
                ],
                "genres":[
                    "Rock",
                    "Classical"
                ],
                "styles":[
                    "Symphonic Rock"
                ]
            },
            "folder_id":1
        },
    ]
}
*/