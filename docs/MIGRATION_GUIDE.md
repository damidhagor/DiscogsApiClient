# Migration Guide

This guide covers breaking changes and the steps needed to migrate between major/breaking versions of
`DiscogsApiClient`. For the full version history (including non-breaking changes), see
[docs/CHANGELOG.md](CHANGELOG.md).

- [Migrating from 4.1.1 to 5.0.0](#migrating-from-411-to-500)
- [Migrating from 4.0.0 to 4.1.0](#migrating-from-400-to-410)
- [Migrating from 3.1.0 to 4.0.0](#migrating-from-310-to-400)
- [Migrating from pre-3.0.0 to 3.0.0](#migrating-from-pre-300-to-300)

If you're jumping across multiple versions (e.g. 2.x straight to 5.0.0), work through each guide above in
order, oldest first.

## Migrating from 4.1.1 to 5.0.0

### Framework targets

The library now targets **.NET 8, 9 and 10**. .NET 6 and .NET 7 are no longer supported — upgrade your application's
target framework before updating.

### Service registration moved namespace

`AddDiscogsApiClient` moved from the `DiscogsApiClient` namespace to `Microsoft.Extensions.DependencyInjection`.

```diff
- using DiscogsApiClient;
+ using Microsoft.Extensions.DependencyInjection;
```

If you already `using Microsoft.Extensions.DependencyInjection;` for other DI setup, no change is needed.

### Authentication is now an explicit opt-in

**Before (4.x):** authentication was handled by `IDiscogsAuthenticationService`, which offered both flows
simultaneously without any registration-time configuration.

```csharp
services.AddDiscogsApiClient(options =>
{
    options.UserAgent = "AwesomeAppDemo/1.0.0";
});

// IDiscogsAuthenticationService offered both PAT and OAuth at runtime.
public Foo(IDiscogsApiClient client, IDiscogsAuthenticationService authService)
{
    _authService = authService;
}
```

**After (5.0.0):** you opt into exactly one authentication mechanism at registration time by chaining
`.WithPatAuthentication(...)` or `.WithOAuthAuthentication(...)`. Calling both throws `InvalidOperationException`;
calling neither yields a valid, permanently unauthenticated client (no `Authorization` header is sent).

```csharp
services.AddDiscogsApiClient(options =>
{
    options.UserAgent = "AwesomeAppDemo/1.0.0";
})
.WithPatAuthentication();

// Inject the specific provider for the mechanism you registered.
public Foo(IDiscogsApiClient client, IDiscogsPatAuthenticationProvider authProvider)
{
    _authProvider = authProvider;
}
```

### Authentication provider/option renames

| Before (4.x) | After (5.0.0) |
|---|---|
| `IPersonalAccessTokenAuthenticationProvider` / `PersonalAccessTokenAuthenticationProvider` | `IDiscogsPatAuthenticationProvider` / `DiscogsPatAuthenticationProvider` |
| `IOAuthAuthenticationProvider` / `OAuthAuthenticationProvider` | `IDiscogsOAuthAuthenticationProvider` / `DiscogsOAuthAuthenticationProvider` |
| `DiscogsApiClientOptions.ConsumerKey`/`ConsumerSecret`/`VerifierCallbackUrl` | `DiscogsOAuthOptions.ConsumerKey`/`ConsumerSecret`/`VerifierCallbackUrl` (bound from `"Discogs:OAuth"`) |
| *(new)* | `DiscogsPatOptions.Token` (bound from `"Discogs:Pat"`) |

`DiscogsOAuthAuthenticationProvider`'s constructor now takes an `IHttpClientFactory` instead of an `HttpClient` — this
only matters if you constructed the provider manually instead of via DI.

### Options validation now happens at startup

**Before:** invalid options (e.g. a missing `UserAgent`) threw `InvalidOperationException` when the options were
registered.

**After:** invalid options throw `OptionsValidationException` when the application starts (`ValidateOnStart`), not
at registration time and not on first use. Make sure your application observes startup exceptions (most hosts do by
default).

### Rate limiting removed

**Before:** the client enforced rate limiting itself via `DiscogsApiClientOptions.RateLimitOptions` and a built-in
sliding-window limiter.

**After:** the limiter was removed entirely. The `x-discogs-ratelimit*` response headers are still parsed after every
request and exposed read-only through `IDiscogsRateLimitStateService`:

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

If you relied on the built-in limiter to throttle requests, you'll need to implement your own throttling/backoff
(e.g. a custom `DelegatingHandler` registered via the `Action<IHttpClientBuilder>` hook on `AddDiscogsApiClient`)
using the state exposed above.

### `IDiscogsApiClient` interface changes

- All `CancellationToken` parameters are now **required** — the previous `CancellationToken cancellationToken = default`
  default value was removed, so every call site must now pass a token explicitly.

  ```diff
  - var identity = await discogsApiClient.GetIdentity();
  + var identity = await discogsApiClient.GetIdentity(cancellationToken);
  ```

- `[ApiClient]` now targets the internal generated implementation class instead of the `IDiscogsApiClient` interface.
  This only matters if you referenced generator internals directly; consumers using `IDiscogsApiClient` via DI are
  unaffected.

### Contract DTO changes

- Collection properties on `Contract` DTOs (e.g. `Release.Tracklist`, `Artist.Members`, etc.) changed from `List<T>`
  to `IReadOnlyList<T>`. Code that only reads/iterates these collections is unaffected; code that mutates them
  (`.Add()`, `.Remove()`, indexer assignment) or assigns them to a `List<T>`-typed variable needs to be updated to
  copy into a new list first, e.g. `[.. release.Tracklist]` or `new List<T>(release.Tracklist)`.
- `OAuthAuthenticationSession.AuthorizeUrl` and `VerifierCallbackUrl` changed from `string` to `Uri`.

  ```diff
  - Process.Start(new ProcessStartInfo(session.AuthorizeUrl) { UseShellExecute = true });
  + Process.Start(new ProcessStartInfo(session.AuthorizeUrl.ToString()) { UseShellExecute = true });
  ```

## Migrating from 3.1.0 to 4.0.0

### OAuth call flow

If you're still on a pre-4.0.0 version, the OAuth flow used a single callback-based call:

```csharp
// Pre-4.0.0
var (accessToken, accessTokenSecret) = await authService.AuthenticateWithOAuth(
    consumerKey, consumerSecret, GetVerifierCallback, cancellationToken);
```

4.0.0 replaced this with two explicit calls, `StartOAuthAuthentication`/`CompleteOAuthAuthentication`:

```csharp
// 4.0.0
var session = await authService.StartOAuthAuthentication(consumerKey, consumerSecret, cancellationToken);
// ... obtain verifierToken from the user via session.AuthorizeUrl / session.VerifierCallbackUrl ...
var (accessToken, accessTokenSecret) = await authService.CompleteOAuthAuthentication(session, verifierToken, cancellationToken);
```

> [!NOTE]
> The provider and method names above changed again in 5.0.0 — see
> ["Authentication is now an explicit opt-in"](#authentication-is-now-an-explicit-opt-in) in the 5.0.0 guide
> for the current call shape.

## Migrating from 4.0.0 to 4.1.0

### Renamed release properties

| Before | After |
|---|---|
| `Release.YearFormatted` | `Release.ReleasedFormatted` |
| `MasterReleaseVersion.Year` | `MasterReleaseVersion.Released` |

## Migrating from pre-3.0.0 to 3.0.0

### Dependency injection setup

DI registration was simplified from manual `HttpClient`/service wiring to a single `AddDiscogsApiClient` call
(now further evolved into the `.WithPatAuthentication()`/`.WithOAuthAuthentication()` pattern described in the
5.0.0 guide), and the contract classes were restructured into sub-namespaces with some properties renamed for
clarity. Refer to the [3.0.0 changelog entry](CHANGELOG.md#300---2023-03-20) for context; there is no automated
migration path for the pre-3.0.0 API shape given how much has changed since.

