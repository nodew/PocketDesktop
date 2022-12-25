namespace PocketClient.Core.Models;

public class ItemAuthor
{
    public long ItemId
    {
        get; set;
    }

    public long AuthorId
    {
        get; set;
    }

    public PocketItem Item
    {
        get; set;
    }

    public Author Author
    {
        get; set;
    }
}
