# CI/CD

This document describes the automated GitHub Actions workflows that run on this repository. See
`docs/MODERNIZATION_PLAN.md` (Phase 7) for the design rationale behind these workflows. The NuGet
publish workflow is a separate, later phase and is not covered here yet.

## Guiding Principle

Every diagnostic fails the job: **compiler errors**, **Warning-severity build/analyzer diagnostics**
(promoted to build errors via `-warnaserror`), **any `dotnet format` drift** (style/formatting, checked
at `--severity info` — the broadest threshold, so it also catches suggestion-level rules that never
appear in build output), **test failures**, and **High/Critical severity vulnerabilities**. Only
**Moderate/Low severity vulnerabilities** remain non-blocking (`::warning::` annotation only).

This intentionally trades "warnings are fine, just don't get lost" for a much simpler and stricter rule.
We first tried the opposite policy — non-failing annotations plus a job-summary table for warnings — but
discovered GitHub has no supported way to surface a non-blocking signal at the PR's checks-list/merge-
button level; annotations only ever show inline on "Files changed" (and only for lines in *that* PR's
diff) or on the workflow run's own Summary/Annotations page, never on the PR Conversation tab or the
compact checks list next to the merge button. The only way to get a merge-button-level indicator for a
passing-with-warnings state is a custom Checks-API check run with `conclusion: neutral` — a regular
workflow job cannot set that conclusion itself, only GitHub can, so achieving it requires a second,
independently-managed check run created via `actions/github-script` or similar. That was judged too much
complexity for what it buys. A failing job, by contrast, already does everything for free: it blocks
merging, shows a red X in the compact checks list without any extra click, and both `dotnet build` and
`dotnet format` already emit native `##[warning]`/`##[error]` GitHub Actions annotations for every
diagnostic when run under `GITHUB_ACTIONS=true` — so no custom parsing/re-emission is needed at all.

> **Below-Warning build diagnostics:** `EnforceCodeStyleInBuild=true` only makes `dotnet build` report
> IDE style rules that are configured at `warning` (or `error`) severity in `.editorconfig` — rules left
> at their default `suggestion`/`silent` severity are never reported during build at all (editor-only),
> regardless of `-warnaserror`. That gap is fully covered by the separate `dotnet format --verify-no-
> changes --severity info` step, which independently re-checks style/formatting at every severity level
> and fails on any drift — so between the two steps, no diagnostic severity is silently ignored.

## `.github/workflows/ci-library.yml`

Validates the main library solution (`src/DiscogsApiClient.slnx`).

- **Triggers:** `pull_request` (any target branch), scoped via `paths` to `src/**`, the workflow file
  itself, and `global.json` — so doc-only or demo-only PRs don't trigger it. `push` to `main` has **no**
  path filter — every merge to `main` always runs the full build/test/coverage pipeline regardless of
  what changed, so `main` never silently skips validation.
- **Runner:** `ubuntu-latest`, with `actions/setup-dotnet@v4` installing `8.0.x`/`9.0.x`/`10.0.x`
  side-by-side (all three are needed so each multi-targeted TFM's tests run against a matching
  installed runtime, not just a build-time-compatible newer SDK).
- **Steps:**
  1. Restore.
  2. **Build** — `dotnet build -c Release --no-restore -p:EnforceCodeStyleInBuild=true -warnaserror`.
     Fails on compiler errors and on any Warning-severity analyzer/style diagnostic (promoted to an error
     by `-warnaserror`). Diagnostics are surfaced via the .NET SDK's own native GitHub Actions
     annotations — no custom parsing.
  3. **Format check** — `dotnet format --verify-no-changes --severity info --no-restore`, run with
     `if: always()` so it still executes (and reports its own findings) even if the Build step failed.
     Fails on any formatting/style drift at `info` severity or above — i.e. everything, since `info` is
     the lowest severity `dotnet format` recognizes.
  4. **Test + coverage** — runs the TUnit test suite via `dotnet test -- --report-trx --coverage
     --coverage-output-format cobertura` (TRX + Cobertura output, auto-named per TargetFramework so the
     three parallel TFM runs don't race on the same output file). This step **does** fail on test
     failures. It has no `if: always()`, so it's correctly skipped if the Build step already failed
     (it depends on `--no-build` reusing that step's output).
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
- **Steps:** restore → build (`-warnaserror`) → format check (`--severity info`, `if: always()`), same
  fail-on-any-diagnostic policy as `ci-library.yml`. No test/coverage steps, since the demo projects have
  no tests.

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
     succeeds. This is the one remaining non-blocking exception to the Guiding Principle above,
     deliberate because low-severity transitive-dependency findings are frequently not actionable
     on a short timeline and a hard fail there would block unrelated PRs too often.
  4. All findings (regardless of severity) are written as a markdown table to the job summary.

## Shared Scripts (`.github/scripts/`)

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
