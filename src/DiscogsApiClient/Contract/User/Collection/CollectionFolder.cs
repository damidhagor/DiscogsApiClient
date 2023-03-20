namespace DiscogsApiClient.Contract.User.Collection;

/// <summary>
/// Represents a folder in the user's collection
/// </summary>
/// <param name="Id">Folder id</param>
/// <param name="ReleaseCount">How many releases the folder contains</param>
/// <param name="Name">Folder name</param>
/// <param name="ResourceUrl">The Api url for this folder</param>
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
