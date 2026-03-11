# Discogs API Coverage

This document tracks the implementation status of all Discogs API endpoints in this library.

**Last Updated:** 2024
**Total Coverage:** 26/59 endpoints (44%)

---

## Coverage Summary

| Category | Implemented | Total | Coverage |
|----------|-------------|-------|----------|
| Authentication & Identity | 1 | 2 | 50% |
| User Profile | 1 | 2 | 50% |
| User Collection | 9 | 13 | 69% |
| User Wantlist | 3 | 3 | **100%** ✅ |
| User Contributions | 0 | 2 | 0% |
| User Lists | 0 | 2 | 0% |
| User Inventory | 0 | 10 | 0% |
| Database - Artists | 2 | 2 | **100%** ✅ |
| Database - Labels | 2 | 2 | **100%** ✅ |
| Database - Masters | 2 | 2 | **100%** ✅ |
| Database - Releases | 3 | 6 | 50% |
| Database - Search | 1 | 1 | **100%** ✅ |
| Marketplace - Listings | 0 | 4 | 0% |
| Marketplace - Orders | 0 | 5 | 0% |
| Marketplace - Pricing | 0 | 3 | 0% |

---

## Detailed Endpoint Status

### Authentication & Identity

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/oauth/identity` | Get authenticated user identity | `GetIdentity()` |
| ❌ | POST | `/oauth/access_token` | Get OAuth access token | - |

---

### User Profile

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/users/{username}` | Get user profile | `GetUser()` |
| ❌ | POST | `/users/{username}` | Edit user profile | - |

---

### User Collection

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/users/{username}/collection/folders` | List collection folders | `GetCollectionFolders()` |
| ✅ | GET | `/users/{username}/collection/folders/{folder_id}` | Get specific folder | `GetCollectionFolder()` |
| ✅ | POST | `/users/{username}/collection/folders` | Create folder | `CreateCollectionFolder()` |
| ✅ | POST | `/users/{username}/collection/folders/{folder_id}` | Update folder name | `UpdateCollectionFolder()` |
| ✅ | DELETE | `/users/{username}/collection/folders/{folder_id}` | Delete folder | `DeleteCollectionFolder()` |
| ✅ | GET | `/users/{username}/collection/folders/{folder_id}/releases` | List releases in folder | `GetCollectionFolderReleases()` |
| ✅ | POST | `/users/{username}/collection/folders/{folder_id}/releases/{release_id}` | Add release to folder | `AddReleaseToCollectionFolder()` |
| ✅ | DELETE | `/users/{username}/collection/folders/{folder_id}/releases/{release_id}/instances/{instance_id}` | Remove release instance | `DeleteReleaseFromCollectionFolder()` |
| ✅ | GET | `/users/{username}/collection/value` | Get collection value | `GetCollectionValue()` |
| ❌ | GET | `/users/{username}/collection/fields` | Get custom collection fields | - |
| ❌ | GET | `/users/{username}/collection/releases/{release_id}` | Get collection release by release ID | - |
| ❌ | POST | `/users/{username}/collection/folders/{folder_id}/releases/{release_id}/instances/{instance_id}` | Edit release instance (notes, rating) | - |
| ❌ | POST | `/users/{username}/collection/folders/{folder_id}/releases/{release_id}/instances/{instance_id}/fields/{field_id}` | Edit custom field value | - |

---

### User Wantlist ✅ 100% Coverage

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/users/{username}/wants` | Get wantlist | `GetWantlistReleases()` |
| ✅ | PUT | `/users/{username}/wants/{release_id}` | Add to wantlist | `AddReleaseToWantlist()` |
| ✅ | DELETE | `/users/{username}/wants/{release_id}` | Remove from wantlist | `DeleteReleaseFromWantlist()` |

---

### User Contributions & Submissions

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ❌ | GET | `/users/{username}/contributions` | Get user's database contributions | - |
| ❌ | GET | `/users/{username}/submissions` | Get user's pending submissions | - |

---

### User Lists

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ❌ | GET | `/users/{username}/lists` | Get user's lists | - |
| ❌ | GET | `/lists/{list_id}` | Get specific list details | - |

---

### User Inventory / Seller Tools

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ❌ | GET | `/users/{username}/inventory` | Get user's marketplace inventory | - |
| ❌ | GET | `/inventory/export` | Get inventory exports | - |
| ❌ | GET | `/inventory/export/{id}` | Get specific export | - |
| ❌ | GET | `/inventory/export/{id}/download` | Download export file | - |
| ❌ | POST | `/inventory/export` | Create new inventory export | - |
| ❌ | GET | `/inventory/upload` | Get CSV upload history | - |
| ❌ | GET | `/inventory/upload/{id}` | Get specific upload | - |
| ❌ | POST | `/inventory/upload/add` | Add items via CSV | - |
| ❌ | POST | `/inventory/upload/change` | Change items via CSV | - |
| ❌ | DELETE | `/inventory/upload/delete` | Delete items via CSV | - |

---

### Database - Artists ✅ 100% Coverage

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/artists/{artist_id}` | Get artist | `GetArtist()` |
| ✅ | GET | `/artists/{artist_id}/releases` | Get artist releases | `GetArtistReleases()` |

---

### Database - Labels ✅ 100% Coverage

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/labels/{label_id}` | Get label | `GetLabel()` |
| ✅ | GET | `/labels/{label_id}/releases` | Get label releases | `GetLabelReleases()` |

---

### Database - Master Releases ✅ 100% Coverage

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/masters/{master_id}` | Get master release | `GetMasterRelease()` |
| ✅ | GET | `/masters/{master_id}/versions` | Get master release versions | `GetMasterReleaseVersions()` |

---

### Database - Releases

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/releases/{release_id}` | Get release | `GetRelease()` |
| ✅ | GET | `/releases/{release_id}/rating` | Get community rating | `GetReleaseCommunityRating()` |
| ✅ | GET | `/releases/{release_id}/stats` | Get release stats | `GetReleaseStats()` |
| ❌ | GET | `/releases/{release_id}/rating/{username}` | Get user's rating for release | - |
| ❌ | PUT | `/releases/{release_id}/rating/{username}` | Set user's rating for release | - |
| ❌ | DELETE | `/releases/{release_id}/rating/{username}` | Delete user's rating | - |

---

### Database - Search ✅ 100% Coverage

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ✅ | GET | `/database/search` | Search database | `SearchDatabase()` |

---

### Marketplace - Listings

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ❌ | GET | `/marketplace/listings/{listing_id}` | Get marketplace listing | - |
| ❌ | POST | `/marketplace/listings` | Create new listing | - |
| ❌ | POST | `/marketplace/listings/{listing_id}` | Edit listing | - |
| ❌ | DELETE | `/marketplace/listings/{listing_id}` | Delete listing | - |

---

### Marketplace - Orders

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ❌ | GET | `/marketplace/orders` | List orders | - |
| ❌ | GET | `/marketplace/orders/{order_id}` | Get order details | - |
| ❌ | POST | `/marketplace/orders/{order_id}` | Update order status | - |
| ❌ | GET | `/marketplace/orders/{order_id}/messages` | Get order messages | - |
| ❌ | POST | `/marketplace/orders/{order_id}/messages` | Send order message | - |

---

### Marketplace - Pricing & Stats

| Status | Method | Endpoint | Description | Interface Method |
|--------|--------|----------|-------------|------------------|
| ❌ | GET | `/marketplace/fee/{price}` | Calculate marketplace fee | - |
| ❌ | GET | `/marketplace/price_suggestions/{release_id}` | Get price suggestions | - |
| ❌ | GET | `/marketplace/stats/{release_id}` | Get marketplace statistics | - |

---

## Notes for Implementers

- All implemented endpoints are in `IDiscogsApiClient.cs`
- When implementing new endpoints, follow the existing pattern:
  - Create internal method with `[HttpGet/Post/Put/Delete]` attribute
  - Create public wrapper method with parameter validation using `Guard` class
  - Add XML documentation comments
  - Update this document with the new endpoint status

## References

- Official API Documentation: https://www.discogs.com/developers/
- Local Documentation Copy: `docs/Documentation.html`
