using System.Runtime.Versioning;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

public class ItemsPage<T> : Page where T : ItemsViewModel
{
    public readonly T ViewModel;

    public ItemsPage()
    {
        ViewModel = App.GetService<T>();
    }

    protected void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.ShowListAndDetails = true;
        }
        else
        {
            ViewModel.ShowListAndDetails = false;
        }

        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
