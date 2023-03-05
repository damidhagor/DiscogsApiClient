namespace DiscogsApiClient.Contract;

public sealed record Pagination(
    [property:JsonPropertyName("page")]
    int Page,
    [property:JsonPropertyName("pages")]
    int TotalPages,
    [property:JsonPropertyName("per_page")]
    int ItemsPerPage,
    [property:JsonPropertyName("items")]
    int TotalItems,
    [property:JsonPropertyName("urls")]
    PaginationUrls Urls);

public sealed record PaginationUrls(
    [property:JsonPropertyName("next")]
    string NextPageUrl,
    [property:JsonPropertyName("last")]
    string LastPageUrl);


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
