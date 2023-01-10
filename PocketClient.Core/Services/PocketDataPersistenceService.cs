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
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddOrUpdateItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        var _tags = new List<Tag>();
        var _authors = new List<Author>();

        var _item = await GetItemByIdWithTrackingAsync(item.Id, cancellationToken);

        foreach (var tag in item.Tags)
        {
            var _tag = await _dbContext.Tags.Where(t => t.Name == tag.Name).AsTracking().FirstOrDefaultAsync(cancellationToken);

            if (_tag == null)
            {
                _tag = new Tag { Id = Guid.NewGuid(), Name = tag.Name, IsPinned = false };
                _dbContext.Tags.Add(_tag);
                _tags.Add(_tag);
            }
            else
            {
                _tags.Add(_tag);
            }
        }

        foreach (var author in item.Authors)
        {
            var _author = await _dbContext.Authors.Where(t => t.Id == author.Id).AsTracking().FirstOrDefaultAsync(cancellationToken);

            if (_author == null)
            {
                _authors.Add(author);
            }
            else
            {
                _authors.Add(_author);
            }
        }

        if (_item == null)
        {
            item.Tags.Clear();
            item.Tags.AddRange(_tags);

            item.Authors.Clear();
            item.Authors.AddRange(_authors);

            _dbContext.Items.Add(item);
        }
        else
        {
            _item.Title = item.Title;
            _item.Url = item.Url;
            _item.Excerpt = item.Excerpt;
            _item.IsFavorited = item.IsFavorited;
            _item.IsArchived = item.IsArchived;
            _item.TimeAdded = item.TimeAdded;
            _item.TimeUpdated = item.TimeUpdated;
            _item.TimeFavorited = item.TimeFavorited;
            _item.TimeRead = item.TimeRead;
            _item.HasImage = item.HasImage;
            _item.TopImageUrl = item.TopImageUrl;
            _item.WordCount = item.WordCount;
            _item.TimeToRead = item.TimeToRead;
            _item.Lang = item.Lang;
            _item.Domain = item.Domain;

            _item.Authors.Clear();
            _item.Authors.AddRange(_authors);

            _item.Tags.Clear();
            _item.Tags.AddRange(_tags);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateItemAsync(PocketItem item, CancellationToken cancellationToken = default)
    {
        var _item = await GetItemByIdWithTrackingAsync(item.Id, cancellationToken);

        if (_item == null)
        {
            throw new Exception("Item is not found");
        }

        _item.Title = item.Title;
        _item.Url = item.Url;
        _item.Excerpt = item.Excerpt;
        _item.IsFavorited = item.IsFavorited;
        _item.IsArchived = item.IsArchived;
        _item.TimeAdded = item.TimeAdded;
        _item.TimeUpdated = item.TimeUpdated;
        _item.TimeFavorited = item.TimeFavorited;
        _item.TimeRead = item.TimeRead;
        _item.HasImage = item.HasImage;
        _item.TopImageUrl = item.TopImageUrl;
        _item.WordCount = item.WordCount;
        _item.TimeToRead = item.TimeToRead;
        _item.Lang = item.Lang;
        _item.Domain = item.Domain;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateItemTagsAsync(long itemId, List<Tag> tags, CancellationToken cancellationToken = default)
    {
        var _tags = new List<Tag>();

        var _item = await GetItemByIdWithTrackingAsync(itemId, cancellationToken);
        if (_item == null)
        {
            throw new Exception("Item is not found");
        }

        foreach (var tag in tags)
        {
            var _tag = await _dbContext.Tags.Where(t => t.Name == tag.Name).AsTracking().FirstOrDefaultAsync();

            if (_tag == null)
            {
                _tag = new Tag { Id = Guid.NewGuid(), Name = tag.Name, IsPinned = false };
                _dbContext.Tags.Add(_tag);
                _tags.Add(_tag);
            }
            else
            {
                _tags.Add(_tag);
            }
        }

        _item.Tags.Clear();
        _item.Tags.AddRange(_tags);

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

    private async Task<PocketItem?> GetItemByIdWithTrackingAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Items
            .Include(item => item.Tags)
            .Include(item => item.Authors)
            .Where(item => item.Id == id)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
}
