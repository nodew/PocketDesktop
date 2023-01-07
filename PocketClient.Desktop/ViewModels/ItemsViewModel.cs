using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Contracts.ViewModels;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class ItemsViewModel : ObservableRecipient, IRecipient<SyncedItemsMessage>, IRecipient<ItemRemovedMessage>, INavigationAware
{
    private PocketItemOrderOption _orderOption;
    private PocketItem? _selected;
    private bool _showListAndDetails;

    public ItemsViewModel()
    {
        _orderOption = PocketItemOrderOption.Newest;

        RefreshListCommand = new AsyncRelayCommand(RefreshListAsync);
    }

    public ObservableCollection<PocketItem> Items = new();

    public PocketItemOrderOption OrderOption
    {
        get => _orderOption;
        set => SetProperty(ref _orderOption, value);
    }

    public PocketItem? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public bool ShowListAndDetails
    {
        get => _showListAndDetails;
        set => _showListAndDetails = value;
    }

    public bool HasItems => Items.Count > 0;

    public ICommand RefreshListCommand
    {
        get; set;
    }

    public async void OnNavigatedTo(object parameter)
    {
        IsActive = true;
        await NavigatedTo(parameter);
    }

    public void OnNavigatedFrom()
    {
        IsActive = false;
    }

    public void EnsureItemSelected()
    {
        if (ShowListAndDetails && Selected == null && Items.Count > 0)
        {
            Selected = Items.First();
        }
    }

    protected virtual BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = new BaseSpecification<PocketItem>();

        if (OrderOption == PocketItemOrderOption.Newest)
        {
            filter.ApplyOrderByDescending(item => item.TimeAdded);
        }
        else if (OrderOption == PocketItemOrderOption.Oldest)
        {
            filter.ApplyOrderBy(item => item.TimeAdded);
        }

        return filter;
    }

    protected async virtual Task NavigatedTo(object parameter)
    {
        await RefreshListAsync();
    }

    public async Task RefreshListAsync()
    {
        var filter = BuildFilter();
        var items = await App.GetService<IPocketDataService>().GetItemsAsync(filter);

        Items.Clear();

        foreach(var item in items )
        {
            Items.Add(item);
        }
    }

    public void Receive(SyncedItemsMessage message)
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, async () =>
        {
            var currentSelected = Selected;

            await RefreshListAsync();

            if (currentSelected != null)
            {
                Selected = Items.Where(item => item.Id == currentSelected.Id).FirstOrDefault();
            }

            OnPropertyChanged(nameof(HasItems));
        });
    }

    public void Receive(ItemRemovedMessage message)
    {
        UpdateSelectedItem(message.Item);
        RemoveItem(message.Item);
    }

    public void RemoveItem(PocketItem item)
    {
        Items.Remove(item);
        OnPropertyChanged(nameof(HasItems));
    }

    public void UpdateSelectedItem(PocketItem item)
    {
        if (Selected != null && item.Id == Selected.Id)
        {
            if (ShowListAndDetails)
            {
                SelectNextItem(item);
            }
            else
            {
                Selected = null;
            }
        }
    }

    public void SelectNextItem(PocketItem item)
    {
        var idx = Items.ToList().FindIndex(e => e.Id == item.Id);

        if (idx == -1)
        {
            return;
        }

        // if current item is not the last item, ant the next item is available, select next item
        // if current item is the last item, and the previous item is available, select previous item
        if (idx < Items.Count - 1)
        {
            Selected = Items[idx + 1];
        }
        else if (idx - 1 >= 0)
        {
            Selected = Items[idx - 1];
        }
    }
}
