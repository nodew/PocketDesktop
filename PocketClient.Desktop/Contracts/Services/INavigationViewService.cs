using Microsoft.UI.Xaml.Controls;
using PocketClient.Core.Models;

namespace PocketClient.Desktop.Contracts.Services;

public interface INavigationViewService
{
    IList<object>? MenuItems
    {
        get;
    }

    object? SettingsItem
    {
        get;
    }

    void Initialize(NavigationView navigationView);

    void UnregisterEvents();

    NavigationViewItem? GetSelectedItem(Type pageType);

    public Task PinTagAsync(Tag tag);

    public Task RemovePinnedTagAsync(Tag tag);

    public Task RenamePinnedTagAsync(Tag tag, string newName);
}
