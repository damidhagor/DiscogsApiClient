namespace DiscogsApiClient.Contract.User.Collection;

public sealed record CollectionFoldersResponse(
    [property:JsonPropertyName("folders")]
    List<CollectionFolder> Folders);


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
