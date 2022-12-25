using CommunityToolkit.WinUI;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Helpers;

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
        filter.SetFilterCondition(item => item.Title.ToLower().Contains(_searchText.ToLower()));
        return filter;
    }

    protected async override Task NavigatedTo(object parameter)
    {
        SearchText = (string)parameter;
        await base.NavigatedTo(parameter);
        OnPropertyChanged(nameof(ListHeader));
    }
}
