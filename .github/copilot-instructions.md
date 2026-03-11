# Copilot Instructions for DiscogsApiClient

## Repository Structure

⚠️ **Important**: This repository has multiple workspaces (solutions in `src/` and `demo/`). The **repository root** is at `F:\DiscogsApiClient\`.

### Directory Layout

```
F:\DiscogsApiClient/                    ← Repository root
├── .github/                            ← GitHub configs, workflows, this file
├── docs/                               ← ALL documentation files
│   ├── API_COVERAGE.md                 ← API endpoint tracking
│   ├── ARCHITECTURE.md                 ← System architecture (READ THIS for details)
│   └── Documentation.html              ← Discogs API reference
├── demo/                               ← Demo applications
│   └── *.sln                           ← Demo solution files
├── src/                                ← Main library source code
│   ├── DiscogsApiClient/
│   ├── DiscogsApiClient.SourceGenerator/
│   ├── DiscogsApiClient.Tests/
│   └── DiscogsApiClient.sln            ← Main solution file
├── .gitignore
└── README.md
```

## File Creation Rules

When creating files, determine the repository root first, then use appropriate paths:

| File Type | Location | Example Path |
|-----------|----------|--------------|
| Documentation | `docs/` at repo root | Use relative path to reach `docs/` |
| Source code | `src/ProjectName/` | Relative to current workspace |
| Demo code | `demo/AppName/` | Use relative path to reach `demo/` |
| Root-level files | Repository root | Use relative path to reach root |

**Key principle:** If you're in a nested workspace (like `src/` or `demo/`), navigate up to repository root to create documentation or cross-workspace files.

---

## Essential Coding Rules

### API Endpoint Pattern
- Internal method with `[HttpGet/Post/Put/Delete]` attribute
- Public wrapper with `Guard` validation and XML docs
- See `ARCHITECTURE.md` for detailed examples

### Parameter Validation
- Always use `Guard.IsNotNullOrWhiteSpace()`, `Guard.IsGreaterThan()`, etc.

### Contract Models
- Use records with init-only properties
- Add to `DiscogsJsonSerializerContext.cs` for JSON serialization
- Organize in `Contract/` folders by domain (Artist, Label, Release, User, Search)

### When Adding New API Endpoints
1. Check `docs/API_COVERAGE.md` for status
2. Create contract models
3. Add to JSON serialization context
4. Define in `IDiscogsApiClient` (internal + public method pair)
5. Update `docs/API_COVERAGE.md`

---

## Key Documentation

**For architecture, design patterns, and detailed coding standards:**  
→ `docs/ARCHITECTURE.md`

**For API coverage and implementation tracking:**  
→ `docs/API_COVERAGE.md`

**For local API reference:**  
→ `docs/Documentation.html`

---

## Project Characteristics

- Source Generator-based HTTP client implementation
- Native AOT compatible (no reflection)
- Multi-targets: .NET 6, 7, 8
- Source generators target: .NET Standard 2.0
- Uses `System.Text.Json` with source generation
