﻿using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class ArchiveViewModel : ItemsViewModel, IRecipient<ItemArchiveStatusChangedMessage>
{
    public ArchiveViewModel() : base()
    {
        WeakReferenceMessenger.Default.Register<ItemArchiveStatusChangedMessage>(this);
    }

    protected override BaseSpecification<PocketItem> BuildFilter() {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsArchived == true);
        return filter;
    }

    public void Receive(ItemArchiveStatusChangedMessage message)
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
