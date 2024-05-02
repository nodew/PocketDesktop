using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private async void HandleLogoutAction(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();
        dialog.XamlRoot = this.XamlRoot;
        dialog.RequestedTheme = App.GetService<IThemeSelectorService>().Theme;
        dialog.Title = "LogoutDialog_Title".Format();
        dialog.Content = "LogoutDialog_Content".Format();
        dialog.PrimaryButtonText = "LogoutDialog_PrimaryBtn".Format();
        dialog.SecondaryButtonText = "Button_Cancel".Format();
        dialog.DefaultButton = ContentDialogButton.Primary;

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            ViewModel.LogoutCommand.Execute(null);
        }
    }

    private void HandleThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedTheme = e.AddedItems.FirstOrDefault();

        if (ViewModel.SwitchThemeCommand.CanExecute(selectedTheme))
        {
            ViewModel.SwitchThemeCommand.Execute(selectedTheme);
        }
    }

    private void HandleLanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedLanguage = e.AddedItems.FirstOrDefault();

        if (ViewModel.SwitchLanguageCommand.CanExecute(selectedLanguage))
        {
            ViewModel.SwitchLanguageCommand.Execute(selectedLanguage);
        }
    }
}
