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

There is no Nuget package yet for this library so for now to use it you have to download the project and build and reference the library manually.

The easiest way to get started is using a personal access token instead of the full OAuth flow:

```csharp
var userAgent = "YourAwesomeApp/1.0.0";
var userToken = "PersonalAccessTokenFromDiscogs";

var authProvider = new UserTokenAuthorizationProvider();
var apiClient = new DiscogsApiClient(authProvider, userAgent);

var authRequest = new UserTokenAuthorizationRequest(userToken);
var authResponse = await apiClient.AuthorizeAsync(authRequest, CancellationToken.None);

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

For the full OAuth flow a callback function needs to be provided which handles the user login via a browser and passes back the retrieved **Verifier Key** to the DiscogsApiClient:

```csharp

var userAgent = "YourAwesomeApp/1.0.0";
var callbackUrl = "http://localhost/verifier"
var consumerKey = "ConsumerKeyFromDiscogs";
var consumerSecret = "ConsumerSecretFromDiscogs";

var accessToken = "StoredAccessTokenIfPreviouslyAuthorized";
var accessTokenSecret = "StoredAccessTokenSecretIfPreviouslyAuthorized";

var authProvider = new PlainOAuthAuthorizationProvider(userAgent, consumerKey, consumerSecret, accessToken, accessTokenSecret);
var apiClient = new DiscogsApiClient(authProvider, userAgent);

var authRequest = new PlainOAuthAuthorizationRequest(callbackUrl, GetVerifierKey);
var authResponse = await apiClient.AuthorizeAsync(authRequest, CancellationToken.None);

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

public Task<string> GetVerifierKey(authUrl, callbackUrl, CancellationToken token)
{
    // Open browser with authUrl
    // Detect redirect to callbackUrl
    // Verifier Token will be appended to the url with  a '?': http://localhost/verifier?verifierkey
    // Parse token from url and return it
}
```

After being successfully authenticated you can make e.g. a database query:

```csharp
var queryParams = new SearchQueryParameters { Query = "hammerfall", Type = "artist" };
var paginationParams = new PaginationQueryParameters(1, 50);

var response = await apiClient.SearchDatabaseAsync(queryParams, paginationParams, CancellationToken.None);
```


## Roadmap

- More granular authentication requirements for requests (not all requests need it)
- Nuget Package
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