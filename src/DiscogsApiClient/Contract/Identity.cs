namespace DiscogsApiClient.Contract;

/**
 {
  "id": 1,
  "username": "example",
  "resource_url": "https://api.discogs.com/users/example",
  "consumer_name": "Your Application Name"
 }
*/

public record Identity(
    int Id,
    string Username,
    string ResourceUrl,
    string ConsumerName);