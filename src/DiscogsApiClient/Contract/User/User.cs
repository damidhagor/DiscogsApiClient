namespace DiscogsApiClient.Contract.User;

/// <summary>
/// Information about a user. If the user is not the currently authenticated user the information in the response is limited.
/// </summary>
/// <param name="Id">User id</param>
/// <param name="ResourceUrl">The Api url to this user</param>
/// <param name="Uri">The url to the user's prifile on the website</param>
/// <param name="Username">Username</param>
/// <param name="Name">Name of the user</param>
/// <param name="HomePage">User's Homepage</param>
/// <param name="Location">User's location</param>
/// <param name="Profile">Profile text of the user</param>
/// <param name="RegisteredSince">When the user was registered</param>
/// <param name="Rank">Rank of the user</param>
/// <param name="NumPending"></param>
/// <param name="NumForSale">How many releases the user has for sale</param>
/// <param name="NumLists">How many lists the user has</param>
/// <param name="ReleasesContributed">To how many releases the user contributed</param>
/// <param name="ReleasesRated">How many releases the user rated</param>
/// <param name="AverageRating">The average release rating given by the user</param>
/// <param name="InventoryUrl">Url to the user's inventory</param>
/// <param name="CollectionFoldersUrl">The Api url to the user's collection folders</param>
/// <param name="CollectionFieldsUrl">The Api url to the user's custom collection fields</param>
/// <param name="WantlistUrl">The api url to the user's wantlist</param>
/// <param name="AvatarUrl">Image url for the avatar</param>
/// <param name="CurrencyAbbreviation">The user's chosen currency (abbreviated)</param>
/// <param name="IsActivated">If the user is active</param>
/// <param name="IsMarketplaceSuspended">If the user is suspended on the marketplace</param>
/// <param name="BannerUrl">Image url to the user's banner</param>
/// <param name="BuyerRating">The rating of the user as a buyer</param>
/// <param name="BuyerRatingStars">The rating of the user as a buyer in stars</param>
/// <param name="BuyerNumRatings">How many times the user was rated as a buyer</param>
/// <param name="SellerRating">The rating of the user as a seller</param>
/// <param name="SellerRatingStars">The rating of the user as a seller in stars</param>
/// <param name="SellerNumRatings">How many times the user was rated as a seller</param>
/// <param name="IsStaff">If the user belongs to the Discogs staff</param>
/// <param name="NumCollection">How many releases are in the user's collection</param>
/// <param name="NumWantlist">How many releases are on the user's wantlist</param>
/// <param name="Email">User's email address</param>
/// <param name="NumUnread"></param>
public sealed record User(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("username")]
    string Username,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("homepage")]
    string HomePage,
    [property:JsonPropertyName("location")]
    string Location,
    [property:JsonPropertyName("profile")]
    string Profile,
    [property:JsonPropertyName("registered")]
    DateTime RegisteredSince,
    [property:JsonPropertyName("rank")]
    float Rank,
    [property:JsonPropertyName("num_pending")]
    int NumPending,
    [property:JsonPropertyName("num_for_sale")]
    int NumForSale,
    [property:JsonPropertyName("num_lists")]
    int NumLists,
    [property:JsonPropertyName("releases_contributed")]
    int ReleasesContributed,
    [property:JsonPropertyName("releases_rated")]
    int ReleasesRated,
    [property:JsonPropertyName("rating_avg")]
    float AverageRating,
    [property:JsonPropertyName("inventory_url")]
    string InventoryUrl,
    [property:JsonPropertyName("collection_folders_url")]
    string CollectionFoldersUrl,
    [property:JsonPropertyName("collection_fields_url")]
    string CollectionFieldsUrl,
    [property:JsonPropertyName("wantlist_url")]
    string WantlistUrl,
    [property:JsonPropertyName("avatar_url")]
    string AvatarUrl,
    [property:JsonPropertyName("curr_abbr")]
    string CurrencyAbbreviation,
    [property:JsonPropertyName("activated")]
    bool IsActivated,
    [property:JsonPropertyName("marketplace_suspended")]
    bool IsMarketplaceSuspended,
    [property:JsonPropertyName("banner_url")]
    string BannerUrl,
    [property:JsonPropertyName("buyer_rating")]
    float BuyerRating,
    [property:JsonPropertyName("buyer_rating_stars")]
    float BuyerRatingStars,
    [property:JsonPropertyName("buyer_num_ratings")]
    int BuyerNumRatings,
    [property:JsonPropertyName("seller_rating")]
    float SellerRating,
    [property:JsonPropertyName("seller_rating_stars")]
    float SellerRatingStars,
    [property:JsonPropertyName("seller_num_ratings")]
    int SellerNumRatings,
    [property:JsonPropertyName("is_staff")]
    bool IsStaff,
    [property:JsonPropertyName("num_collection")]
    int NumCollection,
    [property:JsonPropertyName("num_wantlist")]
    int NumWantlist,
    [property:JsonPropertyName("email")]
    string Email,
    [property:JsonPropertyName("num_unread")]
    int NumUnread);


/**
{
    "id":12579295,
    "resource_url":"https://api.discogs.com/users/DamIDhagor",
    "uri":"https://www.discogs.com/user/DamIDhagor",
    "username":"DamIDhagor",
    "name":"Alexander",
    "home_page":"",
    "location":"",
    "profile":"",
    "registered":"2021-12-17T15:24:46-08:00",
    "rank":0.0,
    "num_pending":0,
    "num_for_sale":0,
    "num_lists":0,
    "releases_contributed":0,
    "releases_rated":0,
    "rating_avg":0.0,
    "inventory_url":"https://api.discogs.com/users/DamIDhagor/inventory",
    "collection_folders_url":"https://api.discogs.com/users/DamIDhagor/collection/folders",
    "collection_fields_url":"https://api.discogs.com/users/DamIDhagor/collection/fields",
    "wantlist_url":"https://api.discogs.com/users/DamIDhagor/wants",
    "avatar_url":"https://img.discogs.com/_P8NxBdy7Q7K9q5RJJXJ7cxPRIc=/500x500/filters:strip_icc():format(jpeg):quality(40)/discogs-avatars/U-12579295-1639783612.jpeg.jpg",
    "curr_abbr":"EUR",
    "activated":true,
    "marketplace_suspended":false,
    "banner_url":"",
    "buyer_rating":0.0,
    "buyer_rating_stars":0.0,
    "buyer_num_ratings":0,
    "seller_rating":0.0,
    "seller_rating_stars":0.0,
    "seller_num_ratings":0,
    "is_staff":false,
    "num_collection":12,
    "num_wantlist":0,
    "email":"alexander.jurk@outlook.com",
    "num_unread":0
}
*/
