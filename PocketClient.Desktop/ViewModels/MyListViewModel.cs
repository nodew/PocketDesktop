using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class MyListViewModel : ItemsViewModel, IRecipient<ItemArchiveStatusChangedMessage>
{
    public MyListViewModel() : base()
    {
        SaveNewUrlCommand = new AsyncRelayCommand<SaveUrlDialogContentViewModel>(SaveNewUrlAsync);
    }

    public IAsyncRelayCommand SaveNewUrlCommand;

    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsArchived == false);
        return filter;
    }

    private async Task SaveNewUrlAsync(SaveUrlDialogContentViewModel? data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (Uri.TryCreate(data.Url, UriKind.Absolute, out var uri))
        {
            try
            {
                var item = await App.GetService<IPocketDataService>().AddItemAsync(uri);
                Items.Insert(0, item);
                Selected = item;
            }
            catch (Exception ex)
            {
                await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".GetLocalized());
            }
        }
        else
        {
            await App.MainWindow.ShowMessageDialogAsync("ErrorMessage_InvalidUrl".GetLocalized());
        }
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
