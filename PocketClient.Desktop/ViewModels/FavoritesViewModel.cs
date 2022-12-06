using CommunityToolkit.Mvvm.ComponentModel;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Contracts.ViewModels;

namespace PocketClient.Desktop.ViewModels;

public class FavoritesViewModel : ItemsViewModel
{
    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsFavorited == true);
        return filter;
    }
}
