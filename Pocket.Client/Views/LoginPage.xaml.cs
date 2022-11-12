using Microsoft.UI.Xaml.Controls;

using Pocket.Client.ViewModels;

namespace Pocket.Client.Views;

public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel
    {
        get;
    }

    public LoginPage()
    {
        ViewModel = App.GetService<LoginViewModel>();
        InitializeComponent();
    }
}
