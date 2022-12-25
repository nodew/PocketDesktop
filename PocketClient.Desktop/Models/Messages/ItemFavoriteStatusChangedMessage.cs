using PocketClient.Core.Models;

namespace PocketClient.Desktop.Models;

public class ItemFavoriteStatusChangedMessage
{
    public ItemFavoriteStatusChangedMessage(PocketItem item)
    {
        Item = item;
    }

    public PocketItem Item
    {
        get; set; 
    }
}
