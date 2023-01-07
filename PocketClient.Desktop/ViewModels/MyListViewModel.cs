using CommunityToolkit.Mvvm.Messaging;
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
        if (message.Item.IsArchived)
        {
            UpdateSelectedItem(message.Item);
            RemoveItem(message.Item);
        }
    }
}
