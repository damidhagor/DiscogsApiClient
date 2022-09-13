﻿namespace DiscogsApiClient.Contract;

public sealed record User(
    int Id,
    string ResourceUrl,
    string Uri,
    string Username,
    string Name,
    string HomePage,
    string Location,
    string Profile,
    DateTime Registered,
    float Rank,
    int NumPending,
    int NumForSale,
    int NumLists,
    int ReleasesContributed,
    int ReleasesRated,
    float RatingAvg,
    string InventoryUrl,
    string CollectionFoldersUrl,
    string CollectionFieldsUrl,
    string WantlistUrl,
    string AvatarUrl,
    string CurrAbbr,
    bool Activated,
    bool MarketplaceSuspended,
    string BannerUrl,
    float BuyerRating,
    float BuyerRatingStars,
    int BuyerNumRatings,
    float SellerRating,
    float SellerRatingStars,
    int SellerNumRatings,
    bool IsStaff,
    int NumCollection,
    int NumWantlist,
    string Email,
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
