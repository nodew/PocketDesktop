using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pocket.Client.Core.Contracts.Specifications;
using Pocket.Client.Core.Models;

namespace Pocket.Client.Core.Contracts.Services;

public interface IPocketDataPersistenceService
{
    #region Pocket items management
    public Task<PocketItem?> GetItemByIdAsync(long id, CancellationToken cancellationToken = default);

    public Task<List<PocketItem>> GetItemsAsync(IBaseSpecification<PocketItem> specification, CancellationToken cancellationToken = default);

    public Task AddItemAsync(PocketItem item, CancellationToken cancellationToken = default);

    public Task UpdateItemAsync(PocketItem item, CancellationToken cancellationToken = default);
    
    public Task UpdateItemAsync(PocketItem item, List<Tag> tags, CancellationToken cancellationToken = default);

    public Task RemoveItemAsync(PocketItem item, CancellationToken cancellationToken = default);

    #endregion

    #region Tags management
    public Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default);

    public Task<Tag> AddTagAsync(Tag tag, CancellationToken cancellationToken = default);

    public Task RenameTagAsync(Tag tag, string newName, CancellationToken cancellationToken = default);

    public Task RemoveTagAsync(Tag tag, CancellationToken cancellationToken = default);

    public Task<Tag?> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default);
    #endregion
}
