using PocketClient.Core.Models;
using PocketClient.Core.Specifications;

namespace PocketClient.Desktop.ViewModels;

public class ArchiveViewModel : ItemsViewModel
{
    protected override BaseSpecification<PocketItem> BuildFilter() {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.IsArchived == true);
        return filter;
    }
}
