﻿using CommunityToolkit.Mvvm.Messaging;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class FavoritesViewModel : ItemsViewModel, IRecipient<ItemFavoriteStatusChangedMessage>
{
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

        filter.SetFilterCondition(item => item.IsFavorited == true);
        
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
