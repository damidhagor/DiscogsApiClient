namespace DiscogsApiClient.Contract;

public sealed record CollectionFolder(
    int Id,
    int Count,
    string Name,
    string ResourceUrl);

public sealed record CollectionFoldersResponse(
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