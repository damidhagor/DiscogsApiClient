# **DiscogsApiClient**

[![NuGet](https://img.shields.io/nuget/v/DiscogsApiClient.svg)](https://www.nuget.org/packages/DiscogsApiClient/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DiscogsApiClient.svg)](https://www.nuget.org/packages/DiscogsApiClient/)
[![.NET](https://img.shields.io/badge/.NET-8%2C%209%2C%2010-blueviolet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/github/license/damidhagor/DiscogsApiClient)](LICENSE)
[![CI - Library](https://github.com/damidhagor/DiscogsApiClient/actions/workflows/ci-library.yml/badge.svg?branch=main)](https://github.com/damidhagor/DiscogsApiClient/actions/workflows/ci-library.yml)
[![CI - Demo](https://github.com/damidhagor/DiscogsApiClient/actions/workflows/ci-demo.yml/badge.svg?branch=main)](https://github.com/damidhagor/DiscogsApiClient/actions/workflows/ci-demo.yml)

A C# library for accessing the [Discogs API v2.0](https://www.discogs.com/developers), targeting .NET 8, 9 and 10.

It allows accessing and modifying a user's collection and wantlist and querying the Discogs database.
Either personal access tokens or OAuth 1.0a can be chosen as authentication methods.

**Disclaimer:** This is a private project and not all Api functions are implemented — see
[docs/API_COVERAGE.md](docs/API_COVERAGE.md) for the current endpoint coverage.
I might add more functionality by request if my time allows for it.

Licensed under the [MIT License](LICENSE).

## **Documentation**

- [docs/CHANGELOG.md](docs/CHANGELOG.md) — full version history, including breaking changes per release
- [docs/MIGRATION_GUIDE.md](docs/MIGRATION_GUIDE.md) — breaking-change migration steps with before/after examples
- [docs/API_COVERAGE.md](docs/API_COVERAGE.md) — detailed endpoint-by-endpoint implementation status

## **Getting Started**

Download the [Nuget Package](https://www.nuget.org/packages/DiscogsApiClient/) or compile the library from source.

### Register the client

`AddDiscogsApiClient` registers the `IDiscogsApiClient`, unauthenticated by default. Chain `.WithPatAuthentication()` or
`.WithOAuthAuthentication()` to opt into an authentication mechanism — without either, requests are sent without an
`Authorization` header.

```csharp
services.AddDiscogsApiClient(options =>
{
    options.UserAgent = "AwesomeAppDemo/1.0.0";
})
.WithPatAuthentication();
```

`AddDiscogsApiClient` and `WithPatAuthentication`/`WithOAuthAuthentication` each have three overloads for configuring
their options:

- `Action<TOptions>` — configure options directly with a delegate (shown above).
- `Action<IServiceProvider, TOptions>` — configure options with access to the `IServiceProvider`, e.g. to resolve a
  secret store for the OAuth consumer key/secret.
- `IConfiguration` — bind options directly from a configuration section, e.g. `configuration.GetSection("Discogs")`.

`AddDiscogsApiClient` also accepts an optional `Action<IHttpClientBuilder>` to customize the underlying `HttpClient`
(e.g. adding your own delegating handlers, which sit outermost in the pipeline, wrapping the client's error,
authentication and rate-limit-state handlers).

### Configuration via `appsettings.json`

If an `IConfiguration` is registered in the service collection, the `"Discogs"` (`DiscogsApiClientOptions.SectionName`),
`"Discogs:Pat"` (`DiscogsPatOptions.SectionName`) and `"Discogs:OAuth"` (`DiscogsOAuthOptions.SectionName`) sections are
bound automatically before any `configureOptions` delegate is applied, so code-based configuration always overrides
configuration values:

```json
{
  "Discogs": {
    "UserAgent": "AwesomeAppDemo/1.0.0"
  },
  "Discogs:Pat": {
    "Token": "YourPersonalAccessToken"
  }
}
```

Invalid options (e.g. a missing `UserAgent`) throw an `OptionsValidationException` at application startup
(`ValidateOnStart`), not when a request is made.

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

### Personal access token authentication

A personal access token can be provided either via configuration (see [Configuration via `appsettings.json`](#configuration-via-appsettingsjson)
above) or by calling `Authenticate` in code — no manual `Authenticate` call is needed if the token is already bound
from configuration. Calling `Authenticate` again at any point (e.g. to swap to a different user's token) overrides
the previously stored token.

```csharp
// At startup register the DiscogsApiClient and opt into
// Personal Access Token authentication with the IServiceCollection.
// If "Discogs:Pat:Token" is already set in configuration, the client is
// immediately authenticated and no manual Authenticate call is required.

services.AddDiscogsApiClient(options =>
{
    options.UserAgent = "AwesomeAppDemo/1.0.0";
})
.WithPatAuthentication();

// Otherwise, inject the IDiscogsPatAuthenticationProvider and IDiscogsApiClient
// and authenticate with the personal access token before using the client.
// Calling Authenticate again later overrides the previously stored token.

public Foo(
    IDiscogsApiClient discogsApiClient,
    IDiscogsPatAuthenticationProvider authProvider)
{
    _discogsApiClient = discogsApiClient;
    _authProvider = authProvider;
}

public void Authenticate(string token)
{
    _authProvider.Authenticate(token);
}

public async Task<string> GetUsername(CancellationToken cancellationToken)
{
    var identity = await _discogsApiClient.GetIdentity(cancellationToken);
    return identity.Username;
}
```

### OAuth authentication

As with PAT authentication, calling `Authenticate`/`CompleteAuthentication` again at any point overrides the
previously stored access token and secret.

The access token and secret returned by `CompleteAuthentication` are not persisted by the library and are only
valid for the lifetime of the process unless you store them yourself (e.g. secure storage, a database) and feed
them back in via `Authenticate(accessToken, accessTokenSecret)` on a later run — this skips the interactive
flow (`StartAuthentication`/`CompleteAuthentication`) entirely, similar to how a PAT is authenticated directly.

```csharp
// At startup register the DiscogsApiClient and opt into OAuth authentication.
// Provide the Consumer Key & Secret & verifier callback url here.
services.AddDiscogsApiClient(options =>
{
    options.UserAgent = "AwesomeAppDemo/1.0.0";
})
.WithOAuthAuthentication(options =>
{
    options.ConsumerKey = "YourConsumerKey";
    options.ConsumerSecret = "YourConsumerSecret";
    options.VerifierCallbackUrl = "http://localhost/verifier_token";
});

// Inject the IDiscogsOAuthAuthenticationProvider and IDiscogsApiClient
// and authenticate with the OAuth flow before using the client.

public Foo(
    IDiscogsApiClient discogsApiClient,
    IDiscogsOAuthAuthenticationProvider authProvider)
{
    _discogsApiClient = discogsApiClient;
    _authProvider = authProvider;
}

// Authenticate with your consumer key & secret from your Discogs application settings.
public async Task Authenticate(CancellationToken cancellationToken)
{
    // Start authentication.
    var session = await _authProvider.StartAuthentication(cancellationToken);

    // Retrieve Verifier Token.
    // 1) Open browser with session.AuthorizeUrl
    // 2) Detect redirect to session.VerifierCallbackUrl
    // 3) Verifier Token will be appended to the url: http://localhost/verifier_token?oauth_token=TOKEN&oauth_verifier=VERIFIER
    // 4) Parse verifier from url and return it
    var verifierToken = "...";

    // Complete authentication.
    var (accessToken, accessTokenSecret) = await _authProvider.CompleteAuthentication(session, verifierToken, cancellationToken);

    // Save the returned access token and secret yourself, e.g. in a database or secure storage,
    // so they can be reused via Authenticate below on a later run.
    await SaveTokenAsync(accessToken, accessTokenSecret, cancellationToken);
}

// On a later run, load the previously saved access token and secret and authenticate
// directly with them, skipping the interactive StartAuthentication/CompleteAuthentication flow.
public async Task AuthenticateWithStoredToken(CancellationToken cancellationToken)
{
    var (accessToken, accessTokenSecret) = await LoadTokenAsync(cancellationToken);
    _authProvider.Authenticate(accessToken, accessTokenSecret);
}
```

### Rate limit state

The client no longer enforces rate limiting itself. Instead, the `x-discogs-ratelimit*` response headers are parsed
after every request and exposed read-only through `IDiscogsRateLimitStateService`, so you can implement whatever
throttling/backoff strategy suits your application:

```csharp
public Foo(IDiscogsRateLimitStateService rateLimitStateService)
{
    _rateLimitStateService = rateLimitStateService;
}

public void LogRateLimit()
{
    if (_rateLimitStateService.TryGetCurrentState(out var state))
    {
        Console.WriteLine($"{state.Remaining}/{state.Limit} requests remaining (used: {state.Used})");
    }
}
```
