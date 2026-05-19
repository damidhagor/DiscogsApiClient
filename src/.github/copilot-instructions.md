# Copilot Instructions

## Project Guidelines
- Never automatically push Git changes. Always make local edits only and wait for the user to explicitly say when to push.

## Testing
- Tests use WireMock.Net for HTTP recording/playback. See [docs/wiremock-recording.md](../docs/wiremock-recording.md) for setup, recording steps, and troubleshooting.
- Before running tests in record mode, open a terminal first so the user can enter the required environment variables (`WIREMOCK_RECORD`, `DISCOGS_USER_TOKEN`). Do not run test commands until the user confirms the variables are set. All subsequent recording commands must be run in that same terminal session — do not open new terminals or the environment variables will be lost.
- Before recording, always clean the existing mappings directory (`DiscogsApiClient.Tests/Fixtures/WireMock/Mappings/`).
