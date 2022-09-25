# DiscogsApiClient

This is a C# .Net6 library for accessing the [Discogs API v2.0](https://www.discogs.com/developers).

It allows for accessing and modifying a user's collection and wantlist and querying the Discogs database.
For authentication can be chosen between personal access tokens or OAuth 1.0a.

This is a private project and not all Api functions are implemented.
I might however add more functionality by request if my time allows for it.


## Authentication

The DiscogsApiClient supports authentication by either using a user's personal access token or the full OAuth 1.0a auth flow.

**Personal access tokens** are the easiest way to make authenticated requests since it only requires the user to generate an access token in the development section of their profile settings.
This might not be a great user experience for an application but is useful if the DicogsApiClient is used behind an Api as part of a service or in any other head-less scenario.

The **OAuth Flow** on the other hand allows the user to log in with their Discogs credentials directly in the application and authorize it to make requests on the user's behalf.
This requires the application to be registered by the developer in their development profile section and obtain its **Consumer Key** and **Consumer Secret** with which the application can fetch a **Request Token** from the Discogs Api.
With this token the application needs to open Discog's login page and specify a local Url to which the page will redirect after successful login to pass back a **Verifier Key**
with which the final **OAuth Token** and **OAuth Token Secret**, needed for the client to be authenticated, are requested.
The final token and secret are permanently valid and should be stored so that the user only needs to log in once.

Note: The OAuth flow is implemented in the plain version without encrypting/hashing the tokens because the Discogs Api is only accessible over Https which ensures a secure connection.
Doing it this way is even recommended by the Discogs documentation.


## Getting Started

The easiest way to use the library now is using it with Dependency Injection
with either a user token

```csharp
// At startup register the DiscogsApiClient and the authentication provider.
services.AddDiscogsApiClient("YourAwesomeApp/1.0.0");
services.AddDiscogsUserTokenAuthentication();

// Inject the IDiscogsApiClient and authenticate with the user token.
var authRequest = new UserTokenAuthenticationRequest("PersonalAccessTokenFromDiscogs");
var authResponse = await discogsApiClient.AuthenticateAsync(authRequest, default);

// Use the authenticated client.
var response = await discogsApiClient.GetIdentityAsync(default);
```

or the OAuth flow:

```csharp
// At startup register the DiscogsApiClient and the authentication provider.
services.AddDiscogsApiClient("YourAwesomeApp/1.0.0");
services.AddDiscogsPlainOAuthAuthentication();

// Inject the IDiscogsApiClient and authenticate with the OAuth flow.
var authRequest = new PlainOAuthAuthenticationRequest("YourAwesomeApp/1.0.0",
    "ConsumerKeyFromDiscogs", "ConsumerSecretFromDiscogs",
    "http://localhost/verifier", GetVerifierKey)
{
    AccessToken = "StoredAccessTokenIfPreviouslyAuthenticated",
    AccessTokenSecret = "StoredAccessTokenSecretIfPreviouslyAuthenticated"
};

var authResponse = await discogsApiClient.AuthenticateAsync(authRequest, default);

// Use the authenticated client.
var response = await discogsApiClient.GetIdentityAsync(default);

// Demo method which handles the user login & returns the verifier token 
public Task<string> GetVerifierKey(string authUrl, string callbackUrl, CancellationToken token)
{
    // Open browser with authUrl
    // Detect redirect to callbackUrl
    // Verifier Token will be appended to the url with  a '?': http://localhost/verifier?verifierkey
    // Parse token from url and return it
}
```

The client can still be constructed and used without Dependency Injection.
Instantiation and handling of the Httpclient then needs to happen manually.

Following is example code for using the DiscogsApiClient without using Dependency Injection.

With user token:

```csharp
var userAgent = "YourAwesomeApp/1.0.0";
var userToken = "PersonalAccessTokenFromDiscogs";

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

var authProvider = new UserTokenAuthenticationProvider();
var apiClient = new DiscogsApiClient(httpClient, authProvider);

var authRequest = new UserTokenAuthenticationRequest(userToken);
var authResponse = await apiClient.AuthenticateAsync(authRequest, CancellationToken.None);

if (authResponse.Success)
{
    // Test call retrieves the identity of the authenticated user
    var identity = await apiClient.GetIdentityAsync(CancellationToken.None);
}
else
{
    log.Error(authResponse.Error);
}
```

With OAuth flow:

```csharp
var userAgent = "YourAwesomeApp/1.0.0";
var callbackUrl = "http://localhost/verifier";
var consumerKey = "ConsumerKeyFromDiscogs";
var consumerSecret = "ConsumerSecretFromDiscogs";

var accessToken = "StoredAccessTokenIfPreviouslyAuthorized";
var accessTokenSecret = "StoredAccessTokenSecretIfPreviouslyAuthorized";

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

var authProvider = new PlainOAuthAuthenticationProvider();
var apiClient = new DiscogsApiClient.DiscogsApiClient(httpClient, authProvider);

var authRequest = new PlainOAuthAuthenticationRequest(userAgent, consumerKey, consumerSecret, callbackUrl, GetVerifierKey)
{
    AccessToken = accessToken,
    AccessTokenSecret = accessTokenSecret
};

var authResponse = await apiClient.AuthenticateAsync(authRequest, CancellationToken.None);

if (authResponse.Success)
{
    var accessToken = authResponse.AccessToken;
    var accessTokenSecret = authResponse.AccessSecret;
    // Store token & secret

    // Test call retrieves the identity of the authenticated user
    var identity = await apiClient.GetIdentityAsync(CancellationToken.None);
}
else
{
    log.Error(authResponse.Error);
}

public Task<string> GetVerifierKey(string authUrl, string callbackUrl, CancellationToken token)
{
    // Open browser with authUrl
    // Detect redirect to callbackUrl
    // Verifier Token will be appended to the url with  a '?': http://localhost/verifier?verifierkey
    // Parse token from url and return it
}
```

After successfully setting the client up and being authenticated you can make e.g. a database query:

```csharp
var queryParams = new SearchQueryParameters { Query = "hammerfall", Type = "artist" };
var paginationParams = new PaginationQueryParameters(1, 50);

var response = await apiClient.SearchDatabaseAsync(queryParams, paginationParams, CancellationToken.None);
```


## Changelog

- ### 1.0.0
  - Initial release.
  - Support for User Token & OAuth 1.0a authentication flows.
  - Implementation of Api functions for user information, collection & wantlist and database queries.

- ### 2.0.0
    - Refactored the library for Dependency Injection support:
        - Added IServiceCollection extension methods to support easy dependency injection.
        - Added IDiscogsApiClient interface for mocking and Dependency Injection.
        - The DiscogsApiClient's HttpClient is now injectable via the constructor.
        - If configured via the IServiceCollection the HttpClient will be injected via the IHttpClientFactory.
        - Needed parameters for the IAuthenticationProviders are moved from their constructors into their IAuthenticationRequest implementations.
    - Sealed all classes for performance.

## Roadmap

- More granular authentication requirements for requests (not all requests need it)
- Rate limiting support
- CI/CD


## Implemented Api Functions

The current implementation of the Api surface is focused on querying the database and accessing the user's collection and wantlist.

For simplicity reasons the client needs to be authenticated for all Api requests.

**User resources**
- Get user information
- Collection
  - Get all collection folders
  - Get, Create, Update, Delete collection folders
  - Get releases in a collection folder
  - Add, Remove releases from a collection folder
- Wantlist
  - Get releases on the wantlist
  - Add, Delete releases to/from the wantlist

**Database resources**
- Discogs database search
- Get master release
- Get artist & artist's releases
- Get release & release's community rating
- Get label & label's releases