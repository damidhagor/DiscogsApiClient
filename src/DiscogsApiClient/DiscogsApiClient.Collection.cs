using DiscogsApiClient.Contract;
using DiscogsApiClient.Exceptions;
using DiscogsApiClient.QueryParameters;

namespace DiscogsApiClient;

public sealed partial class DiscogsApiClient
{
    /// <inheritdoc/>
    public async Task<CollectionFoldersResponse> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFoldersUrl, username);
        return await GetAsync<CollectionFoldersResponse>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CollectionFolder> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        return await GetAsync<CollectionFolder>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CollectionFolder> CreateCollectionFolderAsync(string username, string folderName, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (string.IsNullOrWhiteSpace(folderName))
            throw new ArgumentException(nameof(folderName));

        var url = string.Format(DiscogsApiUrls.CollectionFoldersUrl, username);
        var content = new { Name = folderName };
        return await PostAsync<CollectionFolder>(url, content, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CollectionFolder> UpdateCollectionFolderAsync(string username, int folderId, string folderName, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (string.IsNullOrWhiteSpace(folderName))
            throw new ArgumentException(nameof(folderName));

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        var content = new { Name = folderName };
        return await PostAsync<CollectionFolder>(url, content, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderUrl, username, folderId);
        return await DeleteAsync(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CollectionFolderReleasesResponse> GetCollectionFolderReleasesAsync(string username, int folderId, PaginationQueryParameters paginationQueryParameters, CollectionFolderReleaseSortQueryParameters collectionFolderReleaseSortQueryParameters, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderReleasesUrl, username, folderId)
            .AppendQueryParameters(paginationQueryParameters, collectionFolderReleaseSortQueryParameters);
        return await GetAsync<CollectionFolderReleasesResponse>(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CollectionFolderRelease> AddReleaseToCollectionFolderAsync(string username, int folderId, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderAddReleaseUrl, username, folderId, releaseId);
        return await PostAsync<CollectionFolderRelease>(url, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteReleaseFromCollectionFolderAsync(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionFolderDeleteReleaseUrl, username, folderId, releaseId, instanceId);
        return await DeleteAsync(url, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<CollectionValue> GetCollectionValueAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthenticated)
            throw new UnauthorizedDiscogsException();
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        var url = string.Format(DiscogsApiUrls.CollectionValueUrl, username);
        return await GetAsync<CollectionValue>(url, cancellationToken);
    }
}
