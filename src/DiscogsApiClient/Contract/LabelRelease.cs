namespace DiscogsApiClient.Contract;

public record LabelReleasesResponse(
    Pagination Pagination,
    List<LabelRelease> Releases);

public record LabelRelease(
    string Status,
    string Format,
    string Catno,
    string Thumb,
    string ResourceUrl,
    string Title,
    int Id,
    int Year,
    string Artist,
    LabelReleaseStats Stats);

public record LabelReleaseStats(
    LabelReleaseStatValues Community,
    LabelReleaseStatValues User);

public record LabelReleaseStatValues(
    int InWantlist,
    int InCollection);


/*
{
    "pagination":{
        "page":1,
        "pages":305,
        "per_page":50,
        "items":15215,
        "urls":{
            "last":"https://api.discogs.com/labels/11499/releases?page=305&per_page=50",
            "next":"https://api.discogs.com/labels/11499/releases?page=2&per_page=50"
        }
    },
    "releases":[
        {
            "status":"Accepted",
            "format":"2xLP, Album, Ltd, RE, Gre",
            "catno":" \u200e\u2013 NB 3483-1, Nuclear Blast \u200e\u2013 27361 38175",
            "thumb":"https://i.discogs.com/1wCuFS4FOar1H9plQPKb5AB0MnaghtWIMYVqJhncRYA/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTE3/ODI3MTIwLTE2MTcy/OTIxOTgtMzU4MC5q/cGVn.jpeg",
            "resource_url":"https://api.discogs.com/releases/17827120",
            "title":"The Violent Sleep Of Reason",
            "id":17827120,
            "year":2021,
            "artist":"Meshuggah",
            "stats":{
                "community":{
                    "in_wantlist":30,
                    "in_collection":52
                },
                "user":{
                    "in_wantlist":0,
                    "in_collection":0
                }
            }
        },
        {
            "status":"Accepted",
            "format":"File, FLAC, MP3, Album + CD, Album",
            "catno":"00",
            "thumb":"https://i.discogs.com/n8LvxoJLJCA4tryqYY-uIsMN6Yysn6Ks62T7S9G4820/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTE4/NTY1ODc5LTE2MTk5/OTcxNzUtMjU0NC5q/cGVn.jpeg",
            "resource_url":"https://api.discogs.com/releases/18565879",
            "title":"Beneath This Crown",
            "id":18565879,
            "year":2021,
            "artist":"Reality Grey",
            "stats":{
                "community":{
                    "in_wantlist":1,
                    "in_collection":1
                },
                "user":{
                    "in_wantlist":0,
                    "in_collection":0
                }
            }
        },
    ]
}
 */