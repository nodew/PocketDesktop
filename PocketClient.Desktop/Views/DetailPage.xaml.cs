using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PocketClient.Core.Models;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/.
public sealed partial class DetailPage : Page
{
    public static readonly DependencyProperty SelectedItemProperty =
       DependencyProperty.Register(
          nameof(SelectedItem),
          typeof(PocketItem),
          typeof(DetailPage),
          new PropertyMetadata(null));

    public PocketItem SelectedItem
    {
        get => (PocketItem)GetValue(SelectedItemProperty);
        set
        {
            if (SelectedItem?.Url != value?.Url)
            {
                ViewModel.IsLoading = true;
            }
            ViewModel.SelectedItem = value;
            SetValue(SelectedItemProperty, value);
        }
    }

    public DetailViewModel ViewModel
    {
        get;
    }

    public DetailPage()
    {
        ViewModel = App.GetService<DetailViewModel>();
        InitializeComponent();

        ViewModel.WebViewService.Initialize(WebView);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        ViewModel.OnNavigatedFrom();
    }

    private async void ShowManageTagsDialog(object sender, RoutedEventArgs e)
    {

        var vm = new ManageTagsDialogContentViewModel();
        await vm.InitializeAsync(SelectedItem.Tags);

        var content = new ManageTagsDialogContent(vm);

        var dialog = new ContentDialog();
        dialog.XamlRoot = this.XamlRoot;
        dialog.RequestedTheme = App.GetService<IThemeSelectorService>().Theme;
        dialog.Title = SelectedItem.Tags.Count > 0
            ? "EditTags".Format()
            : "AddTags".Format();
        dialog.PrimaryButtonText = "PrimaryButton_Save".Format();
        dialog.SecondaryButtonText = "Button_Cancel".Format();
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Content = content;

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            await ViewModel.UpdateTagsCommand.ExecuteAsync(vm.SelectedTags.ToList());
        }
    }
}
