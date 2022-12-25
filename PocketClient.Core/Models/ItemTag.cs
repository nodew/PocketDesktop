namespace PocketClient.Core.Models;

public class ItemTag
{
    public long ItemId
    {
        get; set;
    }

    public Guid TagId
    {
        get; set;
    }

    public PocketItem Item
    {
        get; set;
    }

    public Tag Tag
    {
        get; set;
    }
}
