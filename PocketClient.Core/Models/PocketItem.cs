namespace PocketClient.Core.Models;

public class PocketItem : Entity<long>
{
    public string Title
    {
        get; set;
    }

    public Uri Url
    {
        get; set;
    }

    public string? Excerpt
    {
        get; set;
    }

    public bool IsFavorited
    {
        get; set;
    }

    public bool IsArchived
    {
        get; set;
    }

    public DateTime TimeAdded
    {
        get; set;
    }

    public DateTime TimeUpdated
    {
        get; set;
    }

    public DateTime TimeRead
    {
        get; set;
    }

    public DateTime TimeFavorited
    {
        get; set;
    }

    public ItemType Type
    {
        get; set;
    }

    public bool HasImage
    {
        get; set;
    }

    public int WordCount
    {
        get; set;
    }

    public int TimeToRead
    {
        get; set;
    }

    public string? Lang
    {
        get; set;
    }

    public Uri? TopImageUrl
    {
        get; set;
    }

    public string Domain
    {
        get; set;
    }

    public List<Tag> Tags
    {
        get; set;
    }

    public List<ItemTag> ItemTags
    {
        get; set;
    }

    public List<Author> Authors
    {
        get; set;
    }

    public List<ItemAuthor> ItemAuthors
    {
        get; set;
    }
}

public enum ItemType
{
    Article,
    Video,
    Image
}
