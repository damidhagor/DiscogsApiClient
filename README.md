# **DiscogsApiClient**

This is a C# library for accessing the [Discogs API v2.0](https://www.discogs.com/developers)
and is compatible with .Net 6, 7 & 8.

It allows for accessing and modifying a user's collection and wantlist and querying the Discogs database.
Either personal access tokens or OAuth 1.0a can be chosen as authentication methods.

**Disclaimer:** This is a private project and not all Api functions are implemented.
I might however add more functionality by request if my time allows for it.

## **Authentication**

The DiscogsApiClient supports authentication by either using a user's personal access token or the full OAuth 1.0a auth flow.

**Personal access tokens** are the easiest way to make authenticated requests since it only requires the user to generate an access token in the development section of their profile settings.
This might not be a great user experience for an application but is useful if the DiscogsApiClient is used behind an Api as part of a service or in any other head-less scenario.

The **OAuth Flow** on the other hand allows the user to log in with their Discogs credentials directly in the application and authorize it to make requests on the user's behalf.
This requires the application to be registered by the developer in their development profile section and obtain its **Consumer Key** and **Consumer Secret** with which the application can fetch a **Request Token** from the Discogs Api.
With this token the application needs to open Discogs' login page and specify a local Url to which the page will redirect after successful login to pass back a **Verifier Key**
with which the final **OAuth Token** and **OAuth Token Secret**, needed for the client to be authenticated, are requested.
The final token and secret are permanently valid and should be stored so that the user only needs to log in once.

**Note:** The OAuth flow is implemented in the plain version without encrypting/hashing the tokens because the Discogs Api is only accessible over Https which ensures a secure connection.
Doing it this way is even recommended by the Discogs documentation.

## **Getting Started**

**Disclaimer: If you are upgrading from a version earlier than 3.0.0 please read further below about the breaking changes.**

Download the [Nuget Package](https://www.nuget.org/packages/DiscogsApiClient/) or compile the library from source.

Register and use the ```IDiscogsApiClient``` with either a personal access token

```csharp
// At startup register the DiscogsApiClient and the authentication provider
// with the IServiceCollection.

services.AddDiscogsApiClient(options =>
{
    options.UserAgent = "AwesomeAppDemo/1.0.0";
});

// Inject the IDiscogsAuthenticationService and IDiscogsApiClient
// and authenticate with the personal access token before using the client.

public Foo(
    IDiscogsApiClient discogsApiClient,
    IDiscogsAuthenticationService discogsAuthenticationService)
{
    _discogsApiClient = discogsApiClient;
    _discogsAuthenticationService = discogsAuthenticationService;
}

public void Authenticate(string token)
{
    _discogsAuthenticationService.AuthenticateWithPersonalAccessToken(token);
}

public async Task<string> GetUsername(CancellationToken cancellationToken)
{
    var identity = await _discogsApiClient.GetIdentity(cancellationToken);
    return identity.Username
}
```

or the OAuth flow:

```csharp
// At startup register the DiscogsApiClient and the authentication provider.                
services.AddDiscogsApiClient(options =>
{
    options.UserAgent = "AwesomeAppDemo/1.0.0";
});

// Inject the IDiscogsAuthenticationService and IDiscogsApiClient
// and authenticate with the OAuth flow before using the client.

public Foo(
    IDiscogsApiClient discogsApiClient,
    IDiscogsAuthenticationService discogsAuthenticationService)
{
    _discogsApiClient = discogsApiClient;
    _discogsAuthenticationService = discogsAuthenticationService;
}

// Authenticate with your consumer key & secret from your Discogs application settings.
public async Task Authenticate(
    string consumerKey,
    string consumerSecret,
    CancellationToken cancellationToken)
{
    (accessToken, accessTokenSecret) = await _discogsAuthenticationService.AuthenticateWithOAuth(
        consumerKey,
        consumerSecret,
        "",
        "",
        "http://localhost/verifier_token",
        GetVerifier,
        cancellationToken);

    // Save and reuse the retrieved access token and secret.
}

// Demo method which handles the user login & returns the verifier token 
public Task<string> GetVerifier(string authUrl, string callbackUrl, CancellationToken token)
{
    // 1) Open browser with authUrl
    // 2) Detect redirect to callbackUrl
    // 3) Verifier Token will be appended to the url: http://localhost/verifier_token?oauth_token=TOKEN&oauth_verifier=VERIFIER
    // 4) Parse verifier from url and return it
}
```

## **Changelog**

- ### **1.0.0**

  - Initial release.
  - Support for User Token & OAuth 1.0a authentication flows.
  - Implementation of Api functions for user information, collection & wantlist and database queries.

- ### **2.0.0**

  - Refactored the library for Dependency Injection support:
    - Added IServiceCollection extension methods to support easy dependency injection.
    - Added ```IDiscogsApiClient``` interface for mocking and Dependency Injection.
    - The DiscogsApiClient's HttpClient is now injectable via the constructor.
    - If configured via the IServiceCollection the HttpClient will be injected via the IHttpClientFactory.
    - Needed parameters for the ```IAuthenticationProviders``` are moved from their constructors into their ```IAuthenticationRequest``` implementations.
  - Sealed all classes for performance.

- ### **2.1.0**

  - Added missing pagination parameter to ```GetCollectionFolderReleasesByFolderIdAsync``` method.

- ### **2.1.1**

  - Fixed URL not being formatted correctly for ```GetCollectionFolderReleasesByFolderIdAsync``` method.

- ### **3.0.0**

  - The client is now implemented with the [Refit](https://github.com/reactiveui/refit) library.
  - The library now also targets .Net 7 and the .Net 8 preview.
  - The client now supports rate limiting which is handled with middleware in the HttpClient.
  - Dependency Injection registration is simplified to a single method call.
  - Refactored the authentication flow:
    - Authentication is outsourced into the new ```IDiscogsAuthenticationService```.
    - The new service offers both authentication flows simultaneously and the flows no longer needs to be configured at startup.
    - New HttpClient middleware handles authentication headers.
  - Added demo projects to showcase how to get started.
  - Improved test coverage.
  - The contract classes are restructured into sub-namespaces and a few properties are renamed for clarity.
  - The parameters for Api calls are now validated.
  - New Discogs Api methods query parameters are supported now.
- ### **3.0.1**
  - Fixed ```Release.LowestPrice``` & ```MasterRelease.LowestPrice``` deserialization failing due tu ```null``` value being sent by Discogs.
  - Fixed a typo in the ```ReleaseIdentifier``` class name.
  - Added better exception handling to the OAuth flow. ```AuthenticationFailedDiscogsException``` now contains the underlying exception which caused the authentication to fail if one was thrown.

## **Breaking changes in 3.0.0**

- The registration of the ```IDiscogsApiClient``` now happens in a single ```AddDiscogsApiClient``` method
  which offers configuration of the client by providing an options object.

  ```csharp
  services.AddDiscogsApiClient(options =>
  {
    options.UserAgent = "AwesomeApp/1.0.0";
  });
  ```

  This also registers the needed authentication providers which before needed to be registered
  by calling their separate extension methods.

- Authentication is now extracted out of the ```IDiscogsApiClient``` into its own ```IDiscogsAuthenticationService```.
  Instead of calling ```AuthenticateAsync``` on the client the ```IDiscogsAuthenticationService``` needs to be injected and used instead
  which is then used by middleware in the underlying HttpClient to handle the authentication headers for the requests.

  While changing the authentication flow the user token authentication was renamed to personal access token to match the naming in the Discogs developer documentation.

  As another side effect the ```IAuthenticationRequest``` and ```IAuthenticationRequest``` types were removed.
  The two authentication methods of the ```IDiscogsAuthenticationService``` now get their required parameters passed in as arguments
  but the flows and usage of them stayed the same otherwise.

  ```csharp
  // Personal access token (Called user token before)
  public void Foo(IDiscogsAuthenticationService authService)
  {
    _discogsAuthenticationService.AuthenticateWithPersonalAccessToken(UserToken);
  }
  
  // OAuth 1.0a
  public async Task Foo(IDiscogsAuthenticationService authService)
  {
    (token, secret) = await _discogsAuthenticationService.AuthenticateWithOAuth(
        ConsumerKey,
        ConsumerSecret,
        "EmptyOrExistingAcessToken",
        "EmptyOrExistingAccessTokenSecret",
        "http://localhost/verifier_token",
        GetVerifier,
        cancellationToken);
  }
  ```

- The whole implementation of the ```IDiscogsApiClient``` has been redone and now uses the Refit library to generate the client code.
  In process of reimplementing the client the Api methods stayed the same but some might have a small name change.
  Also guards are now used to validate the parameters and throw appropriate ArgumentExceptions.
- A new ```RateLimitExceededDiscogsException``` was added to be able to handle when the rate limit of the Discogs Api is hit.
- The Api contract DTOs where restructured into sub-namespaces to clean up the project structure.
  With the Refit overhaul the contract DTOs now use ```JsonPropertyName``` attributes for serialization
  so a few DTO property names could be renamed for better clarity of their purpose without breaking serialization.

## **Roadmap**

- CI/CD
- Logging

## **Implemented Api Functions**

The current implementation of the Api surface is focused on querying the database and accessing the user's collection and wantlist.

### **User Resources (need authentication)**

- User
  - Get Identity
  - Get User
- Collection
  - Get all collection folders
  - Get, Create, Update, Delete collection folders
  - Get, Add, Remove releases from a collection folder
  - Get collection value
- Wantlist
  - Get releases on the wantlist
  - Add, Delete releases to/from the wantlist

### **Database Resources**

- Get artist & artist's releases
- Get label & label's releases
- Get master release & master release versions
- Get release, release's community rating & release's stats
- Discogs database search
