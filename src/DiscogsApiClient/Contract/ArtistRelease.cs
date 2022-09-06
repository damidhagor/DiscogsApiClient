namespace DiscogsApiClient.Contract;

public record ArtistReleasesResponse(
    Pagination Pagination,
    List<ArtistRelease> Releases);

public record ArtistRelease(
    int Id,
    string Title,
    string Type,
    int MainRelease,
    string Artist,
    string Role,
    string ResourceUrl,
    int Year,
    string Thumb,
    ArtistReleaseStats Stats);

public record ArtistReleaseStats(
    ArtistReleaseStatValues Community,
    ArtistReleaseStatValues User);

public record ArtistReleaseStatValues(
    int InWantlist,
    int InCollection);


/*
{
    "pagination":{
        "page":1,
        "pages":8,
        "per_page":50,
        "items":397,
        "urls":{
            "last":"https://api.discogs.com/artists/287459/releases?page=8&per_page=50",
            "next":"https://api.discogs.com/artists/287459/releases?page=2&per_page=50"
        }
    },
    "releases":[
        {
            "id":388731,
            "title":"Glory To The Brave",
            "type":"master",
            "main_release":4970915,
            "artist":"HammerFall",
            "role":"Main",
            "resource_url":"https://api.discogs.com/masters/388731",
            "year":1997,
            "thumb":"https://i.discogs.com/4ki15UCT95uRyIjAzl7QxpMDfv3sZUBriRM4L4-GFv4/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTQ5/NzA5MTUtMTQzMjcz/MTc2MC0zMjc1Lmpw/ZWc.jpeg",
            "stats":{
                "community":{
                    "in_wantlist":26,
                    "in_collection":29
                },
                "user":{
                    "in_wantlist":0,
                    "in_collection":0
                }
            }
        },
        {
            "id":156551,
            "title":"Glory To The Brave",
            "type":"master",
            "main_release":5134861,
            "artist":"HammerFall",
            "role":"Main",
            "resource_url":"https://api.discogs.com/masters/156551",
            "year":1997,
            "thumb":"https://i.discogs.com/Ltere8XlWzpcylS6RtAJmmDZqViWouYoRVz_RSMR1M4/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWltYWdlcy9SLTUx/MzQ4NjEtMTM4NTQz/NTQ0Ny0yNjU5Lmpw/ZWc.jpeg",
            "stats":{
                "community":{
                    "in_wantlist":28,
                    "in_collection":289
                },
                "user":{
                    "in_wantlist":0,
                    "in_collection":0
                }
            }
        }
    ]
}
 */