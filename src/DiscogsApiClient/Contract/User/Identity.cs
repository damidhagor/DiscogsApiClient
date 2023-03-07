namespace DiscogsApiClient.Contract.User;

/// <summary>
/// Information about the currently authenticated user.
/// </summary>
/// <param name="Id">The user id</param>
/// <param name="Username">The username</param>
/// <param name="ResourceUrl">The Api url to the user's profile</param>
/// <param name="ConsumerName">The name of the application which is consuming the Api (User-Agent)</param>
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
