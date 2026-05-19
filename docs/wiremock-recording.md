# WireMock Recording Guide

This guide explains how to record and maintain WireMock mappings for the DiscogsApiClient test suite.

## Overview

Tests run against a local WireMock server. In **playback mode** (default), WireMock serves pre-recorded responses from `DiscogsApiClient.Tests/Fixtures/WireMock/Mappings/`. In **record mode**, WireMock proxies requests to the real Discogs API and saves the responses as mapping files.

## Prerequisites

- A valid Discogs personal access token (create one at <https://www.discogs.com/settings/developers>).
- The token is **never committed**. It is stored in a gitignored config file and cleaned up automatically from recordings.

## One-Time Setup

Create the file `src/DiscogsApiClient.Tests/DiscogsApiClient.Tests.testconfig.json` with your Discogs PAT:

```json
{
  "DiscogsUserToken": "<your-discogs-pat>"
}
```

This file is gitignored and only needs to be created once. The test fixture reads the token from this file via TUnit's built-in configuration (`TestContext.Configuration`).

## Recording Mappings

From the repository root, run the recording script:

```powershell
.\scripts\record-mappings.ps1
```

The script handles everything automatically:

1. Sets the `WIREMOCK_RECORD` environment variable
2. Cleans the existing mappings directory
3. Runs all test batches with 65-second cooldowns between them (Discogs API allows 60 requests per minute)
4. Verifies no real tokens leaked into the recorded mapping files
5. Unsets the environment variable on completion (even on failure)

The script exits with a non-zero code if any batch fails or a token leak is detected.

## Verifying Playback

After recording, run the full test suite without the script to confirm the mappings work in playback mode:

```powershell
cd src
dotnet run --project DiscogsApiClient.Tests -f net10.0
```

## How It Works

- `WireMockServerFixture` starts a WireMock server per test session.
- In record mode (`WIREMOCK_RECORD=true`), it configures `ProxyAndRecordSettings` pointing to `https://api.discogs.com`.
- Each recorded response is saved as a separate JSON file (with a GUID suffix to avoid overwrites).
- On dispose, the fixture cleans up tokens in the recorded files in-place inside WireMock's `__admin/mappings/` subdirectory, which is where `ReadStaticMappings` expects them.
- In playback mode, `ReadStaticMappings` loads all JSON files from the root mappings folder.

## Notes

- The test runner command uses `dotnet run` (not `dotnet test`) because .NET 10 requires the new test platform.
- `Host` and `traceparent` headers are excluded from recordings to avoid environment-specific mismatches.
