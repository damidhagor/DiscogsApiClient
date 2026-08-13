### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
DISCOGS001 | DiscogsApiClient.SourceGenerator | Warning | Unsupported query parameter property type
DISCOGS002 | DiscogsApiClient.SourceGenerator | Error | API method must return Task or Task<T>
DISCOGS003 | DiscogsApiClient.SourceGenerator | Error | API method has no CancellationToken parameter
DISCOGS004 | DiscogsApiClient.SourceGenerator | Error | Route parameter not found in method parameters
DISCOGS005 | DiscogsApiClient.SourceGenerator | Warning | Enum has no members
DISCOGS006 | DiscogsApiClient.SourceGenerator | Warning | Unknown HTTP method
DISCOGS007 | DiscogsApiClient.SourceGenerator | Error | Duplicate [Body] parameter
DISCOGS008 | DiscogsApiClient.SourceGenerator | Error | API client class must be partial
DISCOGS009 | DiscogsApiClient.SourceGenerator | Error | API client class has no HttpClient member
DISCOGS010 | DiscogsApiClient.SourceGenerator | Error | API client class has no JsonSerializerContext member
DISCOGS011 | DiscogsApiClient.SourceGenerator | Error | API method must be partial
