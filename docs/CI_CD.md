# CI/CD

This document describes the automated GitHub Actions workflows that run on this repository. See
`docs/MODERNIZATION_PLAN.md` (Phase 7) for the design rationale behind these workflows.

## Guiding Principle

Every diagnostic fails the job: **compiler errors**, **Warning-severity build/analyzer diagnostics**
(promoted to build errors via `-warnaserror`), **any `dotnet format` drift** (style/formatting, checked
at `--severity info` — the broadest threshold, so it also catches suggestion-level rules that never
appear in build output), **test failures**, and **High/Critical severity vulnerabilities**. Only
**Moderate/Low severity vulnerabilities** remain non-blocking (`::warning::` annotation only).

**Exception:** `IDE0005` (unnecessary usings) and `IDE0060` (unused parameters) are excluded from the
`dotnet format` check (`--exclude-diagnostics`, configurable via `FORMAT_CHECK_EXCLUDED_DIAGNOSTICS` in
`format-check.sh`). `dotnet format` loads the solution via `MSBuildWorkspace`, which never resolves the
analyzer-only `ProjectReference` to `DiscogsApiClient.SourceGenerator` and so never runs the source
generator during analysis — causing systematic false positives on any rule reasoning about
generator-emitted symbol usage. `IDE0060` is instead enforced correctly by the real `dotnet build`
above (`.editorconfig`: `warning` severity + `EnforceCodeStyleInBuild` + `-warnaserror`, which does
resolve the generator). `IDE0005` cannot be enforced by build at all (requires
`GenerateDocumentationFile`, a separate, unrelated Roslyn limitation) and is left IDE-only — VS's own
live analysis correctly resolves the generator (unlike `dotnet format`), so with "Background analysis
scope" set to "Entire Solution" it reliably catches unused usings without false positives.

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
diagnostic when run under `GITHUB_ACTIONS=true` — so no custom re-emission of *those* diagnostics is
needed. (`format-check.sh` still does its own parsing on top, to group `dotnet format`'s plain-text
output into a deduplicated job-summary table — see Shared Scripts below.)

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
- **Job name:** `Build, Test & Analyze (Library)` — the `(Library)` suffix (added alongside the same
  suffix pattern in `ci-demo.yml`) exists specifically so the job list on a GitHub Actions run page
  (which shows job names, not workflow names) lets you tell the two workflows' jobs apart at a glance.
- **Steps:**
  1. Restore.
  2. **Build** — `dotnet build -c Release --no-restore -p:EnforceCodeStyleInBuild=true -warnaserror`.
     Fails on compiler errors and on any Warning-severity analyzer/style diagnostic (promoted to an error
     by `-warnaserror`). Diagnostics are surfaced via the .NET SDK's own native GitHub Actions
     annotations — no custom parsing.
  3. **Format check** — runs `.github/scripts/format-check.sh <solution>`, which invokes `dotnet format
     --verify-no-changes --severity info --no-restore --exclude-diagnostics IDE0005 IDE0060` (excluded
     rule IDs configurable via `FORMAT_CHECK_EXCLUDED_DIAGNOSTICS`; see Guiding Principle above for why),
     runs with `if: always()` so it still executes (and reports its own findings) even if the Build step
     failed. Fails on any formatting/style drift at `info` severity or above (minus the two excluded
     rules) — i.e. everything else, since `info` is the lowest severity `dotnet format` recognizes. The
     script also converts `dotnet format`'s plain-text output into GitHub Actions `::error`/`::warning`
     annotations and a deduplicated, grouped (by severity, rule, message) job-summary table (Rule |
     Severity | Message | Locations), since `dotnet format` itself only prints plain text.
  4. **Test + coverage** — runs the TUnit test suite via `dotnet test -- --report-trx --coverage
     --coverage-output-format cobertura` (TRX + Cobertura output, auto-named per TargetFramework so the
     three parallel TFM runs don't race on the same output file). This step **does** fail on test
     failures, but runs with `if: always()` so it still executes (and its results are still visible)
     even if the Build step's diagnostics-only failure mode doesn't apply here — practically, this
     matters when the earlier Format check step fails: Build itself already succeeded (binaries exist),
     so `--no-build` can still run the full test suite even though the job is already red. If Build
     itself genuinely fails (a compiler error), this step still attempts to run and fails fast with a
     clear "not built" error, since `--no-build` has nothing to test against.
  5. **Reporting** — each step only runs when its required input files actually exist (guarded via
     `hashFiles(...)` in its `if:` condition, since a Build-step failure means no TRX/Cobertura files
     were ever produced), so a real build failure no longer cascades into a wall of unrelated report-
     generation failures: `dorny/test-reporter` publishes pass/fail results from the TRX files as PR
     check annotations (`fail-on-empty: false` — running unconditionally would otherwise still fail if a
     genuine build failure produced zero TRX files), `danielpalme/ReportGenerator-GitHub-Action` turns
     the Cobertura files into a markdown coverage summary posted to the job summary, and the full HTML
     coverage report is uploaded as a workflow artifact.

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
- **Steps:** restore → build (`-warnaserror`) → format check (`.github/scripts/format-check.sh`,
  `--severity info` minus the excluded IDE0005/IDE0060, `if: always()`), same fail-on-any-diagnostic
  policy as `ci-library.yml`. No test/coverage steps, since the demo projects have no tests.

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

## `.github/workflows/publish-nuget.yml`

Packs and publishes the `DiscogsApiClient` library package. This is the workflow used for every actual
release (see Phase 8 in `docs/MODERNIZATION_PLAN.md`). It deliberately does **not** build against a
solution-wide gate or run tests — quality is already enforced per-PR by `ci-library.yml` before code
ever reaches `main`, so this workflow's only job is to build, pack, and publish exactly what's already
been validated.

- **Trigger:** `workflow_dispatch` only — a release is always a deliberate, manually-initiated action,
  never automatic on a tag or branch push.
- **Input:** `publish_to_test_server` (boolean, default `false`). When `true`, the final push targets
  the NuGet test server (`apiint.nugettest.org`) instead of production `nuget.org` — useful for
  dry-running the whole pack/push pipeline without affecting the real package listing.
- **Runner:** `ubuntu-latest`, with a single `10.0.x` SDK. No test execution happens in this workflow,
  so unlike `ci-library.yml` there's no need for the `8.0.x`/`9.0.x` runtimes too — a single, current
  SDK can compile all three of the library's target frameworks (`net8.0`/`net9.0`/`net10.0`) on its own.
- **Steps:**
  1. **Resolve the package version** — reads `PackageVersion` straight off
     `src/DiscogsApiClient/DiscogsApiClient.csproj` via `dotnet msbuild -getProperty:PackageVersion`
     (no separate version-bump tooling; see `docs/MODERNIZATION_PLAN.md` §7.3) and writes both the
     version and the resolved push target (test server vs. production) to the job summary, so whoever
     triggers the workflow can confirm what's about to be published before it happens.
  2. **Build** — `dotnet build src/DiscogsApiClient/DiscogsApiClient.csproj -c Release`. Required
     before packing: `DiscogsApiClient.csproj` has `GeneratePackageOnBuild=True`, which — somewhat
     counter-intuitively — means `dotnet pack` run on its own does *not* build the project first
     (confirmed locally: packing without a prior build fails with `NU5026`, "file to be packed was not
     found on disk"), so an explicit build step is still needed even though pack normally implies one.
  3. **Pack** — `dotnet pack src/DiscogsApiClient/DiscogsApiClient.csproj -c Release --no-build -o
     ./nupkg -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg`. Only the library project is packed
     (never the test or demo projects); `IncludeSymbols`/`SymbolPackageFormat=snupkg` are passed as
     one-off pack properties rather than added permanently to the `.csproj`, so a plain local `dotnet
     build`/`dotnet pack` (which already runs on every build via `GeneratePackageOnBuild=True`) doesn't
     start producing a `.snupkg` as a side effect.
  4. **Upload the package artifact** — happens immediately after packing and *before* the push step, so
     the built `.nupkg`/`.snupkg` are always retrievable from the run as a workflow artifact even if the
     push step below fails, is skipped, or the run is cancelled.
  5. **NuGet login (OIDC)** — uses [`NuGet/login@v1`](https://github.com/NuGet/login) to exchange
     GitHub's OIDC token for a short-lived (1 hour, single-use) NuGet API key, per
     [NuGet Trusted Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing).
     No long-lived API key secret is stored in the repo at all. Two mutually-exclusive login steps
     (`login_prod`/`login_test`), gated by `if: ${{ !inputs.publish_to_test_server }}`/`if: ${{
     inputs.publish_to_test_server }}`, cover the two targets: the production step uses the action's
     defaults (`https://www.nuget.org/api/v2/token` token endpoint), the test-server step overrides
     `token-service-url`/`audience` to `https://int.nugettest.org/...` — int.nugettest.org supports
     trusted publishing the same way nuget.org does, just with its own separate policy. Both use the
     same `secrets.NUGET_USER` (nuget.org **username**, not email) — trusted publishing policies for
     both targets are expected to be registered under the same username.
  6. **Push** — `dotnet nuget push "./nupkg/*.nupkg" --skip-duplicate` against the source resolved from
     the `publish_to_test_server` input, using whichever login step's `NUGET_API_KEY` output actually
     ran (`steps.login_prod.outputs.NUGET_API_KEY || steps.login_test.outputs.NUGET_API_KEY` — exactly
     one of the two is ever populated). `dotnet nuget push` automatically also pushes the matching
     `.snupkg` alongside each `.nupkg` it finds in the same push (only `--no-symbols` would suppress
     that) — no separate push command is needed for the symbol package.
- **Secrets required:** `NUGET_USER` — the nuget.org account **username** (not email), used for both
  targets. No API key secrets are needed at all; trusted publishing replaces them with short-lived,
  automatically-rotated tokens. No GitHub Environment protection/required-reviewer gate is used —
  single-maintainer project, the manual `workflow_dispatch` trigger plus the explicit boolean switch
  (defaulting to production `false`, i.e. real publish) is judged a sufficient safeguard.
- **Trusted publishing policy setup (one-time, on nuget.org and int.nugettest.org):** on each site, go
  to the account's Trusted Publishing page and add a policy with Repository Owner = `damidhagor`,
  Repository = `DiscogsApiClient`, Workflow File = `publish-nuget.yml` (filename only, no path). No
  Environment is set on either policy, since the workflow doesn't use a GitHub Environment.
- **How to run it:** Actions tab → "Publish to NuGet" → "Run workflow" → leave
  `publish_to_test_server` unchecked for a real release, or check it to dry-run against the test
  server first. The resulting package is always available under the run's Artifacts section
  regardless of outcome.

## Shared Scripts (`.github/scripts/`)

- **`format-check.sh <solution-path>`** — runs `dotnet format --verify-no-changes` (excluding
  `IDE0005`/`IDE0060` by default, see Guiding Principle above) and converts its plain-text diagnostic
  output into GitHub Actions `::error`/`::warning` annotations plus a job-summary table, grouped by
  `(severity, rule, message)` and sorted by severity then rule, with deduplicated file:line locations
  (`dotnet format` doesn't emit annotations or a summary on its own).
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

## IDE Discoverability

The workflow YAMLs and scripts (`.github/workflows/*.yml`, `.github/scripts/*.sh`, plus
`copilot-instructions.md`) are surfaced under `/Solution Items/.github/` in `src/DiscogsApiClient.slnx`
(mirroring the existing `docs/` folder pattern), so they're visible and editable directly from the IDE's
Solution Explorer instead of only through a plain filesystem view.

## Live Validation

All four representative CI failure modes were manually validated end-to-end against PR #20 by pushing a
temporary probe commit for each, confirming it failed at the expected step with a correct annotation,
then reverting: a build warning (fails the `Build` step via `-warnaserror`), a build error (fails
`Build`), a formatting/style violation (fails `Format check` only), and a failing unit test (fails
`Test with coverage` + `Publish test results`). Note MSBuild logs each build diagnostic multiple times
(once per multi-targeted TFM, and again in the end-of-build summary — see AGENTS.md's diagnostics-count
caveat) — this is stock `dotnet build` behavior, not something the CI scripts introduce.
