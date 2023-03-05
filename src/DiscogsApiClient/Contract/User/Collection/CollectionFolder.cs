namespace DiscogsApiClient.Contract.User.Collection;

public sealed record CollectionFolder(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("count")]
    int ReleaseCount,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl);


/**
{
 "folders":[
  {
   "id":0,
   "name":"All",
   "count":12,
   "resource_url":"https://api.discogs.com/users/DamIDhagor/collection/folders/0"
  },
  {
   "id":1,
   "name":"Uncategorized",
   "count":12,
   "resource_url":"https://api.discogs.com/users/DamIDhagor/collection/folders/1"
  }
 ]
}
*/
