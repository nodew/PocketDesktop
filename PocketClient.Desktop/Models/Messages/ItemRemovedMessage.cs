using PocketClient.Core.Models;

namespace PocketClient.Desktop.Models;

public class ItemRemovedMessage
{
    public ItemRemovedMessage(PocketItem item)
    {
        Item = item;
    }

    public PocketItem Item
    {
        get;
        set;
    }
}
