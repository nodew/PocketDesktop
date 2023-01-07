using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

public sealed partial class SearchResultsPage : Page
{
    public SearchResultsViewModel ViewModel
    {
        get;
    }

    public SearchResultsPage()
    {
        ViewModel = App.GetService<SearchResultsViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, bool e)
    {
        ViewModel.ShowListAndDetails = e;
        ViewModel.EnsureItemSelected();
    }
}
