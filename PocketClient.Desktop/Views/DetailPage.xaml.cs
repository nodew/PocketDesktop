using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PocketClient.Core.Models;
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
            SetValue(SelectedItemProperty, value);
            ViewModel.IsLoading = true;
            ViewModel.SelectedItem = value;
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
}
