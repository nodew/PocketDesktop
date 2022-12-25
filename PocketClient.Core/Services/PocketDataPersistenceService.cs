using Microsoft.EntityFrameworkCore;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Contracts.Specifications;
using PocketClient.Core.Data;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;

namespace PocketClient.Core.Services;

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
        var tags = item.Tags.ToList();
        var authors = item.Authors.ToList();

        item.Tags.Clear();
        item.Authors.Clear();

        foreach(var tag in tags)
        {
            var _tag = _dbContext.Tags.Where(t => t.Name == tag.Name).FirstOrDefault();

            if (_tag == null)
            {
                item.Tags.Add(new Tag { Id = Guid.NewGuid(), Name = tag.Name, IsPinned = false });
            }
            else
            {
                _dbContext.Attach(_tag);
                item.Tags.Add(_tag);
            }
        }

        foreach (var author in authors)
        {
            var _author = _dbContext.Authors.Where(t => t.Id == author.Id).FirstOrDefault();

            if (_author == null)
            {
                item.Authors.Add(author);
            }
            else
            {
                _dbContext.Attach(_author);
                item.Authors.Add(_author);
            }
        }

        var _item = _dbContext.Items.Where(t => t.Id == item.Id).FirstOrDefault();

        if (_item == null)
        {
            _dbContext.Items.Add(item);
        }
        else
        {
            _dbContext.Items.Update(item);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        _dbContext.Items.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateItemAsync(PocketItem item, List<Tag> tags, CancellationToken cancellationToken = default)
    {
        var _tags = new List<Tag>();

        foreach (var tag in item.Tags)
        {
            var _tag = _dbContext.Tags.Where(t => t.Name == tag.Name).FirstOrDefault();

            if (_tag == null)
            {
                _tags.Add(new Tag { Id = Guid.NewGuid(), Name = tag.Name });
            }
            else
            {
                _dbContext.Attach(_tag);
                _tags.Add(_tag);
            }
        }

        item.Tags.Clear();
        item.Tags.AddRange(tags);
        _dbContext.Items.Update(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveItemAsync(long itemId, CancellationToken cancellationToken = default)
    {
        var item = _dbContext.Items
            .Include(t => t.ItemAuthors)
            .Include(t => t.Tags)
            .Where(t => t.Id == itemId).FirstOrDefault();

        if (item != null)
        {
            _dbContext.Items.Remove(item);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
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
        var _tag = _dbContext.Tags.Where(tag => tag.Name == tag.Name).FirstOrDefault();

        if (_tag == null)
        {
            _tag = new Tag { Id = Guid.NewGuid(), Name = tag.Name };
            _dbContext.Tags.Add(_tag);
        }

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

    public async Task ClearDbAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContext.ItemTags.Any())
        {
            _dbContext.ItemTags.RemoveRange(_dbContext.ItemTags.ToList());
        }

        if (_dbContext.ItemAuthors.Any())
        {
            _dbContext.ItemAuthors.RemoveRange(_dbContext.ItemAuthors.ToList());
        }

        if (_dbContext.Tags.Any())
        {
            _dbContext.Tags.RemoveRange(_dbContext.Tags.ToList());
        }

        if (_dbContext.Items.Any())
        {
            _dbContext.Items.RemoveRange(_dbContext.Items.ToList());
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
