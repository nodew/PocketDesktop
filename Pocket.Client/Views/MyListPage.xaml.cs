using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using Pocket.Client.ViewModels;

namespace Pocket.Client.Views;

public sealed partial class MyListPage : Page
{
    public MyListViewModel ViewModel
    {
        get;
    }

    public MyListPage()
    {
        ViewModel = App.GetService<MyListViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
