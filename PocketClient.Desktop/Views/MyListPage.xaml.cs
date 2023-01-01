using CommunityToolkit.WinUI.UI.Controls;

using PocketClient.Desktop.ViewModels;

namespace PocketClient.Desktop.Views;

public sealed partial class MyListPage : ItemsPage<MyListViewModel>
{
    public MyListPage() : base()
    {
        InitializeComponent();
    }
}
