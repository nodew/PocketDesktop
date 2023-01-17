using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Contracts.Specifications;
using PocketClient.Core.Models;
using PocketClient.Core.Helpers;
using PocketClient.HttpSdk;

using PocketItem = PocketClient.Core.Models.PocketItem;

namespace PocketClient.Core.Services;

public class PocketDataService : IPocketDataService
{
    private readonly IPocketDataPersistenceService _persistenceService;
    private readonly PocketHttpClient _pocketHttpClient;

    public PocketDataService(
        IPocketDataPersistenceService persistenceService,
        PocketHttpClient pocketHttpClient)
    {
        _persistenceService = persistenceService;
        _pocketHttpClient = pocketHttpClient;
    }

    public Task<List<PocketItem>> GetItemsAsync(IBaseSpecification<PocketItem> specification, CancellationToken cancellationToken = default)
    {
        return _persistenceService.GetItemsAsync(specification, cancellationToken);
    }

    public Task<PocketItem?> GetItemByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return _persistenceService.GetItemByIdAsync(id, cancellationToken);
    }

    public async Task<PocketItem> AddItemAsync(Uri uri, string? title = null, CancellationToken cancellationToken = default)
    {
        var rawItem = await _pocketHttpClient.AddItemAsync(uri, title, cancellationToken);
        var normalizedItem = PocketItemHelper.NormalizeRawPocketItem(rawItem);
        normalizedItem.TimeAdded = DateTime.Now;
        await _persistenceService.AddOrUpdateItemAsync(normalizedItem, cancellationToken);
        return normalizedItem;
    }

    public async Task FavoriteItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        item.IsFavorited = true;
        item.TimeFavorited = DateTime.Now;
        await _pocketHttpClient.FavoriteItemAsync(item.Id, cancellationToken);
        await _persistenceService.UpdateItemAsync(item, cancellationToken);
    }

    public async Task UnfavoriteItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        item.IsFavorited = false;
        await _pocketHttpClient.UnfavoriteItemAsync(item.Id, cancellationToken);
        await _persistenceService.UpdateItemAsync(item, cancellationToken);
    }

    public async Task ArchiveItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        item.IsArchived = true;
        await _pocketHttpClient.ArchiveItemAsync(item.Id, cancellationToken);
        await _persistenceService.UpdateItemAsync(item, cancellationToken);
    }

    public async Task ReAddItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        item.IsArchived = false;
        item.TimeAdded = DateTime.Now;
        await _pocketHttpClient.ReAddItemAsync(item.Id, cancellationToken);
        await _persistenceService.UpdateItemAsync(item, cancellationToken);
    }

    public async Task UpdateItemTags(PocketItem item, List<Tag> newTags, CancellationToken cancellationToken = default)
    {
        await _pocketHttpClient.ReplaceTagsAsync(item.Id, newTags.Select(tag => tag.Name).ToList(), cancellationToken);
        await _persistenceService.UpdateItemTagsAsync(item.Id, newTags, cancellationToken);
    }

    public async Task RemoveItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        await _pocketHttpClient.DeleteItemAsync(item.Id, cancellationToken);
        await _persistenceService.RemoveItemAsync(item.Id, cancellationToken);
    }

    public async Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        return await _persistenceService.GetAllTagsAsync(cancellationToken);
    }

    public async Task<Tag?> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _persistenceService.GetTagByIdAsync(id, cancellationToken);
    }

    public async Task<Tag?> GetTagByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _persistenceService.GetTagByNameAsync(name, cancellationToken);
    }

    public async Task RemoveTagAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await _pocketHttpClient.DeleteTagAsync(tag.Name, cancellationToken);
        await _persistenceService.RemoveTagAsync(tag, cancellationToken);
    }

    public async Task RenameTagAsync(Tag tag, string newName, CancellationToken cancellationToken = default)
    {
        await _pocketHttpClient.RenameTagAsync(tag.Name, newName, cancellationToken);
        await _persistenceService.RenameTagAsync(tag, newName, cancellationToken);
    }
}
