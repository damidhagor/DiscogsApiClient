# Response Recording Guide

This guide explains how to record and maintain the project's custom request/response recording and playback used by the DiscogsApiClient test suite.

## Overview

Tests use a small custom recording/playback system implemented in the test fixtures. In playback mode (default) tests read recorded request/response pairs from the `src/DiscogsApiClient.Tests/Fixtures/Recording/Recordings/` folder. In record mode the test run proxies requests to the real Discogs API and saves sanitized recordings to disk for later playback.

## Prerequisites

- A valid Discogs personal access token (create one at <https://www.discogs.com/settings/developers>).
- The token is never committed. It must be stored in a gitignored config file used only for recording runs.

## One-Time Setup

Create the file `src/DiscogsApiClient.Tests/DiscogsApiClient.Tests.testconfig.json` with your Discogs PAT:

```json
{
  "DiscogsUserToken": "<your-discogs-pat>"
}
```

This file is gitignored and only needs to be created once. The test fixtures read the token from this file at runtime.

## Recording Responses

From the repository root, run the recording script:

```powershell
.\scripts\record-responses.ps1
```

What the script does:

1. Sets the `DISCOGS_RECORD` environment variable to `true` for the process running the tests.
2. Removes existing files under `src/DiscogsApiClient.Tests/Fixtures/Recording/Recordings/` to start with a clean directory.
3. Executes a sequence of test batches by invoking the test project with tree-node filters via:

```powershell
dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "<filter>"
```

4. Sleeps for a cooldown period between batches to avoid hitting the Discogs rate limit.
5. Unsets the `DISCOGS_RECORD` environment variable when the script finishes (the script uses a `finally` block to ensure cleanup).

Note: the script itself does not perform an automated token-leak scan. Sensitive header values (for example `Authorization`) are sanitized by the recording logic in code before they are written to disk.

The script exits with the exit code from the last executed dotnet run command. Check the script output for batch-level warnings.

## Verifying Playback

After recording, run the test project without the `DISCOGS_RECORD` env var to confirm playback works:

```powershell
cd src
dotnet run --project DiscogsApiClient.Tests -f net10.0
```

## How It Works

- `RecordingFixture` collects request/response pairs during test execution and writes JSON files to `src/DiscogsApiClient.Tests/Fixtures/Recording/Recordings/`.
- `PlaybackFixture` and `PlaybackDelegatingHandler` load the saved recordings and return stored responses during playback runs.
- `RecordingHelper.GetCleanedUpHeaders` sanitizes sensitive headers (for example it replaces `Authorization` values with `"****"`) and excludes environment-specific headers like `traceparent`.
- Playback is file-based: the playback handler reads the JSON files and matches requests to recorded responses.

## Implementation Notes

- Environment variable: `DISCOGS_RECORD` controls recording mode. When set to `true`, the suite records; when unset the suite uses playback.
- Recordings are organized by test/namespace keys (see `RecordingHelper.GetRecordingKey`) to keep files isolated and predictable.
- Headers like `traceparent` are excluded from recordings to avoid environment-specific mismatches.

``` 
