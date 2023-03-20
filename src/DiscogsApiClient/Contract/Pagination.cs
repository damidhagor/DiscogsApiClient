namespace DiscogsApiClient.Contract;

/// <summary>
/// Represents the pagination state in requests which support pagination.
/// </summary>
/// <param name="Page">The current page returned in the request</param>
/// <param name="TotalPages">The total pages available</param>
/// <param name="ItemsPerPage">How many pages are returned per page</param>
/// <param name="TotalItems">How many items are totally available</param>
/// <param name="Urls">Urls associated with the page</param>
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

/// <summary>
/// Urls associated with a request page.
/// </summary>
/// <param name="NextPageUrl">Url to query the next page.</param>
/// <param name="LastPageUrl">Url to query the last page.</param>
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
