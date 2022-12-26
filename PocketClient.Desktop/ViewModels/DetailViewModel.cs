﻿using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Web.WebView2.Core;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Models;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

// TODO: Review best practices and distribution guidelines for WebView2.
// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public class DetailViewModel : ObservableRecipient
{
    private bool _isLoading = true;
    private bool _hasFailures;
    private PocketItem? _pocketItem;
    private bool _isUpdating;

    public IWebViewService WebViewService
    {
        get;
    }

    public PocketItem? SelectedItem
    {
        get => _pocketItem; 
        set => SetProperty(ref _pocketItem, value); 
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool HasFailures
    {
        get => _hasFailures;
        set => SetProperty(ref _hasFailures, value);
    }

    public bool IsUpdating
    {
        get => _isUpdating;
        set => SetProperty(ref _isUpdating, value);
    }

    public IAsyncRelayCommand ArchiveCommand { get; }

    public IAsyncRelayCommand AddToListCommand { get; }

    public IAsyncRelayCommand FavoriteCommand { get; }

    public IAsyncRelayCommand UnfavoriteCommand { get; }

    public IAsyncRelayCommand RemoveItemCommand { get; }

    public ICommand ReloadCommand { get; }

    public IAsyncRelayCommand OpenInBrowserCommand { get; }

    public DetailViewModel(IWebViewService webViewService)
    {
        WebViewService = webViewService;
        webViewService.NavigationCompleted += OnNavigationCompleted;

        ReloadCommand = new RelayCommand(OnRetry);
        OpenInBrowserCommand = new AsyncRelayCommand(OpenInBrowserAsync, () => WebViewService.Source != null);
        ArchiveCommand = new AsyncRelayCommand(ArchiveItemAsync, CanUpdateItem);
        AddToListCommand = new AsyncRelayCommand(AddItemToListAsync, CanUpdateItem);
        FavoriteCommand = new AsyncRelayCommand(FavoriteItemAsync, CanUpdateItem);
        UnfavoriteCommand = new AsyncRelayCommand(UnfavoriteItemAsync, CanUpdateItem);
        RemoveItemCommand = new AsyncRelayCommand(RemoveItemAsync, CanUpdateItem);
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

    private async Task OpenInBrowserAsync()
    {
        await Windows.System.Launcher.LaunchUriAsync(WebViewService.Source);
    }

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

        }

        IsUpdating = false;
    }

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

        }

        IsUpdating = false;
    }

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
            Console.WriteLine(ex);
        }

        IsUpdating = false;
    }

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
            Console.WriteLine(ex);
        }

        IsUpdating = false;
    }

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
            Console.WriteLine(ex);
        }

        IsUpdating = false;
    }

    private bool CanUpdateItem()
    {
        return SelectedItem != null && IsUpdating != true;
    }
}
