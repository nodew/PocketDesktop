using CommunityToolkit.Mvvm.Messaging;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class FavoritesViewModel : ItemsViewModel, IRecipient<ItemFavoriteStatusChangedMessage>
{
    public List<PocketItemFilterOption> HiddenOptions = new() { PocketItemFilterOption.Favorited };

    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = new BaseSpecification<PocketItem>();

        if (OrderOption == PocketItemOrderOption.Newest)
        {
            filter.ApplyOrderByDescending(item => item.TimeFavorited);
        }
        else if (OrderOption == PocketItemOrderOption.Oldest)
        {
            filter.ApplyOrderBy(item => item.TimeFavorited);
        }

        if (FilterOption == PocketItemFilterOption.All)
        {
            filter.SetFilterCondition(item => item.IsFavorited == true);
        }
        else if (FilterOption == PocketItemFilterOption.UnArchived)
        {
            filter.SetFilterCondition(item => item.IsFavorited == true && item.IsArchived == false);
        } 
        else if (FilterOption == PocketItemFilterOption.Archived)
        {
            filter.SetFilterCondition(item => item.IsFavorited == true && item.IsArchived == true);
        }
        
        return filter;
    }

    public void Receive(ItemFavoriteStatusChangedMessage message)
    {
        if (!message.Item.IsFavorited)
        {
            UpdateSelectedItem(message.Item);
            RemoveItem(message.Item);
        }
    }
}
