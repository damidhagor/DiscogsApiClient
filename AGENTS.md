# Agent Guidelines

**This document defines code styling conventions and project knowledge for AI agents working on this repository.**
**It is not user-facing documentation — it exists to prevent agents from producing inconsistent code.**

**Keep this document up to date.** When the user makes explicit styling decisions, updates framework/language versions,
or establishes new patterns, update this document to reflect those decisions. When upgrading to newer .NET versions
with newer C# language features, ensure the style guidelines are updated to prefer the latest modern idioms.

## General

- Use the latest stable C# language features (primary constructors, collection expressions, file-scoped namespaces, etc.).
- Prefer modern idioms over legacy patterns. Always use the newest C# features available for the target language version.
- Actively look for and remove redundancy that a newer language feature eliminates — e.g. a redundant constructor
  type name in a `new TypeName(...)` expression where the target type is already clear from context should use
  target-typed `new()` instead (see the Target-typed new rule below). Don't just apply new features to new code;
  when touching existing code, simplify it to the modern idiom if it's trivial to do so.
- Code should be self-explanatory. Avoid verbose XML documentation on internal types — keep doc comments brief or omit them when the implementation is clear from reading.
- **XML documentation is for the public API surface only.** Only document `public`/`protected` members that
  are part of the library's external contract. Do not add XML doc comments to `internal`/`private` members —
  add a brief inline comment only if the implementation genuinely isn't self-explanatory from reading it.
- **Member accessibility modifiers should reflect the member's intended accessibility on its own terms, not be
  downgraded just because the containing type happens to be `internal`.** A `public` member on an `internal` class
  is not a mistake — its effective visibility is still capped to the assembly by the containing type, but declaring
  it `public` means promoting the containing type to `public` later requires no member-by-member audit. Apply the
  "public API surface only" XML-doc rule based on the member's own declared accessibility (`public`/`protected`),
  regardless of whether the containing type is `public` or `internal`.
- Attribution comments (crediting authors of referenced implementations) must always be preserved.

## Diagnostics and Warnings

- All diagnostic messages must be checked for a correct implementation — this includes compiler warnings,
  analyzer warnings, and IDE-level diagnostics (e.g. suggestions, refactoring proposals, code-style hints).
- This does **not** mean every diagnostic must be auto-fixed. Where it isn't clear whether a diagnostic is
  critical, or whether fixing it would introduce other problems or contradict existing code/design
  decisions, at minimum triage it and surface it to the user for their evaluation rather than silently
  fixing or silently ignoring it.

## Formatting

### Line Endings

- This repository uses **CRLF** line endings (`core.autocrlf=true`, consistent with all existing tracked files).
- When creating or editing files, always preserve the existing line-ending style of that file — never mix CRLF and LF
  within the same document, and never introduce an all-LF file into a CRLF repository. Verify line endings after edits
  if there's any doubt (e.g. after tool-based file creation/edits that may default to LF).

### Braces

- **Always** use curly braces `{}` around `if`, `else`, `foreach`, `for`, `while`, and `using` blocks — even for single-line bodies.
- This means single-line/brace-less forms like `if (x) return;` or `if (x) throw new ...;` are **never** allowed, no matter how short the body is — always write the braced multi-line form.
- This rule does **not** apply to expression-bodied members (`=>`), which are a different construct.
- Early return conditions must not be squashed into one-liners.

### Expression Bodies

- Single-expression methods, properties, and operators **should** use expression bodies (`=>`).
- Multi-statement methods **must** use block bodies with explicit `return`.
- When an expression body breaks across lines, the `=>` goes on the **new line**, not at the end of the signature:
  ```csharp
  // Correct
  public bool IsType(this ITypeSymbol symbol, Type type)
      => symbol.GetNamespace() == type.Namespace && type.Name == symbol.Name;

  // Incorrect
  public bool IsType(this ITypeSymbol symbol, Type type) =>
      symbol.GetNamespace() == type.Namespace && type.Name == symbol.Name;
  ```

### Line Length and Wrapping

- A code line is allowed to be moderately long — don't break simple expressions unnecessarily.
- When function call arguments are split across lines, split **all** arguments — one per line. Never mix inline and split arguments.
- LINQ method chains should be split across lines when the chain exceeds ~2 calls or is hard to scan at a glance.
- Primary constructor parameters should be split across lines when there are 3+ parameters.

### Blank Lines

- One blank line between members.
- No excessive blank lines within method bodies.

### Return Statements

- When two simple return branches differ only by a condition, prefer a ternary over `if`/`else`:
  ```csharp
  // Preferred
  return diagnostics.Count > 0
      ? GeneratorResult<T>.Success(model, diagnostics.ToImmutable())
      : GeneratorResult<T>.Success(model);

  // Avoid
  if (diagnostics.Count > 0)
  {
      return GeneratorResult<T>.Success(model, diagnostics.ToImmutable());
  }

  return GeneratorResult<T>.Success(model);
  ```

## Types and Records

- **Records**: Prefer `sealed record` for immutable model types. Records auto-implement `IEquatable<T>` — never declare it explicitly.
- **Primary constructors**: Use for records, classes, and structs where the constructor initializes properties/fields. For structs that need to access fields on `other` instances (e.g. in `Equals`), declare the field explicitly and initialize it from the primary constructor parameter.
- **Readonly structs**: Use `readonly struct` for small, immutable value types (e.g. `EquatableArray<T>`).

## Collections and Expressions

- **Target-typed new (`new()`)**: Use only when the target type is explicitly declared on the left (e.g., fields, properties, or explicitly typed variables) or in constructor/method arguments where the parameter type is clear. Otherwise, prefer using `var` with the explicit constructor on the right (e.g., `var options = new DiscogsApiClientOptions();`).
- Use **collection expressions** (`[]`) for empty collections and short initializers wherever the target type supports it.
- `EquatableArray<T>` supports collection expressions via `[CollectionBuilder]` — prefer `["a", "b"]` over `ImmutableArray.Create(...)`.
- Use `default` only when the target type doesn't support collection expressions.
- Avoid `params` arrays in hot paths — they allocate. Use collection expressions or explicit `ImmutableArray.Create(...)` at call sites instead.

## Using Directives

- Use `global using` in `Usings.cs` for namespaces used across many files.
- **Never** re-declare a global using as a local using — this produces redundant imports.
- Remove unused using directives.

## Naming

- Diagnostics use the `DISCOGS` prefix with sequential numbering (e.g. `DISCOGS001`).
- Source generator tracking names should be descriptive (e.g. `"ApiClientTransform"`, `"EnumCollect"`).

## Source Generator Specifics

- Use `IIncrementalGenerator` with `ForAttributeWithMetadataName` — never the legacy `ISourceGenerator`.
- Pipeline models must be fully equatable for Roslyn caching. Use `EquatableArray<T>` for collections and sealed records for models.
- Never leak Roslyn symbols (`ITypeSymbol`, `Compilation`, etc.) past the transform step.
- Report diagnostics via `DiagnosticInfo` records that decouple from Roslyn's `Location` type.
- Diagnostics are collected via `ImmutableArray<DiagnosticInfo>.Builder` passed through parser methods and surfaced via `GeneratorResult<T>`.
- StringBuilder-based code generation is the standard pattern. Do not use Scriban, T4, or other template engines. Within a template, prefer a single raw string literal (`"""`/`$$"""`) over many successive `builder.Append`/`AppendLine` calls — it is more readable and emits in one call. Reserve individual `Append` calls for genuinely dynamic, per-iteration fragments.
- Emitted code must itself be modern, idiomatic C# for the consuming target (.NET 8+), consistently across **every** generator (API client, JSON converters, and the post-initialization attribute templates): primary constructors instead of an explicit constructor that only assigns parameters (e.g. `internal sealed class HttpGetAttribute(string route) : HttpMethodBaseAttribute(route)`), get-only auto-properties for values only set once (`public string Route { get; } = route;`), index-from-end (`buffer[^1]`) instead of `buffer[buffer.Length - 1]`, no placeholder-free interpolated strings (emit `"/oauth/identity"`, not `$"/oauth/identity"` — gate the `$` prefix on the route actually containing a `{` placeholder), and `return await Xxx(...)` directly rather than `var result = await Xxx(...); return result;`.
- Generated source must be emitted already correctly formatted: 4-space indentation, no tabs, no trailing whitespace, exactly one blank line between members, and no stray blank line directly after an opening `{`. Do not emit empty or unreferenced helper types at all — skip them entirely (e.g. `QueryParameterHelper` is omitted when a client has no query parameters, since nothing references it). If a body-less type genuinely must be emitted, use the single-line form ending in `;` rather than an empty `{ }`. Do not post-process the output through Roslyn (`NormalizeWhitespace`) — parsing every generated tree is wasteful for a source generator; get the raw templates right instead.
- Generator tests assert the **exact** emitted source via `GeneratorTestHelper.NormalizeLineEndings`, which normalizes line endings only (no trimming of any kind). There is a single normalization method — `NormalizeSource`/per-line trimming was removed because it hid real formatting defects. Because assertions are byte-exact, fragment-level tests that exercise a composition helper (e.g. `GenerateApiMethod`) must include that helper's composition seams (a leading blank line and/or a trailing newline) verbatim in the expected raw string, represented as a blank line immediately after the opening `"""` and/or before the closing `"""`. The full-document output has no such seams (starts with `#nullable enable`, ends with `}` and no trailing newline).
- Diagnostics are registered in `AnalyzerReleases.Unshipped.md` until a NuGet release, then moved to `AnalyzerReleases.Shipped.md`.

## Testing

- Use TUnit as the test framework.
- Keep test infrastructure similar to the existing patterns in the project.

## Git Workflow

- **NEVER commit changes without explicit user approval first.**
- **NEVER push changes (e.g. `git push`) without explicit user approval first**, even if a commit was already approved earlier — pushing is a separate approval step.
- Always present changes to the user for review before running `git commit` or `git push`.
- When presenting changes for review, **propose a suitable commit message** following conventional commit format.
- The user must approve both the changes **and** the commit message before proceeding.
- This applies to all commits and pushes, including code changes, test recordings, documentation updates, etc.
- After making changes, inform the user what was changed and wait for their approval to commit and/or push.

## Project Structure

```
DiscogsApiClient/
├── docs/                                ← All documentation files
│   ├── API_COVERAGE.md                 ← API endpoint tracking
│   ├── ARCHITECTURE.md                 ← System architecture
│   └── Documentation.html              ← Discogs API reference
├── demo/                               ← Demo applications
├── src/
│   ├── DiscogsApiClient/               ← Main library
│   ├── DiscogsApiClient.SourceGenerator/ ← Source generator (netstandard2.0)
│   ├── DiscogsApiClient.Tests/         ← Library tests
│   ├── DiscogsApiClient.SourceGenerator.Tests/ ← Generator tests
│   └── DiscogsApiClient.slnx
├── AGENTS.md                           ← This file
└── README.md
```

## Library Conventions

### API Endpoint Pattern

- `[ApiClient(typeof(DiscogsJsonSerializerContext))]` is applied to the `internal sealed partial class DiscogsApiClient`, which implements the pure `IDiscogsApiClient` contract interface.
- Endpoints are `partial` method definitions carrying a `[HttpGet/Post/Put/Delete]` attribute; the generator emits their implementing bodies in the other partial half.
- Validated endpoints: a `private partial XxxInternal` method (generated body) + a hand-written `public` wrapper with native guard validation and XML docs.
- Endpoints without validation: a `public partial` method that directly implements the interface member (generated body).
- The generator discovers dependencies by **type** — an `HttpClient` field/property and a field/property whose type is or derives from the context type — so the constructor and fields are owned by the class, not the generator. Prefer a primary constructor that assigns its parameters to fields (no null-guards needed thanks to nullable reference types); the generator only inspects fields and properties, never constructor parameters.
- See `docs/ARCHITECTURE.md` for detailed examples.

### Parameter Validation

- Prefer the framework-native guard clauses introduced in .NET 7/8 over the CommunityToolkit `Guard` class:
  - `ArgumentNullException.ThrowIfNull()` instead of `Guard.IsNotNull()`
  - `ArgumentException.ThrowIfNullOrWhiteSpace()` instead of `Guard.IsNotNullOrWhiteSpace()`
  - `ArgumentOutOfRangeException.ThrowIfLessThanOrEqual()` instead of `Guard.IsGreaterThan()`
  - `ArgumentOutOfRangeException.ThrowIfNegativeOrZero()` instead of `Guard.IsGreaterThan(value, 0)`
- Do **not** pass the parameter name explicitly to native guards (e.g. use `ArgumentNullException.ThrowIfNull(session)` rather than `ArgumentNullException.ThrowIfNull(session, nameof(session))`). The compiler automatically infers parameter names in C# 11+ via `[CallerArgumentExpression]`.
- Always validate reference parameters in public/externally visible methods (e.g. `ArgumentNullException.ThrowIfNull`) to prevent **CA1062** analyzer warnings.

### Async/Await and ConfigureAwait

- Always append `.ConfigureAwait(false)` to all awaited operations in library code to prevent synchronization context capture/deadlocks and resolve **CA2007** warnings.
- `CancellationToken` parameters on public API methods are **mandatory** — never give them a `= default` value. Cancellation support should be an explicit, deliberate choice by the caller. As a consequence, any preceding optional parameters (e.g. nullable query-parameter objects) must also be required (no `= null`) so the trailing token stays required.


### Contract Models

- Use records with init-only properties.
- Add to `DiscogsJsonSerializerContext.cs` for JSON serialization.
- Organize in `Contract/` folders by domain (Artist, Label, Release, User, Search).

### Adding New API Endpoints

1. Check `docs/API_COVERAGE.md` for status.
2. Create contract models.
3. Add to JSON serialization context.
4. Add the method signature to the `IDiscogsApiClient` contract interface.
5. Add the `partial` method (+ public wrapper if validated) to the `DiscogsApiClient` partial class.
6. Update `docs/API_COVERAGE.md`.

## Project Characteristics

- Source generator-based HTTP client implementation.
- Native AOT compatible (no reflection).
- Uses `System.Text.Json` with source generation.
- Source generators target: .NET Standard 2.0.
