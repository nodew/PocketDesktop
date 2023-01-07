using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

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

    private void OnViewStateChanged(object sender, bool e)
    {
        ViewModel.ShowListAndDetails = e;
        ViewModel.EnsureItemSelected();
    }

    private async void ShowSaveUrlDialog(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();
        SaveUrlDialogContent content = new SaveUrlDialogContent(dialog);

        dialog.XamlRoot = this.XamlRoot;
        dialog.RequestedTheme = App.GetService<IThemeSelectorService>().Theme;
        dialog.Title = "SaveUrlDialog_Title".GetLocalized();
        dialog.PrimaryButtonText = "Save".GetLocalized();
        dialog.SecondaryButtonText = "Cancel".GetLocalized();
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Content = content;
        dialog.IsPrimaryButtonEnabled = false;

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            await ViewModel.SaveNewUrlCommand.ExecuteAsync(content.ViewModel);
        }
    }
}
