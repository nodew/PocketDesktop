using CommunityToolkit.Mvvm.ComponentModel;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Contracts.ViewModels;

namespace PocketClient.Desktop.ViewModels;

public class ArticlesViewModel : ItemsViewModel
{
    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.Type == ItemType.Article && item.IsArchived == false);
        return filter;
    }
}