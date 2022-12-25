using CommunityToolkit.Mvvm.Messaging;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class FavoritesViewModel : ItemsViewModel
{
    public FavoritesViewModel(): base()
    {
        WeakReferenceMessenger.Default.Register<ItemFavoriteStatusChangedMessage>(this, async (recipient, message) =>
        {
            await RefreshList();
            EnsureItemSelected();
        });
    }

    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsFavorited == true);
        return filter;
    }
}
