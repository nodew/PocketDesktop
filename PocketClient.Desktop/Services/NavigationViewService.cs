using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Core.Models;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Services;

public class NavigationViewService : INavigationViewService
{
    private const string PinnedTagsSettingsKey = "Tags";

    private readonly INavigationService _navigationService;

    private readonly IPageService _pageService;

    private readonly ILocalSettingsService _localSettingService;

    private NavigationView? _navigationView;

    public IList<object>? MenuItems => _navigationView?.MenuItems;

    public object? SettingsItem => _navigationView?.SettingsItem;

    public NavigationViewService(INavigationService navigationService, IPageService pageService, ILocalSettingsService localSettingService)
    {
        _navigationService = navigationService;
        _pageService = pageService;
        _localSettingService = localSettingService;
    }

    [MemberNotNull(nameof(_navigationView))]
    public void Initialize(NavigationView navigationView)
    {
        _navigationView = navigationView;
        _navigationView.BackRequested += OnBackRequested;
        _navigationView.ItemInvoked += OnItemInvoked;

        SetupPinnedTags();
    }

    public void UnregisterEvents()
    {
        if (_navigationView != null)
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }
    }

    public NavigationViewItem? GetSelectedItem(Type pageType)
    {
        if (_navigationView != null)
        {
            return GetSelectedItem(_navigationView.MenuItems, pageType) ?? GetSelectedItem(_navigationView.FooterMenuItems, pageType);
        }

        return null;
    }

    public async Task PinTagAsync(Tag tag)
    {
        var tags = await _localSettingService.ReadSettingAsync<List<string>>(PinnedTagsSettingsKey);

        if (tags != null && tags.Contains(tag.Name))
        {
            return;
        }

        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.High, () =>
        {
            PinTagToNavigationMenu(tag.Name);
        });

        if (tags == null)
        {
            await _localSettingService.SaveSettingAsync(PinnedTagsSettingsKey, new List<string>() { tag.Name });
        }
        else
        {
            tags.Add(tag.Name);
            await _localSettingService.SaveSettingAsync(PinnedTagsSettingsKey, tags);
        }
    }

    public async Task RemovePinnedTagAsync(Tag tag)
    {
        var pinnedTagNavItem = MenuItems!.OfType<NavigationViewItem>()
            .Where(IsMenuItemForPinnedTag)
            .FirstOrDefault(item =>
            {
                var content = (string)item.Content;
                return content == tag.Name;
            });

        if (pinnedTagNavItem != null)
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.High, () =>
            {
                MenuItems!.Remove(pinnedTagNavItem);
            });
        }

        var tags = await _localSettingService.ReadSettingAsync<List<string>>(PinnedTagsSettingsKey);

        if (tags != null && tags.Contains(tag.Name))
        {
            tags.Remove(tag.Name);
            await _localSettingService.SaveSettingAsync(PinnedTagsSettingsKey, tags);
        }
    }

    public async Task RenamePinnedTagAsync(Tag tag, string newName)
    {
        var pinnedTagNavItem = MenuItems!.OfType<NavigationViewItem>()
            .Where(IsMenuItemForPinnedTag)
            .FirstOrDefault(item =>
            {
                var content = (string)item.Content;
                return content == tag.Name;
            });

        if (pinnedTagNavItem != null)
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.High, () =>
            {
                pinnedTagNavItem.Content = newName;
            });
        }

        var tags = await _localSettingService.ReadSettingAsync<List<string>>(PinnedTagsSettingsKey);

        if (tags != null && tags.Contains(tag.Name))
        {
            var idx = tags.FindIndex(name => name == tag.Name);
            tags[idx] = newName;
            await _localSettingService.SaveSettingAsync(PinnedTagsSettingsKey, tags);
        }
    }

    private static bool IsMenuItemForPinnedTag(NavigationViewItem menuItem)
    {
        if (menuItem.Tag is string tag)
        {
            return tag == "Tag";
        }

        return false;
    }

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => _navigationService.GoBack();

    private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        }
        else
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem != null)
            {
                if (selectedItem.Tag is string tag && tag == "Tag")
                {
                    _navigationService.NavigateTo(typeof(TaggedItemsViewModel).FullName!, selectedItem.Content);
                }
                else if (selectedItem?.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
                {
                    _navigationService.NavigateTo(pageKey);
                }
            }
        }
    }

    private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
    {
        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(item, pageType))
            {
                return item;
            }

            var selectedChild = GetSelectedItem(item.MenuItems, pageType);
            if (selectedChild != null)
            {
                return selectedChild;
            }
        }

        return null;
    }

    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
    {
        if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
        {
            return _pageService.GetPageType(pageKey) == sourcePageType;
        }

        return false;
    }

    private void SetupPinnedTags()
    {
        App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
        {
            var tags = await _localSettingService.ReadSettingAsync<List<string>>(PinnedTagsSettingsKey);

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    PinTagToNavigationMenu(tag);
                }
            }
        });
    }

    private void PinTagToNavigationMenu(string tag)
    {
        var navigationViewItem = new NavigationViewItem();
        var icon = new SymbolIcon(Symbol.Tag);

        navigationViewItem.Tag = "Tag";
        navigationViewItem.Icon = icon;
        navigationViewItem.Content = tag;

        MenuItems!.Add(navigationViewItem);
    }
}
