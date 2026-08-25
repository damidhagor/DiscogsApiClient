# Discogs API Coverage

This document tracks the implementation status of all Discogs API endpoints in this library.

**Last Updated:** 2026-08-25 (verified against `src/DiscogsApiClient/DiscogsApiClient.cs`)
**Total Coverage:** 54/59 endpoints (92%)

---

## Coverage Summary

| Category                  | Implemented | Total | Coverage   |
| ------------------------- | ----------- | ----- | ---------- |
| Authentication & Identity | 1           | 1     | **100%** ✅ |
| User Profile              | 2           | 2     | **100%** ✅ |
| User Collection           | 13          | 13    | **100%** ✅ |
| User Wantlist             | 3           | 3     | **100%** ✅ |
| User Contributions        | 2           | 2     | **100%** ✅ |
| User Lists                | 2           | 2     | **100%** ✅ |
| User Inventory            | 10          | 10    | **100%** ✅ |
| Database - Artists        | 2           | 2     | **100%** ✅ |
| Database - Labels         | 2           | 2     | **100%** ✅ |
| Database - Masters        | 2           | 2     | **100%** ✅ |
| Database - Releases       | 6           | 6     | **100%** ✅ |
| Database - Search         | 1           | 1     | **100%** ✅ |
| Marketplace - Listings    | 4           | 4     | **100%** ✅ |
| Marketplace - Orders      | 0           | 5     | 0%         |
| Marketplace - Pricing     | 4           | 4     | **100%** ✅ |

---

## Detailed Endpoint Status

### Authentication & Identity

| Status | Method | Endpoint              | Description                     | Interface Method |
| ------ | ------ | --------------------- | ------------------------------- | ---------------- |
| ✅      | GET    | `/oauth/identity`     | Get authenticated user identity | `GetIdentity()`  |

---

### User Profile ✅ 100% Coverage

| Status | Method | Endpoint            | Description       | Interface Method |
| ------ | ------ | ------------------- | ----------------- | ---------------- |
| ✅      | GET    | `/users/{username}` | Get user profile  | `GetUser()`      |
| ✅      | POST   | `/users/{username}` | Edit user profile | `UpdateUser()`   |

---

### User Collection ✅ 100% Coverage

| Status | Method | Endpoint                                                                                                           | Description                           | Interface Method                       |
| ------ | ------ | ------------------------------------------------------------------------------------------------------------------ | ------------------------------------- | -------------------------------------- |
| ✅      | GET    | `/users/{username}/collection/folders`                                                                             | List collection folders               | `GetCollectionFolders()`               |
| ✅      | GET    | `/users/{username}/collection/folders/{folder_id}`                                                                 | Get specific folder                   | `GetCollectionFolder()`                |
| ✅      | POST   | `/users/{username}/collection/folders`                                                                             | Create folder                         | `CreateCollectionFolder()`             |
| ✅      | POST   | `/users/{username}/collection/folders/{folder_id}`                                                                 | Update folder name                    | `UpdateCollectionFolder()`             |
| ✅      | DELETE | `/users/{username}/collection/folders/{folder_id}`                                                                 | Delete folder                         | `DeleteCollectionFolder()`             |
| ✅      | GET    | `/users/{username}/collection/folders/{folder_id}/releases`                                                        | List releases in folder               | `GetCollectionItemsByFolder()`         |
| ✅      | POST   | `/users/{username}/collection/folders/{folder_id}/releases/{release_id}`                                           | Add release to folder                 | `AddReleaseToCollectionFolder()`       |
| ✅      | DELETE | `/users/{username}/collection/folders/{folder_id}/releases/{release_id}/instances/{instance_id}`                   | Remove release instance               | `DeleteReleaseFromCollectionFolder()`  |
| ✅      | GET    | `/users/{username}/collection/value`                                                                               | Get collection value                  | `GetCollectionValue()`                 |
| ✅      | GET    | `/users/{username}/collection/fields`                                                                              | Get custom collection fields          | `GetCollectionFields()`                |
| ✅      | GET    | `/users/{username}/collection/releases/{release_id}`                                                               | Get collection release by release ID  | `GetCollectionItemsByRelease()`        |
| ✅      | POST   | `/users/{username}/collection/folders/{folder_id}/releases/{release_id}/instances/{instance_id}`                   | Edit release instance (notes, rating) | `UpdateCollectionFolderRelease()`      |
| ✅      | POST   | `/users/{username}/collection/folders/{folder_id}/releases/{release_id}/instances/{instance_id}/fields/{field_id}` | Edit custom field value               | `UpdateCollectionFolderReleaseField()` |

---

### User Wantlist ✅ 100% Coverage

| Status | Method | Endpoint                               | Description          | Interface Method              |
| ------ | ------ | -------------------------------------- | -------------------- | ----------------------------- |
| ✅      | GET    | `/users/{username}/wants`              | Get wantlist         | `GetWantlistReleases()`       |
| ✅      | PUT    | `/users/{username}/wants/{release_id}` | Add to wantlist      | `AddReleaseToWantlist()`      |
| ✅      | DELETE | `/users/{username}/wants/{release_id}` | Remove from wantlist | `DeleteReleaseFromWantlist()` |

---

### User Contributions & Submissions ✅ 100% Coverage

| Status | Method | Endpoint                          | Description                       | Interface Method     |
| ------ | ------ | --------------------------------- | --------------------------------- | -------------------- |
| ✅      | GET    | `/users/{username}/contributions` | Get user's database contributions | `GetContributions()` |
| ✅      | GET    | `/users/{username}/submissions`   | Get user's pending submissions    | `GetSubmissions()`   |

---

### User Lists

| Status | Method | Endpoint                  | Description               | Interface Method |
| ------ | ------ | ------------------------- | ------------------------- | ---------------- |
| ✅      | GET    | `/users/{username}/lists` | Get user's lists          | `GetUserLists()` |
| ✅      | GET    | `/lists/{list_id}`        | Get specific list details | `GetList()`      |

---

### User Inventory / Seller Tools ✅ 100% Coverage

| Status | Method | Endpoint                          | Description                      | Interface Method |
| ------ | ------ | --------------------------------- | -------------------------------- | ---------------- |
| ✅      | GET    | `/users/{username}/inventory`     | Get user's marketplace inventory | `GetInventory()`  |
| ✅      | GET    | `/inventory/export`               | Get inventory exports            | `GetInventoryExports()` |
| ✅      | GET    | `/inventory/export/{id}`          | Get specific export              | `GetInventoryExport()` |
| ✅      | GET    | `/inventory/export/{id}/download` | Download export file             | `DownloadInventoryExportAsStream()` / `DownloadInventoryExportAsBytes()` |
| ✅      | POST   | `/inventory/export`               | Create new inventory export      | `CreateInventoryExport()` |
| ✅      | GET    | `/inventory/upload`               | Get CSV upload history           | `GetInventoryUploads()` |
| ✅      | GET    | `/inventory/upload/{id}`          | Get specific upload              | `GetInventoryUpload()` |
| ✅      | POST   | `/inventory/upload/add`           | Add items via CSV                | `AddInventoryListings()` |
| ✅      | POST   | `/inventory/upload/change`        | Change items via CSV             | `ChangeInventoryListings()` |
| ✅      | POST   | `/inventory/upload/delete`        | Delete items via CSV             | `DeleteInventoryListings()` |

---

### Database - Artists ✅ 100% Coverage

| Status | Method | Endpoint                        | Description         | Interface Method      |
| ------ | ------ | ------------------------------- | ------------------- | --------------------- |
| ✅      | GET    | `/artists/{artist_id}`          | Get artist          | `GetArtist()`         |
| ✅      | GET    | `/artists/{artist_id}/releases` | Get artist releases | `GetArtistReleases()` |

---

### Database - Labels ✅ 100% Coverage

| Status | Method | Endpoint                      | Description        | Interface Method     |
| ------ | ------ | ----------------------------- | ------------------ | -------------------- |
| ✅      | GET    | `/labels/{label_id}`          | Get label          | `GetLabel()`         |
| ✅      | GET    | `/labels/{label_id}/releases` | Get label releases | `GetLabelReleases()` |

---

### Database - Master Releases ✅ 100% Coverage

| Status | Method | Endpoint                        | Description                 | Interface Method             |
| ------ | ------ | ------------------------------- | --------------------------- | ---------------------------- |
| ✅      | GET    | `/masters/{master_id}`          | Get master release          | `GetMasterRelease()`         |
| ✅      | GET    | `/masters/{master_id}/versions` | Get master release versions | `GetMasterReleaseVersions()` |

---

### Database - Releases ✅ 100% Coverage

| Status | Method | Endpoint                                   | Description                   | Interface Method              |
| ------ | ------ | ------------------------------------------ | ----------------------------- | ----------------------------- |
| ✅      | GET    | `/releases/{release_id}`                   | Get release                   | `GetRelease()`                |
| ✅      | GET    | `/releases/{release_id}/rating`            | Get community rating          | `GetReleaseCommunityRating()` |
| ✅      | GET    | `/releases/{release_id}/stats`             | Get release stats             | `GetReleaseStats()`           |
| ✅      | GET    | `/releases/{release_id}/rating/{username}` | Get user's rating for release | `GetReleaseRating()`          |
| ✅      | PUT    | `/releases/{release_id}/rating/{username}` | Set user's rating for release | `UpdateReleaseRating()`       |
| ✅      | DELETE | `/releases/{release_id}/rating/{username}` | Delete user's rating          | `DeleteReleaseRating()`       |

---

### Database - Search ✅ 100% Coverage

| Status | Method | Endpoint           | Description     | Interface Method   |
| ------ | ------ | ------------------ | --------------- | ------------------ |
| ✅      | GET    | `/database/search` | Search database | `SearchDatabase()` |

---

### Marketplace - Listings

| Status | Method | Endpoint                             | Description             | Interface Method            |
| ------ | ------ | ------------------------------------ | ----------------------- | ---------------------------- |
| ✅      | GET    | `/marketplace/listings/{listing_id}` | Get marketplace listing | `GetMarketplaceListing()`    |
| ✅      | POST   | `/marketplace/listings`              | Create new listing      | `CreateMarketplaceListing()` |
| ✅      | POST   | `/marketplace/listings/{listing_id}` | Edit listing            | `UpdateMarketplaceListing()` |
| ✅      | DELETE | `/marketplace/listings/{listing_id}` | Delete listing          | `DeleteMarketplaceListing()` |

---

### Marketplace - Orders

| Status | Method | Endpoint                                  | Description         | Interface Method |
| ------ | ------ | ----------------------------------------- | ------------------- | ---------------- |
| ❌      | GET    | `/marketplace/orders`                     | List orders         | -                |
| ❌      | GET    | `/marketplace/orders/{order_id}`          | Get order details   | -                |
| ❌      | POST   | `/marketplace/orders/{order_id}`          | Update order status | -                |
| ❌      | GET    | `/marketplace/orders/{order_id}/messages` | Get order messages  | -                |
| ❌      | POST   | `/marketplace/orders/{order_id}/messages` | Send order message  | -                |

---

### Marketplace - Pricing & Stats

| Status | Method | Endpoint                                      | Description                | Interface Method |
| ------ | ------ | --------------------------------------------- | -------------------------- | ---------------- |
| ✅      | GET    | `/marketplace/fee/{price}`                    | Calculate marketplace fee  | `GetMarketplaceFee()` |
| ✅      | GET    | `/marketplace/fee/{price}/{currency}`         | Calculate marketplace fee in a specific currency | `GetMarketplaceFee()` |
| ✅      | GET    | `/marketplace/price_suggestions/{release_id}` | Get price suggestions      | `GetPriceSuggestions()` |
| ✅      | GET    | `/marketplace/stats/{release_id}`             | Get marketplace statistics | `GetMarketplaceStats()` |

---

## Notes for Implementers

- The public contract lives in `IDiscogsApiClient.cs` — a plain interface with XML-documented method
  signatures (including `<exception>` tags for any validation) but **no** `[HttpGet/Post/Put/Delete]`
  attributes and no implementation.
- The generator-facing implementation lives in `DiscogsApiClient.cs` (the `internal sealed partial class
  DiscogsApiClient`, decorated with `[ApiClient(typeof(DiscogsJsonSerializerContext))]`). Follow the
  existing pattern there when adding a new endpoint:
  - **No parameter validation needed:** add a single `public partial` method decorated with
    `[HttpGet/Post/Put/Delete("/route/{param}")]` that directly implements the interface member — the
    generator emits its body.
  - **Parameter validation needed:** add a `private partial <Name>Internal(...)` method decorated with the
    `[HttpGet/Post/Put/Delete]` attribute (generator emits its body), plus a hand-written `public async`
    wrapper matching the interface signature that validates parameters with native .NET guard clauses
    (`ArgumentException.ThrowIfNullOrWhiteSpace`, `ArgumentOutOfRangeException.ThrowIfLessThanOrEqual`,
    etc. — see `AGENTS.md`'s "Parameter Validation" section) and then calls
    `await XxxInternal(..., cancellationToken).ConfigureAwait(false)`.
  - Route placeholders (`{username}`, `{releaseId}`, ...) must match the method's parameter names.
  - POST/PUT bodies are passed via a parameter attributed `[Body]` (a `Contract` request record).
  - Optional query parameters use a `PaginationQueryParameters?`/`<Domain>QueryParameters?`-style nullable
    parameter type, passed straight through to the internal method (not validated).
- Add any new request/response `Contract` records under `Contract/` by domain (see "Contract Models" in
  `AGENTS.md`) and register them in `DiscogsJsonSerializerContext.cs` for source-generated JSON
  serialization.
- Update this document's endpoint status table with the new row, linking to the `IDiscogsApiClient` method.
- See `docs/ARCHITECTURE.md` ("Core Components" → "API Client Contract & Class") for the full pattern with
  a worked example, and `AGENTS.md` ("API Endpoint Pattern" / "Adding New API Endpoints") for the
  authoritative step-by-step checklist.

## References

- Official API Documentation: https://www.discogs.com/developers/
- Local Documentation Copy: `docs/Documentation.html`
