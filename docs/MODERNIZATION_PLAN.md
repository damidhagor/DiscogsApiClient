# DiscogsApiClient Modernization Plan

**Branch Strategy:** All work is performed in the `modernization` branch. Individual tasks are completed in feature branches and merged back to `modernization` via PRs. A final PR to `main` is created only after all modernization work is complete.

**Version:** 1.1  
**Status:** Phase 5 Complete - Ready for Phase 6  
**Last Updated:** 2026-08-19  

---

## Table of Contents
1. [Overview](#overview)
2. [General Rules & Principles](#general-rules--principles)
3. [Target Frameworks](#target-frameworks)
4. [Modernization Phases](#modernization-phases)
5. [Phase 1: Foundation - Framework & Package Updates](#phase-1-foundation---framework--package-updates)
6. [Phase 2: Testing Infrastructure Modernization](#phase-2-testing-infrastructure-modernization)
7. [Phase 3: Source Generator Modernization](#phase-3-source-generator-modernization)
8. [Phase 4: Library Code Modernization](#phase-4-library-code-modernization)
9. [Phase 5: Demo Projects Modernization](#phase-5-demo-projects-modernization)
10. [Phase 6: Final Validation & Documentation](#phase-6-final-validation--documentation)
11. [Phase 7: CI/CD Automation](#phase-7-cicd-automation)
12. [Phase 8: Release Preparation & Merge to Main](#phase-8-release-preparation--merge-to-main)

---

## Overview

This document outlines the technical modernization of the DiscogsApiClient library. The focus is on updating frameworks, adopting modern C# language features, improving testing infrastructure, and following current best practices. **This modernization does NOT include expanding API coverage or adding new functionality.**

### Current State (as of modernization branch creation)
- **DiscogsApiClient:** Targets .NET 6, 7, 8 with C# implicit
- **DiscogsApiClient.Tests:** Targets .NET 6, 7, 8 using NUnit 3.14.0
- **DiscogsApiClient.SourceGenerator:** Targets .NET Standard 2.0 with C# 11
- **Package Version:** 4.1.0 (will not change until final release to main)

### Goals
- ✅ Update target frameworks to .NET 8, 9, and 10 (Complete)
- ✅ Adopt C# 12 features compatible with target frameworks (Complete)
- ✅ Migrate tests from NUnit to TUnit (Complete - All tests passing)
- ✅ Modernize testing infrastructure with mocking and improved coverage (Complete)
- ✅ Update source generators to follow latest Roslyn best practices (Complete)
- ✅ Ensure all code follows modern C# best practices (Phases 3-5)
- **Breaking changes are acceptable** - this branch will be published as **v5.0.0**

### Strategy
The modernization follows a **risk-minimization approach**:
1. Update frameworks and packages first (minimal code changes)
2. Migrate to TUnit and modernize testing infrastructure (enable reliable validation)
3. Modernize source generators (validated by comprehensive tests, happens AFTER testing modernization)
4. Modernize library code (protected by improved tests and modern generators)
5. Update demo projects (showcase modern patterns)

### Version Strategy
- Library version (4.1.0) remains unchanged through Phases 1-7 (framework/code work, docs, CI)
- Version bump to **5.0.0** happens in Phase 8, as part of preparing the final PR from `modernization` to `main`
- This allows development without premature version commits, while confirming upfront that this branch **will** ship as v5.0.0 (not a "maybe")

---

## General Rules & Principles

### Branching Strategy
- ✅ **Base Branch:** `modernization` (created from `main`)
- ✅ **Feature Branches:** Each phase/task gets its own branch (e.g., `modernization/phase1-frameworks`)
- ✅ **Pull Requests:** Feature branches → `modernization` (reviewed and merged)
- ✅ **Final Merge:** `modernization` → `main` (after all phases complete)

### Code Quality Standards
- ✅ **C# Language Version:** Use the latest C# version compatible with target framework
- ✅ **Best Practices:** Follow official Microsoft guidelines and recommendations
- ✅ **Consistency:** Maintain consistent style across all projects
- ✅ **Documentation:** Update XML docs and markdown documentation as needed

### Testing Requirements
- ✅ **Test Coverage:** Maintain or improve existing test coverage
- ✅ **Build Validation:** All changes must compile successfully
- ✅ **Test Validation:** All tests must pass before merging
- ✅ **No Breaking Changes:** Avoid breaking public API unless absolutely necessary

### Review Checklist (for each PR)
**Note:** This is a template checklist to be used for reviewing each pull request, not a one-time completion task.

- [ ] Code compiles without warnings
- [ ] All tests pass
- [ ] C# language features appropriate for target framework
- [ ] XML documentation updated
- [ ] No unintended breaking changes
- [ ] Performance not degraded

---

## Target Frameworks

### DiscogsApiClient (Main Library)
- **Current:** `net6.0;net7.0;net8.0`
- **Target:** `net8.0;net9.0;net10.0`
- **C# Language Version:** SDK default (no explicit setting needed)
- **Rationale:** 
  - Drop .NET 6 (EOL Nov 2024), drop .NET 7 (EOL May 2024)
  - Add .NET 9 (STS, supported until Nov 2026)
  - Add .NET 10 (LTS, supported until Nov 2028)

### DiscogsApiClient.Tests
- **Current:** `net6.0;net7.0;net8.0` with NUnit 3.14.0
- **Target:** `net8.0;net9.0;net10.0` with xUnit (latest)
- **C# Language Version:** SDK default (no explicit setting needed)
- **Test Framework Migration:** NUnit → xUnit
- **Rationale:** Match library target frameworks, modernize test framework

### DiscogsApiClient.SourceGenerator (Analyzer Project)
- **Current:** `netstandard2.0` with `<LangVersion>11</LangVersion>`
- **Target:** `netstandard2.0` (no change)
- **C# Language Version:** `<LangVersion>latest</LangVersion>` (many C# features are compiler-lowered and runtime-independent)
- **Current Packages:**
  - `Microsoft.CodeAnalysis.CSharp` 4.8.0
  - `Microsoft.CodeAnalysis.Analyzers` 3.3.4
- **Rationale:** 
  - Roslyn analyzers must target .NET Standard 2.0 for IDE compatibility
  - Modern C# features can be used as long as they compile to .NET Standard 2.0 IL
  - Explicit `latest` needed for .NET Standard to use modern features
  - Compiler lowers many language features (records, pattern matching, etc.) independent of runtime
  - Modern C# features can be used as long as they compile to .NET Standard 2.0 IL
  - Compiler lowers many language features (records, pattern matching, etc.) independent of runtime

### Demo Projects
- **Current:** All target `net8.0` (or `net8.0-windows` for WPF apps)
  - `DiscogsApiClientDemo.AotConsole` - Console app with AOT publishing
  - `DiscogsApiClientDemo.OAuth` - WPF app demonstrating OAuth flow
  - `DiscogsApiClientDemo.PersonalAccessToken` - WPF app with personal access token
- **Target:** `net10.0` (or `net10.0-windows` for WPF apps)
- **C# Language Version:** SDK default (no explicit setting needed)
- **Rationale:** Demo projects should showcase the latest platform features

---

## Modernization Phases

```
Phase 1: Foundation (Frameworks & Packages)
   ├─ Low Risk, High Impact
   └─ Framework updates, NUnit→xUnit migration, package updates
         ↓
Phase 2: Testing Infrastructure
   ├─ Medium Risk, High Value
   └─ Enables safe refactoring, mock infrastructure
         ↓
Phase 3: Source Generator Modernization
   ├─ High Complexity
   └─ Enabled by fully modernized test suite
         ↓
Phase 4: Library Code Modernization
   ├─ Medium Risk
   └─ Protected by improved tests and modern generators
         ↓
Phase 5: Demo Projects
   ├─ Low Risk
   └─ Showcase modern features, .NET 10
         ↓
Phase 6: Final Validation & Documentation
   └─ Comprehensive verification, README/CHANGELOG/migration guide split
         ↓
Phase 7: CI/CD Automation
   ├─ Low Risk
   └─ PR-validation workflow (build, test, format, vulnerability check) + NuGet publish workflow
         ↓
Phase 8: Release Preparation & Merge to Main
   └─ Version bump to 5.0.0, PR to main, tag + publish v5.0.0
```

---

## Phase 1: Foundation - Framework & Package Updates

**Goal:** Update target frameworks, migrate from NUnit to xUnit, and update dependencies with minimal code changes to establish a stable foundation.

**Branch:** `modernization/phase1-frameworks`

**Note:** Source generator library (DiscogsApiClient.SourceGenerator) is intentionally NOT modified in this phase. It will be addressed in Phase 4 after testing is fully modernized.

### 1.1 DiscogsApiClient Project Updates
- [x] Update `<TargetFrameworks>` to `net8.0;net9.0;net10.0`
- [x] **DO NOT** update `AssemblyVersion`, `FileVersion`, or `PackageVersion` (version update happens with release to main)
- [x] Update package references to latest stable versions:
  - [x] `CommunityToolkit.Diagnostics` (currently 8.2.2) → 8.4.0
  - [x] `Microsoft.Extensions.Http` (currently 8.0.0) → 10.0.5
  - [x] `System.Threading.RateLimiting` (currently 8.0.0) → 10.0.5
- [x] Add `<AnalysisMode>All</AnalysisMode>` for enhanced static analysis (if not present)
- [x] Build and verify all target frameworks compile
- [x] Run existing tests to ensure no regressions - **Completed with section 1.2** ✅

### 1.2 DiscogsApiClient.Tests Project Updates
- [x] Update `<TargetFrameworks>` to `net8.0;net9.0;net10.0`
- [x] **Migrate from NUnit to TUnit:**
  - [x] Remove NUnit packages:
    - `NUnit` (3.14.0)
    - `NUnit3TestAdapter` (4.5.0)
  - [x] Add TUnit package: `TUnit` (1.19.74) - **NOTE**: Single package includes Core, Engine, and Assertions
  - [x] Remove incompatible packages:
    - `Microsoft.NET.Test.Sdk` - **Incompatible with TUnit**, uses native testing platform
    - `coverlet.collector` - Not needed, TUnit has built-in coverage support via `dotnet-coverage`
  - [x] Update `Microsoft.Extensions.Configuration.Json` to 10.0.5
- [x] **Convert test syntax (COMPLETE):**
  - [x] Replace `[OneTimeSetUp]`/`[OneTimeTearDown]` with `[Before(Class)]`/`[After(Class)]`
  - [x] Replace `[Test]` with `[Test]` (TUnit uses same attribute)
  - [x] Replace basic NUnit assertions with TUnit fluent assertions (`await Assert.That(x).IsEqualTo(y)`)
  - [x] Convert exception assertions: `Assert.ThrowsAsync<T>()` → `await Assert.That(() => ...).Throws<T>()`
  - [x] Fix comparison assertion parameter order (actual first, expected second)
  - [x] Handle nullable types with `.IsNotNull().And.` chaining for fluent assertions
  - [x] Converted all test fixture files:
    - [x] ApiBaseTestFixture, Authentication tests (3), RateLimitingTestFixture, MockMiddleware (2), User tests (2)
    - [x] Database tests (7 files): ArtistsTestFixture, LabelsTestFixture, MasterReleaseTestFixture, ReleasesTestFixture, SearchTestFixture
    - [x] Collection tests (3 files): WantlistTestFixture, CollectionFoldersTestFixture, CollectionFolderReleasesTestFixture
  - [x] Added `CancellationToken cancellationToken` parameter to all test methods
  - [x] Updated all ApiClient method calls to pass `cancellationToken` parameter
  - [x] Converted `[TestCase]` attributes to `[Arguments]` for parameterized tests
  - [x] Fixed all assertion parameter order issues (TUnit uses actual-first convention)
- [x] Build and verify all target frameworks compile
- [x] Run all converted tests to ensure they still pass - **ALL TESTS PASSING** ✅

**Rationale for TUnit:**
- **AOT Compatible**: Works seamlessly with Native AOT compilation (aligns with library's `IsAotCompatible` setting)
- **Source Generated**: Uses source generators for zero reflection, better performance
- **Modern Design**: Built with latest C# features and best practices
- **Better Integration**: Superior integration with modern .NET tooling
- **Native Testing Platform**: Uses .NET's native testing platform, eliminating need for `Microsoft.NET.Test.Sdk`

**Important TUnit Package Notes:**
- **Single Package Setup**: Only `TUnit` package is needed - it includes Core, Engine, and Assertions as dependencies
- **Incompatible Packages**: Remove `Microsoft.NET.Test.Sdk` and `coverlet.collector` (TUnit uses native testing platform)
- **IDE Support**: Works out-of-box with Visual Studio 2022 17.13+, requires settings for earlier versions
- **CLI Support**: Works with `dotnet test`, `dotnet run`, and direct execution
- **Coverage**: Use `dotnet-coverage` tool instead of coverlet for code coverage

**Note:** This step only migrates test framework syntax; mocking infrastructure comes in Phase 2

### 1.3 DiscogsApiClient.SourceGenerator Project Updates
- [x] **NOT MODIFIED** - Source generator updates happen in Phase 4 after testing is fully modernized
- [x] Verify it still compiles and generates code correctly after other project updates - **Verified working** ✅

### Acceptance Criteria - Phase 1
- [x] All projects compile without errors or warnings
- [x] DiscogsApiClient targets .NET 8, 9, 10
- [x] DiscogsApiClient.Tests targets .NET 8, 9, 10 and uses TUnit
- [x] DiscogsApiClient.SourceGenerator untouched (verified it still works)
- [x] All existing tests (now TUnit) pass on all target frameworks - **VERIFIED ✅**
- [x] Package versions are up to date
- [x] No functional changes to library behavior
- [x] Library version remains 4.1.0 (no version bump yet)

**Phase 1 Status:** ✅ **COMPLETE** - All acceptance criteria met, TUnit migration successful with all tests passing.

---

## Phase 2: Testing Infrastructure Modernization

**Goal:** Modernize the testing approach to enable safe refactoring in subsequent phases.

**Branch:** `modernization/phase2-testing`

### 2.1 Setup Mock Infrastructure
 - [x] Decide mocking approach: keep the repository's custom recording/playback fixtures as the primary source for deterministic test responses; a separate mock-data fixture set is not required.
 - [x] Design mock strategy for Discogs API responses — NOT REQUIRED (covered by recording/playback)
 - [x] Create mock data fixtures for common API responses — NOT REQUIRED (recordings cover representative responses)
   - [x] Authentication responses — covered by recordings
   - [x] Artist data — covered by recordings
   - [x] Release data — covered by recordings
   - [x] Label data — covered by recordings
   - [x] Search results — covered by recordings
   - [x] User collection/wantlist data — covered by recordings
   - [x] Error responses (rate limits, 404s, etc.) — covered by recordings
 - [x] Document mock approach in test documentation (`docs/response-recording.md`)

### Decision: Custom Recording/Playback Implementation
- Decision: Use a custom recording/playback implementation (RecordingFixture/PlaybackFixture) as the primary test response source.
- Rationale: The custom implementation provides deterministic, real-API-derived responses without requiring a separate external service. It simplifies maintenance because recordings are produced from real responses and can be sanitized automatically; it supports both integration-style tests (via recordings) and selective unit tests where hand-authored mocks are later required.
- Impact: WireMock.Net and an external server are not required. The test project removes the WireMock.Net package. CI must avoid running recording mode and should rely on playback artifacts present in the repository.

### 2.2 Refactor Existing Tests
- [x] Identify all tests that make real API calls (all tests run in playback mode by default; recording must be explicitly enabled)
 - [x] Refactor tests to use mocked HTTP responses — NOT REQUIRED for most tests because playback recordings provide deterministic responses; convert only where a fast unit test is needed.
 - [x] Ensure test isolation (no shared state between tests)
 - [x] Add test categories/traits (Unit, Integration, etc.) — NOT REQUIRED (requested by user)
 - [x] Update test naming conventions to modern standards
 - [x] Remove any hardcoded API tokens or credentials (test config file used, token not committed)

### 2.3 Improve Test Coverage
- [x] Run code coverage analysis (using built-in or Coverlet)
- [x] Identify untested or under-tested areas
- [x] Add tests for:
  - [x] Error handling paths
  - [x] Edge cases (null, empty, invalid inputs)
  - [x] Rate limiting behavior (ignored/deferred as requested by user)
  - [x] Authentication flows (DI setup/options validation covered)
  - [x] Serialization/deserialization
- [x] Target **reasonable coverage** for critical paths based on code complexity and risk

### 2.4 Test Performance & Organization
- [x] Organize tests into logical namespaces/folders (verified already correctly organized)
- [x] Add XML documentation to test classes (decided unnecessary; test names are self-explanatory)
- [x] Implement test fixtures and shared contexts where appropriate (already using standard TUnit fixtures)
- [x] Ensure tests run quickly (playback responses are fast)
- [x] Add integration test project if needed (decided not needed; playback integration tests in single project is sufficient)

### 2.5 Testing Best Practices
- [x] Follow AAA pattern (Arrange, Act, Assert) **without comments marking sections** (verified no explicit section comments are present)
- [x] Use TUnit's modern features (data-driven tests, fluent assertions)
- [x] **Use TUnit's fluent assertions** - built-in, no external assertion libraries needed
- [x] **Make test methods async** - avoid synchronous `.Result` or `.Wait()` calls
- [x] Ensure proper async/await usage throughout test code (completed comprehensive audit; all tests are fully async with cancellation token propagation)
- [x] Leverage TUnit's source generation for better performance and AOT compatibility

### 2.6 Optional: End-to-End Test Suite
- [x] **Evaluate need for E2E tests** against real Discogs API (decided to skip E2E tests against live API due to rate-limiting risks; playback recordings already serve as our integration test suite using real API payloads)
- [x] Document decision (implement or skip) and reasoning

### Acceptance Criteria - Phase 2
- [x] Playback-based testing available and documented. Tests run in playback mode by default unless recording is explicitly enabled (`DISCOGS_RECORD=true`).
- [x] Recording/playback is the primary mechanism for deterministic test responses (no separate mock data fixture required).
- [x] Test coverage is reasonable for critical paths. Targeted unit tests added for ServiceCollectionExtensions, custom exception types, and serialization enum JSON converters, all reaching 100% coverage.
- [x] Tests organized into logical folder namespaces.
- [x] Documentation for recording/playback is present (`docs/response-recording.md`).

---

## Phase 3: Source Generator Modernization

**Goal:** Update source generators to use latest Roslyn APIs and best practices.

**Branch:** `modernization/phase3-generators`

### 3.1 Source Generator Project Modernization
- [x] Review [Microsoft's source generator documentation](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview)
- [x] Update to latest Roslyn packages:
  - [x] `Microsoft.CodeAnalysis.CSharp` (from 4.8.0 to latest)
  - [x] `Microsoft.CodeAnalysis.Analyzers` (from 3.3.4 to latest)
- [x] Update `<LangVersion>latest</LangVersion>` (use all modern C# features compatible with .NET Standard 2.0)
- [x] Implement incremental generators (`IIncrementalGenerator`) and align with caching best practices
- [x] Use `IncrementalGeneratorInitializationContext` properly
- [x] Optimize for performance (caching, minimal re-generation)
- [x] **Implement comprehensive diagnostics:**
  - [x] Define diagnostic IDs (e.g., DISCOGS001, DISCOGS002)
  - [x] Create diagnostic descriptors with severity levels
  - [x] Add helpful error messages
  - [x] Report diagnostics for invalid input/configuration
  - [x] Provide code fix providers where appropriate (N/A / considered, none needed currently)
  - [x] Document all diagnostic IDs
- [x] Use `SourceProductionContext` for diagnostics

### 3.2 Source Generator Testing
- [x] Create test project for source generators (`DiscogsApiClient.SourceGenerator.Tests`)
- [x] Use `Microsoft.CodeAnalysis.CSharp.SourceGenerators.Testing` (or similar)
- [x] Add tests for:
  - [x] Successful generation scenarios
  - [x] Error handling (invalid input)
  - [x] Incremental generation behavior
  - [x] Diagnostic reporting
- [x] Add snapshot testing for generated output (verify stability) — N/A (decided to skip)
- [x] Document testing approach (covered by tests structure and comments)

### 3.3 Generator Best Practices
- [x] Ensure deterministic output (same input → same output)
- [x] Handle edge cases gracefully
- [x] Provide helpful diagnostics
- [x] Minimize dependencies in generator project
- [x] Add XML documentation to generator code

### Acceptance Criteria - Phase 3
- [x] Source generators use incremental generator API and follow caching best practices
- [x] Comprehensive generator tests implemented
- [x] All tests pass
- [x] Performance is acceptable (fast builds)

---

## Phase 4: Library Code Modernization — ✅ Completed

**Goal:** Modernize the main library code using latest C# features and best practices.

**Branch:** `modernization/phase4-library`

### 4.1 C# Language Feature Adoption
- [x] **File-scoped namespaces** - Convert to `namespace DiscogsApiClient;` (Audited: already implemented across all library files)
- [x] **Global usings** - Create `GlobalUsings.cs` for common imports (Audited: Usings.cs already contains global usings)
- [x] **Record types** - Use `record` for DTOs/contracts where appropriate (Audited: DTO contracts are already record types)
- [x] **Init-only properties** - Convert to `init` where mutability not needed (Audited: DTO records already use positional init-only properties. `DiscogsApiClientOptions` intentionally keeps `set` — see §4.7 — because `init` is incompatible with the `Action<TOptions>` options pattern)
- [x] **Pattern matching** - Modernize switch statements and conditionals (Audited: switch expressions already modern where present)
- [x] **Null-coalescing assignments** - Use `??=` where appropriate (Audited: no new opportunities found in the codebase)
- [x] **Target-typed new** - Use `new()` where type is obvious (Implemented in OAuth provider & ServiceCollectionExtensions; local variables prefer var + explicit new)
- [x] **Collection expressions** - Use `[...]` for arrays/collections (C# 12) (Audited: no collection instantiations found in library code)
- [x] **Primary constructors** - Consider for simple classes (C# 12) (Implemented in Auth service, OAuth provider, and delegating handlers)
- [x] **String interpolation** - Use `$"..."` over `string.Format` (Audited: string interpolation already preferred; no string.Format found)



### 4.2 IDiscogsApiClient Interface Refactoring — ✅ Completed

**Outcome:** Refactored. The endpoint definitions and guard-validating wrappers were moved off
the interface into a hand-written partial class, leaving `IDiscogsApiClient` as a pure public
contract. Done as a clean break (no obsolete/transition shims), which is acceptable because the
modernization ships as a new major version.

**Adopted design:**
- `IDiscogsApiClient` is a **clean public contract** — public method signatures with XML docs
  only. No HTTP attributes, no `internal` members, no default-interface-method bodies.
- `DiscogsApiClient` is an `internal sealed partial class` with a primary constructor
  (`HttpClient`, `DiscogsJsonSerializerContext`). It hosts:
  - public `partial` HTTP methods (e.g. `GetIdentity`) that need no argument validation,
  - `private partial *Internal` methods carrying the `[HttpGet/Post/Put/Delete]` attributes,
  - hand-written **public wrapper methods** with native guard clauses + `ConfigureAwait(false)`
    that delegate to the `*Internal` partials.
- The `[ApiClient]` attribute now targets a **class** (`AttributeTargets.Class`), and the
  source generator emits the second partial half (`Send`/`SendAsync`/route builders).
- DI registration resolves the interface: `AddHttpClient<IDiscogsApiClient, DiscogsApiClient>()`.
- See `docs/ARCHITECTURE.md` (Contract Interface + partial class + generated partial half).

**Breaking changes introduced by this refactor:**
- `CancellationToken` parameters are now **required** on `IDiscogsApiClient` methods — the
  previous `CancellationToken cancellationToken = default` default was removed.
- `[ApiClient]` moved from interfaces to classes; the previously `internal` endpoint methods
  are no longer part of the public interface surface.

**Checklist (all satisfied):**
- [x] Analyze current structure / document the internal-vs-public method pattern
- [x] Evaluate necessity and decide (decision: **refactor** — interface becomes a pure contract)
- [x] Design and implement the new structure (partial-class client + contract interface)
- [x] Update XML documentation (now lives on the interface)
- [x] Document breaking changes (see above)
- [x] **Note:** Breaking changes are acceptable as this ships as a new major version

### 4.3 Async/Await Modernization
- [x] Ensure `ConfigureAwait(false)` used appropriately (library code)
- [x] Use `ValueTask` where appropriate for hot paths (Audited: not recommended for these purely I/O bound methods as it introduces micro-pessimization and breaking changes for no gain)
- [x] Consider `IAsyncEnumerable` for paginated results (if applicable) (Audited: not applicable for library endpoints without pagination streaming changes)
- [x] Ensure cancellation tokens passed through properly (Audited: all async paths propagate CancellationToken correctly)


### 4.4 Rate Limiting
- Background: The repository contains an existing client-side rate limiting layer but it has been observed to behave unreliably in production-like scenarios.
- Goal: Either rework the rate limiting implementation to be reliable across target frameworks, or remove the built-in limiter and provide first-class access to Discogs rate-limit metadata so library users can implement their own strategies.
- Tasks:
  - [x] Audit current rate limiting implementation and reproduce failure modes in tests or a harness
  - [x] **Decision: Remove** (document choice and rationale)
  - ~~If Rework:~~
    - ~~Implement a robust, cross-target rate limiter (prefer `System.Threading.RateLimiting` primitives or a tested token-bucket implementation) integrated via an `HttpMessageHandler` or `DelegatingHandler`~~
    - ~~Add resiliency for clock skew and transient errors, and ensure behavior is deterministic under CI and AOT scenarios~~
    - ~~Add unit and integration tests that simulate high-concurrency scenarios and validate correctness~~
  - If Remove:
    - [x] Remove the built-in rate limiter implementation
    - [x] Add a public model to expose parsed Discogs rate-limit headers (for example `RateLimit`, `RateLimitRemaining`, `RateLimitReset`)
    - [x] Surface the parsed rate-limit metadata on responses or via a light-weight client API so consumers can implement custom policies
- Acceptance criteria:
  - [x] A decision is recorded (Rework or Remove) and implemented
  - ~~If reworked: limiter passes stress tests and is documented~~
  - [x] If removed: consumers have access to rate-limit metadata via `IDiscogsRateLimitStateService` (documentation deferred to Phase 6)

**Decision Rationale (Remove):**
- The existing sliding-window rate limiter was unreliable and did not align with Discogs' actual rate limiting methodology
- Feature was not widely used by library consumers and added maintenance burden
- Exposing raw rate-limit metadata provides maximum flexibility for consumers to implement their own strategies
- Reduces library complexity and eliminates the `System.Threading.RateLimiting` dependency

**Implementation Summary:**
- **Removed Components:**
  - `RateLimitedDelegatingHandler` (old sliding window implementation)
  - Rate limiting properties from `DiscogsApiClientOptions` (`UseRateLimiting`, `RateLimitingWindow`, `RateLimitingWindowSegments`, `RateLimitingPermits`, `RateLimitingQueueSize`)
  - `System.Threading.RateLimiting` NuGet package dependency

- **Added Components:**
  - `IDiscogsRateLimitStateService` - Public interface providing read-only access to current rate limit state
  - `IDiscogsRateLimitStateUpdateService` - Internal interface for updating state (not exposed to consumers)
  - `DiscogsRateLimitState` - Immutable record containing `Limit`, `Remaining`, and `Used` values
  - `DiscogsRateLimitStateService` - Thread-safe singleton service implementation using `Interlocked.Exchange`
  - `RateLimitStateDelegatingHandler` - Handler that extracts Discogs rate-limit headers (`x-discogs-ratelimit`, `x-discogs-ratelimit-remaining`, `x-discogs-ratelimit-used`) and updates the state service

- **Updated Components:**
  - `ServiceCollectionExtensions` - Registers rate limit state service as singleton (always enabled, not conditional)
  - Tests updated to verify `IDiscogsRateLimitStateService` registration
  - Demo projects updated to remove obsolete `UseRateLimiting` configuration

- **Design Decisions:**
  - Service is always registered and handler always runs (no opt-in/opt-out flag)
  - Both public and internal interfaces resolve to the same singleton instance
  - State is only updated when all three headers are present and parseable (all-or-nothing)
  - No circular dependencies: separate service prevents DI issues if consumers want to use it in custom handlers
  - Thread-safe using lock-free atomic reference exchange

### 4.5 Code Quality Improvements
- [x] Enable nullable reference types verification (Audited: already enabled globally in csproj)
- [x] Address all analyzer warnings (Resolved CA1002/CA1008/CA1031/CA1032/CA1054/CA1305/CA1724, plus a full IDE-diagnostics pass down to Info severity via `dotnet format --verify-no-changes --severity info` — clean fresh rebuild with 0 warnings)
- [x] **Migrate `Guard.*` calls to framework-native guard clauses** — replace CommunityToolkit `Guard.IsNotNull()`, `Guard.IsNotNullOrWhiteSpace()`, `Guard.IsGreaterThan()` with `ArgumentNullException.ThrowIfNull()`, `ArgumentException.ThrowIfNullOrWhiteSpace()`, `ArgumentOutOfRangeException.ThrowIfLessThanOrEqual()`, etc. to remove the CommunityToolkit dependency

- [x] Simplify complex methods (reduce cyclomatic complexity) (Audited: no methods exceed a reasonable complexity threshold; handlers/services are already small and single-purpose)
- [x] Extract magic strings/numbers to constants (Audited: rate-limit header names are already constants; endpoint route templates are single-use declarative `[HttpGet/...]` attribute arguments, not magic strings. The only repeated literals are OAuth protocol values confined to a single file that won't change — deliberately left inline. No extraction warranted)
- [x] Review and optimize LINQ usage (Audited: the only LINQ in the library is a single idiomatic `FirstOrDefault()` in `RateLimitStateDelegatingHandler`; nothing to simplify)
- [x] Ensure proper disposal patterns (`IDisposable`, `IAsyncDisposable`) (Audited: `HttpRequestMessage`/`HttpResponseMessage` are scoped with `using var`; `HttpClient`/handlers are `IHttpClientFactory`-managed; no library type owns unmanaged/disposable state requiring a custom `IDisposable`)

### 4.6 Performance Considerations
- [x] Review allocations (use `Span<T>`, `Memory<T>` where beneficial) (Audited: no allocation hot paths in library code; the generated route builder already precomputes `StringBuilder` capacity. `Span`/`Memory` offers no benefit for the remaining cold-path string work)
- [x] Optimize string operations (Audited: OAuth header building uses `+=` on a cold, per-auth-request path — negligible allocation impact; left readable one-field-per-line. No hot-path string concatenation found)
- [x] Review collection usage (use appropriate collection types) (Audited: contracts use `List<T>` for JSON deserialization as required by STJ; no inappropriate collection choices)
- [x] Consider `ArrayPool` for temporary buffers if applicable (Audited: N/A — no manual buffer management in library code; HTTP/JSON buffering is handled by the framework)

### 4.7 Service Registration Modernization — ✅ Completed

**Goal:** Refactor `ServiceCollectionExtensions` and `DiscogsApiClientOptions` to follow the patterns used by the .NET ecosystem's own libraries (e.g., `AddHttpClient`, `AddAuthentication`, `AddHealthChecks`).

> **Delivered design (some decisions changed from the original draft below after a dedicated
> research + assessment pass):**
> - **Return type:** kept `IServiceCollection` (HttpClient wiring stays encapsulated). **No**
>   `IDiscogsApiClientBuilder`. Customization is instead offered via an optional
>   `Action<IHttpClientBuilder>? configureClient` hook on every overload (user handlers sit outermost).
> - **Options mutability:** properties **stay `set` (mutable)**, **not** `init`. `init`-only is
>   incompatible with the `Action<TOptions>` configuration pattern / `OptionsBuilder.Configure(...)`,
>   which mutate an already-constructed instance — this matches every framework options class.
> - **OAuth options:** **not** extracted. `ConsumerKey` / `ConsumerSecret` / `VerifierCallbackUrl`
>   remain on `DiscogsApiClientOptions` (one options type, one validator).
> - **Validation:** uses a hand-written, AOT-safe **`IValidateOptions<DiscogsApiClientOptions>`**
>   (`DiscogsApiClientOptionsValidator`) instead of reflection-based `ValidateDataAnnotations()` — it
>   checks required fields, that URL values are constructable absolute URIs, and the all-or-nothing OAuth credential triple in one place.
> - **Overloads:** three — `Action<TOptions>`, `Action<IServiceProvider, TOptions>`, and
>   explicit `IConfiguration`. The delegate overloads bind the `"Discogs"` section first (only if an
>   `IConfiguration` is registered — silent no-op otherwise), then apply the delegate on top.
> - **Namespace:** moved to `Microsoft.Extensions.DependencyInjection` (discoverable without an extra `using`).
> - **Handler-order bug fixed:** effective pipeline is now `Error → Auth → RateLimit` (RateLimit
>   innermost) so rate-limit state updates on every response, including 429s, before Error throws.
> - **Idempotency:** all infrastructure services use `TryAdd*`.
> - Section name exposed as `DiscogsApiClientOptions.SectionName = "Discogs"`.
>
> **Breaking changes for callers:**
> - The extension namespace moved to `Microsoft.Extensions.DependencyInjection`; a `using DiscogsApiClient;`
>   that only existed for the registration call can be removed.
> - Invalid options now surface as an `OptionsValidationException` at startup (`ValidateOnStart`) / on
>   first `IOptions<T>.Value` access, replacing the old eager `InvalidOperationException` at registration time.
> - `OAuthAuthenticationProvider`'s constructor now takes `IOptions<DiscogsApiClientOptions>`.
>
> **README refresh reminder:** when the final versioned release docs are written, refresh the README's
> registration + configuration examples (including the `"Discogs"` `appsettings.json` section) and add a
> changelog entry covering the breaking changes above.

**Current problems with `ServiceCollectionExtensions.cs` and `DiscogsApiClientOptions.cs`:**
- `DiscogsApiClientOptions` is instantiated eagerly inside the extension method and registered as a raw singleton — bypassing `IOptions<T>` entirely
- Validation (null/empty checks) throws `InvalidOperationException` at registration time instead of at startup via `IValidateOptions<T>` / `ValidateOnStart()`
- No `IConfiguration` overload — callers cannot bind options from `appsettings.json`
- `AddDiscogsApiClient` returns `IServiceCollection`; there is no builder for fluent post-registration customization
- OAuth credentials (`ConsumerKey`, `ConsumerSecret`, `VerifierCallbackUrl`) are mixed into the top-level options class alongside unrelated settings
- `DiscogsApiClientOptions` properties are mutable (`set`); they should use `init` to prevent post-construction mutation

> **Note:** The client-side rate limiter was removed in §4.4 and replaced by the read-only
> `IDiscogsRateLimitStateService` (registered as a singleton, always enabled). That service is
> already wired up in `AddDiscogsApiClient` and is **out of scope** for this options refactor —
> there are no rate-limiting options to register or validate.

**Reference patterns to follow:**
- `Microsoft.Extensions.Http` (`AddHttpClient`) — returns `IHttpClientBuilder` for chaining
- `Microsoft.AspNetCore.Authentication` (`AddAuthentication`) — returns a builder for scheme registration
- `Microsoft.Extensions.Diagnostics.HealthChecks` (`AddHealthChecks`) — returns `IHealthChecksBuilder`
- Options validation via `OptionsBuilder<T>.Validate()` / `ValidateDataAnnotations()` / `ValidateOnStart()`

**Tasks:**

#### 4.7.1 Refactor `DiscogsApiClientOptions`
- [-] ~~Convert all property setters to `init`-only~~ — **not done**: `init` is incompatible with the `Action<TOptions>` options pattern; properties stay `set` (matches framework options classes)
- [-] ~~Add `[Required]` / `[Url]` data annotations~~ — **not done**: replaced with a hand-written validator; the options type carries no data annotations (validation logic lives in one place)
- [-] ~~Extract OAuth-specific settings into a dedicated `DiscogsOAuthOptions`~~ — **not done**: credentials stay on `DiscogsApiClientOptions` (one options type, one validator)
- [x] Add `public const string SectionName = "Discogs";` and refresh XML documentation

#### 4.7.2 ~~Introduce `IDiscogsApiClientBuilder`~~ — superseded
- [-] Superseded: `AddDiscogsApiClient` keeps returning `IServiceCollection`; per-call customization is offered via an optional `Action<IHttpClientBuilder>? configureClient` hook instead of a custom builder

#### 4.7.3 Adopt `IOptions<T>` and Proper Options Registration
- [x] Replaced raw singleton with `services.AddOptions<DiscogsApiClientOptions>()` + `.Configure(...)` / `.Bind(...)`
- [x] Inject `IOptions<DiscogsApiClientOptions>` into the `HttpClient` configuration delegate and `OAuthAuthenticationProvider`
- [-] ~~Register `DiscogsOAuthOptions`~~ — N/A (not extracted)
- [x] Removed all eager validation from the extension method body

#### 4.7.4 Add Options Validation
- [x] Validation via a hand-written, AOT-safe **`IValidateOptions<DiscogsApiClientOptions>`** (`DiscogsApiClientOptionsValidator`) instead of reflection-based `.ValidateDataAnnotations()`; enforces required fields, constructable absolute URI URLs, and the all-or-nothing OAuth credential triple (`ConsumerKey`, `ConsumerSecret`, `VerifierCallbackUrl`)
- [x] Validator registered with `services.AddSingleton<IValidateOptions<T>, DiscogsApiClientOptionsValidator>()`
- [x] Chained `.ValidateOnStart()` on the options builder
- [x] Added tests verifying validation throws (`OptionsValidationException`) for invalid configurations

#### 4.7.5 Add `IConfiguration` Overload
- [x] Added `AddDiscogsApiClient(this IServiceCollection services, IConfiguration configuration, ...)` binding the passed config directly
- [x] Documented the `"Discogs"` section convention via `DiscogsApiClientOptions.SectionName` (in XML docs and ARCHITECTURE; consumer-facing README deferred to §6.2)
- [x] Added tests for the `IConfiguration`-based overload

#### 4.7.6 Update Tests for New Registration API
- [x] Tests verifying required services are registered and are idempotent (`TryAdd*`)
- [x] Tests verifying `IDiscogsApiClient` / `IDiscogsAuthenticationService` resolve from the container
- [x] Tests verifying options validation fires for invalid configs
- [x] Handler-order test (rate-limit state updates on a 429 response before Error throws)
- [x] Updated existing tests / `DiscogsApiClientFixture` for the new signature and namespace

**Delivered API shape:**
```csharp
// Minimal registration (code-based)
services.AddDiscogsApiClient(options =>
{
    options.UserAgent = "MyApp/1.0";
});

// DI-aware configuration
services.AddDiscogsApiClient((serviceProvider, options) =>
{
    options.UserAgent = serviceProvider.GetRequiredService<IAppInfo>().UserAgent;
});

// With IConfiguration binding (appsettings.json "Discogs" section)
services.AddDiscogsApiClient(configuration.GetSection(DiscogsApiClientOptions.SectionName));

// Any overload can customize the underlying HttpClient (added handlers wrap the built-in pipeline)
services.AddDiscogsApiClient(
    options => options.UserAgent = "MyApp/1.0",
    client => client.AddHttpMessageHandler<MyLoggingHandler>());
```

### 4.8 Generated Code Modernization — ✅ Completed

Delivered by the generator modernization commit ("refactor(generator): modernize
emitted code across all generators") and the generated-code marking work that added the
`// <auto-generated/>` header and `[GeneratedCode]` attribute.

- [x] Generated code uses modern C# features (compatible with .NET 8+):
  - [x] File-scoped namespaces (generator emits `namespace X;`)
  - [-] Target-typed new expressions (N/A — generated locals use `var x = new global::Type(...)`; target-typed `new` cannot apply with `var`)
  - [x] Pattern matching where appropriate (`switch` expressions in the generator; `is not null` in emitted bodies)
  - [-] Collection expressions (N/A — no arrays/collections are emitted)
- [x] Ensure generated code is AOT-compatible (STJ source-generated `JsonTypeInfo`, `HttpClient.Send/SendAsync`, no reflection)
- [x] Mark generated code as generated:
  - [x] `// <auto-generated/>` header emitted at the top of every generated file — the reliable trigger for analyzer/coverage generated-code suppression
  - [x] `[global::System.CodeDom.Compiler.GeneratedCodeAttribute("DiscogsApiClient.SourceGenerator", "1.0.0")]` emitted per generated member and fully-generated helper/marker type (following the dotnet/runtime generator convention; not placed on the user's co-owned partial class declaration to avoid coverage-tool bleed onto hand-written members)
  - [x] Tool name/version sourced from `Constants.ToolName` / `Constants.ToolVersion` (fixed `1.0.0` — the generator is not independently versioned; bump the constant if that changes)
  - [-] `[ExcludeFromCodeCoverage]` / `[EditorBrowsable(Never)]` (N/A — the `// <auto-generated/>` header already covers coverage/analyzer suppression; not adopted to keep the emitted surface minimal)
- [x] Add `#nullable enable` to generated files (emitted at the top of every generated file)
- [x] Optimize generated code (capacity-precomputed `StringBuilder` route building; reduced allocations)

### 4.9 Authentication Setup Modernization — ✅ Done

Reassessed how the two authentication flows (Personal Access Token and OAuth 1.0a) are modeled,
registered, resolved and activated. The trigger was that a consumer previously **could not
pre-authenticate at service registration** — tokens were runtime-only state that had to be pushed
in imperatively after the container was built.

**Previous-state analysis (as-was):**
- `IDiscogsAuthenticationService` was a mutable, stateful **singleton facade** over both mechanisms,
  whose credentials were set imperatively *after* the container was built:
  `AuthenticateWithPersonalAccessToken(token)`, `AuthenticateWithOAuth(accessToken, accessTokenSecret)`,
  or the interactive `StartOAuthAuthentication`/`CompleteOAuthAuthentication` pair.
- The actual secrets lived in provider fields (`PersonalAccessTokenAuthenticationProvider._userToken`,
  `OAuthAuthenticationProvider._accessToken`/`_accessTokenSecret`) — **not** in
  `DiscogsApiClientOptions`. `DiscogsApiClientOptions` only carried the OAuth *consumer* key/secret
  and callback URL (the app identity), never the *user* access tokens.
- Consequence: there was **no way to supply a PAT through options or `IConfiguration`**, so the
  client couldn't be usable immediately after `AddDiscogsApiClient(...)`; both providers were also
  always registered regardless of which (if either) mechanism a consumer actually wanted, allowing
  "half-configured"/conflicting auth state.
- Thread-safety / correctness smells on the singleton: mutable "last authenticated wins" flags
  selected which header to emit, mutated without synchronization and shared across all requests.
- The mutating `Authenticate*` methods were part of the **public** facade contract, exposing token
  mutation to consumers even when they only wanted config-driven, immutable credentials.

**Chosen direction (implemented):** authentication is now an **explicit, single choice made at DI
registration time**, mirroring `AddAuthentication().AddJwtBearer(...)`-style ecosystem patterns,
rather than the originally-sketched "seed everything through one shared options surface" idea below.

- `AddDiscogsApiClient(...)` alone registers **no** auth mechanism — requests are sent unauthenticated
  by default (valid Discogs usage for public endpoints), with no exception thrown for that state.
- `.WithPatAuthentication(Action<DiscogsPatOptions>? configure = null)` opts into the Personal Access
  Token mechanism. `DiscogsPatOptions.Token` is bindable from `IConfiguration` (`"Discogs:Pat"`) —
  a PAT is a single static app-level secret, so config-binding it is appropriate. Supplying it via
  options/config means the resolved `IDiscogsPatAuthenticationProvider` is **already authenticated**
  immediately after the container is built, with no imperative call required; an interactive
  `Authenticate(token)` call remains available for apps that collect the token at runtime instead.
- `.WithOAuthAuthentication(Action<DiscogsOAuthOptions>? configure = null)` opts into the OAuth 1.0a
  mechanism. `DiscogsOAuthOptions` carries only the static app-identity secrets (`ConsumerKey`,
  `ConsumerSecret`, `VerifierCallbackUrl`) — **not** user access tokens. The OAuth user access
  token/secret are dynamic, per-user values obtained (and cached by the consuming application) at
  runtime through the interactive `StartAuthentication`/`CompleteAuthentication` flow, or supplied
  via a direct `Authenticate(accessToken, accessTokenSecret)` call when an app has cached tokens from
  a previous run — this remains a **code/runtime** action, not static configuration (decided against
  adding a pluggable token-store abstraction: DI-registration time can't do async retrieval anyway,
  and multi-tenant apps would need their own per-tenant caching layer regardless).
- Calling **both** `.WithPatAuthentication()` and `.WithOAuthAuthentication()` on the same registration
  throws `InvalidOperationException` at registration time (fail-fast, not "last wins").
- The facade `IDiscogsAuthenticationService`/`DiscogsAuthenticationService` was **removed**.
  Consumers who need the mechanism-specific API resolve the concrete provider interface they
  registered (`IDiscogsPatAuthenticationProvider`/`IDiscogsOAuthAuthenticationProvider`) directly —
  these were promoted from implementation details to polished, customer-facing public API with a
  `Discogs`-prefixed rename pass (`IPersonalAccessTokenAuthenticationProvider` →
  `IDiscogsPatAuthenticationProvider`, `IOAuthAuthenticationProvider` →
  `IDiscogsOAuthAuthenticationProvider`), matching the existing `Discogs`-prefix convention
  (`DiscogsApiClientOptions`, `DiscogsRateLimitState*`).
- An internal `IDiscogsAuthenticationHeaderProvider` contract (single implementation,
  `DiscogsAuthenticationHeaderProvider`) composes two optional nullable provider dependencies
  (`IDiscogsPatAuthenticationProvider?`, `IDiscogsOAuthAuthenticationProvider?`) — the built-in DI
  container supplies `null` for an unregistered service, so the default "unauthenticated" state falls
  out naturally with no sentinel/no-op type needed. `AuthenticationDelegatingHandler` depends only on
  this internal contract and is itself now `internal`.
- Both providers hold their token state in an immutable snapshot swapped via
  `Interlocked.Exchange`/`Volatile.Read`, mirroring the existing `DiscogsRateLimitStateService`
  pattern — no more unsynchronized flags.
- Fixed a lifetime bug surfaced by DI tests during implementation: `DiscogsOAuthAuthenticationProvider`
  holds mutable token state and must be a **singleton** so the instance a consumer authenticates is
  the same instance the header provider reads from; `AddHttpClient<TClient, TImplementation>()`
  registers the typed client as **transient**, so its `HttpClient` is instead created via a named
  client (`IHttpClientFactory`) and the provider registered as a manual singleton factory.

**Tests:** provider unit tests updated for the renamed types/thread-safe implementation (including a
new "authenticated from options at registration" PAT test); DI/registration tests cover
`.WithPatAuthentication()`, `.WithOAuthAuthentication()`, the mutual-exclusivity conflict, and the
unauthenticated-default state; a new `DiscogsAuthenticationHeaderProviderTests` covers the internal
composition logic directly. All 257 tests pass.

**Demos updated:** `DiscogsApiClientDemo.PersonalAccessToken`, `DiscogsApiClientDemo.OAuth`, and
`DiscogsApiClientDemo.AotConsole` all updated to the new `.With*Authentication()` registration +
provider-resolution model.


- [x] All C# 12 features adopted where appropriate (§4.1: file-scoped namespaces, records, init-only properties, pattern matching, target-typed `new`, primary constructors, collection expressions, string interpolation — all audited/implemented)
- [x] IDiscogsApiClient interface refactored and simplified — now a pure contract; endpoints + guards moved to the `DiscogsApiClient` partial class
- [x] Service registration follows modern `IOptions<T>` patterns (`AddOptions<T>`, `IOptions<T>`, `ValidateOnStart`)
- [-] ~~`AddDiscogsApiClient` returns `IDiscogsApiClientBuilder`~~ — superseded: returns `IServiceCollection` + optional `Action<IHttpClientBuilder>` hook
- [x] `IConfiguration` overload available for binding from `appsettings.json`
- [x] Options validation uses a hand-written AOT-safe `IValidateOptions<T>` and `ValidateOnStart()`
- [x] Authentication setup reviewed (§4.9): pre-authentication supported at registration (PAT + existing OAuth tokens) and shared mutable auth state reassessed
- [x] Generated code uses modern C# features (file-scoped ns, `#nullable enable`, pattern matching, AOT-safe, `// <auto-generated/>` header + `[GeneratedCode]` attribute)
- [x] Generated code is well-documented (XML docs live on the hand-written public interfaces per the "public API surface only" doc convention; generated members intentionally carry none)
- [x] All tests pass (262 DiscogsApiClient.Tests + 83 DiscogsApiClient.SourceGenerator.Tests, all passing on a clean rebuild)
- [x] No compiler warnings (fresh `-t:Rebuild` with `EnforceCodeStyleInBuild=true`: 0 warnings, 0 errors)
- [x] XML documentation complete and accurate (public library interfaces — `IDiscogsApiClient`, provider interfaces, options types — carry XML docs; internal/private members intentionally do not, per style convention)
- [x] Breaking changes documented (if any) (documented inline in §4.2, §4.7, §4.9)

---

## Phase 5: Demo Projects Modernization — ✅ Completed

**Goal:** Update demo projects to .NET 10 and showcase modern features.

**Branch:** `modernization/phase5-demos`

### 5.1 DiscogsApiClientDemo.AotConsole Project Updates
- [x] Update `<TargetFramework>` from `net8.0` to `net10.0`
- [x] Set `<LangVersion>latest</LangVersion>` (C# 14) — **skipped**: minimal demo project, not worth pinning explicitly
- [x] Update package references to latest versions (currently none beyond project reference)
- [x] Verify `PublishAot` and `InvariantGlobalization` settings still appropriate
- [x] Build and verify compilation
- [x] Test AOT publish works correctly
- [x] Update code to use C# 14 features where appropriate

### 5.2 DiscogsApiClientDemo.OAuth Project Updates
- [x] Update `<TargetFramework>` from `net8.0-windows` to `net10.0-windows`
- [x] Set `<LangVersion>latest</LangVersion>` (C# 14) — **skipped**: minimal demo project, not worth pinning explicitly
- [x] Update package references to latest versions:
  - [x] `CommunityToolkit.Mvvm` (8.2.2 → 8.4.2)
  - [x] `Microsoft.Extensions.Hosting` (8.0.0-rc.2.23479.6 → 10.0.11)
  - [x] `Microsoft.Web.WebView2` (1.0.2088.41 → 1.0.4129.50)
  - [x] **Remove** old `DiscogsApiClient` package reference (2.0.0) - uses project reference
- [x] Build and verify compilation
- [x] Test OAuth flow demonstration works correctly — build-only verification; live OAuth login requires real Discogs consumer key/secret, not available in this environment
- [x] Update code to use C# 14 features where appropriate

### 5.3 DiscogsApiClientDemo.PersonalAccessToken Project Updates
- [x] Update `<TargetFramework>` from `net8.0-windows` to `net10.0-windows`
- [x] Set `<LangVersion>latest</LangVersion>` (C# 14) — **skipped**: minimal demo project, not worth pinning explicitly
- [x] Update package references to latest versions:
  - [x] `CommunityToolkit.Mvvm` (8.2.2 → 8.4.2)
  - [x] `Microsoft.Extensions.Hosting` (8.0.0-rc.2.23479.6 → 10.0.11)
  - [x] **Remove** old `DiscogsApiClient` package reference (2.0.0) - uses project reference
- [x] Build and verify compilation
- [x] Test personal access token demonstration works correctly — build-only verification; live run requires a real Discogs PAT, not available in this environment
- [x] Update code to use C# 14 features where appropriate

### 5.4 Demo Code Modernization
- [x] Use modern C# 14 features in demo code where it adds value — enabled `ImplicitUsings` on both WPF demos and trimmed now-redundant `using` directives; converted `[ObservableProperty]`-backed fields to partial auto-properties (current CommunityToolkit.Mvvm recommended pattern, MVVMTK0042); converted host-builder configuration lambdas to expression-bodied form (IDE0053)
- [x] Update to show latest library API patterns — no library API surface changed in this phase, demo usage already matches current `IDiscogsApiClient` contract
- [x] Add examples of new features (if any from modernization) — none introduced by Phases 1-4 that change demo-visible API shape
- [x] Ensure async/await patterns are exemplary — reviewed; already correct (`async Task`/`async void` only at UI event-handler boundaries, `CancellationToken` threaded through)
- [x] Add comments explaining modern patterns for educational value — existing inline comments retained; no additional comments needed since changes are mechanical framework/package/style updates, not new concepts

### 5.5 Demo Documentation
- [x] Update README files in demo projects (if they exist) — **N/A**: no README files exist in any of the three demo projects
- [x] Ensure demos compile and run successfully — all three build cleanly; OAuth/PAT UI flows can't be smoke-tested live without real Discogs credentials (build-only verification for those two)
- [x] Add setup instructions if needed — not needed; no new setup steps introduced
- [x] Document any changes from previous version — captured in this checklist and the PR description

### Acceptance Criteria - Phase 5
- [x] All demo projects target .NET 10 (or .NET 10 Windows)
- [x] Demo code uses modern C# 14 features appropriately
- [x] Demos compile and run successfully
- [x] Documentation is clear and helpful
- [x] Old package references cleaned up

---

## Phase 6: Final Validation & Documentation

**Goal:** Comprehensive validation and documentation rework before Phase 7 (CI) and Phase 8 (release).

**Branch:** `modernization` (final validation)

### 6.1 Comprehensive Testing
- [x] Run full test suite on all target frameworks (.NET 8, 9, 10) — 262/262 passed on net8.0/net9.0/net10.0; SourceGenerator tests 83/83 on net10.0
- [x] Perform manual testing of key scenarios — user manually tested the demo projects (PAT, OAuth, AOT console) with real Discogs credentials in Visual Studio. **Found and fixed a real bug in the process:** `demo/DiscogsApiClientDemo.slnx` didn't include `DiscogsApiClient.SourceGenerator.csproj`, so Visual Studio failed to build `DiscogsApiClient.dll` (the generator wasn't built/available as an analyzer in the VS session). Root cause pre-dates this modernization branch — the original `.sln` (before the Phase 4 `.slnx` migration) never referenced the generator project either; `dotnet build`/CLI never surfaced it because `DiscogsApiClient.csproj` already has its own `Analyzer`-only `ProjectReference` to the generator, which MSBuild resolves independent of solution membership — this is a Visual Studio-specific out-of-solution-reference quirk. Fixed by adding the generator project to the demo `.slnx`. All demo scenarios passed after the fix.
- [x] Run performance benchmarks — N/A, no BenchmarkDotNet project exists in the repo; out of scope for this modernization
- [x] Test NuGet package generation — `dotnet pack` (Release) succeeds; verified the produced `.nupkg` contains `lib/net8.0`, `lib/net9.0`, `lib/net10.0` assemblies, the embedded `README.md`, and a valid `.nuspec`. Also verified end-to-end integration: packed into a local folder feed, consumed via `PackageReference` (not a project reference) from a scratch console app, called `AddDiscogsApiClient(...).WithPatAuthentication(...)`, resolved `IDiscogsApiClient` via DI, and ran successfully. All done locally with scratch temp directories — nothing published to nuget.org, no repo files touched.
- [x] Test in a clean environment (fresh clone) — cloned the branch into a fresh temp directory and ran a full build + test cycle (net10.0: 262/262 passed). **Finding:** on default Windows `git` settings (`core.longpaths` not enabled), the clone failed with "Filename too long" due to deeply-nested test recording fixture filenames (~241+ chars); succeeded once `core.longpaths=true` was set for the clone. Affects only contributors building from source (not NuGet consumers). Decision: deferred — revisit only if Phase 7 CI surfaces the same failure.

### 6.2 Documentation Rework

**Consumer-facing docs are intentionally deferred to this phase.** Throughout the modernization,
internal/dev docs (`ARCHITECTURE.md`, this plan) are kept current, but consumer-facing docs (README,
changelog) are **not** touched until the branch is being prepared for publishing — updating them
prematurely would mislead 4.x users reading `main`. This is the single consolidated docs-rework step.

**Doc split:** The current README bundles getting-started content, an inline version-by-version
changelog, and scattered breaking-change callouts into one long file. This phase splits it into
focused, purpose-specific documents:

| Doc | Purpose | Audience |
|---|---|---|
| `README.md` | Quick intro, authentication overview, getting-started code samples, links out to the docs below | New/casual consumers |
| `docs/CHANGELOG.md` | Full version history (Keep a Changelog style); each entry lists its own breaking changes inline, including the new v5.0.0 entry | Consumers checking what changed |
| `docs/MIGRATION_GUIDE.md` | Consolidated breaking-change/migration guide with before/after code samples, covering the v5.0.0 changes and any still-relevant migration steps carried forward from prior breaking releases (3.0.0, 4.0.0, 4.1.0, etc.) | Consumers upgrading from any older version |
| `docs/API_COVERAGE.md` *(existing)* | Detailed endpoint-by-endpoint implementation status | Consumers checking API surface coverage |

- [ ] **Rework the README:**
  - [ ] Trim to: intro/disclaimer, authentication overview, getting-started samples (refreshed against the modernized API)
  - [ ] Service registration: the three `AddDiscogsApiClient` overloads (`Action<TOptions>`,
        `Action<IServiceProvider, TOptions>`, `IConfiguration`) + the optional `Action<IHttpClientBuilder>` hook
  - [ ] Configuration via `appsettings.json` (the `"Discogs"` section / `DiscogsApiClientOptions.SectionName`)
        and startup validation surfacing as `OptionsValidationException`
  - [ ] Rate limiting removal and the new `IDiscogsRateLimitStateService`
  - [ ] Remove the inline "Changelog" section, the "Implemented Api Functions" list, and the "Roadmap" section entirely
  - [ ] Add a short "Documentation" section linking to `docs/CHANGELOG.md`, `docs/MIGRATION_GUIDE.md`,
        and `docs/API_COVERAGE.md` so the extracted docs stay discoverable
- [ ] **Extract and rework `docs/CHANGELOG.md`** — move the existing README version history here,
      keep it in Keep a Changelog-style format, with each version's own **Breaking** subsection called out
      inline (not just the latest release), and add the v5.0.0 entry consolidating every phase's changes
- [ ] **Consolidate all v5.0.0 breaking changes** into a single, authoritative list (namespace move,
      `OptionsValidationException` at startup, `OAuthAuthenticationProvider` ctor takes `IOptions<T>`,
      rate-limiting removal, `IDiscogsApiClient` interface changes)
- [ ] Create `docs/MIGRATION_GUIDE.md` covering breaking changes and migration steps
  - [ ] Document rate limiting removal and new `IDiscogsRateLimitStateService` usage with examples
  - [ ] Document the service-registration / options changes with before/after examples
  - [ ] Include code samples showing how consumers can implement custom rate limiting if needed
  - [ ] Review the old README changelog entries for prior breaking releases (3.0.0, 4.0.0, 4.1.0) and
        carry forward any still-relevant migration steps/before-after samples (e.g. Refit → source
        generator, OAuth flow call changes, renamed properties) so the guide is a single place consumers
        jumping from any older 3.x/4.x version can follow, not just the v4.x → v5.0.0 delta
- [ ] Update `docs/ARCHITECTURE.md` with any remaining architectural changes (if applicable)
- [ ] Update this modernization plan status to completed

### 6.3 Code Quality Gates
- [x] Zero compiler warnings
- [x] Zero analyzer warnings
- [ ] Full IDE/style diagnostics pass (`dotnet format --verify-no-changes --severity info`) is clean —
      **known false positives deferred:** 67x `IDE0060` on `DiscogsApiClient.cs`'s generator-implemented
      `private partial` method declarations, and 9x `IDE0005` on files using `[GenerateJsonConverter]`/
      `[AliasAs(...)]` (both confirmed by revert-and-rebuild testing to be real usages the analyzer can't
      see across generated code). Deferred; revisit if Phase 7 CI surfaces the same findings.
- [ ] Code coverage reports generated
- [ ] Static analysis passes
- [x] NuGet package builds successfully (even if not published yet) — see 6.1

### 6.4 Final Review
- [ ] Review all merged PRs
- [ ] Verify branching strategy was followed
- [ ] Check for any missed tasks

### Acceptance Criteria - Phase 6
- [ ] All tests pass on all frameworks
- [ ] README, CHANGELOG, and migration guide split completed and cross-linked
- [ ] Migration guide created
- [ ] No warnings or errors

---

## Phase 7: CI/CD Automation

**Goal:** Add two GitHub Actions workflows: (1) a PR-validation workflow that enforces the project's
quality bar (build, tests, style/diagnostics, dependency security) on every pull request, and (2) a
manually-triggered NuGet publish workflow, with a switch to redirect a publish to the NuGet test
server (`int.nugettest.org`) instead of production `nuget.org` for dry-run validation. The actual
v5.0.0 release publish itself happens in Phase 8, using the workflow built here.

**Branch:** `modernization/phase7-ci-automation`

### 7.1 PR Validation Workflow
- [ ] Trigger: `pull_request` (targeting `main` and/or `modernization`)
- [ ] Restore and build (all target frameworks) as a gate
- [ ] Run the full test suite (all target frameworks)
- [ ] Diagnostics/style check: `dotnet format <solution> --verify-no-changes --severity info` (mirrors the
      AGENTS.md "IDE/style diagnostics pass"), failing the check on any violation
- [ ] NuGet dependency/vulnerability check: `dotnet list package --vulnerable --include-transitive`
      (or `nuget-fix_vulnerable_packages`-equivalent CLI check), failing the workflow if any vulnerable
      package is found
- [ ] Run against both `src/DiscogsApiClient.slnx` and `demo/DiscogsApiClientDemo.slnx`
- [ ] Workflow file: e.g. `.github/workflows/pr-validation.yml`

### 7.2 NuGet Publish Workflow Design
- [ ] Trigger: `workflow_dispatch` only (no tag-push or branch-push trigger) — every release is a deliberate, manual action
- [ ] Manual input: boolean switch (e.g. `publish_to_test_server`, default `false`) — when `true`, the publish step targets `int.nugettest.org` instead of `nuget.org`
- [ ] Restore, build (Release configuration), and run the full test suite (all target frameworks) as a gate before packing
- [ ] `dotnet pack` only the `DiscogsApiClient` library project (not test/demo projects) in Release configuration, producing `.nupkg` + `.snupkg` (symbol package)
- [ ] Upload the produced package(s) as a workflow artifact regardless of publish outcome (so a failed/aborted publish still leaves the built package retrievable)
- [ ] Push the package via `dotnet nuget push`:
  - [ ] Default (`publish_to_test_server = false`): push to `https://api.nuget.org/v3/index.json` using the `NUGET_API_KEY` secret
  - [ ] Test mode (`publish_to_test_server = true`): push to `https://apiint.nugettest.org/v3/index.json` using the `NUGET_TEST_API_KEY` secret
- [ ] Workflow file: e.g. `.github/workflows/publish-nuget.yml`

### 7.3 Versioning
- [ ] Package version is read as-is from the existing `<PackageVersion>`/`<AssemblyVersion>`/`<FileVersion>` properties already hand-maintained in `DiscogsApiClient.csproj` — no automated version-bump tooling introduced in this phase
- [ ] Workflow surfaces the resolved version in its run summary/logs so the person triggering it can confirm what will be published before approving

### 7.4 Secrets
- [ ] `NUGET_API_KEY` — nuget.org API key, repository secret
- [ ] `NUGET_TEST_API_KEY` — separate API key for `int.nugettest.org` (distinct account/service from nuget.org), repository secret
- [ ] No GitHub Environment protection/required-reviewer gate — single-maintainer project, the manual `workflow_dispatch` + explicit boolean switch is a sufficient safeguard

### 7.5 Documentation
- [ ] Document both workflows (what the PR-validation workflow enforces; how to trigger the release workflow, what the test-server switch does, where to find the resulting package artifact) in `docs/` or the README

### Acceptance Criteria - Phase 7
- [ ] PR-validation workflow file exists and runs build + tests + format check + vulnerability check on every PR
- [ ] Publish workflow file exists (e.g. `.github/workflows/publish-nuget.yml`)
- [ ] Publish workflow builds, tests, and packs the library before any publish step runs
- [ ] Publish workflow can be manually triggered with the test-server switch, defaulting to nuget.org
- [ ] At least one successful end-to-end test-server publish validated
- [ ] Both API key secrets configured in repository settings
- [ ] Both workflows documented for future use

---

## Phase 8: Release Preparation & Merge to Main

**Goal:** Bump the version, finalize the PR from `modernization` to `main`, and execute the actual
v5.0.0 NuGet release using the Phase 7 publish workflow.

**Branch:** `modernization` (final)

### 8.1 Version Bump
- [ ] Bump `<PackageVersion>`/`<AssemblyVersion>`/`<FileVersion>` in `DiscogsApiClient.csproj` to `5.0.0`
- [ ] Confirm `docs/CHANGELOG.md`'s v5.0.0 entry and release date are up to date

### 8.2 Merge to Main
- [ ] Create PR: `modernization` → `main`
- [ ] Comprehensive PR description with summary of all changes across all phases
- [ ] List all breaking changes (link to `docs/MIGRATION_GUIDE.md`)
- [ ] Final review and approval
- [ ] Merge to main

### 8.3 Release
- [ ] Tag release `v5.0.0` on `main`
- [ ] Trigger the Phase 7 publish workflow (`publish_to_test_server = false`) to push v5.0.0 to `nuget.org`
- [ ] Create a GitHub release with notes sourced from `docs/CHANGELOG.md`

### Acceptance Criteria - Phase 8
- [ ] Version bumped to 5.0.0 and merged to `main`
- [ ] `v5.0.0` tag and GitHub release created
- [ ] Package published to `nuget.org` and installable

---

## Progress Tracking

### Overall Status
- **Phase 1:** ✅ Completed
- **Phase 2:** ✅ Completed
- **Phase 3:** ✅ Completed
- **Phase 4:** ✅ Completed
- **Phase 5:** ⬜ Not Started
- **Phase 6:** ⬜ Not Started
- **Phase 7:** ⬜ Not Started
- **Phase 8:** ⬜ Not Started

**Legend:**
- ⬜ Not Started
- 🟡 In Progress
- ✅ Completed
- ⚠️ Blocked

---

## Notes & Decisions

### Decision Log
*(Document key decisions made during modernization)*

| Date | Decision | Rationale | Impact |
|------|----------|-----------|--------|
| TBD | Drop .NET 6 and .NET 7 support | Both frameworks are EOL | Users must upgrade to .NET 8+ |
| TBD | Target .NET 8, 9, and 10 | .NET 10 is LTS (Nov 2028), .NET 9 is STS (Nov 2026) | Modern platform support |
| TBD | Keep .NET Standard 2.0 for generators | Required for Roslyn analyzer compatibility | No impact on consumers |
| TBD | Don't set LangVersion explicitly for .NET projects | SDK automatically provides appropriate C# version | Cleaner project files, less maintenance |
| TBD | Set `<LangVersion>latest</LangVersion>` only for .NET Standard 2.0 | Enables modern compiler-lowered features | Better development experience for generators |
| TBD | Migrate from NUnit to TUnit | TUnit is AOT-compatible and uses source generators | Test code changes only |
| TBD | Source generator changes in Phase 4 only | Wait until tests are fully modernized | Risk mitigation |
| TBD | Optional E2E test suite | TBD - evaluate necessity and feasibility | May improve real-world validation |
| TBD | Breaking changes permitted if necessary | Will result in new major version (v5.0.0+) | Consumer migration required |
| 2026-08-19 | Version bump timing: confirmed 5.0.0, deferred to Phase 8 | Modernization branch is confirmed to ship as v5.0.0; version stays untouched through Phases 6-7 (docs/CI work) and is only bumped in Phase 8 as part of preparing the `modernization` → `main` PR | Avoids premature version commits while removing ambiguity about the target version |
| 2026-08-19 | Docs split into README + `docs/CHANGELOG.md` + `docs/MIGRATION_GUIDE.md` | One long README mixing intro, full version history, and breaking changes was hard to navigate; `docs/API_COVERAGE.md` already covers endpoint status in more detail than README's list | README becomes a short entry point that links to the other docs; each doc has one clear audience/purpose |
| 2026-08-19 | Phase 7 also adds a PR-validation workflow (build+tests+`dotnet format --verify-no-changes`+NuGet vulnerability audit) | Merge to main ≠ release, so quality gates should already be enforced automatically before that point, not just checked manually in Phase 6 | Every PR is now gated by the same checks documented in AGENTS.md, reducing reliance on manual review |
| TBD | Merge to main ≠ release | Package publication is a separate, deliberate step (Phase 8), executed via the Phase 7 publish workflow | Cleaner release workflow |
| 2026-08-19 | Deferred: `IDE0060`/`IDE0005` false positives from `dotnet format --severity info` | 67x `IDE0060` on `DiscogsApiClient.cs`'s generator-implemented `private partial` method declarations (parameters are used by the source-generator-emitted body, which the analyzer doesn't see across); 9x `IDE0005` on files using `[GenerateJsonConverter]`/`[AliasAs(...)]` (usings resolve source-generator-emitted attribute types the analyzer doesn't recognize). Both confirmed as false positives via revert-and-rebuild testing (removal breaks the build with `CS0246`). Both are inherent to the project's source-generator architecture, not real issues. | Left unresolved for now (already `suggestion` severity, doesn't fail `dotnet build`); revisit suppression (e.g. `.editorconfig`) only if Phase 7's CI format-check surfaces the same findings |
| TBD | Service registration modernization (§4.7) | Align with `IOptions<T>`, hand-written `IValidateOptions<T>` + `ValidateOnStart()`, `TryAdd*` idempotency, `IConfiguration` binding, and DI-namespace discoverability used by modern .NET libraries | **Breaking** — extension moved to `Microsoft.Extensions.DependencyInjection`; invalid options now throw `OptionsValidationException` at startup instead of `InvalidOperationException` at registration; `OAuthAuthenticationProvider` ctor takes `IOptions<T>` |
| 2025-01-XX | **Rate limiting: Remove built-in limiter** | Sliding window implementation was unreliable and didn't align with Discogs methodology; feature not widely used; exposing raw metadata provides maximum flexibility | **Breaking** — consumers must remove `UseRateLimiting` and related config; can now access rate limit state via `IDiscogsRateLimitStateService` |
| TBD | Authentication setup modernization (§4.9) | `IDiscogsAuthenticationService` facade allowed half-configured/conflicting auth state and had no way to pre-authenticate at registration; tokens were runtime-only mutable singleton state, not bindable via options/`IConfiguration` | **Breaking** — `IDiscogsAuthenticationService`/`DiscogsAuthenticationService` removed; `IPersonalAccessTokenAuthenticationProvider`/`PersonalAccessTokenAuthenticationProvider` renamed to `IDiscogsPatAuthenticationProvider`/`DiscogsPatAuthenticationProvider`; `IOAuthAuthenticationProvider`/`OAuthAuthenticationProvider` renamed to `IDiscogsOAuthAuthenticationProvider`/`DiscogsOAuthAuthenticationProvider`; `DiscogsApiClientOptions.ConsumerKey`/`ConsumerSecret`/`VerifierCallbackUrl` moved to new `DiscogsOAuthOptions`; new `DiscogsPatOptions`; consumers must call `.WithPatAuthentication(...)` or `.WithOAuthAuthentication(...)` to opt into a mechanism (calling both throws `InvalidOperationException`); calling neither yields a valid, permanent unauthenticated client |
| 2026-08-19 | Manual testing found a real bug: demo `.slnx` missing `SourceGenerator` project reference | Visual Studio failed to build `DiscogsApiClient.dll` when opening `demo/DiscogsApiClientDemo.slnx` because the generator project wasn't part of the solution (pre-existing gap since before this modernization branch; CLI `dotnet build` never surfaced it because `DiscogsApiClient.csproj`'s own `Analyzer`-only `ProjectReference` is resolved by MSBuild independent of solution membership — this is a VS-specific out-of-solution-reference quirk) | Fixed by adding `DiscogsApiClient.SourceGenerator.csproj` to `demo/DiscogsApiClientDemo.slnx`; all demo scenarios (PAT, OAuth, AOT console) manually verified working afterward with real credentials |
| 2026-08-19 | Performance benchmarks marked N/A for this modernization | No BenchmarkDotNet project exists in the repo; setting one up is out of scope for Phase 6 | Benchmarking left as a possible future addition, not tracked as a blocking item |
| 2026-08-19 | NuGet package validated locally (build + real `PackageReference` consumption), nothing published | `dotnet pack` output verified to contain all 3 TFM assemblies + embedded README + valid `.nuspec`; consumed from a local folder feed by a scratch console app (DI registration, PAT auth, `IDiscogsApiClient` resolution all worked) | Confirms the package is structurally sound and consumable before Phase 7/8 introduce the real publish pipeline; no nuget.org publication occurred |
| 2026-08-19 | Deferred: fresh-clone failure on default Windows git settings | Cloning without `core.longpaths=true` fails with "Filename too long" due to deeply-nested test recording fixture filenames (~241+ chars); only reproduces in deep clone paths (e.g. `%TEMP%`) and only affects contributors building from source, not NuGet consumers | Left unresolved for now; revisit (e.g. shorten fixture names or document the `core.longpaths` requirement) only if Phase 7 CI surfaces the same failure |

### Risks & Mitigations
- **Risk:** Breaking changes impact existing consumers
  - **Mitigation:** This is acceptable as modernization will result in a new major version (v5.0.0+). Provide clear migration guide and changelog.

- **Risk:** Source generator changes introduce bugs
  - **Mitigation:** Phase 3 happens AFTER tests are fully modernized in Phase 2. Comprehensive generator tests validate behavior.

- **Risk:** Performance regression
  - **Mitigation:** Benchmark critical paths, optimize as needed

- **Risk:** Test migration introduces test failures
  - **Mitigation:** Migrate tests incrementally, validate each step, use mocking to ensure consistent test behavior

- **Risk:** Service registration refactoring (Phase 3.6) breaks consumer call sites
  - **Mitigation:** Clearly document all breaking changes in `docs/MIGRATION_GUIDE.md`; provide before/after examples.

### Open Questions
*(Track questions that need resolution)*
- [ ] Should we implement the optional E2E test suite in Phase 2.6?
- [ ] What breaking changes (if any) should we make to IDiscogsApiClient interface?
- [ ] Should README and version be updated in modernization→main merge or deferred to release?
- [x] ~~Phase 3.6.6: implicit `UseRateLimiting` flag OR explicit `.AddRateLimiting()` builder extension?~~ **Resolved:** Removed rate limiting entirely; replaced with `IDiscogsRateLimitStateService` that exposes raw Discogs rate limit metadata
- [ ] *Add questions as they arise*

---

## Resources

### Microsoft Documentation
- [.NET 10 What's New](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview)
- [.NET 9 What's New](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9/overview)
- [.NET Support Policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core)
- [C# 14 What's New](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14)
- [C# 13 What's New](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-13)
- [C# 12 What's New](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12)
- [Source Generators Overview](https://learn.microsoft.com/dotnet/csharp/roslyn-sdk/source-generators-overview)
- [Incremental Generators](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)
- [.NET API Analyzer](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/overview)
- [Options pattern in .NET](https://learn.microsoft.com/dotnet/core/extensions/options)
- [Options validation in .NET](https://learn.microsoft.com/dotnet/core/extensions/options#options-validation)
- [TUnit Documentation](https://tunit.dev/)

### Tools
- [BenchmarkDotNet](https://benchmarkdotnet.org/) - For performance testing
- [dotnet-coverage](https://learn.microsoft.com/dotnet/core/additional-tools/dotnet-coverage) - For code coverage
- [Source Generator Playground](https://github.com/davidwengier/SourceGeneratorPlayground)

---

## Appendix

### C# Feature Compatibility Matrix

| Feature | C# Version | .NET 8 | .NET 9 | .NET 10 | .NET Standard 2.0 (Compiler-lowered) |
|---------|-----------|--------|--------|---------|--------------------------------------|
| File-scoped namespaces | 10 | ✅ | ✅ | ✅ | ✅ (syntax only) |
| Global usings | 10 | ✅ | ✅ | ✅ | ❌ |
| Record types | 9 | ✅ | ✅ | ✅ | ✅ (compiler-lowered) |
| Init-only properties | 9 | ✅ | ✅ | ✅ | ✅ (compiler-lowered) |
| Collection expressions | 12 | ✅ | ✅ | ✅ | ⚠️ (limited) |
| Primary constructors | 12 | ✅ | ✅ | ✅ | ✅ (for records) |
| Required members | 11 | ✅ | ✅ | ✅ | ✅ (compiler-lowered) |
| Raw string literals | 11 | ✅ | ✅ | ✅ | ✅ (syntax only) |
| Pattern matching | 7+ | ✅ | ✅ | ✅ | ✅ (most patterns) |

**Note:** Many modern C# features work with .NET Standard 2.0 because the compiler lowers them to compatible IL.
The generator project can use `<LangVersion>latest</LangVersion>` despite targeting .NET Standard 2.0.

### Breaking Changes Checklist
*(Use when interface or public API changes)*
- [ ] Document all breaking changes
- [ ] Provide migration examples
- [ ] Consider `[Obsolete]` attributes for transition period
- [ ] Update semantic version appropriately (major bump)
- [ ] Add to release notes
- [ ] Update samples/demos

---

**Document Maintenance:**
- This document should be updated as work progresses
- Mark tasks as complete with checkboxes
- Add notes and decisions to relevant sections
- Keep progress tracking current
- Update dates and timeline estimates

