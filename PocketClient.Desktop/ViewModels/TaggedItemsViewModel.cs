using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class TaggedItemsViewModel : ItemsViewModel
{
    private Tag? _currentTag;

    public TaggedItemsViewModel() : base()
    {
    }

    public Tag? CurrentTag
    {
        get => _currentTag;
        set => SetProperty(ref _currentTag, value);
    }

    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();

        if (FilterOption == PocketItemFilterOption.All)
        {
            filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!));
        }
        else if (FilterOption == PocketItemFilterOption.UnArchived)
        {
            filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!) && item.IsArchived == false);
        }
        else if (FilterOption == PocketItemFilterOption.Archived)
        {
            filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!) && item.IsArchived == true);
        }
        else if (FilterOption == PocketItemFilterOption.Favorited)
        {
            filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!) && item.IsFavorited == true);
        }
        
        return filter;
    }

    protected async override Task NavigatedTo(object parameter)
    {
        CurrentTag = (Tag)parameter;
        await base.NavigatedTo(parameter);
    }
}
