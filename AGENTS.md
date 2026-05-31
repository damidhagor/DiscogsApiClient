# Agent Guidelines

**This document defines code styling conventions and project knowledge for AI agents working on this repository.**
**It is not user-facing documentation — it exists to prevent agents from producing inconsistent code.**

**Keep this document up to date.** When the user makes explicit styling decisions, updates framework/language versions,
or establishes new patterns, update this document to reflect those decisions. When upgrading to newer .NET versions
with newer C# language features, ensure the style guidelines are updated to prefer the latest modern idioms.

## General

- Use the latest stable C# language features (primary constructors, collection expressions, file-scoped namespaces, etc.).
- Prefer modern idioms over legacy patterns. Always use the newest C# features available for the target language version.
- Code should be self-explanatory. Avoid verbose XML documentation on internal types — keep doc comments brief or omit them when the implementation is clear from reading.
- Attribution comments (crediting authors of referenced implementations) must always be preserved.

## Formatting

### Braces

- **Always** use curly braces `{}` around `if`, `else`, `foreach`, `for`, `while`, and `using` blocks — even for single-line bodies.
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
- StringBuilder-based code generation is the standard pattern. Do not use Scriban, T4, or other template engines.
- Diagnostics are registered in `AnalyzerReleases.Unshipped.md` until a NuGet release, then moved to `AnalyzerReleases.Shipped.md`.

## Testing

- Use TUnit as the test framework.
- Keep test infrastructure similar to the existing patterns in the project.

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
│   └── DiscogsApiClient.sln
├── AGENTS.md                           ← This file
└── README.md
```

## Library Conventions

### API Endpoint Pattern

- Internal method with `[HttpGet/Post/Put/Delete]` attribute (source-generated).
- Public wrapper with `Guard` validation and XML docs.
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


### Contract Models

- Use records with init-only properties.
- Add to `DiscogsJsonSerializerContext.cs` for JSON serialization.
- Organize in `Contract/` folders by domain (Artist, Label, Release, User, Search).

### Adding New API Endpoints

1. Check `docs/API_COVERAGE.md` for status.
2. Create contract models.
3. Add to JSON serialization context.
4. Define in `IDiscogsApiClient` (internal + public method pair).
5. Update `docs/API_COVERAGE.md`.

## Project Characteristics

- Source generator-based HTTP client implementation.
- Native AOT compatible (no reflection).
- Uses `System.Text.Json` with source generation.
- Source generators target: .NET Standard 2.0.
