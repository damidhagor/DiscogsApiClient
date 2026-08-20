# CI/CD

This document describes the automated GitHub Actions workflows that run on this repository. See
`docs/MODERNIZATION_PLAN.md` (Phase 7) for the design rationale behind these workflows. The NuGet
publish workflow is a separate, later phase and is not covered here yet.

## Guiding Principle

Only **actual compiler errors**, **test failures**, and **High/Critical severity vulnerabilities** fail
a workflow job. Build warnings, analyzer suggestions, code-style/formatting drift, and Moderate/Low
severity vulnerabilities are surfaced as non-failing `::warning::` annotations (visible on the PR's
"Files changed"/"Checks" tabs and in the job summary) — they don't block merging, but they are never
silently swallowed either.

## `.github/workflows/ci-library.yml`

Validates the main library solution (`src/DiscogsApiClient.slnx`).

- **Triggers:** `pull_request` (any target branch), scoped via `paths` to `src/**`, the workflow file
  itself, `.github/scripts/annotate-diagnostics.sh`, and `global.json` — so doc-only or demo-only PRs
  don't trigger it. `push` to `main` has **no** path filter — every merge to `main` always runs the full
  build/test/coverage pipeline regardless of what changed, so `main` never silently skips validation.
- **Runner:** `ubuntu-latest`, with `actions/setup-dotnet@v4` installing `8.0.x`/`9.0.x`/`10.0.x`
  side-by-side (all three are needed so each multi-targeted TFM's tests run against a matching
  installed runtime, not just a build-time-compatible newer SDK).
- **Steps:**
  1. Restore.
  2. **Build** — `dotnet build -c Release --no-restore /p:EnforceCodeStyleInBuild=true`. Fails only on
     compiler errors; analyzer/style warnings are captured and re-emitted as non-failing `::warning::`
     annotations by `.github/scripts/annotate-diagnostics.sh`.
  3. **Format check** — `dotnet format --verify-no-changes --severity info --no-restore`. Its exit code
     is intentionally not propagated; formatting drift is reported the same way as build warnings.
  4. **Test + coverage** — runs the TUnit test suite via `dotnet test -- --report-trx --coverage
     --coverage-output-format cobertura` (TRX + Cobertura output, auto-named per TargetFramework so the
     three parallel TFM runs don't race on the same output file). This step **does** fail on test
     failures.
  5. **Reporting** — `dorny/test-reporter` publishes pass/fail results from the TRX files as PR check
     annotations; `danielpalme/ReportGenerator-GitHub-Action` turns the Cobertura files into a markdown
     coverage summary posted to the job summary, and the full HTML coverage report is uploaded as a
     workflow artifact.

> **Known limitation:** `dorny/test-reporter` needs a `GITHUB_TOKEN` with `checks: write`, which forked
> `pull_request` runs don't receive (GitHub grants read-only tokens to PRs from forks). For a PR opened
> from a fork, the test-reporter step may silently no-op instead of publishing a check. The standard fix
> is a two-workflow `workflow_run` split (a privileged workflow triggered by the completion of the
> untrusted one), which adds meaningful complexity and hasn't been implemented since this repo doesn't
> currently receive external fork PRs. Revisit if that changes.

## `.github/workflows/ci-demo.yml`

Validates the demo solution (`demo/DiscogsApiClientDemo.slnx`).

- **Triggers:** `pull_request` scoped via `paths` to `demo/**` + its own workflow file (instead of
  `src/**`), same as `ci-library.yml`. `push` to `main` has **no** path filter — always runs on every
  merge to `main`.
- **Runner:** `windows-latest` — required because two of the three demo projects
  (`DiscogsApiClientDemo.OAuth`, `DiscogsApiClientDemo.PersonalAccessToken`) target `net10.0-windows`
  (WPF) and cannot even restore/build on Linux (the Windows Desktop targeting pack isn't available
  there).
- **SDK setup:** a single .NET 10 SDK — the demos don't multi-target and have no tests, so the
  8/9/10 array used by `ci-library.yml` isn't needed here.
- **Steps:** restore → build → format check, using the same non-failing warning/format-drift annotation
  approach as `ci-library.yml`. No test/coverage steps, since the demo projects have no tests.

## `.github/workflows/dependency-check.yml`

Scans the library solution's (`src/DiscogsApiClient.slnx`) NuGet dependencies (direct + transitive) for
known vulnerabilities. The demo solution is intentionally **not** scanned — it's never published or
consumed by end users, so vulnerable transitive dependencies there don't carry the same risk.

- **Triggers:**
  - `schedule` — nightly at 03:00 UTC, so CVEs published against unchanged dependencies are still
    caught even with no new commits.
  - `workflow_dispatch` — manual on-demand run.
  - `pull_request` — scoped via `paths` to `src/**` and the workflow's own files (skips doc-only or
    demo-only PRs).
  - `push` to `main` — **no** path filter, so every merge to `main` always gets a full scan regardless
    of what changed.
- **Runner:** `ubuntu-latest`, single SDK (only `dotnet list package` runs here, no test execution).
- **Steps:** restore `src/DiscogsApiClient.slnx`, then run
  `.github/scripts/check-vulnerabilities.sh src/DiscogsApiClient.slnx`, which:
  1. Runs `dotnet list package --vulnerable --include-transitive --format json` and parses the JSON
     output with `jq`.
  2. **High or Critical** severity findings emit a `::error::` annotation and fail the job.
  3. **Moderate or Low** severity findings emit a `::warning::` annotation only — the job still
     succeeds.
  4. All findings (regardless of severity) are written as a markdown table to the job summary.

## Shared Scripts (`.github/scripts/`)

- **`annotate-diagnostics.sh <log-file>`** — parses a `dotnet build`/`dotnet format` log for
  MSBuild-style warning lines and re-emits each as a GitHub Actions `::warning::` command (with
  file/line/diagnostic-code attribution when the line matches the standard
  `<file>(<line>,<col>): warning <CODE>: <message> [<project>]` shape, otherwise as a generic
  annotation). Always exits `0` — never fails the calling step.
- **`check-vulnerabilities.sh <solution-path>`** — runs and parses `dotnet list package --vulnerable`
  for one solution, applying the High/Critical-fails vs. Moderate/Low-warns severity policy described
  above and writing a findings table to `$GITHUB_STEP_SUMMARY`.

## `global.json`

Adds an opt-in to the native Microsoft.Testing.Platform `dotnet test` runner (`{"test": {"runner":
"Microsoft.Testing.Platform"}}`), required starting with the .NET 10 SDK — the older VSTest-bridge mode
that `dotnet test` used automatically on earlier SDKs is no longer supported once a .NET 10 SDK is
installed alongside the project. This does **not** pin a specific SDK version; it only selects the test
runner. Confirmed the same restriction still applies against the installed `11.0.100-preview` SDK, so
this setting will remain necessary when the repo eventually adopts .NET 11 — revisit at that point in
case a newer TUnit/Microsoft.Testing.Platform release changes the story.
