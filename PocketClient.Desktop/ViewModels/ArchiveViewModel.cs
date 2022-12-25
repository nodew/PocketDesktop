using CommunityToolkit.Mvvm.Messaging;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class ArchiveViewModel : ItemsViewModel
{
    public ArchiveViewModel() : base()
    {
        WeakReferenceMessenger.Default.Register<ItemArchiveStatusChangedMessage>(this, async (recipient, message) =>
        {
            await RefreshList();
            EnsureItemSelected();
        });
    }

    protected override BaseSpecification<PocketItem> BuildFilter() {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsArchived == true);
        return filter;
    }
}
