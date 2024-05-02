using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Models;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

// TODO: Review best practices and distribution guidelines for WebView2.
// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public partial class DetailViewModel : ObservableRecipient
{
    private readonly ILogger<DetailViewModel> _logger;

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private bool hasFailures;

    [ObservableProperty]
    private PocketItem? selectedItem;

    [ObservableProperty]
    private bool isUpdating;

    [ObservableProperty]
    private bool isReadMode;

    [ObservableProperty]
    private Uri? source;

    public IWebViewService WebViewService
    {
        get;
    }

    public bool HasTags => SelectedItem?.Tags?.Count > 0;

    public DetailViewModel(
        IWebViewService webViewService,
        ILogger<DetailViewModel> logger
    )
    {
        WebViewService = webViewService;
        _logger = logger;

        WebViewService.NavigationCompleted += OnNavigationCompleted;

        source = null;
    }

    public void UpdateSelectedItem(PocketItem item)
    {
        if (SelectedItem?.Url != item?.Url)
        {
            IsLoading = true;
        }

        SelectedItem = item;

        Source = item?.Url;
    }

    public void OnNavigatedTo(object parameter)
    {
        WebViewService.NavigationCompleted += OnNavigationCompleted;
    }

    public void OnNavigatedFrom()
    {
        WebViewService.UnregisterEvents();
        WebViewService.NavigationCompleted -= OnNavigationCompleted;
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2WebErrorStatus webErrorStatus)
    {
        IsLoading = false;

        if (webErrorStatus != default)
        {
            HasFailures = true;
        }
    }

    private void OnRetry()
    {
        HasFailures = false;
        IsLoading = true;
        WebViewService?.Reload();
    }

    [RelayCommand]
    private void Reload()
    {
        OnRetry();
    }

    [RelayCommand(CanExecute = nameof(CanOpenInBrowser))]
    private async Task OpenInBrowserAsync()
    {
        await Windows.System.Launcher.LaunchUriAsync(WebViewService.Source);
    }

    private bool CanOpenInBrowser()
    {
        return WebViewService.Source != null;
    }

    [RelayCommand(CanExecute = nameof(CanUpdateItem))]
    private async Task ArchiveItemAsync()
    {
        IsUpdating = true;
        try
        {
            await App.GetService<IPocketDataService>().ArchiveItemAsync(SelectedItem!);
            SelectedItem!.IsArchived = true;
            OnPropertyChanged(nameof(SelectedItem));
            WeakReferenceMessenger.Default.Send(new ItemArchiveStatusChangedMessage(SelectedItem));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to archive item");
            await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".Format());
        }

        IsUpdating = false;
    }

    [RelayCommand(CanExecute = nameof(CanUpdateItem))]
    private async Task AddItemToListAsync()
    {
        IsUpdating = true;
        try
        {
            await App.GetService<IPocketDataService>().ReAddItemAsync(SelectedItem!);
            SelectedItem!.IsArchived = false;
            OnPropertyChanged(nameof(SelectedItem));
            WeakReferenceMessenger.Default.Send(new ItemArchiveStatusChangedMessage(SelectedItem));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add item");
            await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".Format());
        }

        IsUpdating = false;
    }

    [RelayCommand(CanExecute = nameof(CanUpdateItem))]
    private async Task FavoriteItemAsync()
    {
        IsUpdating = true;
        try
        {
            await App.GetService<IPocketDataService>().FavoriteItemAsync(SelectedItem!);
            SelectedItem!.IsFavorited = true;
            OnPropertyChanged(nameof(SelectedItem));
            WeakReferenceMessenger.Default.Send(new ItemFavoriteStatusChangedMessage(SelectedItem));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to favorite item");
            await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".Format());
        }

        IsUpdating = false;
    }

    [RelayCommand(CanExecute = nameof(CanUpdateItem))]
    private async Task UnfavoriteItemAsync()
    {
        IsUpdating = true;
        try
        {
            await App.GetService<IPocketDataService>().UnfavoriteItemAsync(SelectedItem!);
            SelectedItem!.IsFavorited = false;
            OnPropertyChanged(nameof(SelectedItem));
            WeakReferenceMessenger.Default.Send(new ItemFavoriteStatusChangedMessage(SelectedItem));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to unfavorite item");
            await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".Format());
        }

        IsUpdating = false;
    }

    [RelayCommand(CanExecute = nameof(CanUpdateItem))]
    private async Task RemoveItemAsync()
    {
        IsUpdating = true;
        try
        {
            await App.GetService<IPocketDataService>().RemoveItemAsync(SelectedItem!);
            WeakReferenceMessenger.Default.Send(new ItemRemovedMessage(SelectedItem!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove item");
            await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".Format());
        }

        IsUpdating = false;
    }

    [RelayCommand(CanExecute = nameof(CanUpdateItem))]
    private async Task UpdateTagsAsync(List<Tag> newTags)
    {
        IsUpdating = true;

        try
        {
            await App.GetService<IPocketDataService>().UpdateItemTags(SelectedItem!, newTags);
            var item = await App.GetService<IPocketDataService>().GetItemByIdAsync(SelectedItem!.Id);
            SelectedItem!.Tags = item!.Tags;
            OnPropertyChanged(nameof(SelectedItem));
            WeakReferenceMessenger.Default.Send(new ItemTagsUpdatedMessage(SelectedItem!, item!.Tags));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update tags");
            await App.MainWindow.ShowMessageDialogAsync(ex.Message, "Exception_DialogTitle".Format());
        }

        IsUpdating = false;
    }

    private bool CanUpdateItem()
    {
        return SelectedItem != null && IsUpdating != true;
    }
}
