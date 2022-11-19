using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pocket.Client.Core.Contracts.Services;
using Pocket.Client.Core.Contracts.Specifications;
using Pocket.Client.Core.Data;
using Pocket.Client.Core.Models;
using Pocket.Client.Core.Specifications;

namespace Pocket.Client.Core.Services;

public class PocketDataPersistenceService : IPocketDataPersistenceService
{
    private readonly PocketDbContext _dbContext;
    
    public PocketDataPersistenceService(PocketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region Pocket items management
    public async Task<List<PocketItem>> GetItemsAsync(IBaseSpecification<PocketItem> specification, CancellationToken cancellationToken = default)
    {
        specification = specification ?? throw new ArgumentNullException(nameof(specification));

        var baseQuery = _dbContext.Items
            .Include(item => item.Tags)
            .Include(item => item.Authors)
            .AsQueryable();

        var query = SpecificationEvaluator<PocketItem>.GetQuery(baseQuery, specification);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
    
    public async Task<PocketItem?> GetItemByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Items
            .Include(item => item.Tags)
            .Include(item => item.Authors)
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task AddItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        SaveItemAuthors(item);
        SaveItemTags(item);
        _dbContext.Items.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        _dbContext.Items.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateItemAsync(PocketItem item, List<Tag> tags, CancellationToken cancellationToken = default)
    {
        item.Tags = tags;
        SaveItemTags(item);
        _dbContext.Items.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        var itemAuthors = _dbContext.ItemAuthors.Where(itemAuthor => itemAuthor.ItemId == item.Id);
        var itemTags = _dbContext.ItemTags.Where(itemTag => itemTag.ItemId == item.Id);
        _dbContext.ItemAuthors.RemoveRange(itemAuthors);
        _dbContext.ItemTags.RemoveRange(itemTags);
        _dbContext.Items.Remove(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Tags management
    public async Task<List<Tag>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tags.ToListAsync(cancellationToken);
    }

    public async Task<Tag?> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tags
            .Where(tag => tag.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<Tag> AddTagAsync(Tag tag, CancellationToken cancellationToken = default) 
    {
        UpsertTag(tag);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return tag;
    }

    public async Task RemoveTagAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        _dbContext.ItemTags.RemoveRange(_dbContext.ItemTags.Where(itemTag => itemTag.TagId == tag.Id));
        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RenameTagAsync(Tag tag, string newName, CancellationToken cancellationToken = default)
    {
        var _tag = _dbContext.Tags.Where(tag => tag.Name == tag.Name).FirstOrDefault();

        if (_tag == null)
        {
            throw new Exception("Tag is not found");
        }

        _tag.Name = newName;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion
    
    private void SaveItemTags(PocketItem item)
    {
        var itemTags = _dbContext.ItemTags.Where(itemTag => itemTag.ItemId == item.Id);
        _dbContext.ItemTags.RemoveRange(itemTags);

        foreach (var tag in item.Tags)
        {
            var _tag = UpsertTag(tag);
            _dbContext.ItemTags.Add(new ItemTag { ItemId = item.Id, TagId = _tag.Id });
        }
    }
    
    private void SaveItemAuthors(PocketItem item)
    {
        var itemAuthors = _dbContext.ItemAuthors.Where(itemAuthor => itemAuthor.ItemId == item.Id);
        _dbContext.ItemAuthors.RemoveRange(itemAuthors);

        foreach (var author in item.Authors)
        {
            UpsertAuthor(author);
            _dbContext.ItemAuthors.Add(new ItemAuthor { ItemId = item.Id, Author = author });
        }
    }

    private Tag UpsertTag(Tag tag)
    {
        var _tag = _dbContext.Tags.Where(tag => tag.Name == tag.Name).FirstOrDefault();

        if (_tag == null)
        {
            _tag = new Tag { Id = Guid.NewGuid(), Name = tag.Name };
            _dbContext.Tags.Add(_tag);
        }

        return _tag;
    }

    private void UpsertAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }

        var _author = _dbContext.Authors.Where(a => a.Id == author.Id).FirstOrDefault();

        if (_author != null)
        {
            _dbContext.Authors.Update(author);
        }
        else 
        { 
            _dbContext.Authors.Add(author);
        }
    }
}
