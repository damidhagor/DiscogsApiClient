namespace DiscogsApiClient.Contract;

public record Pagination(
    int Page,
    int Pages,
    int PerPage,
    int Items,
    PaginationUrls Urls);

public record PaginationUrls(
    string Next,
    string Last);


/*
"pagination":{
        "page":1,
        "pages":305,
        "per_page":50,
        "items":15215,
        "urls":{
            "last":"https://api.discogs.com/labels/11499/releases?page=305&per_page=50",
            "next":"https://api.discogs.com/labels/11499/releases?page=2&per_page=50"
        }
    }
 */