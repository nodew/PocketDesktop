using PocketClient.Core.Models;
using PocketClient.Core.Specifications;

namespace PocketClient.Desktop.ViewModels;
public class VideosViewModel : ItemsViewModel
{
    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.Type == ItemType.Video && item.IsArchived == false);
        return filter;
    }
}