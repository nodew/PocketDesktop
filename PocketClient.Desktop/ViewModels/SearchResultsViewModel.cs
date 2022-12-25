using PocketClient.Core.Models;
using PocketClient.Core.Specifications;

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

    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.Title.Contains(_searchText));
        return filter;
    }

    protected async override Task NavigatedTo(object parameter)
    {
        SearchText = (string)parameter;
        await base.NavigatedTo(parameter);
    }
}
