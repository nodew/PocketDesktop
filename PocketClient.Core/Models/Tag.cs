namespace PocketClient.Core.Models;

public class Tag : Entity<Guid>
{
    public string Name
    {
        get; set;
    }

    public bool IsPinned
    {
        get; set;
    }

    public List<PocketItem> Items
    {
        get; set;
    }

    public List<ItemTag> ItemTags
    {
        get; set;
    }
}
