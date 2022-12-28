using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class FavoritesViewModel : ItemsViewModel, IRecipient<ItemFavoriteStatusChangedMessage>
{
    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsFavorited == true);
        return filter;
    }

    public void Receive(ItemFavoriteStatusChangedMessage message)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, async () =>
        {
            long? nextItemId = null;

            if (message.Item.IsFavorited == false)
            {
                if (Selected != null && message.Item.Id == Selected.Id)
                {
                    SelectNextItem(message.Item);
                }
                
                RemoveItem(message.Item);
            }
        });
    }
}
