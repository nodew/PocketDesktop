using Pocket.Client.Core.Contracts.Services;
using Pocket.Client.Core.Contracts.Specifications;
using Pocket.Core;
using Pocket.Client.Core.Models;
using Pocket.Client.Core.Helpers;

using PocketItem = Pocket.Client.Core.Models.PocketItem;

namespace Pocket.Client.Core.Services;

public class PocketDataService : IPocketDataService
{
    private readonly IPocketDataPersistenceService _persistenceService;
    private readonly PocketClient _pocketClient;

    public PocketDataService(
        IPocketDataPersistenceService persistenceService,
        PocketClient pocketClient)
    {
        _persistenceService = persistenceService;
        _pocketClient = pocketClient;
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
        var rawItem = await _pocketClient.AddItemAsync(uri, title, cancellationToken);
        var normalizedItem = PocketItemHelper.NormalizeRawPocketItem(rawItem);
        await _persistenceService.AddItemAsync(normalizedItem, cancellationToken);
        return normalizedItem;
    }

    public async Task FavoriteItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        item.IsFavorited = true;
        await _pocketClient.FavoriteItemAsync(item.Id, cancellationToken);
        await _persistenceService.UpdateItemAsync(item, cancellationToken);
    }

    public async Task UnfavoriteItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        item.IsFavorited = false;
        await _pocketClient.UnfavoriteItemAsync(item.Id, cancellationToken);
        await _persistenceService.UpdateItemAsync(item, cancellationToken);
    }

    public async Task ArchiveItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        item.IsArchived = true;
        await _pocketClient.ArchiveItemAsync(item.Id, cancellationToken);
        await _persistenceService.UpdateItemAsync(item, cancellationToken);
    }

    public async Task ReAddItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        item.IsArchived = false;
        await _pocketClient.ReAddItemAsync(item.Id, cancellationToken);
        await _persistenceService.UpdateItemAsync(item, cancellationToken);
    }

    public async Task UpdateItemTags(PocketItem item, List<Tag> newTags, CancellationToken cancellationToken = default)
    {
        var unChangedTags = new List<Tag>();
        var tagsToAdd = new List<Tag>();
        var tagsToRemove = new List<Tag>();
        foreach (var tag in item.Tags)
        {
            if (newTags.Find(tag => tag.Id == tag.Id) is not null)
            {
                unChangedTags.Add(tag);
            }
            else
            {
                tagsToRemove.Add(tag);
            }
        }

        foreach (var tag in newTags)
        {
            if (unChangedTags.Find(tag => tag.Id == tag.Id) is null)
            {
                tagsToAdd.Add(tag);
            }
        }

        if (tagsToRemove.Count > 0)
        {
            await _pocketClient.RemoveTagsAsync(item.Id, tagsToRemove.Select(item => item.Name).ToList(), cancellationToken);
        }
        
        if (tagsToAdd.Count > 0)
        {
            await _pocketClient.AddTagsAsync(item.Id, tagsToRemove.Select(item => item.Name).ToList(), cancellationToken);
        }

        await _persistenceService.UpdateItemAsync(item, newTags, cancellationToken);
    }

    public async Task RemoveItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        await _pocketClient.DeleteItemAsync(item.Id, cancellationToken);
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

    public async Task RemoveTagAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await _pocketClient.DeleteTagAsync(tag.Name, cancellationToken);
        await _persistenceService.RemoveTagAsync(tag, cancellationToken);
    }

    public async Task RenameTagAsync(Tag tag, string newName, CancellationToken cancellationToken = default)
    {
        await _pocketClient.RenameTagAsync(tag.Name, newName, cancellationToken);
        await _persistenceService.RenameTagAsync(tag, newName, cancellationToken);
    }
}
