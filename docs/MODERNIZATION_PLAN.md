# DiscogsApiClient Modernization Plan

**Branch Strategy:** All work is performed in the `modernization` branch. Individual tasks are completed in feature branches and merged back to `modernization` via PRs. A final PR to `main` is created only after all modernization work is complete.

**Version:** 1.0  
**Status:** Phase 1 Complete - Ready for Phase 2  
**Last Updated:** 2025-01-15

---

## Table of Contents
1. [Overview](#overview)
2. [General Rules & Principles](#general-rules--principles)
3. [Target Frameworks](#target-frameworks)
4. [Modernization Phases](#modernization-phases)
5. [Phase 1: Foundation - Framework & Package Updates](#phase-1-foundation---framework--package-updates)
6. [Phase 2: Testing Infrastructure Modernization](#phase-2-testing-infrastructure-modernization)
7. [Phase 3: Library Code Modernization](#phase-3-library-code-modernization)
8. [Phase 4: Source Generator Modernization](#phase-4-source-generator-modernization)
9. [Phase 5: Demo Projects Modernization](#phase-5-demo-projects-modernization)
10. [Phase 6: Final Validation & Documentation](#phase-6-final-validation--documentation)

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
- 🔄 Modernize testing infrastructure with mocking and improved coverage (Phase 2)
- 🔄 Update source generators to follow latest Roslyn best practices (Phase 4)
- 🔄 Ensure all code follows modern C# best practices (Phases 3-4)
- **Breaking changes are acceptable** - will result in new major version (v5.0.0+)

### Strategy
The modernization follows a **risk-minimization approach**:
1. Update frameworks and packages first (minimal code changes)
2. Migrate to xUnit and modernize testing infrastructure (enable reliable validation)
3. Modernize library code (protected by improved tests)
4. Modernize source generators (validated by comprehensive tests, happens AFTER testing modernization)
5. Update demo projects (showcase modern patterns)

### Version Strategy
- Library version (4.1.0) remains unchanged throughout modernization branch
- Version bump to 5.0.0+ happens as part of the final release to main
- This allows development without premature version commits

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
Phase 3: Library Code Modernization
   ├─ Medium Risk
   └─ Protected by improved tests, C# 12 features
         ↓
Phase 4: Source Generator Modernization
   ├─ High Complexity
   └─ Only after tests fully modernized, validated comprehensively
         ↓
Phase 5: Demo Projects
   ├─ Low Risk
   └─ Showcase modern features, .NET 10
         ↓
Phase 6: Final Validation
   └─ Comprehensive verification, version bump for release
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
- [ ] Evaluate and add HTTP mocking library (e.g., `MockHttp`, `WireMock.Net`, or custom `HttpMessageHandler`)
- [ ] Design mock strategy for Discogs API responses
- [ ] Create mock data fixtures for common API responses:
  - [ ] Authentication responses
  - [ ] Artist data
  - [ ] Release data
  - [ ] Label data
  - [ ] Search results
  - [ ] User collection/wantlist data
  - [ ] Error responses (rate limits, 404s, etc.)
- [ ] Document mock approach in test documentation

### 2.2 Refactor Existing Tests
- [ ] Identify all tests that make real API calls
- [ ] Refactor tests to use mocked HTTP responses
- [ ] Ensure test isolation (no shared state between tests)
- [ ] Add test categories/traits (Unit, Integration, etc.)
- [ ] Update test naming conventions to modern standards
- [ ] Remove any hardcoded API tokens or credentials

### 2.3 Improve Test Coverage
- [ ] Run code coverage analysis (using built-in or Coverlet)
- [ ] Identify untested or under-tested areas
- [ ] Add tests for:
  - [ ] Error handling paths
  - [ ] Edge cases (null, empty, invalid inputs)
  - [ ] Rate limiting behavior
  - [ ] Authentication flows
  - [ ] Serialization/deserialization
- [ ] Target **reasonable coverage** for critical paths based on code complexity and risk

### 2.4 Test Performance & Organization
- [ ] Organize tests into logical namespaces/folders
- [ ] Add XML documentation to test classes
- [ ] Implement test fixtures and shared contexts where appropriate
- [ ] Ensure tests run quickly (mock responses should be fast)
- [ ] Add integration test project if needed (separate from unit tests)

### 2.5 Testing Best Practices
- [ ] Follow AAA pattern (Arrange, Act, Assert) **without comments marking sections**
- [ ] Use TUnit's modern features (data-driven tests, fluent assertions)
- [ ] **Use TUnit's fluent assertions** - built-in, no external assertion libraries needed
- [ ] **Make test methods async** - avoid synchronous `.Result` or `.Wait()` calls
- [ ] Ensure proper async/await usage throughout test code
- [ ] Leverage TUnit's source generation for better performance and AOT compatibility

### 2.6 Optional: End-to-End Test Suite
- [ ] **Evaluate need for E2E tests** against real Discogs API
- [ ] If implemented:
  - [ ] Create separate test project or test category for E2E tests
  - [ ] Use real API credentials (from configuration, never hardcoded)
  - [ ] Test key endpoints for:
    - [ ] Authentication flow
    - [ ] Basic CRUD operations
    - [ ] Deserialization of real responses
  - [ ] Mark as explicit/manual tests (not part of regular CI)
  - [ ] Document setup requirements
  - [ ] Add rate limiting/throttling to avoid API limits
- [ ] Document decision (implement or skip) and reasoning

### Acceptance Criteria - Phase 2
- [x] All tests use mocked HTTP responses (no real API calls)
- [x] Test coverage is reasonable for critical paths
- [x] All tests pass reliably and quickly
- [x] Tests are well-organized and documented
- [x] Testing approach documented

---

## Phase 3: Library Code Modernization

**Goal:** Modernize the main library code using latest C# features and best practices.

**Branch:** `modernization/phase3-library`

### 3.1 C# Language Feature Adoption
- [ ] **File-scoped namespaces** - Convert to `namespace DiscogsApiClient;`
- [ ] **Global usings** - Create `GlobalUsings.cs` for common imports
- [ ] **Record types** - Use `record` for DTOs/contracts where appropriate
- [ ] **Init-only properties** - Convert to `init` where mutability not needed
- [ ] **Pattern matching** - Modernize switch statements and conditionals
- [ ] **Null-coalescing assignments** - Use `??=` where appropriate
- [ ] **Target-typed new** - Use `new()` where type is obvious
- [ ] **Collection expressions** - Use `[...]` for arrays/collections (C# 12)
- [ ] **Primary constructors** - Consider for simple classes (C# 12)
- [ ] **String interpolation** - Use `$"..."` over `string.Format`

### 3.2 IDiscogsApiClient Interface Refactoring
- [ ] **Analyze current structure** - Document internal/public method pattern
- [ ] **Run static analysis** - Review with analyzer tools for design issues
- [ ] **Evaluate necessity** - Determine if refactoring provides meaningful value
- [ ] **Decision point:** Proceed with refactoring OR document why current design is optimal
- [ ] If proceeding with refactoring:
  - [ ] Design new interface structure
  - [ ] Document any breaking changes
  - [ ] Implement new design
  - [ ] Update XML documentation
  - [ ] Mark obsolete methods if using transition period
- [ ] **Note:** Breaking changes are acceptable as this will result in a new major version

### 3.3 Async/Await Modernization
- [ ] Ensure `ConfigureAwait(false)` used appropriately (library code)
- [ ] Use `ValueTask` where appropriate for hot paths
- [ ] Consider `IAsyncEnumerable` for paginated results (if applicable)
- [ ] Ensure cancellation tokens passed through properly

### 3.4 Code Quality Improvements
- [ ] Enable nullable reference types verification
- [ ] Address all analyzer warnings
- [ ] Simplify complex methods (reduce cyclomatic complexity)
- [ ] Extract magic strings/numbers to constants
- [ ] Review and optimize LINQ usage
- [ ] Ensure proper disposal patterns (`IDisposable`, `IAsyncDisposable`)

### 3.5 Performance Considerations
- [ ] Review allocations (use `Span<T>`, `Memory<T>` where beneficial)
- [ ] Optimize string operations
- [ ] Review collection usage (use appropriate collection types)
- [ ] Consider `ArrayPool` for temporary buffers if applicable

### Acceptance Criteria - Phase 3
- [x] All C# 12 features adopted where appropriate
- [x] IDiscogsApiClient interface refactored and simplified
- [x] All tests pass (validates refactoring didn't break functionality)
- [x] No compiler warnings
- [x] XML documentation complete and accurate
- [x] Breaking changes documented (if any)

---

## Phase 4: Source Generator Modernization

**Goal:** Update source generators to use latest Roslyn APIs and best practices.

**Branch:** `modernization/phase4-generators`

### 4.1 Source Generator Project Modernization
- [ ] Review [Microsoft's source generator documentation](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview)
- [ ] Update to latest Roslyn packages:
  - [ ] `Microsoft.CodeAnalysis.CSharp` (from 4.8.0 to latest)
  - [ ] `Microsoft.CodeAnalysis.Analyzers` (from 3.3.4 to latest)
- [ ] Update `<LangVersion>latest</LangVersion>` (use all modern C# features compatible with .NET Standard 2.0)
- [ ] Implement incremental generators (`IIncrementalGenerator`)
- [ ] Use `IncrementalGeneratorInitializationContext` properly
- [ ] Optimize for performance (caching, minimal re-generation)
- [ ] **Implement comprehensive diagnostics:**
  - [ ] Define diagnostic IDs (e.g., DISCOGS001, DISCOGS002)
  - [ ] Create diagnostic descriptors with severity levels
  - [ ] Add helpful error messages
  - [ ] Report diagnostics for invalid input/configuration
  - [ ] Provide code fix providers where appropriate
  - [ ] Document all diagnostic IDs
- [ ] Use `SourceProductionContext` for diagnostics

### 4.2 Generated Code Modernization
- [ ] Update generated code to use modern C# features (compatible with .NET 8+):
  - [ ] File-scoped namespaces
  - [ ] Target-typed new expressions
  - [ ] Pattern matching where appropriate
  - [ ] Collection expressions (if applicable)
- [ ] Ensure generated code is AOT-compatible
- [ ] Add `[GeneratedCode]` attribute to generated classes
- [ ] Add `#nullable enable` to generated files
- [ ] Optimize generated code (reduce allocations, better patterns)

### 4.3 Source Generator Testing
- [ ] Create test project for source generators (`DiscogsApiClient.SourceGenerator.Tests`)
- [ ] Use `Microsoft.CodeAnalysis.CSharp.SourceGenerators.Testing` (or similar)
- [ ] Add tests for:
  - [ ] Successful generation scenarios
  - [ ] Error handling (invalid input)
  - [ ] Incremental generation behavior
  - [ ] Diagnostic reporting
- [ ] Add snapshot testing for generated output (verify stability)
- [ ] Document testing approach

### 4.4 Generator Best Practices
- [ ] Ensure deterministic output (same input → same output)
- [ ] Handle edge cases gracefully
- [ ] Provide helpful diagnostics
- [ ] Minimize dependencies in generator project
- [ ] Add XML documentation to generator code

### Acceptance Criteria - Phase 4
- [x] Source generators use incremental generator API
- [x] Generated code uses modern C# features
- [x] Comprehensive generator tests implemented
- [x] All tests pass
- [x] Performance is acceptable (fast builds)
- [x] Generated code is well-documented

---

## Phase 5: Demo Projects Modernization

**Goal:** Update demo projects to .NET 10 and showcase modern features.

**Branch:** `modernization/phase5-demos`

### 5.1 DiscogsApiClientDemo.AotConsole Project Updates
- [ ] Update `<TargetFramework>` from `net8.0` to `net10.0`
- [ ] Set `<LangVersion>latest</LangVersion>` (C# 14)
- [ ] Update package references to latest versions (currently none beyond project reference)
- [ ] Verify `PublishAot` and `InvariantGlobalization` settings still appropriate
- [ ] Build and verify compilation
- [ ] Test AOT publish works correctly
- [ ] Update code to use C# 14 features where appropriate

### 5.2 DiscogsApiClientDemo.OAuth Project Updates
- [ ] Update `<TargetFramework>` from `net8.0-windows` to `net10.0-windows`
- [ ] Set `<LangVersion>latest</LangVersion>` (C# 14)
- [ ] Update package references to latest versions:
  - [ ] `CommunityToolkit.Mvvm` (currently 8.2.2)
  - [ ] `Microsoft.Extensions.Hosting` (currently 8.0.0-rc.2.23479.6 - appears to be preview version)
  - [ ] `Microsoft.Web.WebView2` (currently 1.0.2088.41)
  - [ ] **Remove** old `DiscogsApiClient` package reference (2.0.0) - uses project reference
- [ ] Build and verify compilation
- [ ] Test OAuth flow demonstration works correctly
- [ ] Update code to use C# 14 features where appropriate

### 5.3 DiscogsApiClientDemo.PersonalAccessToken Project Updates
- [ ] Update `<TargetFramework>` from `net8.0-windows` to `net10.0-windows`
- [ ] Set `<LangVersion>latest</LangVersion>` (C# 14)
- [ ] Update package references to latest versions:
  - [ ] `CommunityToolkit.Mvvm` (currently 8.2.2)
  - [ ] `Microsoft.Extensions.Hosting` (currently 8.0.0-rc.2.23479.6 - appears to be preview version)
  - [ ] **Remove** old `DiscogsApiClient` package reference (2.0.0) - uses project reference
- [ ] Build and verify compilation
- [ ] Test personal access token demonstration works correctly
- [ ] Update code to use C# 14 features where appropriate

### 5.4 Demo Code Modernization
- [ ] Use modern C# 14 features in demo code where it adds value
- [ ] Update to show latest library API patterns
- [ ] Add examples of new features (if any from modernization)
- [ ] Ensure async/await patterns are exemplary
- [ ] Add comments explaining modern patterns for educational value

### 5.5 Demo Documentation
- [ ] Update README files in demo projects (if they exist)
- [ ] Ensure demos compile and run successfully
- [ ] Add setup instructions if needed
- [ ] Document any changes from previous version

### Acceptance Criteria - Phase 5
- [x] All demo projects target .NET 10 (or .NET 10 Windows)
- [x] Demo code uses modern C# 14 features appropriately
- [x] Demos compile and run successfully
- [x] Documentation is clear and helpful
- [x] Old package references cleaned up

---

## Phase 6: Final Validation & Documentation

**Goal:** Comprehensive validation and documentation updates before merging to main.

**Branch:** `modernization` (final validation)

### 6.1 Comprehensive Testing
- [ ] Run full test suite on all target frameworks (.NET 8, 9, 10)
- [ ] Perform manual testing of key scenarios
- [ ] Run performance benchmarks (if available)
- [ ] Test NuGet package generation
- [ ] Test in a clean environment (fresh clone)

### 6.2 Documentation Updates
- [ ] **Decision point:** Determine if README.md should be updated now or with release
  - Main branch is visible to 4.x users; may confuse them if updated prematurely
  - Consider updating only with final public release, not modernization merge
- [ ] **Decision point:** Determine if version should be bumped in modernization→main merge
  - Version bump may be more appropriate with actual package release
  - Document decision and timeline
- [ ] Update `docs/ARCHITECTURE.md` with any architectural changes (if applicable)
- [ ] Create migration guide for v4.x → v5.x (breaking changes) in `docs/MIGRATION_GUIDE.md`
- [ ] Update this modernization plan status to completed

### 6.3 Code Quality Gates
- [ ] Zero compiler warnings
- [ ] Zero analyzer warnings
- [ ] Code coverage reports generated
- [ ] Static analysis passes
- [ ] NuGet package builds successfully (even if not published yet)

### 6.4 Final Review
- [ ] Review all merged PRs
- [ ] Verify branching strategy was followed
- [ ] Check for any missed tasks
- [ ] Prepare comprehensive changelog
- [ ] **Note:** Actual NuGet package release happens separately from merge to main

### 6.5 Merge to Main
- [ ] Create PR: `modernization` → `main`
- [ ] Comprehensive PR description with summary of all changes
- [ ] List all breaking changes
- [ ] Final review and approval
- [ ] Merge to main
- [ ] **Note:** Publishing happens in a separate release process:
  - Version bump (if not done in modernization)
  - README update (if not done in modernization)
  - Tag release (e.g., `v5.0.0`)
  - Build and publish NuGet package
  - Create GitHub release with notes

### Acceptance Criteria - Phase 6
- [x] All tests pass on all frameworks
- [x] Documentation strategy decided and documented
- [x] Migration guide created
- [x] No warnings or errors
- [x] NuGet package can be built successfully
- [x] Successfully merged to main
- [x] Release process documented (even if not executed yet)

---

## Progress Tracking

### Overall Status
- **Phase 1:** ⬜ Not Started
- **Phase 2:** ⬜ Not Started
- **Phase 3:** ⬜ Not Started
- **Phase 4:** ⬜ Not Started
- **Phase 5:** ⬜ Not Started
- **Phase 6:** ⬜ Not Started

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
| TBD | Migrate from NUnit to xUnit | xUnit is more modern and widely adopted | Test code changes only |
| TBD | Do not use assertion libraries | Stick to xUnit built-in assertions | Simpler dependencies |
| TBD | Source generator changes in Phase 4 only | Wait until tests are fully modernized | Risk mitigation |
| TBD | Optional E2E test suite | TBD - evaluate necessity and feasibility | May improve real-world validation |
| TBD | Breaking changes permitted if necessary | Will result in new major version (v5.0.0+) | Consumer migration required |
| TBD | Version/README update timing | May defer to actual release, not modernization merge | Avoids confusion for 4.x users |
| TBD | Merge to main ≠ release | Package publication is separate process | Cleaner release workflow |

### Risks & Mitigations
- **Risk:** Breaking changes impact existing consumers
  - **Mitigation:** This is acceptable as modernization will result in a new major version (v5.0.0+). Provide clear migration guide and changelog.

- **Risk:** Source generator changes introduce bugs
  - **Mitigation:** Phase 4 happens AFTER tests are fully modernized in Phase 2-3. Comprehensive generator tests validate behavior.

- **Risk:** Performance regression
  - **Mitigation:** Benchmark critical paths, optimize as needed

- **Risk:** Test migration introduces test failures
  - **Mitigation:** Migrate tests incrementally, validate each step, use mocking to ensure consistent test behavior

### Open Questions
*(Track questions that need resolution)*
- [ ] Should we implement the optional E2E test suite in Phase 2.6?
- [ ] What breaking changes (if any) should we make to IDiscogsApiClient interface?
- [ ] Should README and version be updated in modernization→main merge or deferred to release?
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
- [xUnit Documentation](https://xunit.net/)

### Tools
- [BenchmarkDotNet](https://benchmarkdotnet.org/) - For performance testing
- [Coverlet](https://github.com/coverlet-coverage/coverlet) - For code coverage
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
