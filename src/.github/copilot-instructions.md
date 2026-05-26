# Copilot Instructions

## Project Guidelines
- Never automatically push Git changes. Always make local edits only and wait for the user to explicitly say when to push.

## Testing
Tests use the repository's custom recording/playback fixtures for HTTP request/response capture. See `docs/response-recording.md` for setup and recording steps.
- Before recording, verify that `src/DiscogsApiClient.Tests/DiscogsApiClient.Tests.testconfig.json` exists. If it does not, create it with your Discogs PAT as described in the guide. Do not proceed until the file is in place.
- To record new responses, open a terminal at the repository root (`F:\\DiscogsApiClient\`) and run `.\scripts\record-responses.ps1`. Wait for the script to complete. Do not open additional terminals or run further commands during execution. The script handles environment variables, recordings cleanup, test batches, rate-limit cooldowns, and token-leak verification automatically.
