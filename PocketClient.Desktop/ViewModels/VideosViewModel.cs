using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;
public class VideosViewModel : ItemsViewModel
{
    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();

        if (FilterOption == PocketItemFilterOption.All)
        {
            filter.SetFilterCondition(item => item.Type == ItemType.Video);
        }
        else if (FilterOption == PocketItemFilterOption.UnArchived)
        {
            filter.SetFilterCondition(item => item.Type == ItemType.Video && item.IsArchived == false);
        }
        else if (FilterOption == PocketItemFilterOption.Archived)
        {
            filter.SetFilterCondition(item => item.Type == ItemType.Video && item.IsArchived == true);
        }
        else if (FilterOption == PocketItemFilterOption.Favorited)
        {
            filter.SetFilterCondition(item => item.Type == ItemType.Video && item.IsFavorited == true);
        }
        
        return filter;
    }
}