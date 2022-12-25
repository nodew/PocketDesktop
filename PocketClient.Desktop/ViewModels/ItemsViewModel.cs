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

public class ItemsViewModel : ObservableRecipient, IRecipient<SyncedItemsMessage>, INavigationAware
{
    private PocketItemOrderOption _orderOption;
    private PocketItem? _selected;

    public ItemsViewModel()
    {
        _orderOption = PocketItemOrderOption.Newest;

        RefreshListCommand = new AsyncRelayCommand(RefreshList);

        WeakReferenceMessenger.Default.Register<SyncedItemsMessage>(this);
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

    public ICommand RefreshListCommand
    {
        get; set;
    }

    public async void OnNavigatedTo(object parameter)
    {
        await NavigatedTo(parameter);
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        if (Selected == null && Items.Count > 0)
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
        await RefreshList();
    }

    public async Task RefreshList()
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
            await RefreshList();
        });
    }
}
