using CommunityToolkit.WinUI;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class SearchResultsViewModel : ItemsViewModel
{
    private string _searchText;

    public SearchResultsViewModel() : base()
    {
        _searchText = string.Empty;
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public string ListHeader => "SearchResultsPageTitle".GetLocalized(SearchText, Items.Count.ToString());

    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();

        if (FilterOption == PocketItemFilterOption.All)
        {
            filter.SetFilterCondition(item => item.Title.ToLower().Contains(_searchText.ToLower()));
        }
        else if (FilterOption == PocketItemFilterOption.UnArchived)
        {
            filter.SetFilterCondition(item => item.Title.ToLower().Contains(_searchText.ToLower()) && item.IsArchived == false);
        }
        else if (FilterOption == PocketItemFilterOption.Archived)
        {
            filter.SetFilterCondition(item => item.Title.ToLower().Contains(_searchText.ToLower()) && item.IsArchived == true);
        }
        else if (FilterOption == PocketItemFilterOption.Favorited)
        {
            filter.SetFilterCondition(item => item.Title.ToLower().Contains(_searchText.ToLower()) && item.IsFavorited == true);
        }
        
        return filter;
    }

    protected async override Task NavigatedTo(object parameter)
    {
        SearchText = (string)parameter;
        await base.NavigatedTo(parameter);
        OnPropertyChanged(nameof(ListHeader));
    }
}
