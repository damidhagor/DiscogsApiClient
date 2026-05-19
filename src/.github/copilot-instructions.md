# Copilot Instructions

## Project Guidelines
- Never automatically push Git changes. Always make local edits only and wait for the user to explicitly say when to push.

## Testing
- Tests use WireMock.Net for HTTP recording/playback. See [docs/wiremock-recording.md](../docs/wiremock-recording.md) for setup and recording steps.
- Before recording, verify that `src/DiscogsApiClient.Tests/DiscogsApiClient.Tests.testconfig.json` exists. If it does not, tell the user to create it with the JSON snippet from `docs/wiremock-recording.md` and fill in their Discogs PAT. Do not proceed until the user confirms the file is in place.
- To record new mappings, open a terminal at the repository root (`F:\DiscogsApiClient\`) and run `.\scripts\record-mappings.ps1`. Wait for the script to complete. Do not open additional terminals or run further commands during execution. The script handles environment variables, mappings cleanup, test batches, rate-limit cooldowns, and token-leak verification automatically.
