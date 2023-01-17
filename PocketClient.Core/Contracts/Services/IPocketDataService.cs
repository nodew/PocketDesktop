using PocketClient.Core.Contracts.Specifications;
using PocketClient.Core.Models;

namespace PocketClient.Core.Contracts.Services;

public interface IPocketDataService
{
    public Task<PocketItem?> GetItemByIdAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<PocketItem>> GetItemsAsync(IBaseSpecification<PocketItem> specification, CancellationToken cancellationToken = default);

    public Task<PocketItem> AddItemAsync(Uri uri, string? title = null, CancellationToken cancellationToken = default);

    public Task FavoriteItemAsync(PocketItem item, CancellationToken cancellationToken = default);

    public Task UnfavoriteItemAsync(PocketItem item, CancellationToken cancellationToken = default);

    public Task ArchiveItemAsync(PocketItem item, CancellationToken cancellationToken = default);

    public Task ReAddItemAsync(PocketItem item, CancellationToken cancellationToken = default);

    public Task RemoveItemAsync(PocketItem item, CancellationToken cancellationToken = default);

    public Task UpdateItemTags(PocketItem item, List<Tag> tags, CancellationToken cancellationToken = default);

    public Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default);

    public Task RenameTagAsync(Tag tag, string newName, CancellationToken cancellationToken = default);

    public Task RemoveTagAsync(Tag tag, CancellationToken cancellationToken = default);

    public Task<Tag?> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Tag?> GetTagByNameAsync(string name, CancellationToken cancellationToken = default);
}
