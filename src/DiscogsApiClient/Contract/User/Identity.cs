namespace DiscogsApiClient.Contract.User;

public sealed record Identity(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("username")]
    string Username,
    [property:JsonPropertyName("resource_url")]
    string ResourceUrl,
    [property:JsonPropertyName("consumer_name")]
    string ConsumerName);


/**
 {
  "id": 1,
  "username": "example",
  "resource_url": "https://api.discogs.com/users/example",
  "consumer_name": "Your Application Name"
 }
*/
