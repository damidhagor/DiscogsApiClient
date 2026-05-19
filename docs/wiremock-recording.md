# WireMock Recording Guide

This guide explains how to record and maintain WireMock mappings for the DiscogsApiClient test suite.

## Overview

Tests run against a local WireMock server. In **playback mode** (default), WireMock serves pre-recorded responses from `DiscogsApiClient.Tests/Fixtures/WireMock/Mappings/`. In **record mode**, WireMock proxies requests to the real Discogs API and saves the responses as mapping files.

## Prerequisites

- A valid Discogs personal access token (create one at <https://www.discogs.com/settings/developers>).
- The token is **never committed**. It is passed via environment variable and cleaned up automatically after recording.

## Environment Variables

| Variable            | Required For | Description                                  |
|---------------------|--------------|----------------------------------------------|
| `WIREMOCK_RECORD`   | Recording    | Set to `true` to enable record mode.         |
| `DISCOGS_USER_TOKEN`| Recording    | Your Discogs personal access token.          |

## Recording Mappings

### 1. Open a terminal and set environment variables

Open a terminal first, then set the variables in that terminal session:

```powershell
$env:WIREMOCK_RECORD = "true"
$env:DISCOGS_USER_TOKEN = "<your-token>"
```

### 2. Clean the mappings directory

Remove any existing mapping files so recordings start fresh:

```powershell
Remove-Item -Recurse -Force "DiscogsApiClient.Tests/Fixtures/WireMock/Mappings/*" -ErrorAction SilentlyContinue
```

### 3. Run tests by namespace to stay within rate limits

The Discogs API allows **60 requests per minute**. Record one test namespace at a time with a cooldown between batches.

```powershell
# User tests
dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "/DiscogsApiClient.Tests/DiscogsApiClient.Tests.User/**"

Start-Sleep -Seconds 65

# Database tests
dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Database/**"

Start-Sleep -Seconds 65

# Collection tests — split into individual fixtures to avoid rate limits
dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionFoldersTests/**"

Start-Sleep -Seconds 65

dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionFolderReleasesTests/**"

Start-Sleep -Seconds 65

dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionValueTests/**"

Start-Sleep -Seconds 65

dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/WantlistTests/**"
```

### 4. Verify token cleanup

After recording, confirm no real tokens remain in the mappings:

```powershell
Select-String -Path "DiscogsApiClient.Tests/Fixtures/WireMock/Mappings/__admin/mappings/*.json" -Pattern "Discogs token=" | Where-Object { $_ -notmatch "Discogs token=\*" }
```

This should produce **no output**. If it does, a token was not cleaned up — do not commit those files.

### 5. Run tests in playback mode

Unset the recording variables and run the full suite to verify mappings work:

```powershell
Remove-Item Env:\WIREMOCK_RECORD -ErrorAction SilentlyContinue
Remove-Item Env:\DISCOGS_USER_TOKEN -ErrorAction SilentlyContinue

dotnet run --project DiscogsApiClient.Tests -f net10.0
```

## How It Works

- `WireMockServerFixture` starts a WireMock server per test session.
- In record mode, it configures `ProxyAndRecordSettings` pointing to `https://api.discogs.com`.
- Each recorded response is saved as a separate JSON file (with a GUID suffix to avoid overwrites).
- On dispose, the fixture cleans up tokens in the recorded files in-place inside WireMock's `__admin/mappings/` subdirectory, which is where `ReadStaticMappings` expects them.
- In playback mode, `ReadStaticMappings` loads all JSON files from the root mappings folder.

## Notes

- The test runner command uses `dotnet run` (not `dotnet test`) because .NET 10 requires the new test platform.
- `Host` and `traceparent` headers are excluded from recordings to avoid environment-specific mismatches.
- Environment variables are set per terminal session. If the terminal is closed between runs, the variables must be set again. When using an agent, have it open a terminal first so you can enter the variables before any test commands are executed.
