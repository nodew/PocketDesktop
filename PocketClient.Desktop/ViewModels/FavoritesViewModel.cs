using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class FavoritesViewModel : ItemsViewModel, IRecipient<ItemFavoriteStatusChangedMessage>
{
    public FavoritesViewModel(): base()
    {
        WeakReferenceMessenger.Default.Register<ItemFavoriteStatusChangedMessage>(this);
    }

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
            if (Selected != null && message.Item.Id == Selected.Id)
            {
                Selected = null;
            }

            await RefreshList();
        });
    }
}
