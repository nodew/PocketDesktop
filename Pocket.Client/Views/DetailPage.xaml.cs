using Microsoft.UI.Xaml.Controls;

using Pocket.Client.ViewModels;

namespace Pocket.Client.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/.
public sealed partial class DetailPage : Page
{
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
}
