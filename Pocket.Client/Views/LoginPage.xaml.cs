using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Pocket.Client.Contracts.Services;
using Pocket.Client.ViewModels;

namespace Pocket.Client.Views;

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
