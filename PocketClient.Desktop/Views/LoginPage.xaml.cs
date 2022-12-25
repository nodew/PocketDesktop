using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Desktop.Contracts.Services;

namespace PocketClient.Desktop.Views;

public sealed partial class LoginPage : Page
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void Login(object sender, RoutedEventArgs e)
    {
        await App.GetService<IAuthService>().LaunchAuthorizationAsync();
    }
}
