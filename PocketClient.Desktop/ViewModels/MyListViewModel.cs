using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class MyListViewModel : ItemsViewModel, IRecipient<ItemArchiveStatusChangedMessage>
{
    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsArchived == false) ;
        return filter;
    }

    public void Receive(ItemArchiveStatusChangedMessage message)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, async () =>
        {
            if (message.Item.IsArchived)
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
