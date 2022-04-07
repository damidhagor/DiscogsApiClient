# DiscogsApiClient
This is a C# .Net6 library for accessing the [Discogs API v2.0](https://www.discogs.com/developers).

only be used authenticated


## Implemented Api Functions

The current implementation of the Api surface is focused on querying the database and accessing the user's collection and wantlist. 

**User resources**
- Get user information
- Collection
  - Get all collection folders
  - Get, Create, Update, Delete collection folders
  - Get releases in a collection folder
  - Add, Remove releases from a collection folder
- Wantlist
  - Get releases on the wantlist
  - Add, Delete releases to/from the wantlist

**Database resources**
- Discog database search
- Get master release
- Get artist & artist's releases
- Get release & release's community rating
- Get label & label's releases


## Authentication

The DiscogsApiClient supports authentication by either using a personal access token or the full OAuth 1.0a auth flow.

**Personal access tokens** are the easiest way to make authenticated requests since it only requires the user to generate an access token in the ''Development'' section of their profile settings.

The **OAuth Flow** on the other hand allows the user to log in with their Discogs credentials directly in the application and authorize it to make requests on the user's behalf.

pros/cons
examples