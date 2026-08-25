namespace DiscogsApiClient;

/// <summary>
/// A client for accessing the Discogs API.
/// </summary>
public interface IDiscogsApiClient
{
    /// <summary>
    /// Queries the <see cref="Identity"/> of the currently authenticated user.
    /// </summary>
    Task<Identity> GetIdentity(CancellationToken cancellationToken);

    /// <summary>
    /// Queries the <see cref="User"/> object of the currently authenticated user.
    /// </summary>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<User> GetUser(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the profile of a user. Authentication as that user is required.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="request">The profile fields to change. Omitted (<see langword="null"/>) properties are left unchanged.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<User> UpdateUser(string username, UserProfileUpdateRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the collection folders of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<CollectionFoldersResponse> GetCollectionFolders(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a specific collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The id of the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    Task<CollectionFolder> GetCollectionFolder(string username, int folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderName">The new folder's name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    Task<CollectionFolder> CreateCollectionFolder(string username, string folderName, CancellationToken cancellationToken);

    /// <summary>
    /// Changes a collection folder's name for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="folderName">The new folder name.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or folder name is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    Task<CollectionFolder> UpdateCollectionFolder(string username, int folderId, string folderName, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a collection folder for the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    Task DeleteCollectionFolder(string username, int folderId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases in a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder id is invalid.</exception>
    Task<CollectionFolderReleasesResponse> GetCollectionItemsByFolder(string username, int folderId, PaginationQueryParameters? paginationQueryParameters, CollectionFolderReleaseSortQueryParameters? collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the collection folders of the user which contain a specified release, along with information about each release instance.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<CollectionFolderReleasesResponse> GetCollectionItemsByRelease(string username, int releaseId, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a release to the collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder or release id is invalid.</exception>
    Task<CollectionFolderRelease> AddReleaseToCollectionFolder(string username, int folderId, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a release from a collection folder of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The folder's id.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="instanceId">The release's instance id in the folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder, release or instance id is invalid.</exception>
    Task DeleteReleaseFromCollectionFolder(string username, int folderId, int releaseId, long instanceId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the estimated value of the user's collection.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<CollectionValue> GetCollectionValue(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the user-defined custom collection fields. These fields are available on every release in the collection.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<CollectionFieldsResponse> GetCollectionFields(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Changes the rating and/or moves a release instance to another folder in the user's collection.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The id of the folder the instance currently resides in.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="instanceId">The release's instance id in the folder.</param>
    /// <param name="rating">The new rating of the instance. Omitted (<see langword="null"/>) leaves the rating unchanged.</param>
    /// <param name="targetFolderId">The id of the folder to move the instance to. Omitted (<see langword="null"/>) leaves the instance in its current folder.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder, release, instance or target folder id is invalid.</exception>
    Task UpdateCollectionFolderRelease(string username, int folderId, int releaseId, long instanceId, int? rating, int? targetFolderId, CancellationToken cancellationToken);

    /// <summary>
    /// Changes the value of a custom collection field on a release instance in the user's collection.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="folderId">The id of the folder the instance currently resides in.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="instanceId">The release's instance id in the folder.</param>
    /// <param name="fieldId">The id of the field to change.</param>
    /// <param name="value">The new value of the field. If the field's type is dropdown, the value must match one of the field's options.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username or value is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the folder, release, instance or field id is invalid.</exception>
    Task UpdateCollectionFolderReleaseField(string username, int folderId, int releaseId, long instanceId, int fieldId, string value, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases on the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<WantlistReleasesResponse> GetWantlistReleases(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a release to the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<WantlistRelease> AddReleaseToWantlist(string username, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a release from the wantlist of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task DeleteReleaseFromWantlist(string username, int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the submissions of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<SubmissionsResponse> GetSubmissions(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the contributions of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <param name="contributionSortQueryParameters">Sorting parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<ContributionsResponse> GetContributions(string username, PaginationQueryParameters? paginationQueryParameters, ContributionSortQueryParameters? contributionSortQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the lists of the user.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="paginationQueryParameters">Pagination parameters for the results.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<UserListsResponse> GetUserLists(string username, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the details and items of a list.
    /// </summary>
    /// <param name="listId">The list's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the list id is invalid.</exception>
    Task<ListDetails> GetList(int listId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an artist from the Discogs database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the artist id is invalid.</exception>
    Task<Artist> GetArtist(int artistId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases of an artist from the Discogs database.
    /// </summary>
    /// <param name="artistId">The artist's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the artist id is invalid.</exception>
    Task<ArtistReleasesResponse> GetArtistReleases(int artistId, PaginationQueryParameters? paginationQueryParameters, ArtistReleaseSortQueryParameters? artistReleaseSortQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a label from the Discogs database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the label id is invalid.</exception>
    Task<Label> GetLabel(int labelId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the releases of a label from the Discogs database.
    /// </summary>
    /// <param name="labelId">The label's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the label id is invalid.</exception>
    Task<LabelReleasesResponse> GetLabelReleases(int labelId, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a master release from the Discogs database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<MasterRelease> GetMasterRelease(int masterReleaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the release versions of a master release from the Discogs database.
    /// </summary>
    /// <param name="masterReleaseId">The master release's id.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<MasterReleaseVersionsResponse> GetMasterReleaseVersions(int masterReleaseId, PaginationQueryParameters? paginationQueryParameters, MasterReleaseVersionFilterQueryParameters? masterReleaseVersionQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<Release> GetRelease(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a user's rating for a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<ReleaseRatingResponse> GetReleaseRating(int releaseId, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Sets a user's rating for a release in the Discogs database. Authentication as that user is required.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="username">The name of the user.</param>
    /// <param name="rating">The new rating between 1 and 5.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid or the rating is not between 1 and 5.</exception>
    Task<ReleaseRatingResponse> UpdateReleaseRating(int releaseId, string username, int rating, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a user's rating for a release in the Discogs database. Authentication as that user is required.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <param name="username">The name of the user.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task DeleteReleaseRating(int releaseId, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the community rating of a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRating(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets statistics of a release from the Discogs database.
    /// </summary>
    /// <param name="releaseId">The release's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<ReleaseStatsResponse> GetReleaseStats(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Queries the Discogs database for entries.
    /// </summary>
    /// <param name="searchQueryParameters">The search parameters to query for.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    Task<SearchResultsResponse> SearchDatabase(SearchQueryParameters searchQueryParameters, PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a Marketplace listing.
    /// </summary>
    /// <param name="listingId">The listing's id.</param>
    /// <param name="currencyQueryParameters">Currency parameters for the listing's price fields.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the listing id is invalid.</exception>
    Task<MarketplaceListing> GetMarketplaceListing(long listingId, MarketplaceCurrencyQueryParameters? currencyQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new Marketplace listing. The listing is added to the authenticated user's inventory. Authentication is required.
    /// </summary>
    /// <param name="request">The listing to create.</param>
    /// <exception cref="ArgumentNullException">Fires this exception if no request is provided.</exception>
    Task<MarketplaceListingCreateResponse> CreateMarketplaceListing(MarketplaceListingCreateRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing Marketplace listing. Authentication as the listing owner is required.
    /// </summary>
    /// <param name="listingId">The listing's id.</param>
    /// <param name="request">The updated listing data.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the listing id is invalid.</exception>
    /// <exception cref="ArgumentNullException">Fires this exception if no request is provided.</exception>
    Task UpdateMarketplaceListing(long listingId, MarketplaceListingUpdateRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Permanently deletes a Marketplace listing. Authentication as the listing owner is required.
    /// </summary>
    /// <param name="listingId">The listing's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the listing id is invalid.</exception>
    Task DeleteMarketplaceListing(long listingId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a seller's Marketplace inventory. If the caller is not authenticated as the inventory owner, only listings with a status of "For Sale" are returned.
    /// </summary>
    /// <param name="username">The name of the user whose inventory is being fetched.</param>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    /// <param name="inventoryQueryParameters">Filtering and sorting parameters for the inventory listings.</param>
    /// <exception cref="ArgumentException">Fires this exception if no username is provided.</exception>
    Task<MarketplaceInventoryResponse> GetInventory(string username, PaginationQueryParameters? paginationQueryParameters, MarketplaceInventoryQueryParameters? inventoryQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Calculates the Marketplace fee for selling an item at the given price, in the Marketplace's default
    /// currency (USD).
    /// </summary>
    /// <param name="price">The price to calculate a fee from.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the price is not positive.</exception>
    Task<MarketplacePrice> GetMarketplaceFee(decimal price, CancellationToken cancellationToken);

    /// <summary>
    /// Calculates the Marketplace fee for selling an item at the given price, converted into the given currency.
    /// </summary>
    /// <param name="price">The price to calculate a fee from.</param>
    /// <param name="currency">The currency to calculate the fee in.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the price is not positive.</exception>
    Task<MarketplacePrice> GetMarketplaceFee(decimal price, MarketplaceCurrency currency, CancellationToken cancellationToken);

    /// <summary>
    /// Gets price suggestions for a release, keyed by <see cref="MarketplaceListingCondition"/> display name
    /// (e.g. <c>"Mint (M)"</c>). Empty if no suggestions are available for the release. Authentication is required,
    /// and the authenticated user needs to have filled out their seller settings. Suggested prices are denominated
    /// in the user's selling currency.
    /// </summary>
    /// <param name="releaseId">The release's id to calculate price suggestions from.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<IReadOnlyDictionary<string, MarketplacePrice>> GetPriceSuggestions(int releaseId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets current Marketplace statistics for a release: the number of items currently for sale, the lowest listed
    /// price of any item for sale, and whether the release is blocked for sale in the marketplace.
    /// </summary>
    /// <param name="releaseId">The release's id whose statistics are desired.</param>
    /// <param name="currencyQueryParameters">Currency parameters for the lowest price field.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the release id is invalid.</exception>
    Task<MarketplaceStatsResponse> GetMarketplaceStats(int releaseId, MarketplaceCurrencyQueryParameters? currencyQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Requests an export of the authenticated user's inventory as a CSV file. Authentication is required.
    /// </summary>
    /// <returns>The id of the requested export. Use <see cref="GetInventoryExport"/> to poll its processing status.</returns>
    Task<long> CreateInventoryExport(CancellationToken cancellationToken);

    /// <summary>
    /// Gets a list of the authenticated user's recent inventory exports. Authentication is required.
    /// </summary>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    Task<InventoryExportListResponse> GetInventoryExports(PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the status of a requested inventory export. Authentication is required.
    /// </summary>
    /// <param name="exportId">The export's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the export id is invalid.</exception>
    Task<InventoryExport> GetInventoryExport(long exportId, CancellationToken cancellationToken);

    /// <summary>
    /// Downloads the CSV file generated by a finished inventory export as a readable <see cref="Stream"/>. Authentication is required.
    /// </summary>
    /// <param name="exportId">The export's id.</param>
    /// <returns>A stream of the exported inventory's CSV content. The caller is responsible for disposing it.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the export id is invalid.</exception>
    Task<Stream> DownloadInventoryExportAsStream(long exportId, CancellationToken cancellationToken);

    /// <summary>
    /// Downloads the CSV file generated by a finished inventory export. Authentication is required.
    /// </summary>
    /// <param name="exportId">The export's id.</param>
    /// <returns>The exported inventory's CSV content.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the export id is invalid.</exception>
    Task<byte[]> DownloadInventoryExportAsBytes(long exportId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a list of the authenticated user's recent inventory uploads. Authentication is required.
    /// </summary>
    /// <param name="paginationQueryParameters">Pagination parameters.</param>
    Task<InventoryUploadListResponse> GetInventoryUploads(PaginationQueryParameters? paginationQueryParameters, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the status of a requested inventory upload. Authentication is required.
    /// </summary>
    /// <param name="uploadId">The upload's id.</param>
    /// <exception cref="ArgumentOutOfRangeException">Fires this exception if the upload id is invalid.</exception>
    Task<InventoryUpload> GetInventoryUpload(long uploadId, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a CSV of listing ids to permanently delete from the authenticated user's inventory. Authentication is required.
    /// See the <see href="https://www.discogs.com/developers/#page:inventory-upload,header:inventory-upload-delete-inventory">Delete inventory</see>
    /// documentation for the required CSV format.
    /// </summary>
    /// <param name="csvContent">The CSV file content listing the ids of the listings to delete.</param>
    /// <returns>The id of the created upload. Use <see cref="GetInventoryUpload"/> to poll its processing status.</returns>
    /// <exception cref="ArgumentNullException">Fires this exception if <paramref name="csvContent"/> is null.</exception>
    Task<long> DeleteInventoryListings(byte[] csvContent, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a CSV of listing ids to permanently delete from the authenticated user's inventory. Authentication is required.
    /// See the <see href="https://www.discogs.com/developers/#page:inventory-upload,header:inventory-upload-delete-inventory">Delete inventory</see>
    /// documentation for the required CSV format.
    /// </summary>
    /// <param name="csvContent">The CSV file content listing the ids of the listings to delete.</param>
    /// <returns>The id of the created upload. Use <see cref="GetInventoryUpload"/> to poll its processing status.</returns>
    /// <exception cref="ArgumentNullException">Fires this exception if <paramref name="csvContent"/> is null.</exception>
    Task<long> DeleteInventoryListings(Stream csvContent, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a CSV of listings to add to the authenticated user's inventory as new, immediately for-sale listings.
    /// Authentication is required. See the
    /// <see href="https://www.discogs.com/developers/#page:inventory-upload,header:inventory-upload-add-inventory">Add inventory</see>
    /// documentation for the required CSV format.
    /// </summary>
    /// <param name="csvContent">The CSV file content listing the new listings to add.</param>
    /// <returns>The id of the created upload. Use <see cref="GetInventoryUpload"/> to poll its processing status.</returns>
    /// <exception cref="ArgumentNullException">Fires this exception if <paramref name="csvContent"/> is null.</exception>
    Task<long> AddInventoryListings(byte[] csvContent, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a CSV of listings to add to the authenticated user's inventory as new, immediately for-sale listings.
    /// Authentication is required. See the
    /// <see href="https://www.discogs.com/developers/#page:inventory-upload,header:inventory-upload-add-inventory">Add inventory</see>
    /// documentation for the required CSV format.
    /// </summary>
    /// <param name="csvContent">The CSV file content listing the new listings to add.</param>
    /// <returns>The id of the created upload. Use <see cref="GetInventoryUpload"/> to poll its processing status.</returns>
    /// <exception cref="ArgumentNullException">Fires this exception if <paramref name="csvContent"/> is null.</exception>
    Task<long> AddInventoryListings(Stream csvContent, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a CSV of existing listing ids and the fields to change on them in the authenticated user's inventory.
    /// Authentication is required. See the
    /// <see href="https://www.discogs.com/developers/#page:inventory-upload,header:inventory-upload-change-inventory">Change inventory</see>
    /// documentation for the required CSV format.
    /// </summary>
    /// <param name="csvContent">The CSV file content listing the listings to change and their new field values.</param>
    /// <returns>The id of the created upload. Use <see cref="GetInventoryUpload"/> to poll its processing status.</returns>
    /// <exception cref="ArgumentNullException">Fires this exception if <paramref name="csvContent"/> is null.</exception>
    Task<long> ChangeInventoryListings(byte[] csvContent, CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a CSV of existing listing ids and the fields to change on them in the authenticated user's inventory.
    /// Authentication is required. See the
    /// <see href="https://www.discogs.com/developers/#page:inventory-upload,header:inventory-upload-change-inventory">Change inventory</see>
    /// documentation for the required CSV format.
    /// </summary>
    /// <param name="csvContent">The CSV file content listing the listings to change and their new field values.</param>
    /// <returns>The id of the created upload. Use <see cref="GetInventoryUpload"/> to poll its processing status.</returns>
    /// <exception cref="ArgumentNullException">Fires this exception if <paramref name="csvContent"/> is null.</exception>
    Task<long> ChangeInventoryListings(Stream csvContent, CancellationToken cancellationToken);
}
