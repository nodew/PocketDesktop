using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class ArchiveViewModel : ItemsViewModel, IRecipient<ItemArchiveStatusChangedMessage>
{
    protected override BaseSpecification<PocketItem> BuildFilter() {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsArchived == true);
        return filter;
    }

    public void Receive(ItemArchiveStatusChangedMessage message)
    {
        if (!message.Item.IsArchived)
        {
            if (Selected != null && message.Item.Id == Selected.Id)
            {
                if (ShowListAndDetails)
                {
                    SelectNextItem(message.Item);
                } 
                else
                {
                    Selected = null;
                }
            } 

            RemoveItem(message.Item);
        }
    }
}
